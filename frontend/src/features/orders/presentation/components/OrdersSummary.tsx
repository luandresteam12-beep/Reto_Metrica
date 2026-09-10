interface OrdersSummaryProps {
  count: number
  role?: string
  isLoading: boolean
}

export function OrdersSummary({ count, role, isLoading }: OrdersSummaryProps) {
  const value = isLoading ? '—' : count

  return (
    <div className="metrics-grid">
      <article className="metric-card"><span>Total visibles</span><strong>{value}</strong><small>Pedidos activos</small></article>
      <article className="metric-card accent"><span>Resultados</span><strong>{value}</strong><small>Coincidencias actuales</small></article>
      <article className="metric-card"><span>Perfil</span><strong>{role ?? '—'}</strong><small>Permisos actuales</small></article>
    </div>
  )
}
