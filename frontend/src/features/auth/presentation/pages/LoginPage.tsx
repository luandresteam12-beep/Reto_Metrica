import { useLogin } from '../../application/useLogin'
import { LoginForm } from '../components/LoginForm'

export function LoginPage() {
  const loginModel = useLogin()

  return (
    <main className="auth-page">
      <section className="auth-card" aria-labelledby="login-title">
        <div className="brand-mark">M</div>
        <p className="eyebrow">METRICA ORDERS</p>
        <h1 id="login-title">Bienvenido de vuelta</h1>
        <p className="muted">Ingresa para gestionar tus pedidos de forma segura.</p>
        <LoginForm {...loginModel} />
      </section>
    </main>
  )
}
