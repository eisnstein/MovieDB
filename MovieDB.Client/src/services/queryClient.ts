import { QueryClient } from '@tanstack/vue-query'
import { ApiError } from '@/api/client'

export const queryClient = new QueryClient({
  defaultOptions: {
    queries: {
      retry: (failureCount, error) => {
        // Client errors (401, 404, ...) won't succeed on retry
        if (error instanceof ApiError && error.status < 500) {
          return false
        }

        return failureCount < 3
      },
    },
  },
})
