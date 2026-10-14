<script setup lang="ts">
const search = defineModel<string>({ required: true })

const { addTo, addLabel } = defineProps<{
  addTo: string
  addLabel: string
}>()
</script>
<template>
  <div class="flex flex-row items-stretch gap-2">
    <input
      id="filter"
      v-model="search"
      class="bg-white appearance-none border-2 border-gray-300 rounded-sm w-full px-4 text-gray-700 leading-tight focus:outline-hidden focus:border-purple-500"
      placeholder="Filter by Title..."
      aria-label="Filter by title"
      type="text"
    />
    <router-link
      :to="addTo"
      class="hidden md:inline-flex whitespace-nowrap text-white bg-blue-700 hover:bg-blue-800 focus:ring-4 focus:ring-blue-300 font-medium rounded-sm text-sm px-5 py-2.5 text-center items-center"
    >
      {{ addLabel }}
    </router-link>
    <router-link
      :to="addTo"
      :aria-label="addLabel"
      class="md:hidden whitespace-nowrap text-white bg-blue-700 hover:bg-blue-800 focus:ring-4 focus:ring-blue-300 font-medium rounded-sm text-sm px-5 py-2.5 text-center inline-flex items-center"
    >
      <i class="far fa-plus"></i>
    </router-link>
  </div>
</template>
