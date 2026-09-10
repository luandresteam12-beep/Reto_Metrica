import axios from 'axios'
import type { ApiProblemDetails } from '../types/api'

export function getApiErrorMessage(error: unknown, fallback = 'No se pudo completar la operación.') {
  if (axios.isAxiosError<ApiProblemDetails>(error)) {
    const response = error.response?.data
    if (response?.errors) {
      return Object.values(response.errors).flat().join(' ')
    }
    return response?.detail ?? response?.title ?? fallback
  }

  return error instanceof Error ? error.message : fallback
}
