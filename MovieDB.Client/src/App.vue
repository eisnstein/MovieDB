<script setup lang="ts">
import { useAuthStore } from './services/store'
import NavMenu from './components/NavMenu.vue'

const authStore = useAuthStore()
</script>

<template>
  <NavMenu v-if="authStore.isAuthenticated" />
  <main class="mt-16">
    <router-view></router-view>
  </main>
</template>

<style>
</style>
