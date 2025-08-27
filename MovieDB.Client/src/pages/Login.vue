<script setup lang="ts">
import { ref, computed } from 'vue'
import { useStore } from '../services/store';

const store = useStore()
const email = ref('')
const password = ref('')

const alertClass = computed(() => ({
  'bg-red-600': store.state.alert?.type === 'error',
  'bg-green-500': store.state.alert?.type === 'success',
}))

function onSubmit() {
  store.dispatch('login', { email: email.value, password: password.value})
}
</script>
<template>
  <div class="container mx-auto pt-6">
    <div class="w-full max-w-xs mx-auto">
      <div class="p-6 text-center">
          <i class="fad fa-dice-d10 fa-2x" style="color: dodgerblue;"></i>
      </div>
      <form class="bg-white shadow-md rounded-sm p-6" @submit.prevent="onSubmit">
        <div class="mb-4">
            <label class="block text-gray-700 text-sm mb-1" for="email">
                Email
            </label>
            <input type="email" v-model="email" id="email" placeholder="your@email.com" class="focus:ring-indigo-500 focus:border-indigo-500 block w-full shadow-xs sm:text-sm border-gray-300 rounded-md">
        </div>
        <div class="mb-6">
            <label class="block text-gray-700 text-sm mb-1" for="password">
                Password
            </label>
            <input type="password" v-model="password" id="password" placeholder="*****" class="focus:ring-indigo-500 focus:border-indigo-500 block w-full shadow-xs sm:text-sm border-gray-300 rounded-md">
        </div>
        <div class="flex items-center justify-between">
            <button class="w-full bg-blue-500 hover:bg-blue-700 text-white text-center font-bold py-2 px-4 rounded-sm focus:outline-hidden focus:shadow-outline" type="submit" :disabled="store.state.loading">
              <span v-if="!store.state.loading">Sign In</span>
              <i v-else class="fad fa-spinner-third fa-spin" style="color: white;"></i>
            </button>
        </div>
      </form>
      <div v-if="store.state.alert" class="mt-4 shadow-md rounded-sm p-6 text-white text-center" :class="alertClass" >
        {{ store.state.alert.message }}
      </div>
    </div>
  </div>
</template>