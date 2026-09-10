import { z } from 'zod'
import { orderStatuses } from '../domain/order'

export const orderSchema = z.object({
  numeroPedido: z.string().trim().min(1, 'El número de pedido es obligatorio.').max(50),
  cliente: z.string().trim().min(1, 'El cliente es obligatorio.').max(150),
  fecha: z.string().min(1, 'La fecha es obligatoria.'),
  total: z.coerce.number().positive('El total debe ser mayor que cero.'),
  estado: z.enum(orderStatuses),
})

export type OrderFormValues = z.infer<typeof orderSchema>
