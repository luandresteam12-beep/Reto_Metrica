import { Link } from 'react-router-dom'
import type { Order } from '../../domain/order'
import { StatusBadge } from './StatusBadge'

interface OrdersTableProps {
  orders: Order[]
  canDelete: boolean
  isDeleting: boolean
  onDelete: (id: number) => void
}

export function OrdersTable({ orders, canDelete, isDeleting, onDelete }: OrdersTableProps) {
  return (
    <div className="table-wrap">
      <table>
        <thead><tr><th>Pedido</th><th>Cliente</th><th>Fecha</th><th>Total</th><th>Estado</th><th aria-label="Acciones" /></tr></thead>
        <tbody>
          {orders.map((order) => (
            <tr key={order.id}>
              <td><strong>{order.numeroPedido}</strong></td>
              <td>{order.cliente}</td>
              <td>{new Date(order.fecha).toLocaleDateString('es-PE')}</td>
              <td className="amount">S/ {order.total.toLocaleString('es-PE', { minimumFractionDigits: 2 })}</td>
              <td><StatusBadge status={order.estado} /></td>
              <td className="actions">
                <Link className="text-button" to={`/pedidos/${order.id}/editar`}>Editar</Link>
                {canDelete && <button className="text-button danger-text" onClick={() => onDelete(order.id)} disabled={isDeleting}>Eliminar</button>}
              </td>
            </tr>
          ))}
        </tbody>
      </table>
    </div>
  )
}
