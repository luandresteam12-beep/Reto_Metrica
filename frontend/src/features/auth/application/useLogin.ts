import { zodResolver } from '@hookform/resolvers/zod'
import { useState } from 'react'
import { useForm, type SubmitHandler } from 'react-hook-form'
import { useLocation, useNavigate } from 'react-router-dom'
import { getApiErrorMessage } from '../../../shared/lib/errors'
import { useAuth } from '../presentation/useAuth'
import { loginSchema, type LoginFormValues } from './loginSchema'

export function useLogin() {
  const { login } = useAuth()
  const navigate = useNavigate()
  const location = useLocation()
  const [serverError, setServerError] = useState('')
  const form = useForm<LoginFormValues>({ resolver: zodResolver(loginSchema) })

  const onSubmit: SubmitHandler<LoginFormValues> = async (values) => {
    setServerError('')
    try {
      await login(values)
      const destination = (location.state as { from?: { pathname?: string } } | null)?.from?.pathname ?? '/pedidos'
      navigate(destination, { replace: true })
    } catch (error) {
      setServerError(getApiErrorMessage(error, 'Correo o contraseña incorrectos.'))
    }
  }

  return { form, serverError, onSubmit }
}
