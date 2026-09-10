import { useEffect, useMemo, useState, type PropsWithChildren } from 'react'
import { tokenStore } from '../../../shared/infrastructure/httpClient'
import { login as loginRequest } from '../infrastructure/authApi'
import type { AuthenticatedUser } from '../domain/auth'
import { AuthContext } from './auth-context'

const userKey = 'metrica.authenticated-user'

function getStoredUser(): AuthenticatedUser | null {
  const value = sessionStorage.getItem(userKey)
  if (!value) return null

  try {
    return JSON.parse(value) as AuthenticatedUser
  } catch {
    sessionStorage.removeItem(userKey)
    tokenStore.clear()
    return null
  }
}

export function AuthProvider({ children }: PropsWithChildren) {
  const [user, setUser] = useState<AuthenticatedUser | null>(getStoredUser)

  useEffect(() => {
    const handleExpiredSession = () => {
      tokenStore.clear()
      sessionStorage.removeItem(userKey)
      setUser(null)
    }

    window.addEventListener('metrica:auth-expired', handleExpiredSession)
    return () => window.removeEventListener('metrica:auth-expired', handleExpiredSession)
  }, [])

  const value = useMemo(() => ({
    user,
    isAuthenticated: Boolean(user && tokenStore.get()),
    async login(credentials: Parameters<typeof loginRequest>[0]) {
      const response = await loginRequest(credentials)
      tokenStore.set(response.token)
      sessionStorage.setItem(userKey, JSON.stringify(response.user))
      setUser(response.user)
    },
    logout() {
      tokenStore.clear()
      sessionStorage.removeItem(userKey)
      setUser(null)
    },
  }), [user])

  return <AuthContext.Provider value={value}>{children}</AuthContext.Provider>
}
