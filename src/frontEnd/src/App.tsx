import { Routes, Route, Navigate } from 'react-router-dom';
import { Layout } from 'antd';
import { useUIStore } from './store';

// Компоненты
import AppLayout from './components/AppLayout';
import Dashboard from './pages/Dashboard';
import Projects from './pages/Projects';
import ProjectTasks from './pages/ProjectTasks';
import Profile from './pages/Profile';
import NotFound from './pages/NotFound';

const { Content } = Layout;

function App() {
  const { theme } = useUIStore();

  return (
    <div className={`min-h-screen ${theme === 'dark' ? 'dark' : ''}`}>
      <AppLayout>
        <Routes>
          {/* Главная страница - перенаправляем на дашборд */}
          <Route path="/" element={<Navigate to="/dashboard" replace />} />
          
          {/* Дашборд */}
          <Route path="/dashboard" element={<Dashboard />} />
          
          {/* Проекты */}
          <Route path="/projects" element={<Projects />} />
          
          {/* Задачи проекта */}
          <Route path="/projects/:projectId/tasks" element={<ProjectTasks />} />
          
          {/* Профиль пользователя */}
          <Route path="/profile" element={<Profile />} />
          
          {/* 404 страница */}
          <Route path="*" element={<NotFound />} />
        </Routes>
      </AppLayout>
    </div>
  );
}

export default App;
