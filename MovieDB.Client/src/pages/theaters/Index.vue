<script setup lang="ts">
import { useQuery } from '@tanstack/vue-query'
import { ref, computed } from 'vue'
import { fetchTheaters } from '@/api/theater'
import FilterBar from '@/components/FilterBar.vue'
import StatsHeader from '@/components/StatsHeader.vue'
import Theater from '@/components/theaters/Theater.vue'
import { filterByTitle } from '@/services/filterByTitle'

const searchValue = ref('')

const { isPending, isError, data } = useQuery({
  queryKey: ['theaters'],
  queryFn: fetchTheaters,
})

const theaters = computed(() => data.value ?? [])
const filteredTheaters = computed(() => filterByTitle(theaters.value, searchValue.value))
</script>
<template>
  <StatsHeader :items="theaters" :loading="isPending" />
  <div class="container mx-auto px-2 sm:px-4 lg:px-6">
    <div v-if="isPending" class="p-6 flex align-items justify-center">
      <i class="fad fa-spinner-third fa-spin fa-2x" style="color: blue;"></i>
    </div>
    <div v-else-if="isError" class="p-6 flex justify-center">
      <div class="text-red-600 font-bold">Failed to load theaters</div>
    </div>
    <div v-else>
      <FilterBar v-model="searchValue" add-to="/theaters/new" add-label="Add Theater" />
      <div class="py-6 grid grid-cols-1 md:grid-cols-3 lg:grid-cols-4 gap-4">
        <Theater v-for="theater in filteredTheaters" :key="theater.id" :theater="theater"/>
      </div>
    </div>
  </div>
</template>
