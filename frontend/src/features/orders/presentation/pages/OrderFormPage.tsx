import { Link } from 'react-router-dom'
import { getApiErrorMessage } from '../../../../shared/lib/errors'
import { useOrderForm } from '../../application/useOrderForm'
import { OrderForm } from '../components/OrderForm'

export function OrderFormPage() {
  const formModel = useOrderForm()
  const { form, isEditing, orderQuery, mutation, serverError, onSubmit } = formModel

  if (orderQuery.isLoading) return <section className="content-section"><div className="empty-state">Cargando pedido…</div></section>
  if (orderQuery.isError) return <section className="content-section"><div className="alert error">{getApiErrorMessage(orderQuery.error, 'No se pudo cargar el pedido.')}</div></section>

  return (
    <section className="content-section narrow-section">
      <div className="page-heading"><div><Link className="back-link" to="/pedidos">← Volver a pedidos</Link><p className="eyebrow">PEDIDOS</p><h1>{isEditing ? 'Editar pedido' : 'Registrar pedido'}</h1><p className="muted">Completa los datos; el backend validará las reglas de negocio.</p></div></div>
      <OrderForm form={form} isEditing={isEditing} isSaving={mutation.isPending} serverError={serverError} onSubmit={onSubmit} />
    </section>
  )
}
