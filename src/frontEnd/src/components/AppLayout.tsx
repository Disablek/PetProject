import { FC, ReactNode } from 'react';
import { Layout, Menu, Button, Avatar, Dropdown, Space, Typography } from 'antd';
import { 
  DashboardOutlined, 
  ProjectOutlined, 
  UserOutlined, 
  MenuFoldOutlined, 
  MenuUnfoldOutlined,
  LogoutOutlined,
  SettingOutlined
} from '@ant-design/icons';
import { useNavigate, useLocation } from 'react-router-dom';
import { useUIStore, useCurrentUser } from '../store';

const { Header, Sider, Content } = Layout;
const { Text } = Typography;

interface AppLayoutProps {
  children: ReactNode;
}

const AppLayout: FC<AppLayoutProps> = ({ children }) => {
  const navigate = useNavigate();
  const location = useLocation();
  const { sidebarCollapsed, setSidebarCollapsed, theme } = useUIStore();
  const currentUser = useCurrentUser();

  // Меню навигации
  const menuItems = [
    {
      key: '/dashboard',
      icon: <DashboardOutlined />,
      label: 'Дашборд',
    },
    {
      key: '/projects',
      icon: <ProjectOutlined />,
      label: 'Проекты',
    },
    {
      key: '/profile',
      icon: <UserOutlined />,
      label: 'Профиль',
    },
  ];

  // Меню пользователя
  const userMenuItems = [
    {
      key: 'profile',
      icon: <UserOutlined />,
      label: 'Профиль',
    },
    {
      key: 'settings',
      icon: <SettingOutlined />,
      label: 'Настройки',
    },
    {
      type: 'divider' as const,
    },
    {
      key: 'logout',
      icon: <LogoutOutlined />,
      label: 'Выйти',
      danger: true,
    },
  ];

  const handleMenuClick = ({ key }: { key: string }) => {
    navigate(key);
  };

  const handleUserMenuClick = ({ key }: { key: string }) => {
    switch (key) {
      case 'profile':
        navigate('/profile');
        break;
      case 'settings':
        // TODO: Реализовать настройки
        break;
      case 'logout':
        // TODO: Реализовать выход
        break;
    }
  };

  return (
    <Layout className="min-h-screen">
      {/* Боковая панель */}
      <Sider
        trigger={null}
        collapsible
        collapsed={sidebarCollapsed}
        className="bg-white shadow-lg"
        width={250}
        collapsedWidth={80}
      >
        <div className="p-4 border-b border-gray-200">
          <div className="flex items-center justify-center">
            {sidebarCollapsed ? (
              <div className="w-8 h-8 bg-blue-600 rounded-lg flex items-center justify-center">
                <Text className="text-white font-bold text-lg">T</Text>
              </div>
            ) : (
              <div className="flex items-center space-x-2">
                <div className="w-8 h-8 bg-blue-600 rounded-lg flex items-center justify-center">
                  <Text className="text-white font-bold text-lg">T</Text>
                </div>
                <Text className="text-xl font-bold text-gray-800">TaskFlow</Text>
              </div>
            )}
          </div>
        </div>

        <Menu
          mode="inline"
          selectedKeys={[location.pathname]}
          items={menuItems}
          onClick={handleMenuClick}
          className="border-none"
        />
      </Sider>

      <Layout>
        {/* Шапка */}
        <Header className="bg-white shadow-sm border-b border-gray-200 px-4 flex items-center justify-between">
          <div className="flex items-center">
            <Button
              type="text"
              icon={sidebarCollapsed ? <MenuUnfoldOutlined /> : <MenuFoldOutlined />}
              onClick={() => setSidebarCollapsed(!sidebarCollapsed)}
              className="text-gray-600 hover:text-gray-900"
            />
          </div>

          <div className="flex items-center space-x-4">
            {/* Уведомления */}
            <Button type="text" icon={<UserOutlined />} className="text-gray-600" />
            
            {/* Профиль пользователя */}
            <Dropdown
              menu={{
                items: userMenuItems,
                onClick: handleUserMenuClick,
              }}
              placement="bottomRight"
              arrow
            >
              <Space className="cursor-pointer hover:bg-gray-50 px-2 py-1 rounded-lg">
                <Avatar 
                  size="small" 
                  icon={<UserOutlined />}
                  src={currentUser?.avatar}
                />
                {!sidebarCollapsed && (
                  <Text className="text-gray-700">
                    {currentUser?.firstName || currentUser?.username || 'Пользователь'}
                  </Text>
                )}
              </Space>
            </Dropdown>
          </div>
        </Header>

        {/* Основной контент */}
        <Content className="p-6 bg-gray-50 min-h-screen">
          <div className="max-w-7xl mx-auto">
            {children}
          </div>
        </Content>
      </Layout>
    </Layout>
  );
};

export default AppLayout;
