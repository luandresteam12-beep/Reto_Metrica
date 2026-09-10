import { Navigate, Route, Routes } from 'react-router-dom'
import { AppLayout } from './AppLayout'
import { LoginPage } from '../features/auth/presentation/pages/LoginPage'
import { ProtectedRoute } from '../features/auth/presentation/ProtectedRoute'
import { OrdersPage } from '../features/orders/presentation/pages/OrdersPage'
import { OrderFormPage } from '../features/orders/presentation/pages/OrderFormPage'

export function App() {
  return (
    <Routes>
      <Route path="/login" element={<LoginPage />} />
      <Route element={<ProtectedRoute />}>
        <Route element={<AppLayout />}>
          <Route index element={<Navigate to="/pedidos" replace />} />
          <Route path="pedidos" element={<OrdersPage />} />
          <Route path="pedidos/nuevo" element={<OrderFormPage />} />
          <Route path="pedidos/:id/editar" element={<OrderFormPage />} />
        </Route>
      </Route>
      <Route path="*" element={<Navigate to="/pedidos" replace />} />
    </Routes>
  )
}
