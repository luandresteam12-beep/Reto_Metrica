import { zodResolver } from '@hookform/resolvers/zod'
import { useMutation, useQuery, useQueryClient } from '@tanstack/react-query'
import { useEffect, useState } from 'react'
import { useForm, type SubmitHandler } from 'react-hook-form'
import { useNavigate, useParams } from 'react-router-dom'
import { getApiErrorMessage } from '../../../shared/lib/errors'
import { createOrder, getOrder, updateOrder } from '../infrastructure/ordersApi'
import type { OrderInput } from '../domain/order'
import { orderSchema, type OrderFormValues } from './orderSchema'

const defaultValues: OrderFormValues = {
  numeroPedido: '',
  cliente: '',
  fecha: new Date().toISOString().slice(0, 10),
  total: 0,
  estado: 'Registrado',
}

export function useOrderForm() {
  const { id } = useParams()
  const orderId = id ? Number(id) : undefined
  const isEditing = Number.isInteger(orderId) && orderId !== undefined
  const navigate = useNavigate()
  const queryClient = useQueryClient()
  const [serverError, setServerError] = useState('')
  const form = useForm<OrderFormValues>({
    resolver: zodResolver(orderSchema),
    defaultValues,
  })

  const orderQuery = useQuery({
    queryKey: ['order', orderId],
    queryFn: () => getOrder(orderId!),
    enabled: isEditing,
  })

  useEffect(() => {
    if (orderQuery.data) {
      form.reset({ ...orderQuery.data, fecha: orderQuery.data.fecha.slice(0, 10) })
    }
  }, [form, orderQuery.data])

  const mutation = useMutation({
    mutationFn: (input: OrderInput) => isEditing ? updateOrder(orderId!, input) : createOrder(input),
    onSuccess: async () => {
      await queryClient.invalidateQueries({ queryKey: ['orders'] })
      navigate('/pedidos')
    },
    onError: (error) => setServerError(getApiErrorMessage(error)),
  })

  const onSubmit: SubmitHandler<OrderFormValues> = (values) => {
    setServerError('')
    mutation.mutate({ ...values, fecha: `${values.fecha}T00:00:00` })
  }

  return {
    form,
    isEditing,
    orderQuery,
    mutation,
    serverError,
    onSubmit,
  }
}
