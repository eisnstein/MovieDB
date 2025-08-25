/// <reference types="vite/client" />

declare module '*.vue' {
  import type { DefineComponent } from 'vue'
  const component: DefineComponent<{}, {}, any>
  export default component
}

// Ensure Vue module is properly typed
declare module 'vue' {
  export * from '@vue/runtime-core'
}

interface ImportMetaEnv {
  VITE_API_URL: string
  VITE_OMDB_API_KEY: string
}
