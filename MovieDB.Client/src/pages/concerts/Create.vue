<script setup lang="ts">
import { computed, ref } from 'vue'
import { useRouter } from 'vue-router'
import { useMutation, useQueryClient } from '@tanstack/vue-query'
import { errorMessage } from '../../api/client'
import { storeConcert } from '../../api/concert'
import { concertGenres, TConcertGenre } from '../../types/concert'
import RatingInput from '../../components/RatingInput.vue'

const router = useRouter()
const queryClient = useQueryClient()
const title = ref('')
const date = ref('')
const location = ref('')
const genre = ref<TConcertGenre>(0)
const rating = ref(1)

const { mutate, isPending: isSubmitting, error } = useMutation({
  mutationFn: storeConcert,
  onSuccess: () => {
    queryClient.invalidateQueries({ queryKey: ['concerts'] })
    router.push('/concerts')
  },
})

const saveError = computed(() => error.value ? errorMessage(error.value, 'Could not save the concert') : null)

function onSubmit() {
  if (isSubmitting.value) {
    return
  }

  mutate({
    title: title.value,
    seenAt: date.value,
    location: location.value,
    genre: genre.value,
    rating: rating.value,
  })
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
        <RatingInput v-model="rating" class="mt-4" />
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
