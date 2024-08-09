<script setup lang="ts">
import { useStore } from './services/store'
import NavMenu from './components/NavMenu.vue'

const store = useStore()
</script>

<template>
  <NavMenu v-if="store.state.isAuthenticated" />
  <main class="mt-16">
    <router-view></router-view>
  </main>
</template>

<style>
</style>
