import { FC } from 'react';
import { Card, Typography, Avatar, Space, Tag, Row, Col, Statistic } from 'antd';
import { UserOutlined, CalendarOutlined, MailOutlined } from '@ant-design/icons';
import { useCurrentUser } from '../store';
import { useTasks } from '../hooks/useApi';
import { TaskStatus } from '../types';

const { Title, Text } = Typography;

const Profile: FC = () => {
  const currentUser = useCurrentUser();
  const { data: tasks } = useTasks();

  // Подсчет статистики пользователя
  const userTasks = tasks?.filter(task => 
    task.assigneeIds.includes(currentUser?.id || '')
  ) || [];

  const completedTasks = userTasks.filter(task => task.status === TaskStatus.Done).length;
  const inProgressTasks = userTasks.filter(task => task.status === TaskStatus.InProgress).length;
  const totalTasks = userTasks.length;

  return (
    <div className="space-y-6">
      <div>
        <Title level={2} className="mb-2">Профиль пользователя</Title>
        <p className="text-gray-600">Информация о вашем профиле и статистика</p>
      </div>

      <Row gutter={[16, 16]}>
        {/* Информация о пользователе */}
        <Col xs={24} lg={8}>
          <Card>
            <div className="text-center">
              <Avatar 
                size={80} 
                icon={<UserOutlined />}
                src={currentUser?.avatar}
                className="mb-4"
              />
              <Title level={3} className="mb-2">
                {currentUser?.firstName && currentUser?.lastName 
                  ? `${currentUser.firstName} ${currentUser.lastName}`
                  : currentUser?.username || 'Пользователь'
                }
              </Title>
              
              <Space direction="vertical" size="small" className="w-full">
                <div className="flex items-center justify-center space-x-2">
                  <MailOutlined className="text-gray-400" />
                  <Text>{currentUser?.email || 'Не указан'}</Text>
                </div>
                
                <div className="flex items-center justify-center space-x-2">
                  <CalendarOutlined className="text-gray-400" />
                  <Text>
                    Регистрация: {currentUser?.createdAt 
                      ? new Date(currentUser.createdAt).toLocaleDateString('ru-RU')
                      : 'Не указана'
                    }
                  </Text>
                </div>
              </Space>
            </div>
          </Card>
        </Col>

        {/* Статистика задач */}
        <Col xs={24} lg={16}>
          <Card title="Статистика задач">
            <Row gutter={[16, 16]}>
              <Col xs={12} sm={6}>
                <Statistic
                  title="Всего задач"
                  value={totalTasks}
                  valueStyle={{ color: '#1890ff' }}
                />
              </Col>
              
              <Col xs={12} sm={6}>
                <Statistic
                  title="Выполнено"
                  value={completedTasks}
                  valueStyle={{ color: '#52c41a' }}
                />
              </Col>
              
              <Col xs={12} sm={6}>
                <Statistic
                  title="В работе"
                  value={inProgressTasks}
                  valueStyle={{ color: '#faad14' }}
                />
              </Col>
              
              <Col xs={12} sm={6}>
                <Statistic
                  title="Прогресс"
                  value={totalTasks > 0 ? Math.round((completedTasks / totalTasks) * 100) : 0}
                  suffix="%"
                  valueStyle={{ color: '#722ed1' }}
                />
              </Col>
            </Row>
          </Card>
        </Col>
      </Row>

      {/* Последние задачи пользователя */}
      <Card title="Мои задачи">
        {userTasks.length > 0 ? (
          <div className="space-y-3">
            {userTasks.slice(0, 10).map((task) => (
              <div key={task.id} className="flex items-center justify-between p-3 bg-gray-50 rounded-lg">
                <div className="flex items-center space-x-3">
                  <div className="font-medium">{task.title}</div>
                  <Tag color={
                    task.status === TaskStatus.Done ? 'green' :
                    task.status === TaskStatus.InProgress ? 'blue' :
                    task.status === TaskStatus.Blocked ? 'red' :
                    'default'
                  }>
                    {task.status}
                  </Tag>
                </div>
                <div className="text-sm text-gray-500">
                  {task.project?.name}
                </div>
              </div>
            ))}
          </div>
        ) : (
          <div className="text-center text-gray-500 py-8">
            У вас пока нет назначенных задач
          </div>
        )}
      </Card>
    </div>
  );
};

export default Profile;
