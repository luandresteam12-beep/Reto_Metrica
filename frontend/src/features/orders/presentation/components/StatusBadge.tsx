import type { OrderStatus } from '../../domain/order'

const statusClass: Record<OrderStatus, string> = {
  Registrado: 'status registered',
  Confirmado: 'status confirmed',
  Enviado: 'status shipped',
  Entregado: 'status delivered',
  Cancelado: 'status cancelled',
}

export function StatusBadge({ status }: { status: OrderStatus }) {
  return <span className={statusClass[status]}>{status}</span>
}
