import { FC } from 'react';
import { Card, Row, Col, Statistic, Typography, Spin } from 'antd';
import { 
  ProjectOutlined, 
  CheckCircleOutlined, 
  ClockCircleOutlined, 
  ExclamationCircleOutlined 
} from '@ant-design/icons';
import { useTasks, useProjects } from '../hooks/useApi';
import { TaskStatus } from '../types';
import TestApi from '../components/TestApi';

const { Title } = Typography;

const Dashboard: FC = () => {
  const { data: tasks, isLoading: tasksLoading } = useTasks();
  const { data: projects, isLoading: projectsLoading } = useProjects();

  const isLoading = tasksLoading || projectsLoading;

  // Подсчет статистики
  const totalTasks = tasks?.length || 0;
  const completedTasks = tasks?.filter(task => task.status === TaskStatus.Done).length || 0;
  const inProgressTasks = tasks?.filter(task => task.status === TaskStatus.InProgress).length || 0;
  const blockedTasks = tasks?.filter(task => task.status === TaskStatus.Blocked).length || 0;
  const totalProjects = projects?.length || 0;

  if (isLoading) {
    return (
      <div className="flex justify-center items-center h-64">
        <Spin size="large" />
      </div>
    );
  }

  return (
    <div className="space-y-6">
      <div>
        <Title level={2} className="mb-2">Дашборд</Title>
        <p className="text-gray-600">Обзор ваших проектов и задач</p>
      </div>

      {/* Тестирование API */}
      <TestApi />

      {/* Статистика */}
      <Row gutter={[16, 16]}>
        <Col xs={24} sm={12} lg={6}>
          <Card>
            <Statistic
              title="Всего проектов"
              value={totalProjects}
              prefix={<ProjectOutlined className="text-blue-500" />}
            />
          </Card>
        </Col>
        
        <Col xs={24} sm={12} lg={6}>
          <Card>
            <Statistic
              title="Всего задач"
              value={totalTasks}
              prefix={<CheckCircleOutlined className="text-green-500" />}
            />
          </Card>
        </Col>
        
        <Col xs={24} sm={12} lg={6}>
          <Card>
            <Statistic
              title="В работе"
              value={inProgressTasks}
              prefix={<ClockCircleOutlined className="text-orange-500" />}
            />
          </Card>
        </Col>
        
        <Col xs={24} sm={12} lg={6}>
          <Card>
            <Statistic
              title="Заблокировано"
              value={blockedTasks}
              prefix={<ExclamationCircleOutlined className="text-red-500" />}
            />
          </Card>
        </Col>
      </Row>

      {/* Последние проекты */}
      <Card title="Последние проекты">
        {projects && projects.length > 0 ? (
          <div className="space-y-3">
            {projects.slice(0, 5).map((project) => (
              <div key={project.id} className="flex items-center justify-between p-3 bg-gray-50 rounded-lg">
                <div className="flex items-center space-x-3">
                  <div 
                    className="w-4 h-4 rounded-full" 
                    style={{ backgroundColor: project.color || '#0ea5e9' }}
                  />
                  <span className="font-medium">{project.name}</span>
                </div>
                <span className="text-sm text-gray-500">
                  {new Date(project.createdAt).toLocaleDateString('ru-RU')}
                </span>
              </div>
            ))}
          </div>
        ) : (
          <p className="text-gray-500 text-center py-8">Проекты не найдены</p>
        )}
      </Card>

      {/* Последние задачи */}
      <Card title="Последние задачи">
        {tasks && tasks.length > 0 ? (
          <div className="space-y-3">
            {tasks.slice(0, 5).map((task) => (
              <div key={task.id} className="flex items-center justify-between p-3 bg-gray-50 rounded-lg">
                <div className="flex items-center space-x-3">
                  <span className="font-medium">{task.title}</span>
                  <span className={`px-2 py-1 rounded-full text-xs ${
                    task.status === TaskStatus.Done ? 'bg-green-100 text-green-800' :
                    task.status === TaskStatus.InProgress ? 'bg-blue-100 text-blue-800' :
                    task.status === TaskStatus.Blocked ? 'bg-red-100 text-red-800' :
                    'bg-gray-100 text-gray-800'
                  }`}>
                    {task.status}
                  </span>
                </div>
                <span className="text-sm text-gray-500">
                  {task.project?.name}
                </span>
              </div>
            ))}
          </div>
        ) : (
          <p className="text-gray-500 text-center py-8">Задачи не найдены</p>
        )}
      </Card>
    </div>
  );
};

export default Dashboard;
