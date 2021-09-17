<script setup lang="ts">
import { ref } from 'vue'
import { useStore } from '../services/store';

const store = useStore()
const email = ref('')
const password = ref('')

const alertClass = ref({
  'bg-red-600': store.state.alert?.type === 'error',
  'bg-green-500': store.state.alert?.type === 'success',
})

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
      <form class="bg-white shadow-md rounded p-6" @submit.prevent="onSubmit">
        <div class="mb-4">
            <label class="block text-gray-700 text-sm mb-1" for="email">
                Email
            </label>
            <input v-model="email" class="appearance-none border rounded w-full py-2 px-3 text-gray-700 leading-tight focus:outline-none focus:shadow-outline" id="email" type="email" placeholder="your@email.com" />
        </div>
        <div class="mb-6">
            <label class="block text-gray-700 text-sm mb-1" for="password">
                Password
            </label>
            <input v-model="password" class="appearance-none border rounded w-full py-2 px-3 text-gray-700 leading-tight focus:outline-none focus:shadow-outline" id="password" type="password" placeholder="********" />
        </div>
        <div class="flex items-center justify-between">
            <button class="w-full bg-blue-500 hover:bg-blue-700 text-white text-center font-bold py-2 px-4 rounded focus:outline-none focus:shadow-outline" type="submit" :disabled="store.state.loading">
              <span v-if="!store.state.loading">Sign In</span>
              <i v-else class="fad fa-spinner-third fa-spin" style="color: white;"></i>
            </button>
        </div>
      </form>
      <div v-if="store.state.alert" class="mt-4 shadow-md rounded p-6 text-white text-center" :class="alertClass" >
        {{ store.state.alert.message }}
      </div>
    </div>
  </div>
</template>