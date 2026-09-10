import { NavLink, Outlet } from 'react-router-dom'
import { useAuth } from '../features/auth/presentation/useAuth'

export function AppLayout() {
  const { user, logout } = useAuth()

  return (
    <div className="app-shell">
      <header className="topbar">
        <div className="topbar-inner">
          <NavLink className="brand" to="/pedidos">
            <span className="brand-mark small">M</span>
            <span>Metrica <strong>Orders</strong></span>
          </NavLink>
          <nav className="main-nav" aria-label="Navegación principal">
            <NavLink className={({ isActive }) => isActive ? 'nav-link active' : 'nav-link'} to="/pedidos">Pedidos</NavLink>
            <NavLink className={({ isActive }) => isActive ? 'nav-link active' : 'nav-link'} to="/pedidos/nuevo">Nuevo pedido</NavLink>
          </nav>
          <div className="account-menu">
            <div className="avatar" aria-hidden="true">{user?.email.slice(0, 1).toUpperCase()}</div>
            <div className="account-copy">
              <strong>{user?.email}</strong>
              <span>{user?.role}</span>
            </div>
            <button className="button ghost" onClick={logout} type="button">Salir</button>
          </div>
        </div>
      </header>
      <main className="page-content"><Outlet /></main>
    </div>
  )
}
