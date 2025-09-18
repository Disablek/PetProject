import axios, { AxiosInstance, AxiosResponse } from 'axios';
import {
  Task,
  Project,
  User,
  CreateTaskRequest,
  UpdateTaskRequest,
  CreateProjectRequest,
  UpdateProjectRequest,
  TaskFilters,
  ProjectFilters
} from '../types';

// Базовый API клиент для работы с .NET 8 API
class ApiClient {
  private client: AxiosInstance;

  constructor() {
    this.client = axios.create({
      baseURL: '/api', // Используем прокси из Vite
      timeout: 10000,
      headers: {
        'Content-Type': 'application/json',
      },
    });

    // Интерцептор для обработки ошибок
    this.client.interceptors.response.use(
      (response) => {
        console.log('API Success:', response.config.method?.toUpperCase(), response.config.url, response.data);
        
        // Преобразуем числовые enum'ы в строки для фронтенда
        if (response.data) {
          this.transformEnumsToStrings(response.data);
        }
        
        return response;
      },
      (error) => {
        console.error('API Error:', {
          method: error.config?.method?.toUpperCase(),
          url: error.config?.url,
          status: error.response?.status,
          data: error.response?.data,
          message: error.message
        });
        return Promise.reject(error);
      }
    );

    // Интерцептор для логирования запросов
    this.client.interceptors.request.use(
      (config) => {
        console.log('API Request:', config.method?.toUpperCase(), config.url, config.data);
        return config;
      },
      (error) => {
        console.error('Request Error:', error);
        return Promise.reject(error);
      }
    );
  }

  // Методы для работы с задачами
  async getTasks(filters?: TaskFilters): Promise<Task[]> {
    const response: AxiosResponse<Task[]> = await this.client.get('/tasks', {
      params: filters,
    });
    return response.data;
  }

  async getTaskById(id: string): Promise<Task> {
    const response: AxiosResponse<Task> = await this.client.get(`/tasks/${id}`);
    return response.data;
  }

  async createTask(task: CreateTaskRequest): Promise<Task> {
    // Преобразуем данные в формат, ожидаемый .NET API
    const requestData = {
      title: task.title,
      description: task.description,
      priority: this.mapPriorityToNumber(task.priority || 'Low'),
      dueTime: task.dueDate ? new Date(task.dueDate).toISOString() : null,
      projectId: task.projectId,
      assigneeId: task.assigneeIds && task.assigneeIds.length > 0 ? task.assigneeIds[0] : null,
      creatorId: '00000000-0000-0000-0000-000000000000', // Пустой GUID - будет заменен на мок-пользователя
    };
    
    const response: AxiosResponse<Task> = await this.client.post('/tasks', requestData);
    return response.data;
  }

  async updateTask(id: string, task: UpdateTaskRequest): Promise<Task> {
    // Преобразуем данные в формат, ожидаемый .NET API
    const requestData = {
      title: task.title,
      description: task.description,
      priority: task.priority ? this.mapPriorityToNumber(task.priority) : undefined,
      status: task.status ? this.mapStatusToNumber(task.status) : undefined,
      dueTime: task.dueDate ? new Date(task.dueDate).toISOString() : null,
      assigneeId: task.assigneeIds && task.assigneeIds.length > 0 ? task.assigneeIds[0] : null,
    };
    
    const response: AxiosResponse<Task> = await this.client.put(`/tasks/${id}`, requestData);
    return response.data;
  }

  async deleteTask(id: string): Promise<void> {
    await this.client.delete(`/tasks/${id}`);
  }

  // Методы для работы с проектами
  async getProjects(filters?: ProjectFilters): Promise<Project[]> {
    const response: AxiosResponse<Project[]> = await this.client.get('/projects', {
      params: filters,
    });
    return response.data;
  }

  async getProjectById(id: string): Promise<Project> {
    const response: AxiosResponse<Project> = await this.client.get(`/projects/${id}`);
    return response.data;
  }

  async createProject(project: CreateProjectRequest): Promise<Project> {
    // Преобразуем данные в формат, ожидаемый .NET API
    const requestData = {
      name: project.name,
      description: project.description,
      // color не поддерживается в CreateProjectDto, пропускаем
    };
    
    const response: AxiosResponse<Project> = await this.client.post('/projects', requestData);
    return response.data;
  }

