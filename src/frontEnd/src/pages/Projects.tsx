import { FC, useState } from 'react';
import { Card, Button, Typography, Row, Col, Spin, Empty, Modal, Form, Input, ColorPicker } from 'antd';
import { PlusOutlined, EditOutlined, DeleteOutlined, FolderOutlined } from '@ant-design/icons';
import { useNavigate } from 'react-router-dom';
import { useProjects, useCreateProject, useUpdateProject, useDeleteProject } from '../hooks/useApi';
import { CreateProjectRequest, UpdateProjectRequest, Project } from '../types';

const { Title } = Typography;

const Projects: FC = () => {
  const navigate = useNavigate();
  const [isModalVisible, setIsModalVisible] = useState(false);
  const [editingProject, setEditingProject] = useState<Project | null>(null);
  const [form] = Form.useForm();

  const { data: projects, isLoading } = useProjects();
  const createProjectMutation = useCreateProject();
  const updateProjectMutation = useUpdateProject();
  const deleteProjectMutation = useDeleteProject();

  const handleCreateProject = () => {
    setEditingProject(null);
    form.resetFields();
    setIsModalVisible(true);
  };

  const handleEditProject = (project: Project) => {
    setEditingProject(project);
    form.setFieldsValue({
      name: project.name,
      description: project.description,
      color: project.color || '#0ea5e9',
    });
    setIsModalVisible(true);
  };

  const handleDeleteProject = async (projectId: string) => {
    try {
      await deleteProjectMutation.mutateAsync(projectId);
    } catch (error) {
      console.error('Ошибка при удалении проекта:', error);
    }
  };

  const handleModalOk = async () => {
    try {
      const values = await form.validateFields();
      
      if (editingProject) {
        await updateProjectMutation.mutateAsync({
          id: editingProject.id,
          project: values as UpdateProjectRequest,
        });
      } else {
        await createProjectMutation.mutateAsync(values as CreateProjectRequest);
      }
      
      setIsModalVisible(false);
      form.resetFields();
    } catch (error) {
      console.error('Ошибка при сохранении проекта:', error);
    }
  };

  const handleProjectClick = (projectId: string) => {
    navigate(`/projects/${projectId}/tasks`);
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
          <Title level={2} className="mb-2">Проекты</Title>
          <p className="text-gray-600">Управление вашими проектами</p>
        </div>
        <Button 
          type="primary" 
          icon={<PlusOutlined />} 
          onClick={handleCreateProject}
        >
          Создать проект
        </Button>
      </div>

      {projects && projects.length > 0 ? (
        <Row gutter={[16, 16]}>
          {projects.map((project) => (
            <Col xs={24} sm={12} lg={8} xl={6} key={project.id}>
              <Card
                hoverable
                className="h-full"
                actions={[
                  <EditOutlined 
                    key="edit" 
                    onClick={() => handleEditProject(project)}
                  />,
                  <DeleteOutlined 
                    key="delete" 
                    onClick={() => handleDeleteProject(project.id)}
                  />,
                ]}
                onClick={() => handleProjectClick(project.id)}
              >
                <div className="flex items-center space-x-3 mb-3">
                  <div 
                    className="w-6 h-6 rounded-lg flex items-center justify-center"
                    style={{ backgroundColor: project.color || '#0ea5e9' }}
                  >
                    <FolderOutlined className="text-white" />
                  </div>
                  <Title level={4} className="mb-0">{project.name}</Title>
                </div>
                
                {project.description && (
                  <p className="text-gray-600 text-sm mb-3">{project.description}</p>
                )}
                
                <div className="text-xs text-gray-500">
                  Создан: {new Date(project.createdAt).toLocaleDateString('ru-RU')}
                </div>
              </Card>
            </Col>
          ))}
        </Row>
      ) : (
        <Card>
          <Empty
            image={Empty.PRESENTED_IMAGE_SIMPLE}
            description="Проекты не найдены"
          >
            <Button type="primary" onClick={handleCreateProject}>
              Создать первый проект
            </Button>
          </Empty>
        </Card>
      )}

      {/* Модальное окно для создания/редактирования проекта */}
      <Modal
        title={editingProject ? 'Редактировать проект' : 'Создать проект'}
        open={isModalVisible}
        onOk={handleModalOk}
        onCancel={() => setIsModalVisible(false)}
        confirmLoading={createProjectMutation.isPending || updateProjectMutation.isPending}
      >
        <Form form={form} layout="vertical">
          <Form.Item
            name="name"
            label="Название проекта"
            rules={[{ required: true, message: 'Введите название проекта' }]}
          >
            <Input placeholder="Введите название проекта" />
          </Form.Item>
          
          <Form.Item
            name="description"
            label="Описание"
          >
            <Input.TextArea 
              placeholder="Введите описание проекта" 
              rows={3}
            />
          </Form.Item>
          
          <Form.Item
            name="color"
            label="Цвет проекта"
            initialValue="#0ea5e9"
          >
            <ColorPicker />
          </Form.Item>
        </Form>
      </Modal>
    </div>
  );
};

export default Projects;
