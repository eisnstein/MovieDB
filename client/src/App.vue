<script setup lang="ts">
// This starter template is using Vue 3 <script setup> SFCs
// Check out https://v3.vuejs.org/api/sfc-script-setup.html#sfc-script-setup
import NavMenu from './components/NavMenu.vue'
</script>

<template>
  <div class="main">
    <NavMenu />
    <main>
      <router-view></router-view>
    </main>
  </div>
</template>

<style>
</style>
