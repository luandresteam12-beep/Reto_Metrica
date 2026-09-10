import { httpClient } from '../../../shared/infrastructure/httpClient'
import type { LoginRequest, LoginResponse } from '../domain/auth'

export async function login(request: LoginRequest) {
  const response = await httpClient.post<LoginResponse>('/auth/login', request)
  return response.data
}
