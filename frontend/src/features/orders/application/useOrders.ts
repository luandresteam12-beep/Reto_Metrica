import { useMutation, useQuery, useQueryClient } from '@tanstack/react-query'
import { useState } from 'react'
import { getApiErrorMessage } from '../../../shared/lib/errors'
import { deleteOrder, listOrders } from '../infrastructure/ordersApi'
import type { OrderStatus } from '../domain/order'

export function useOrders() {
  const queryClient = useQueryClient()
  const [search, setSearch] = useState('')
  const [status, setStatus] = useState<OrderStatus | ''>('')
  const [feedback, setFeedback] = useState('')

  const ordersQuery = useQuery({
    queryKey: ['orders', search, status],
    queryFn: () => listOrders({ search: search || undefined, estado: status || undefined }),
  })

  const deleteMutation = useMutation({
    mutationFn: deleteOrder,
    onSuccess: async () => {
      setFeedback('Pedido eliminado correctamente.')
      await queryClient.invalidateQueries({ queryKey: ['orders'] })
    },
    onError: (error) => setFeedback(getApiErrorMessage(error)),
  })

  return {
    search,
    setSearch,
    status,
    setStatus,
    feedback,
    orders: ordersQuery.data ?? [],
    ordersQuery,
    deleteMutation,
  }
}
