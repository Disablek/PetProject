// Базовые типы для системы управления задачами TaskFlow

export enum TaskStatus {
  New = 'New',
  InProgress = 'InProgress',
  Review = 'Review',
  Done = 'Done',
  Blocked = 'Blocked'
}

export enum TaskPriority {
  Low = 'Low',
  Medium = 'Medium',
  High = 'High',
  Critical = 'Critical'
}

export interface User {
  id: string;
  username: string;
  email: string;
  firstName?: string;
  lastName?: string;
  avatar?: string;
  createdAt: string;
  updatedAt: string;
}

export interface Project {
  id: string;
  name: string;
  description?: string;
  color?: string;
  createdAt: string;
  updatedAt: string;
  ownerId: string;
  owner?: User;
}

export interface Task {
  id: string;
  title: string;
  description?: string;
  status: TaskStatus;
  priority: TaskPriority;
  projectId: string;
  project?: Project;
  assigneeIds: string[];
  assignees?: User[];
  createdById: string;
  createdBy?: User;
  dueDate?: string;
  createdAt: string;
  updatedAt: string;
}

// Типы для создания/обновления сущностей
export interface CreateTaskRequest {
  title: string;
  description?: string;
  status?: TaskStatus;
  priority?: TaskPriority;
  projectId: string;
  assigneeIds?: string[];
  dueDate?: string;
}

export interface UpdateTaskRequest {
  title?: string;
  description?: string;
  status?: TaskStatus;
  priority?: TaskPriority;
  assigneeIds?: string[];
  dueDate?: string;
}

export interface CreateProjectRequest {
  name: string;
  description?: string;
  color?: string;
}

export interface UpdateProjectRequest {
  name?: string;
  description?: string;
  color?: string;
}

// Типы для API ответов
export interface ApiResponse<T> {
  data: T;
  success: boolean;
  message?: string;
}

export interface PaginatedResponse<T> {
  data: T[];
  totalCount: number;
  pageNumber: number;
  pageSize: number;
  totalPages: number;
}

// Типы для фильтрации и поиска
export interface TaskFilters {
  status?: TaskStatus[];
  priority?: TaskPriority[];
  projectId?: string;
  assigneeId?: string;
  search?: string;
}

export interface ProjectFilters {
  search?: string;
  ownerId?: string;
}
