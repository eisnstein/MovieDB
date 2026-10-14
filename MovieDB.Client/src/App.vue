<script setup lang="ts">
import { watch } from 'vue'
import { useRouter } from 'vue-router'
import { useAuthStore } from '@/services/store'
import NavMenu from '@/components/NavMenu.vue'

const authStore = useAuthStore()
const router = useRouter()

// Covers both "Sign out" and the automatic logout on a 401
watch(() => authStore.isAuthenticated, (isAuthenticated) => {
  if (!isAuthenticated) {
    router.push('/login')
  }
})
</script>

<template>
  <NavMenu v-if="authStore.isAuthenticated" />
  <main class="mt-16">
    <router-view></router-view>
  </main>
</template>

<style>
</style>
