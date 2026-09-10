export interface AuthenticatedUser {
  id: number
  email: string
  role: string
}

export interface LoginRequest {
  email: string
  password: string
}

export interface LoginResponse {
  token: string
  expiresIn: number
  user: AuthenticatedUser
}
