import { defineConfig, loadEnv } from 'vite'
import vue from '@vitejs/plugin-vue'
import tailwindcss from '@tailwindcss/vite'
import { resolve } from 'path'

// https://vitejs.dev/config/
export default defineConfig(({ mode }) => {
  const env = loadEnv(mode, import.meta.dirname, '')

  return {
    plugins: [tailwindcss(), vue()],
    resolve: {
      alias: {
        '@': resolve(import.meta.dirname, 'src'),
      },
    },
    server: {
      // Serve the API from the dev server's origin, like in production,
      // so the refresh token cookie is first-party
      proxy: {
        '/api': env.API_PROXY_TARGET || 'http://localhost:4000',
      },
    },
    build: {
      target: 'esnext',
    },
  }
})
