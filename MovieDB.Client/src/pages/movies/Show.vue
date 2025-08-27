<script lang="ts" setup>
import {ref, onMounted, computed} from 'vue'
import { useRoute } from 'vue-router'
import router from '@/services/router'
import { TMovie } from '@/types/movie'
import { fetchMovie, deleteMovie } from '@/api/movie'
import { genreOptions } from '@/constants'

const route = useRoute()
const loading = ref(true)
const deleting = ref(false)
const movie = ref<TMovie | null>(null)

onMounted(async () => {
  try {
    movie.value = await fetchMovie(route.params.id)
  } catch (error) {
    console.error(error)
  } finally {
    loading.value = false
  }
})

const seenAt = computed(() => {
  if (movie.value) {
    const date = Date.parse(movie.value.seenAt)
    return new Intl.DateTimeFormat('de-AT', { year: 'numeric', month: 'numeric', day: 'numeric'}).format(date)
  }

  return null
})

const genre = computed(() => {
  if (movie.value) {
    return genreOptions.find(g => g.value === movie.value!.genre)?.text ?? null
  }

  return null
})

async function handleDelete() {
  deleting.value = true
  try {
    await deleteMovie(movie.value!.id)
  } catch (error) {
    console.error(error)
  } finally {
    deleting.value = false
    router.push('/movies')
  }
}
</script>
<template>
  <div class="container mx-auto py-6 px-2 sm:px-4 lg:px-6">
    <div v-if="loading" class="p-6 flex align-items justify-center">
      <i class="fad fa-spinner-third fa-spin fa-2x" style="color: blue;"></i>
    </div>
    <div v-else>
      <div class="container max-w-2xl mx-auto flex bg-white shadow-md rounded-sm p-3 md:p-6">
        <div class="w-1/2 flex flex-col justify-between">
          <div>
            <div class="text-sm">{{ seenAt }}</div>
            <div class="mt-1 font-bold">{{ movie.title }}</div>
            <div class="mt-3">
              <span v-for="i in [1, 2, 3, 4, 5]">
                <i v-if="i <= movie.rating" class="fad fa-fire-alt mr-1" style="color: darkorange;"></i>
                <i v-else class="fad fa-fire-alt mr-1"></i>
              </span>
            </div>
            <div class="mt-2"><span class="px-2 py-1 inline-flex items-center rounded-full bg-blue-700 text-xs text-white">{{ genre }}</span></div>
          </div>
          <div>
            <button class="px-3 py-1 inline-flex items-center rounded-full bg-red-700 text-white" type="button" @click="handleDelete">
              Delete <i class="ml-2 fad" :class="{'fa-trash': !deleting, 'fa-spinner-third fa-spin': deleting}"></i>
            </button>
          </div>
        </div>
        <div class="w-1/2 ml-2">
          <img class="rounded-sm" :src="movie.posterUrl" />
        </div>
      </div>
    </div>
  </div>
</template>