  async updateProject(id: string, project: UpdateProjectRequest): Promise<Project> {
    // Преобразуем данные в формат, ожидаемый .NET API
    const requestData = {
      name: project.name,
      description: project.description,
      // color не поддерживается в UpdateProjectDto, пропускаем
      userIds: [], // Пустой массив по умолчанию
      adminId: '', // Пустой GUID по умолчанию
    };
    
    const response: AxiosResponse<Project> = await this.client.put(`/projects/${id}`, requestData);
    return response.data;
  }

  async deleteProject(id: string): Promise<void> {
    await this.client.delete(`/projects/${id}`);
  }

  // Методы для работы с пользователями
  async getUsers(): Promise<User[]> {
    const response: AxiosResponse<User[]> = await this.client.get('/users');
    return response.data;
  }

  async getUserById(id: string): Promise<User> {
    const response: AxiosResponse<User> = await this.client.get(`/users/${id}`);
    return response.data;
  }

  // Методы для работы с задачами проекта
  async getProjectTasks(projectId: string): Promise<Task[]> {
    const response: AxiosResponse<Task[]> = await this.client.get(`/projects/${projectId}/tasks`);
    return response.data;
  }

  // Методы для работы с пользователями задач
  async getTaskAssignees(taskId: string): Promise<User[]> {
    const response: AxiosResponse<User[]> = await this.client.get(`/tasks/${taskId}/assignees`);
    return response.data;
  }

  async assignTaskToUser(taskId: string, userId: string): Promise<void> {
    await this.client.post(`/tasks/${taskId}/assignees/${userId}`);
  }

  async unassignTaskFromUser(taskId: string, userId: string): Promise<void> {
    await this.client.delete(`/tasks/${taskId}/assignees/${userId}`);
  }

  // Методы для изменения статуса и других свойств задачи
  async changeTaskStatus(taskId: string, status: string, performedBy: string): Promise<Task> {
    const response: AxiosResponse<Task> = await this.client.patch(
      `/tasks/${taskId}/status?status=${this.mapStatusToNumber(status)}&performedBy=${performedBy}`
    );
    return response.data;
  }

  async changeTaskPriority(taskId: string, priority: string, currentUserId: string): Promise<Task> {
    const response: AxiosResponse<Task> = await this.client.patch(
      `/tasks/${taskId}/priority?priority=${this.mapPriorityToNumber(priority)}&currentUserId=${currentUserId}`
    );
    return response.data;
  }

  async updateTaskDueTime(taskId: string, dueTime: string | null): Promise<Task> {
    const url = dueTime 
      ? `/tasks/${taskId}/due-time?dueTime=${encodeURIComponent(dueTime)}`
      : `/tasks/${taskId}/due-time`;
    
    const response: AxiosResponse<Task> = await this.client.patch(url);
    return response.data;
  }

  // Вспомогательные методы для преобразования enum'ов
  private mapPriorityToNumber(priority: string): number {
    const priorityMap: { [key: string]: number } = {
      'Low': 0,
      'Medium': 1,
      'High': 2,
      'Critical': 3
    };
    return priorityMap[priority] || 0;
  }

  private mapPriorityToString(priority: number): string {
    const priorityMap: { [key: number]: string } = {
      0: 'Low',
      1: 'Medium',
      2: 'High',
      3: 'Critical'
    };
    return priorityMap[priority] || 'Low';
  }

  private mapStatusToNumber(status: string): number {
    const statusMap: { [key: string]: number } = {
      'New': 0,
      'InProgress': 1,
      'Review': 2,
      'Done': 3,
      'Blocked': 4
    };
    return statusMap[status] || 0;
  }

  private mapStatusToString(status: number): string {
    const statusMap: { [key: number]: string } = {
      0: 'New',
      1: 'InProgress',
      2: 'Review',
      3: 'Done',
      4: 'Blocked'
    };
    return statusMap[status] || 'New';
  }

  // Преобразует числовые enum'ы в строки для фронтенда
  private transformEnumsToStrings(data: any): void {
    if (Array.isArray(data)) {
      data.forEach(item => this.transformEnumsToStrings(item));
    } else if (data && typeof data === 'object') {
      // Преобразуем priority
      if (typeof data.priority === 'number') {
        data.priority = this.mapPriorityToString(data.priority);
      }
      
      // Преобразуем status
      if (typeof data.status === 'number') {
        data.status = this.mapStatusToString(data.status);
      }
      
      // Рекурсивно обрабатываем вложенные объекты
      Object.values(data).forEach(value => {
        if (value && typeof value === 'object') {
          this.transformEnumsToStrings(value);
        }
      });
    }
  }
}

// Экспортируем единственный экземпляр клиента
export const apiClient = new ApiClient();
export default apiClient;
