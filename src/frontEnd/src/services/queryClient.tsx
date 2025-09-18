import { QueryClient, QueryClientProvider } from '@tanstack/react-query';
import { ReactQueryDevtools } from '@tanstack/react-query-devtools';
import { ReactNode } from 'react';

// Создаем клиент React Query с базовыми настройками
const queryClient = new QueryClient({
  defaultOptions: {
    queries: {
      // Время кэширования данных (5 минут)
      staleTime: 5 * 60 * 1000,
      // Время жизни кэша (10 минут)
      gcTime: 10 * 60 * 1000,
      // Повторные попытки при ошибке
      retry: 3,
      // Рефетч при фокусе окна
      refetchOnWindowFocus: false,
    },
    mutations: {
      // Повторные попытки для мутаций
      retry: 1,
    },
  },
});

interface QueryProviderProps {
  children: ReactNode;
}

// Провайдер для React Query
export const QueryProvider = ({ children }: QueryProviderProps) => {
  return (
    <QueryClientProvider client={queryClient}>
      {children}
      {/* DevTools для разработки */}
      {process.env.NODE_ENV === 'development' && (
        <ReactQueryDevtools initialIsOpen={false} />
      )}
    </QueryClientProvider>
  );
};

export { queryClient };
