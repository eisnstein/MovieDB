<script setup lang="ts">
import { ref } from 'vue'
import { useStore } from '../../services/store'
import { storeTheater } from '../../api/theater'
import { theaterGenres } from '../../types/theater'
import router from '../../services/router'

const store = useStore()
const title = ref('')
const date = ref('')
const location = ref('')
const genre = ref(0)
const rating = ref(1)
const isSubmitting = ref(false)


async function onSubmit() {
  isSubmitting.value = true

  await storeTheater({
    title: title.value,
    seenAt: date.value,
    location: location.value,
    genre: genre.value,
    rating: rating.value,
  })

  isSubmitting.value = false

  router.push('/theaters')
}

function setRating(value: number) {
  rating.value = value
}
</script>
<template>
  <div class="mt-6 container max-w-md mx-auto flex bg-white shadow-md rounded p-6 pt-6">
    <form class="w-full" @submit.prevent="onSubmit">
      <div>
        <input type="text" v-model="title" id="title" placeholder="Artist" class="focus:ring-indigo-500 focus:border-indigo-500 block w-full shadow-sm sm:text-sm border-gray-300 rounded-md">
      </div>
      <div class="mt-4">
        <input type="date" v-model="date" id="date" placeholder="Seen at" class="mt-1 focus:ring-indigo-500 focus:border-indigo-500 block w-full shadow-sm sm:text-sm border-gray-300 rounded-md">
      </div>
      <div class="mt-4">
        <input type="text" v-model="location" id="location" placeholder="Location" class="mt-1 focus:ring-indigo-500 focus:border-indigo-500 block w-full shadow-sm sm:text-sm border-gray-300 rounded-md">
      </div>
      <div class="mt-4">
        <select v-model="genre" class="mt-1 block w-full py-2 px-3 border border-gray-300 bg-white rounded-md shadow-sm focus:outline-none focus:ring-indigo-500 focus:border-indigo-500 sm:text-sm">
          <option v-for="option in theaterGenres" :value="option.value">{{ option.text }}</option>
        </select>
      </div>
      <div class="mt-4 flex items-center justify-between">
        <span class="rating" :class="{'active': rating === 1}" @click="setRating(1)">1</span>
        <span class="rating" :class="{'active': rating === 2}" @click="setRating(2)">2</span>
        <span class="rating" :class="{'active': rating === 3}" @click="setRating(3)">3</span>
        <span class="rating" :class="{'active': rating === 4}" @click="setRating(4)">4</span>
        <span class="rating" :class="{'active': rating === 5}" @click="setRating(5)">5</span>
      </div>
      <div class="mt-4 flex items-center justify-between">
        <button class="w-full bg-blue-500 hover:bg-blue-700 text-white text-center font-bold py-2 px-4 rounded focus:outline-none focus:shadow-outline" type="submit" :disabled="store.state.loading">
          <span v-if="!isSubmitting">Save</span>
          <i v-else class="fad fa-spinner-third fa-spin" style="color: white;"></i>
        </button>
      </div>
    </form>
    <div v-if="store.state.alert" class="mt-4 shadow-md rounded p-6 text-white text-center">
      {{ store.state.alert.message }}
    </div>
  </div>
</template>

<style scoped>
.rating {
  @apply w-8 h-8 rounded-xl flex items-center justify-center border bg-white hover:bg-gray-300 cursor-pointer;
}

.rating.active {
  @apply bg-gray-700 text-white;
}
</style>