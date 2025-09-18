import { FC, useState } from 'react';
import { Card, Button, Typography, Row, Col, Spin, Modal, Form, Input, Select, DatePicker } from 'antd';
import { PlusOutlined, EditOutlined, DeleteOutlined, UserOutlined } from '@ant-design/icons';
import { useParams } from 'react-router-dom';
import { useProjectTasks, useCreateTask, useUpdateTask, useDeleteTask, useUsers, useChangeTaskStatus } from '../hooks/useApi';
import { CreateTaskRequest, UpdateTaskRequest, Task, TaskStatus, TaskPriority } from '../types';
import dayjs from 'dayjs';
import {
  DndContext,
  DragEndEvent,
  DragOverlay,
  DragStartEvent,
  PointerSensor,
  useSensor,
  useSensors,
  closestCorners,
} from '@dnd-kit/core';
import {
  SortableContext,
  verticalListSortingStrategy,
} from '@dnd-kit/sortable';
import {
  useSortable,
} from '@dnd-kit/sortable';
import { CSS } from '@dnd-kit/utilities';

const { Title } = Typography;
const { TextArea } = Input;

// Компонент для перетаскиваемой задачи
interface DraggableTaskProps {
  task: Task;
  onEdit: (task: Task) => void;
  onDelete: (taskId: string) => void;
  priorityColors: { [key in TaskPriority]: string };
}

const DraggableTask: FC<DraggableTaskProps> = ({ task, onEdit, onDelete, priorityColors }) => {
  const {
    attributes,
    listeners,
    setNodeRef,
    transform,
    transition,
    isDragging,
  } = useSortable({ id: task.id });

  const style = {
    transform: CSS.Transform.toString(transform),
    transition,
    opacity: isDragging ? 0.5 : 1,
  };

  return (
    <div ref={setNodeRef} style={style} {...attributes} {...listeners}>
      <Card
        size="small"
        className="cursor-pointer hover:shadow-md transition-shadow"
        actions={[
          <EditOutlined 
            key="edit" 
            onClick={() => onEdit(task)}
          />,
          <DeleteOutlined 
            key="delete" 
            onClick={() => onDelete(task.id)}
          />,
        ]}
      >
        <div className="space-y-2">
          <div className="font-medium">{task.title}</div>
          
          {task.description && (
            <div className="text-sm text-gray-600 line-clamp-2">
              {task.description}
            </div>
          )}
          
          <div className="flex items-center justify-between">
            <span className={`px-2 py-1 rounded-full text-xs ${priorityColors[task.priority]}`}>
              {task.priority}
            </span>
            
            {task.assignees && task.assignees.length > 0 && (
              <div className="flex items-center space-x-1">
                <UserOutlined className="text-gray-400" />
                <span className="text-xs text-gray-500">
                  {task.assignees.length}
                </span>
              </div>
            )}
          </div>
          
          {task.dueDate && (
            <div className="text-xs text-gray-500">
              До: {dayjs(task.dueDate).format('DD.MM.YYYY')}
            </div>
          )}
        </div>
      </Card>
    </div>
  );
};

