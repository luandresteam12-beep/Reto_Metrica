import axios from 'axios'

const accessTokenKey = 'metrica.access-token'

export const tokenStore = {
  get: () => sessionStorage.getItem(accessTokenKey),
  set: (token: string) => sessionStorage.setItem(accessTokenKey, token),
  clear: () => sessionStorage.removeItem(accessTokenKey),
}

export const httpClient = axios.create({
  baseURL: import.meta.env.VITE_API_URL ?? 'https://localhost:7248',
  headers: {
    'Content-Type': 'application/json',
  },
  timeout: 10000,
})

httpClient.interceptors.request.use((config) => {
  const token = tokenStore.get()
  if (token) {
    config.headers.Authorization = `Bearer ${token}`
  }
  return config
})

httpClient.interceptors.response.use(
  (response) => response,
  (error: unknown) => {
    if (axios.isAxiosError(error) && error.response?.status === 401 && !error.config?.url?.includes('/auth/login')) {
      tokenStore.clear()
      window.dispatchEvent(new Event('metrica:auth-expired'))
    }
    return Promise.reject(error)
  },
)
