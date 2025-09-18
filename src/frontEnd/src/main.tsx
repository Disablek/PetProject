import { StrictMode } from 'react'
import ReactDOM from 'react-dom/client'
import { BrowserRouter } from 'react-router-dom'
import { ConfigProvider } from 'antd'
import ruRU from 'antd/locale/ru_RU'
import dayjs from 'dayjs'
import 'dayjs/locale/ru'

import App from './App.tsx'
import { QueryProvider } from './services/queryClient.tsx'
import './index.css'

// Настройка локализации для dayjs
dayjs.locale('ru')

// Настройка темы Ant Design
const theme = {
  token: {
    colorPrimary: '#0ea5e9',
    borderRadius: 8,
    fontFamily: 'Inter, system-ui, sans-serif',
  },
}

ReactDOM.createRoot(document.getElementById('root')!).render(
  <StrictMode>
    <BrowserRouter>
      <QueryProvider>
        <ConfigProvider 
          locale={ruRU} 
          theme={theme}
        >
          <App />
        </ConfigProvider>
      </QueryProvider>
    </BrowserRouter>
  </StrictMode>,
)
