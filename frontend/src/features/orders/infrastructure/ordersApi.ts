import { httpClient } from '../../../shared/infrastructure/httpClient'
import type { Order, OrderFilters, OrderInput } from '../domain/order'

export async function listOrders(filters: OrderFilters) {
  const response = await httpClient.get<Order[]>('/api/pedidos', { params: filters })
  return response.data
}

export async function getOrder(id: number) {
  const response = await httpClient.get<Order>(`/api/pedidos/${id}`)
  return response.data
}

export async function createOrder(input: OrderInput) {
  const response = await httpClient.post<Order>('/api/pedidos', input)
  return response.data
}

export async function updateOrder(id: number, input: OrderInput) {
  const response = await httpClient.put<Order>(`/api/pedidos/${id}`, input)
  return response.data
}

export async function deleteOrder(id: number) {
  await httpClient.delete(`/api/pedidos/${id}`)
}
