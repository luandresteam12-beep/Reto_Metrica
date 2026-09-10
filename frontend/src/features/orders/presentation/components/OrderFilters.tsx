import { orderStatuses, type OrderStatus } from '../../domain/order'

interface OrderFiltersProps {
  search: string
  status: OrderStatus | ''
  onSearchChange: (value: string) => void
  onStatusChange: (value: OrderStatus | '') => void
}

export function OrderFilters({ search, status, onSearchChange, onStatusChange }: OrderFiltersProps) {
  return (
    <div className="toolbar">
      <div className="search-field">
        <span aria-hidden="true">⌕</span>
        <input value={search} onChange={(event) => onSearchChange(event.target.value)} placeholder="Buscar por número o cliente…" aria-label="Buscar pedidos" />
      </div>
      <select value={status} onChange={(event) => onStatusChange(event.target.value as OrderStatus | '')} aria-label="Filtrar por estado">
        <option value="">Todos los estados</option>
        {orderStatuses.map((item) => <option key={item} value={item}>{item}</option>)}
      </select>
    </div>
  )
}
