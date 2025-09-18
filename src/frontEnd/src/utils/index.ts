import dayjs from 'dayjs';
import { TaskStatus, TaskPriority } from '../types';

// Утилиты для работы с датами
export const formatDate = (date: string | Date): string => {
  return dayjs(date).format('DD.MM.YYYY');
};

export const formatDateTime = (date: string | Date): string => {
  return dayjs(date).format('DD.MM.YYYY HH:mm');
};

export const isOverdue = (dueDate: string | Date): boolean => {
  return dayjs(dueDate).isBefore(dayjs(), 'day');
};

export const getDaysUntilDue = (dueDate: string | Date): number => {
  return dayjs(dueDate).diff(dayjs(), 'day');
};

// Утилиты для работы с задачами
export const getTaskStatusColor = (status: TaskStatus): string => {
  const colors = {
    [TaskStatus.New]: '#6b7280',
    [TaskStatus.InProgress]: '#3b82f6',
    [TaskStatus.Review]: '#f59e0b',
    [TaskStatus.Done]: '#10b981',
    [TaskStatus.Blocked]: '#ef4444',
  };
  return colors[status];
};

export const getTaskPriorityColor = (priority: TaskPriority): string => {
  const colors = {
    [TaskPriority.Low]: '#6b7280',
    [TaskPriority.Medium]: '#3b82f6',
    [TaskPriority.High]: '#f59e0b',
    [TaskPriority.Critical]: '#ef4444',
  };
  return colors[priority];
};

export const getTaskStatusLabel = (status: TaskStatus): string => {
  const labels = {
    [TaskStatus.New]: 'Новая',
    [TaskStatus.InProgress]: 'В работе',
    [TaskStatus.Review]: 'На проверке',
    [TaskStatus.Done]: 'Выполнено',
    [TaskStatus.Blocked]: 'Заблокировано',
  };
  return labels[status];
};

export const getTaskPriorityLabel = (priority: TaskPriority): string => {
  const labels = {
    [TaskPriority.Low]: 'Низкий',
    [TaskPriority.Medium]: 'Средний',
    [TaskPriority.High]: 'Высокий',
    [TaskPriority.Critical]: 'Критический',
  };
  return labels[priority];
};

// Утилиты для работы с текстом
export const truncateText = (text: string, maxLength: number): string => {
  if (text.length <= maxLength) return text;
  return text.substring(0, maxLength) + '...';
};

export const capitalizeFirst = (text: string): string => {
  return text.charAt(0).toUpperCase() + text.slice(1).toLowerCase();
};

// Утилиты для работы с массивом
export const groupBy = <T, K extends keyof T>(array: T[], key: K): Record<string, T[]> => {
  return array.reduce((groups, item) => {
    const group = String(item[key]);
    groups[group] = groups[group] || [];
    groups[group].push(item);
    return groups;
  }, {} as Record<string, T[]>);
};

// Утилиты для работы с URL
export const buildApiUrl = (endpoint: string): string => {
  const baseUrl = 'https://localhost:7000/api';
  return `${baseUrl}${endpoint}`;
};

// Утилиты для работы с localStorage
export const getFromStorage = <T>(key: string, defaultValue: T): T => {
  try {
    const item = localStorage.getItem(key);
    return item ? JSON.parse(item) : defaultValue;
  } catch {
    return defaultValue;
  }
};

export const setToStorage = <T>(key: string, value: T): void => {
  try {
    localStorage.setItem(key, JSON.stringify(value));
  } catch {
    console.error('Ошибка при сохранении в localStorage');
  }
};

// Утилиты для работы с ошибками
export const getErrorMessage = (error: unknown): string => {
  if (error instanceof Error) {
    return error.message;
  }
  if (typeof error === 'string') {
    return error;
  }
  return 'Произошла неизвестная ошибка';
};
