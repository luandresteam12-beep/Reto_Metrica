export const orderStatuses = ['Registrado', 'Confirmado', 'Enviado', 'Entregado', 'Cancelado'] as const
export type OrderStatus = typeof orderStatuses[number]

export interface Order {
  id: number
  numeroPedido: string
  cliente: string
  fecha: string
  total: number
  estado: OrderStatus
}

export interface OrderInput {
  numeroPedido: string
  cliente: string
  fecha: string
  total: number
  estado: OrderStatus
}

export interface OrderFilters {
  search?: string
  estado?: OrderStatus
}
