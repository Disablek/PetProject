import { create } from 'zustand';
import { devtools, persist } from 'zustand/middleware';
import { User, TaskStatus, TaskPriority } from '../types';

// Интерфейс для состояния пользователя
interface UserState {
  currentUser: User | null;
  isAuthenticated: boolean;
  setCurrentUser: (user: User | null) => void;
  logout: () => void;
}

// Интерфейс для состояния UI
interface UIState {
  sidebarCollapsed: boolean;
  theme: 'light' | 'dark';
  loading: boolean;
  setSidebarCollapsed: (collapsed: boolean) => void;
  setTheme: (theme: 'light' | 'dark') => void;
  setLoading: (loading: boolean) => void;
}

// Интерфейс для состояния фильтров задач
interface TaskFilterState {
  statusFilter: TaskStatus[];
  priorityFilter: TaskPriority[];
  projectFilter: string | null;
  assigneeFilter: string | null;
  searchQuery: string;
  setStatusFilter: (status: TaskStatus[]) => void;
  setPriorityFilter: (priority: TaskPriority[]) => void;
  setProjectFilter: (projectId: string | null) => void;
  setAssigneeFilter: (assigneeId: string | null) => void;
  setSearchQuery: (query: string) => void;
  clearFilters: () => void;
}

// Store для пользователя
export const useUserStore = create<UserState>()(
  devtools(
    persist(
      (set) => ({
        currentUser: null,
        isAuthenticated: false,
        setCurrentUser: (user) =>
          set(
            { currentUser: user, isAuthenticated: !!user },
            false,
            'setCurrentUser'
          ),
        logout: () =>
          set(
            { currentUser: null, isAuthenticated: false },
            false,
            'logout'
          ),
      }),
      {
        name: 'user-storage',
        partialize: (state) => ({
          currentUser: state.currentUser,
          isAuthenticated: state.isAuthenticated,
        }),
      }
    ),
    {
      name: 'user-store',
    }
  )
);

// Store для UI состояния
export const useUIStore = create<UIState>()(
  devtools(
    persist(
      (set) => ({
        sidebarCollapsed: false,
        theme: 'light',
        loading: false,
        setSidebarCollapsed: (collapsed) =>
          set({ sidebarCollapsed: collapsed }, false, 'setSidebarCollapsed'),
        setTheme: (theme) => set({ theme }, false, 'setTheme'),
        setLoading: (loading) => set({ loading }, false, 'setLoading'),
      }),
      {
        name: 'ui-storage',
        partialize: (state) => ({
          sidebarCollapsed: state.sidebarCollapsed,
          theme: state.theme,
        }),
      }
    ),
    {
      name: 'ui-store',
    }
  )
);

// Store для фильтров задач
export const useTaskFilterStore = create<TaskFilterState>()(
  devtools(
    (set) => ({
      statusFilter: [],
      priorityFilter: [],
      projectFilter: null,
      assigneeFilter: null,
      searchQuery: '',
      setStatusFilter: (status) =>
        set({ statusFilter: status }, false, 'setStatusFilter'),
      setPriorityFilter: (priority) =>
        set({ priorityFilter: priority }, false, 'setPriorityFilter'),
      setProjectFilter: (projectId) =>
        set({ projectFilter: projectId }, false, 'setProjectFilter'),
      setAssigneeFilter: (assigneeId) =>
        set({ assigneeFilter: assigneeId }, false, 'setAssigneeFilter'),
      setSearchQuery: (query) =>
        set({ searchQuery: query }, false, 'setSearchQuery'),
      clearFilters: () =>
        set(
          {
            statusFilter: [],
            priorityFilter: [],
            projectFilter: null,
            assigneeFilter: null,
            searchQuery: '',
          },
          false,
          'clearFilters'
        ),
    }),
    {
      name: 'task-filter-store',
    }
  )
);

// Селекторы для удобного использования
export const useCurrentUser = () => useUserStore((state) => state.currentUser);
export const useIsAuthenticated = () => useUserStore((state) => state.isAuthenticated);

export const useSidebarCollapsed = () => useUIStore((state) => state.sidebarCollapsed);
export const useTheme = () => useUIStore((state) => state.theme);
export const useLoading = () => useUIStore((state) => state.loading);

export const useTaskFilters = () => useTaskFilterStore((state) => ({
  statusFilter: state.statusFilter,
  priorityFilter: state.priorityFilter,
  projectFilter: state.projectFilter,
  assigneeFilter: state.assigneeFilter,
  searchQuery: state.searchQuery,
}));
