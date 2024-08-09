<script setup lang="ts">
import Concert from '../../components/concerts/Concert.vue'
import {ref, onMounted, computed} from 'vue'
import {TConcert} from '../../types/concert'
import {fetchConcerts} from '../../api/concert'

const date = new Date()
const year = date.getFullYear()
const month = date.getMonth()

const searchValue = ref('')
const loading = ref(true)

const theaters = ref<Array<TConcert>>([])
const filteredConcerts = computed(() => {
  if (searchValue.value.length < 3) {
    return theaters.value
  }

  return theaters.value.filter((m) => m.title.toLowerCase().startsWith(searchValue.value.toLowerCase()))
})

onMounted(async () => {
  theaters.value = await fetchConcerts()
  loading.value = false
})

const totalCount = computed(() => theaters.value.length ?? 0)
const yearCount = computed(() => {
  return theaters.value.filter((m) => {
    const seenAt = new Date(Date.parse(m.seenAt))
    return seenAt.getFullYear() === year
  }).length
})
const monthCount = computed(() => {
  return theaters.value.filter((m) => {
    const seenAt = new Date(Date.parse(m.seenAt))
    return seenAt.getFullYear() === year && seenAt.getMonth() === month
  }).length
})

</script>
<template>
  <header class="bg-gray-100">
    <div class="container mx-auto py-6 px-2 sm:px-4 lg:px-6">
      <div class="flex justify-center align-items">
        <div class="w-1/3 rounded bg-white p-2 md:p-4 border">
          <div class="text-gray-600 text-sm">Total</div>
          <i v-if="loading" class="mt-2 fad fa-spinner-third fa-spin fa-lg" style="color: blue;"></i>
          <div v-else class="text-4xl font-bold leading-tight">
            {{ totalCount }}
          </div>
        </div>
        <div class="w-1/3 ml-4 rounded bg-white p-2 md:p-4 border">
          <div class="text-gray-600 text-sm">This Year</div>
          <i v-if="loading" class="mt-2 fad fa-spinner-third fa-spin fa-lg" style="color: blue;"></i>
          <div v-else class="text-4xl font-bold leading-tight">
            {{ yearCount }}
          </div>
        </div>
        <div class="w-1/3 ml-4 rounded bg-white p-2 md:p-4 border">
          <div class="text-gray-600 text-sm">This Month</div>
          <i v-if="loading" class="mt-2 fad fa-spinner-third fa-spin fa-lg" style="color: blue;"></i>
          <div v-else class="text-4xl font-bold leading-tight">
            {{ monthCount }}
          </div>
        </div>
      </div>
    </div>
  </header>
  <div class="container mx-auto px-2 sm:px-4 lg:px-6">
    <div v-if="loading" class="p-6 flex align-items justify-center">
      <i class="fad fa-spinner-third fa-spin fa-2x" style="color: blue;"></i>
    </div>
    <div v-else>
      <div class="flex flex-row items-stretch gap-2">
        <input
          id="filter"
          v-model="searchValue"
          class="bg-white appearance-none border-2 border-gray-300 rounded w-full px-4 text-gray-700 leading-tight focus:outline-none focus:border-purple-500"
          placeholder="Filter by Title..."
          type="text"
          />
        <router-link to="/concerts/new"
                     class="hidden md:inline-flex whitespace-nowrap text-white bg-blue-700 hover:bg-blue-800 focus:ring-4 focus:ring-blue-300 font-medium rounded text-sm px-5 py-2.5 text-center inline-flex items-center">
          Add Concert
        </router-link>
        <router-link class="md:hidden whitespace-nowrap text-white bg-blue-700 hover:bg-blue-800 focus:ring-4 focus:ring-blue-300 font-medium rounded text-sm px-5 py-2.5 text-center inline-flex items-center"
                     to="/theaters/new">
          <i class="far fa-plus"></i>
        </router-link>
      </div>
      <div class="py-6 grid grid-cols-1 md:grid-cols-3 lg:grid-cols-4 gap-4">
        <Concert v-for="concert in filteredConcerts" :key="concert.id" :concert="concert"/>
      </div>
    </div>
  </div>
</template>
