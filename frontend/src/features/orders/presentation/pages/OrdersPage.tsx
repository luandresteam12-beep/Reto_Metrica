import { Link } from 'react-router-dom'
import { getApiErrorMessage } from '../../../../shared/lib/errors'
import { useAuth } from '../../../auth/presentation/useAuth'
import { useOrders } from '../../application/useOrders'
import { OrderFilters } from '../components/OrderFilters'
import { OrdersSummary } from '../components/OrdersSummary'
import { OrdersTable } from '../components/OrdersTable'

export function OrdersPage() {
  const { user } = useAuth()
  const { search, setSearch, status, setStatus, feedback, orders, ordersQuery, deleteMutation } = useOrders()

  const handleDelete = (id: number) => {
    if (window.confirm('¿Seguro que deseas eliminar este pedido? La eliminación será lógica.')) {
      deleteMutation.mutate(id)
    }
  }

  return (
    <section className="content-section">
      <div className="page-heading">
        <div><p className="eyebrow">OPERACIONES</p><h1>Gestión de pedidos</h1><p className="muted">Administra el ciclo de vida de tus pedidos desde un solo lugar.</p></div>
        <Link className="button primary" to="/pedidos/nuevo">+ Nuevo pedido</Link>
      </div>

      <OrdersSummary count={orders.length} role={user?.role} isLoading={ordersQuery.isLoading} />

      <div className="panel">
        <OrderFilters search={search} status={status} onSearchChange={setSearch} onStatusChange={setStatus} />
        {feedback && <div className="alert info" role="status">{feedback}</div>}
        {ordersQuery.isLoading && <div className="empty-state">Cargando pedidos…</div>}
        {ordersQuery.isError && <div className="alert error" role="alert">{getApiErrorMessage(ordersQuery.error, 'No se pudieron cargar los pedidos.')}</div>}
        {!ordersQuery.isLoading && !ordersQuery.isError && orders.length === 0 && <div className="empty-state">No hay pedidos que coincidan con los filtros.</div>}
        {orders.length > 0 && <OrdersTable orders={orders} canDelete={user?.role === 'Admin'} isDeleting={deleteMutation.isPending} onDelete={handleDelete} />}
      </div>
    </section>
  )
}
