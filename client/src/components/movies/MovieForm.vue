<script setup lang="ts">
import { ref } from 'vue'
import { useStore } from '../../services/store'
import { fetchMoviePoster, storeMovie } from '../../api/movie'
import router from '../../services/router'

const store = useStore()
const title = ref('')
const date = ref('')
const imdb = ref('')
const posterUrl = ref<string | undefined>()
const genre = ref(0)
const rating = ref(1)
const isSubmitting = ref(false)

const genreOptions = [
  { text: 'Action', value: 0 },
  { text: 'Comedy', value: 1 },
  { text: 'Drama', value: 2 },
  { text: 'Fantasy', value: 3 },
  { text: 'Horror', value: 4 },
  { text: 'SciFi', value: 5 },
  { text: 'Thriller', value: 6 },
]

async function onSubmit() {
  isSubmitting.value = true

  await storeMovie({
    title: title.value,
    seenAt: date.value,
    imdbIdentifier: imdb.value,
    genre: String(genre.value),
    rating: rating.value,
    posterUrl: posterUrl.value ?? null
  })

  isSubmitting.value = false

  router.push('/movies')
}

function setRating(value: number) {
  rating.value = value
}

function fetchPoster() {
  if (imdb.value.length === 0) {
    return
  }

  fetchMoviePoster(imdb.value).then((url) => posterUrl.value = url)
}
</script>
<template>
  <div class="mt-6 container max-w-2xl mx-auto flex bg-white shadow-md rounded p-6 pt-6">
    <div class="w-1/2">
      <form class="" @submit.prevent="onSubmit">
        <div>
          <input type="text" v-model="title" id="title" placeholder="Title of the movie" class="focus:ring-indigo-500 focus:border-indigo-500 block w-full shadow-sm sm:text-sm border-gray-300 rounded-md">
        </div>
        <div class="mt-4">
          <input type="date" v-model="date" id="date" placeholder="Seen at" class="mt-1 focus:ring-indigo-500 focus:border-indigo-500 block w-full shadow-sm sm:text-sm border-gray-300 rounded-md">
        </div>
        <div class="mt-4">
          <input type="text" v-model="imdb" id="imdb" placeholder="Imdb Identifier" @blur="fetchPoster" class="mt-1 focus:ring-indigo-500 focus:border-indigo-500 block w-full shadow-sm sm:text-sm border-gray-300 rounded-md">
        </div>
        <div class="mt-4">
          <select v-model="genre" class="mt-1 block w-full py-2 px-3 border border-gray-300 bg-white rounded-md shadow-sm focus:outline-none focus:ring-indigo-500 focus:border-indigo-500 sm:text-sm">
            <option v-for="option in genreOptions" :value="option.value">{{ option.text }}</option>
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
    <div class="w-1/2 ml-3">
      <img class="poster" :src="posterUrl" />
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

.poster {
  @apply border border-dashed rounded;
  height: 304px;
  object-fit: contain;
  width: 306px;
}
</style>