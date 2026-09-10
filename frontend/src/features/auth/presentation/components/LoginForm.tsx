import type { SubmitHandler, UseFormReturn } from 'react-hook-form'
import type { LoginFormValues } from '../../application/loginSchema'

interface LoginFormProps {
  form: UseFormReturn<LoginFormValues>
  serverError: string
  onSubmit: SubmitHandler<LoginFormValues>
}

export function LoginForm({ form, serverError, onSubmit }: LoginFormProps) {
  const { register, handleSubmit, formState: { errors, isSubmitting } } = form

  return (
    <form className="form-stack" onSubmit={handleSubmit(onSubmit)} noValidate>
      <label className="field">
        <span>Correo electrónico</span>
        <input {...register('email')} type="email" autoComplete="email" placeholder="correo@ejemplo.com" />
        {errors.email && <small className="field-error">{errors.email.message}</small>}
      </label>

      <label className="field">
        <span>Contraseña</span>
        <input {...register('password')} type="password" autoComplete="current-password" placeholder="••••••••" />
        {errors.password && <small className="field-error">{errors.password.message}</small>}
      </label>

      {serverError && <div className="alert error" role="alert">{serverError}</div>}
      <button className="button primary full-width" type="submit" disabled={isSubmitting}>
        {isSubmitting ? 'Validando…' : 'Iniciar sesión'}
      </button>
    </form>
  )
}