const ProjectTasks: FC = () => {
  const { projectId } = useParams<{ projectId: string }>();
  const [isModalVisible, setIsModalVisible] = useState(false);
  const [editingTask, setEditingTask] = useState<Task | null>(null);
  const [form] = Form.useForm();
  const [activeTask, setActiveTask] = useState<Task | null>(null);

  const { data: tasks, isLoading } = useProjectTasks(projectId!);
  const { data: users } = useUsers();
  const createTaskMutation = useCreateTask();
  const updateTaskMutation = useUpdateTask();
  const deleteTaskMutation = useDeleteTask();
  const changeStatusMutation = useChangeTaskStatus();

  // Настройка сенсоров для DragAndDrop
  const sensors = useSensors(
    useSensor(PointerSensor, {
      activationConstraint: {
        distance: 8,
      },
    })
  );

  const handleCreateTask = () => {
    setEditingTask(null);
    form.resetFields();
    setIsModalVisible(true);
  };

  const handleEditTask = (task: Task) => {
    setEditingTask(task);
    form.setFieldsValue({
      title: task.title,
      description: task.description,
      status: task.status,
      priority: task.priority,
      assigneeIds: task.assigneeIds,
      dueDate: task.dueDate ? dayjs(task.dueDate) : null,
    });
    setIsModalVisible(true);
  };

  const handleDeleteTask = async (taskId: string) => {
    try {
      await deleteTaskMutation.mutateAsync(taskId);
    } catch (error) {
      console.error('Ошибка при удалении задачи:', error);
    }
  };

  // Обработчики для DragAndDrop
  const handleDragStart = (event: DragStartEvent) => {
    const { active } = event;
    console.log('DragStart event:', { active: active.id });
    const task = tasks?.find(t => t.id === active.id);
    if (task) {
      console.log('Setting active task:', task);
      setActiveTask(task);
    }
  };

  const handleDragEnd = async (event: DragEndEvent) => {
    const { active, over } = event;
    setActiveTask(null);

    console.log('DragEnd event:', { active: active.id, over: over?.id });

    if (!over || !active) return;

    const taskId = active.id as string;
    
    // Извлекаем статус из ID колонки (убираем префикс "status-")
    const overId = over.id as string;
    const newStatus = overId.startsWith('status-') 
      ? overId.replace('status-', '') as TaskStatus
      : overId as TaskStatus;

    console.log('Task ID:', taskId, 'Over ID:', overId, 'New Status:', newStatus);

    // Находим задачу и проверяем, изменился ли статус
    const task = tasks?.find(t => t.id === taskId);
    console.log('Found task:', task);
    
    if (!task || task.status === newStatus) {
      console.log('No task found or status unchanged');
      return;
    }

    console.log('Changing status from', task.status, 'to', newStatus);

    try {
      // Используем мок-пользователя для performedBy
      const mockUserId = '00000000-0000-0000-0000-000000000000';
      await changeStatusMutation.mutateAsync({
        taskId,
        status: newStatus,
        performedBy: mockUserId,
      });
      console.log('Status changed successfully');
    } catch (error) {
      console.error('Ошибка при изменении статуса задачи:', error);
    }
  };

  const handleModalOk = async () => {
    try {
      const values = await form.validateFields();
      
      if (editingTask) {
        await updateTaskMutation.mutateAsync({
          id: editingTask.id,
          task: {
            ...values,
            dueDate: values.dueDate?.format('YYYY-MM-DD'),
          } as UpdateTaskRequest,
        });
      } else {
        await createTaskMutation.mutateAsync({
          ...values,
          projectId: projectId!,
          dueDate: values.dueDate?.format('YYYY-MM-DD'),
        } as CreateTaskRequest);
      }
      
      setIsModalVisible(false);
      form.resetFields();
    } catch (error) {
      console.error('Ошибка при сохранении задачи:', error);
    }
  };

  // Группировка задач по статусам
  const tasksByStatus = {
    [TaskStatus.New]: tasks?.filter(task => task.status === TaskStatus.New) || [],
    [TaskStatus.InProgress]: tasks?.filter(task => task.status === TaskStatus.InProgress) || [],
    [TaskStatus.Review]: tasks?.filter(task => task.status === TaskStatus.Review) || [],
    [TaskStatus.Done]: tasks?.filter(task => task.status === TaskStatus.Done) || [],
    [TaskStatus.Blocked]: tasks?.filter(task => task.status === TaskStatus.Blocked) || [],
  };

  const statusLabels = {
    [TaskStatus.New]: 'Новые',
    [TaskStatus.InProgress]: 'В работе',
    [TaskStatus.Review]: 'На проверке',
    [TaskStatus.Done]: 'Выполнено',
    [TaskStatus.Blocked]: 'Заблокировано',
  };

  const statusColors = {
    [TaskStatus.New]: 'bg-gray-100 text-gray-800',
    [TaskStatus.InProgress]: 'bg-blue-100 text-blue-800',
    [TaskStatus.Review]: 'bg-yellow-100 text-yellow-800',
    [TaskStatus.Done]: 'bg-green-100 text-green-800',
    [TaskStatus.Blocked]: 'bg-red-100 text-red-800',
  };

  const priorityColors = {
    [TaskPriority.Low]: 'bg-gray-100 text-gray-800',
    [TaskPriority.Medium]: 'bg-blue-100 text-blue-800',
    [TaskPriority.High]: 'bg-orange-100 text-orange-800',
    [TaskPriority.Critical]: 'bg-red-100 text-red-800',
  };

  if (isLoading) {
    return (
      <div className="flex justify-center items-center h-64">
        <Spin size="large" />
      </div>
    );
  }

  return (
    <div className="space-y-6">
      <div className="flex justify-between items-center">
        <div>
          <Title level={2} className="mb-2">Задачи проекта</Title>
          <p className="text-gray-600">Канбан доска для управления задачами</p>
        </div>
        <Button 
          type="primary" 
          icon={<PlusOutlined />} 
          onClick={handleCreateTask}
        >
          Создать задачу
        </Button>
      </div>

      {/* Канбан доска с DragAndDrop */}
      <DndContext
        sensors={sensors}
        collisionDetection={closestCorners}
        onDragStart={handleDragStart}
        onDragEnd={handleDragEnd}
      >
        <Row gutter={[16, 16]}>
          {Object.values(TaskStatus).map((status) => (
            <Col xs={24} sm={12} lg={6} key={status}>
              <Card 
                title={
                  <div className="flex items-center justify-between">
                    <span>{statusLabels[status]}</span>
                    <span className="text-sm text-gray-500">
                      {tasksByStatus[status].length}
                    </span>
                  </div>
                }
                className="h-full"
              >
                <SortableContext
                  id={`status-${status}`}
                  items={tasksByStatus[status].map(task => task.id)}
                  strategy={verticalListSortingStrategy}
                >
                  <div className="space-y-3 min-h-[400px]">
                    {tasksByStatus[status].map((task) => (
                      <DraggableTask
                        key={task.id}
                        task={task}
                        onEdit={handleEditTask}
                        onDelete={handleDeleteTask}
                        priorityColors={priorityColors}
                      />
                    ))}
                    
                    {tasksByStatus[status].length === 0 && (
                      <div className="text-center text-gray-500 py-8">
                        Нет задач
                      </div>
                    )}
                  </div>
                </SortableContext>
              </Card>
            </Col>
          ))}
        </Row>

        {/* DragOverlay для отображения перетаскиваемой задачи */}
        <DragOverlay>
          {activeTask ? (
            <div className="opacity-50">
              <Card
                size="small"
                className="cursor-pointer hover:shadow-md transition-shadow"
              >
                <div className="space-y-2">
                  <div className="font-medium">{activeTask.title}</div>
                  
                  {activeTask.description && (
                    <div className="text-sm text-gray-600 line-clamp-2">
                      {activeTask.description}
                    </div>
                  )}
                  
                  <div className="flex items-center justify-between">
                    <span className={`px-2 py-1 rounded-full text-xs ${priorityColors[activeTask.priority]}`}>
                      {activeTask.priority}
                    </span>
                    
                    {activeTask.assignees && activeTask.assignees.length > 0 && (
                      <div className="flex items-center space-x-1">
                        <UserOutlined className="text-gray-400" />
                        <span className="text-xs text-gray-500">
                          {activeTask.assignees.length}
                        </span>
                      </div>
                    )}
                  </div>
                  
                  {activeTask.dueDate && (
                    <div className="text-xs text-gray-500">
                      До: {dayjs(activeTask.dueDate).format('DD.MM.YYYY')}
                    </div>
                  )}
                </div>
              </Card>
            </div>
          ) : null}
        </DragOverlay>
      </DndContext>

      {/* Модальное окно для создания/редактирования задачи */}
      <Modal
        title={editingTask ? 'Редактировать задачу' : 'Создать задачу'}
        open={isModalVisible}
        onOk={handleModalOk}
        onCancel={() => setIsModalVisible(false)}
        confirmLoading={createTaskMutation.isPending || updateTaskMutation.isPending}
        width={600}
      >
        <Form form={form} layout="vertical">
          <Form.Item
            name="title"
            label="Название задачи"
            rules={[{ required: true, message: 'Введите название задачи' }]}
          >
            <Input placeholder="Введите название задачи" />
          </Form.Item>
          
          <Form.Item
            name="description"
            label="Описание"
          >
            <TextArea 
              placeholder="Введите описание задачи" 
              rows={3}
            />
          </Form.Item>
          
          <Row gutter={16}>
            <Col span={12}>
              <Form.Item
                name="status"
                label="Статус"
                rules={[{ required: true, message: 'Выберите статус' }]}
              >
                <Select placeholder="Выберите статус">
                  {Object.values(TaskStatus).map((status) => (
                    <Select.Option key={status} value={status}>
                      {statusLabels[status]}
                    </Select.Option>
                  ))}
                </Select>
              </Form.Item>
            </Col>
            
            <Col span={12}>
              <Form.Item
                name="priority"
                label="Приоритет"
                rules={[{ required: true, message: 'Выберите приоритет' }]}
              >
                <Select placeholder="Выберите приоритет">
                  {Object.values(TaskPriority).map((priority) => (
                    <Select.Option key={priority} value={priority}>
                      {priority}
                    </Select.Option>
                  ))}
                </Select>
              </Form.Item>
            </Col>
          </Row>
          
          <Form.Item
            name="assigneeIds"
            label="Исполнители"
          >
            <Select
              mode="multiple"
              placeholder="Выберите исполнителей"
              options={users?.map(user => ({
                label: user.firstName ? `${user.firstName} ${user.lastName}` : user.username,
                value: user.id,
              }))}
            />
          </Form.Item>
          
          <Form.Item
            name="dueDate"
            label="Срок выполнения"
          >
            <DatePicker 
              placeholder="Выберите дату"
              className="w-full"
            />
          </Form.Item>
        </Form>
      </Modal>
    </div>
  );
};

export default ProjectTasks;
