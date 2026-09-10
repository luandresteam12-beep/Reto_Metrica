import type { SubmitHandler, UseFormReturn } from 'react-hook-form'
import { Link } from 'react-router-dom'
import { orderStatuses } from '../../domain/order'
import type { OrderFormValues } from '../../application/orderSchema'

interface OrderFormProps {
  form: UseFormReturn<OrderFormValues>
  isEditing: boolean
  isSaving: boolean
  serverError: string
  onSubmit: SubmitHandler<OrderFormValues>
}

export function OrderForm({ form, isEditing, isSaving, serverError, onSubmit }: OrderFormProps) {
  const { register, handleSubmit, formState: { errors, isSubmitting } } = form

  return (
    <div className="panel form-panel">
      <form className="form-grid" onSubmit={handleSubmit(onSubmit)} noValidate>
        <label className="field"><span>Número de pedido</span><input {...register('numeroPedido')} placeholder="PED-001" />{errors.numeroPedido && <small className="field-error">{errors.numeroPedido.message}</small>}</label>
        <label className="field"><span>Cliente</span><input {...register('cliente')} placeholder="Juan Perez" />{errors.cliente && <small className="field-error">{errors.cliente.message}</small>}</label>
        <label className="field"><span>Fecha</span><input {...register('fecha')} type="date" />{errors.fecha && <small className="field-error">{errors.fecha.message}</small>}</label>
        <label className="field"><span>Total (S/)</span><input {...register('total')} type="number" min="0.01" step="0.01" placeholder="250.75" />{errors.total && <small className="field-error">{errors.total.message}</small>}</label>
        <label className="field"><span>Estado</span><select {...register('estado')}>{orderStatuses.map((status) => <option key={status} value={status}>{status}</option>)}</select>{errors.estado && <small className="field-error">{errors.estado.message}</small>}</label>
        {serverError && <div className="alert error form-full" role="alert">{serverError}</div>}
        <div className="form-actions form-full"><Link className="button ghost" to="/pedidos">Cancelar</Link><button className="button primary" type="submit" disabled={isSubmitting || isSaving}>{isSubmitting || isSaving ? 'Guardando…' : isEditing ? 'Actualizar pedido' : 'Guardar pedido'}</button></div>
      </form>
    </div>
  )
}
