<script setup lang="ts">
import { useStore } from './services/store';
import NavMenu from './components/NavMenu.vue'

const store = useStore()
</script>

<template>
  <div class="main">
    <NavMenu v-if="store.state.isAuthenticated" />
    <main>
      <router-view></router-view>
    </main>
  </div>
</template>

<style>
</style>
