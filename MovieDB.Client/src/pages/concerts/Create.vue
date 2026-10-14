<script setup lang="ts">
import { ref } from 'vue'
import { useRouter } from 'vue-router'
import { errorMessage } from '../../api/client'
import { storeConcert } from '../../api/concert'

const router = useRouter()
const title = ref('')
const date = ref('')
const location = ref('')
const genre = ref(0)
const rating = ref(1)
const isSubmitting = ref(false)
const saveError = ref<string | null>(null)

const concertGenres = [
  { text: 'Rock', value: 0 },
  { text: 'Classic', value: 1 },
  { text: 'Reggae', value: 2 },
  { text: 'Pop', value: 3 },
  { text: 'Latin', value: 4 },
  { text: 'Electro', value: 5 },
  { text: 'DrumnBass', value: 6 },
]


async function onSubmit() {
  if (isSubmitting.value) {
    return
  }

  isSubmitting.value = true
  saveError.value = null

  try {
    await storeConcert({
      title: title.value,
      seenAt: date.value,
      location: location.value,
      genre: genre.value,
      rating: rating.value,
    })

    // Redirect to concerts overview site
    router.push('/concerts')
  } catch (error) {
    saveError.value = errorMessage(error, 'Could not save the concert')
  } finally {
    isSubmitting.value = false
  }
}

function setRating(value: number) {
  rating.value = value
}
</script>
<template>
  <div class="container w-full lg:w-1/3 mx-auto py-6 px-2 sm:px-4 lg:px-6">
    <div class="flex flex-col md:flex-row bg-white shadow-md rounded-sm p-3 md:p-6">
      <form class="w-full" @submit.prevent="onSubmit">
        <div>
          <input type="text" v-model="title" id="title" placeholder="Artist" class="focus:ring-indigo-500 focus:border-indigo-500 block w-full shadow-xs sm:text-sm border-gray-300 rounded-sm">
        </div>
        <div class="mt-4">
          <input type="date" required v-model="date" id="date" placeholder="Seen at" class="mt-1 focus:ring-indigo-500 focus:border-indigo-500 block w-full shadow-xs sm:text-sm border-gray-300 rounded-sm">
        </div>
        <div class="mt-4">
          <input type="text" v-model="location" id="location" placeholder="Location" class="mt-1 focus:ring-indigo-500 focus:border-indigo-500 block w-full shadow-xs sm:text-sm border-gray-300 rounded-md">
        </div>
        <div class="mt-4">
          <select v-model="genre" class="mt-1 block w-full py-2 px-3 border border-gray-300 bg-white rounded-md shadow-xs focus:outline-hidden focus:ring-indigo-500 focus:border-indigo-500 sm:text-sm">
            <option v-for="option in concertGenres" :value="option.value">{{ option.text }}</option>
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
          <button class="w-full bg-blue-500 hover:bg-blue-700 text-white text-center font-bold py-2 px-4 rounded-sm focus:outline-hidden focus:shadow-outline" type="submit" :disabled="isSubmitting">
            <span v-if="!isSubmitting">Save</span>
            <i v-else class="fad fa-spinner-third fa-spin" style="color: white;"></i>
          </button>
        </div>
      </form>
    </div>
    <div v-if="saveError" class="mt-4 rounded-sm p-3 bg-red-100 text-red-700 text-center whitespace-pre-line">
      {{ saveError }}
    </div>
  </div>
</template>

<style scoped>
@reference "../../index.css";

.rating {
  @apply w-8 h-8 rounded-xl flex items-center justify-center border bg-white hover:bg-gray-300 cursor-pointer;
}

.rating.active {
  @apply bg-gray-700 text-white;
}
</style>