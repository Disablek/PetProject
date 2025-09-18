import { FC, useState } from 'react';
import { Button, Card, Form, Input, Select, message, Alert, Descriptions } from 'antd';
import { apiClient } from '../services/api';
import { CreateTaskRequest, TaskStatus, TaskPriority } from '../types';

const TestApi: FC = () => {
  const [loading, setLoading] = useState(false);
  const [lastError, setLastError] = useState<any>(null);
  const [lastResponse, setLastResponse] = useState<any>(null);
  const [form] = Form.useForm();

  const handleTestCreateTask = async () => {
    setLoading(true);
    setLastError(null);
    setLastResponse(null);
    
    try {
      const testTask: CreateTaskRequest = {
        title: 'Тестовая задача',
        description: 'Тест описание',
        status: TaskStatus.New,
        priority: TaskPriority.Low,
        projectId: '3132b7f1-cb69-4575-878b-fa6b1dd89c84',
        assigneeIds: ['31dd9d8d-e103-4368-8aca-deb2e3619ff9'],
        dueDate: '2025-09-25'
      };

      console.log('Отправляем данные:', testTask);
      const result = await apiClient.createTask(testTask);
      console.log('Результат:', result);
      setLastResponse(result);
      message.success('Задача создана успешно!');
    } catch (error: any) {
      console.error('Ошибка при создании задачи:', error);
      setLastError({
        message: error.message,
        status: error.response?.status,
        statusText: error.response?.statusText,
        data: error.response?.data,
        config: {
          method: error.config?.method,
          url: error.config?.url,
          data: error.config?.data
        }
      });
      message.error(`Ошибка: ${error.response?.status} ${error.response?.statusText}`);
    } finally {
      setLoading(false);
    }
  };

  const handleTestGetTasks = async () => {
    setLoading(true);
    setLastError(null);
    setLastResponse(null);
    
    try {
      const tasks = await apiClient.getTasks();
      console.log('Полученные задачи:', tasks);
      setLastResponse(tasks);
      message.success(`Получено ${tasks.length} задач`);
    } catch (error: any) {
      console.error('Ошибка при получении задач:', error);
      setLastError({
        message: error.message,
        status: error.response?.status,
        statusText: error.response?.statusText,
        data: error.response?.data,
        config: {
          method: error.config?.method,
          url: error.config?.url
        }
      });
      message.error(`Ошибка: ${error.response?.status} ${error.response?.statusText}`);
    } finally {
      setLoading(false);
    }
  };

  const handleTestConnection = async () => {
    setLoading(true);
    setLastError(null);
    setLastResponse(null);
    
    try {
      // Простой GET запрос для проверки подключения
      const response = await fetch('/api/tasks', {
        method: 'GET',
        headers: {
          'Content-Type': 'application/json',
        },
      });
      
      if (!response.ok) {
        throw new Error(`HTTP ${response.status}: ${response.statusText}`);
      }
      
      const data = await response.json();
      console.log('Тест подключения успешен:', data);
      setLastResponse(data);
      message.success('Подключение к API работает!');
    } catch (error: any) {
      console.error('Ошибка подключения:', error);
      setLastError({
        message: error.message,
        status: 'Connection Error',
        statusText: 'Failed to connect',
        data: null
      });
      message.error(`Ошибка подключения: ${error.message}`);
    } finally {
      setLoading(false);
    }
  };

  const handleTestGetProjects = async () => {
    setLoading(true);
    setLastError(null);
    setLastResponse(null);
    
    try {
      const projects = await apiClient.getProjects();
      console.log('Полученные проекты:', projects);
      setLastResponse(projects);
      message.success(`Получено ${projects.length} проектов`);
    } catch (error: any) {
      console.error('Ошибка при получении проектов:', error);
      setLastError({
        message: error.message,
        status: error.response?.status,
        statusText: error.response?.statusText,
        data: error.response?.data,
        config: {
          method: error.config?.method,
          url: error.config?.url
        }
      });
      message.error(`Ошибка: ${error.response?.status} ${error.response?.statusText}`);
    } finally {
      setLoading(false);
    }
  };

  return (
    <Card title="Тестирование API" className="mb-4">
      <div className="space-y-4">
        <div className="flex gap-2 flex-wrap">
          <Button 
            onClick={handleTestConnection}
            loading={loading}
            type="default"
          >
            Тест подключения
          </Button>
          
          <Button 
            type="primary" 
            onClick={handleTestCreateTask}
            loading={loading}
          >
            Тест создания задачи
          </Button>
          
          <Button 
            onClick={handleTestGetTasks}
            loading={loading}
          >
            Тест получения задач
          </Button>
          
          <Button 
            onClick={handleTestGetProjects}
            loading={loading}
          >
            Тест получения проектов
          </Button>
        </div>
        
        {lastError && (
          <Alert
            message="Ошибка API"
            description={
              <div>
                <p><strong>Статус:</strong> {lastError.status} {lastError.statusText}</p>
                <p><strong>Сообщение:</strong> {lastError.message}</p>
                <p><strong>URL:</strong> {lastError.config?.method?.toUpperCase()} {lastError.config?.url}</p>
                {lastError.data && (
                  <div>
                    <p><strong>Данные ошибки:</strong></p>
                    <pre className="text-xs bg-gray-100 p-2 rounded mt-2 overflow-auto">
                      {JSON.stringify(lastError.data, null, 2)}
                    </pre>
                  </div>
                )}
                {lastError.config?.data && (
                  <div>
                    <p><strong>Отправленные данные:</strong></p>
                    <pre className="text-xs bg-gray-100 p-2 rounded mt-2 overflow-auto">
                      {JSON.stringify(JSON.parse(lastError.config.data), null, 2)}
                    </pre>
                  </div>
                )}
              </div>
            }
            type="error"
            showIcon
          />
        )}
        
        {lastResponse && (
          <Alert
            message="Успешный ответ"
            description={
              <div>
                <p><strong>Получено данных:</strong> {Array.isArray(lastResponse) ? lastResponse.length : 1}</p>
                <pre className="text-xs bg-gray-100 p-2 rounded mt-2 overflow-auto max-h-40">
                  {JSON.stringify(lastResponse, null, 2)}
                </pre>
              </div>
            }
            type="success"
            showIcon
          />
        )}
        
        <div className="text-sm text-gray-600">
          <p>Откройте консоль браузера (F12) для просмотра подробных логов API запросов</p>
        </div>
      </div>
    </Card>
  );
};

export default TestApi;
