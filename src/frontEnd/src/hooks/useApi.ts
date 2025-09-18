import { useQuery, useMutation, useQueryClient } from '@tanstack/react-query';
import { apiClient } from '../services/api';
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

// Хуки для работы с задачами
export const useTasks = (filters?: TaskFilters) => {
  return useQuery({
    queryKey: ['tasks', filters],
    queryFn: () => apiClient.getTasks(filters),
  });
};

export const useTask = (id: string) => {
  return useQuery({
    queryKey: ['task', id],
    queryFn: () => apiClient.getTaskById(id),
    enabled: !!id,
  });
};

export const useCreateTask = () => {
  const queryClient = useQueryClient();
  
  return useMutation({
    mutationFn: (task: CreateTaskRequest) => apiClient.createTask(task),
    onSuccess: () => {
      // Инвалидируем кэш задач после создания
      queryClient.invalidateQueries({ queryKey: ['tasks'] });
    },
  });
};

export const useUpdateTask = () => {
  const queryClient = useQueryClient();
  
  return useMutation({
    mutationFn: ({ id, task }: { id: string; task: UpdateTaskRequest }) =>
      apiClient.updateTask(id, task),
    onSuccess: (data) => {
      // Обновляем кэш конкретной задачи
      queryClient.setQueryData(['task', data.id], data);
      // Инвалидируем все связанные кэши
      queryClient.invalidateQueries({ queryKey: ['tasks'] });
      queryClient.invalidateQueries({ queryKey: ['project-tasks'] });
      queryClient.invalidateQueries({ queryKey: ['project-tasks', data.projectId] });
    },
  });
};

export const useDeleteTask = () => {
  const queryClient = useQueryClient();
  
  return useMutation({
    mutationFn: (id: string) => apiClient.deleteTask(id),
    onSuccess: () => {
      queryClient.invalidateQueries({ queryKey: ['tasks'] });
    },
  });
};

// Хуки для работы с проектами
export const useProjects = (filters?: ProjectFilters) => {
  return useQuery({
    queryKey: ['projects', filters],
    queryFn: () => apiClient.getProjects(filters),
  });
};

export const useProject = (id: string) => {
  return useQuery({
    queryKey: ['project', id],
    queryFn: () => apiClient.getProjectById(id),
    enabled: !!id,
  });
};

export const useCreateProject = () => {
  const queryClient = useQueryClient();
  
  return useMutation({
    mutationFn: (project: CreateProjectRequest) => apiClient.createProject(project),
    onSuccess: () => {
      queryClient.invalidateQueries({ queryKey: ['projects'] });
    },
  });
};

export const useUpdateProject = () => {
  const queryClient = useQueryClient();
  
  return useMutation({
    mutationFn: ({ id, project }: { id: string; project: UpdateProjectRequest }) =>
      apiClient.updateProject(id, project),
    onSuccess: (data) => {
      queryClient.setQueryData(['project', data.id], data);
      queryClient.invalidateQueries({ queryKey: ['projects'] });
    },
  });
};

export const useDeleteProject = () => {
  const queryClient = useQueryClient();
  
  return useMutation({
    mutationFn: (id: string) => apiClient.deleteProject(id),
    onSuccess: () => {
      queryClient.invalidateQueries({ queryKey: ['projects'] });
    },
  });
};

// Хуки для работы с пользователями
export const useUsers = () => {
  return useQuery({
    queryKey: ['users'],
    queryFn: () => apiClient.getUsers(),
  });
};

export const useUser = (id: string) => {
  return useQuery({
    queryKey: ['user', id],
    queryFn: () => apiClient.getUserById(id),
    enabled: !!id,
  });
};

// Хуки для работы с задачами проекта
export const useProjectTasks = (projectId: string) => {
  return useQuery({
    queryKey: ['project-tasks', projectId],
    queryFn: () => apiClient.getProjectTasks(projectId),
    enabled: !!projectId,
  });
};

// Хуки для работы с назначениями задач
export const useTaskAssignees = (taskId: string) => {
  return useQuery({
    queryKey: ['task-assignees', taskId],
    queryFn: () => apiClient.getTaskAssignees(taskId),
    enabled: !!taskId,
  });
};

export const useAssignTask = () => {
  const queryClient = useQueryClient();
  
  return useMutation({
    mutationFn: ({ taskId, userId }: { taskId: string; userId: string }) =>
      apiClient.assignTaskToUser(taskId, userId),
    onSuccess: (_, { taskId }) => {
      queryClient.invalidateQueries({ queryKey: ['task-assignees', taskId] });
      queryClient.invalidateQueries({ queryKey: ['task', taskId] });
    },
  });
};

export const useUnassignTask = () => {
  const queryClient = useQueryClient();
  
  return useMutation({
    mutationFn: ({ taskId, userId }: { taskId: string; userId: string }) =>
      apiClient.unassignTaskFromUser(taskId, userId),
    onSuccess: (_, { taskId }) => {
      queryClient.invalidateQueries({ queryKey: ['task-assignees', taskId] });
      queryClient.invalidateQueries({ queryKey: ['task', taskId] });
    },
  });
};

// Хук для изменения статуса задачи
export const useChangeTaskStatus = () => {
  const queryClient = useQueryClient();
  
  return useMutation({
    mutationFn: ({ taskId, status, performedBy }: { 
      taskId: string; 
      status: string; 
      performedBy: string; 
    }) => apiClient.changeTaskStatus(taskId, status, performedBy),
    onSuccess: (data) => {
      // Обновляем кэш конкретной задачи
      queryClient.setQueryData(['task', data.id], data);
      // Инвалидируем все связанные кэши
      queryClient.invalidateQueries({ queryKey: ['tasks'] });
      queryClient.invalidateQueries({ queryKey: ['project-tasks'] });
      queryClient.invalidateQueries({ queryKey: ['project-tasks', data.projectId] });
    },
  });
};

// Хук для изменения приоритета задачи
export const useChangeTaskPriority = () => {
  const queryClient = useQueryClient();
  
  return useMutation({
    mutationFn: ({ taskId, priority, currentUserId }: { 
      taskId: string; 
      priority: string; 
      currentUserId: string; 
    }) => apiClient.changeTaskPriority(taskId, priority, currentUserId),
    onSuccess: (data) => {
      queryClient.setQueryData(['task', data.id], data);
      queryClient.invalidateQueries({ queryKey: ['tasks'] });
      queryClient.invalidateQueries({ queryKey: ['project-tasks'] });
    },
  });
};

// Хук для назначения исполнителя задачи
export const useAssignTaskToUser = () => {
  const queryClient = useQueryClient();
  
  return useMutation({
    mutationFn: ({ taskId, assigneeId, changedBy }: { 
      taskId: string; 
      assigneeId: string; 
      changedBy: string; 
    }) => apiClient.assignTaskToUser(taskId, assigneeId),
    onSuccess: (data) => {
      queryClient.setQueryData(['task', data.id], data);
      queryClient.invalidateQueries({ queryKey: ['tasks'] });
      queryClient.invalidateQueries({ queryKey: ['project-tasks'] });
    },
  });
};