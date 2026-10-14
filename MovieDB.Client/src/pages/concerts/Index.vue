<script setup lang="ts">
import { useQuery } from '@tanstack/vue-query'
import { ref, computed } from 'vue'
import { fetchConcerts } from '@/api/concert'
import FilterBar from '@/components/FilterBar.vue'
import StatsHeader from '@/components/StatsHeader.vue'
import Concert from '@/components/concerts/Concert.vue'
import { filterByTitle } from '@/services/filterByTitle'

const searchValue = ref('')

const { isPending, isError, data } = useQuery({
  queryKey: ['concerts'],
  queryFn: fetchConcerts,
})

const concerts = computed(() => data.value ?? [])
const filteredConcerts = computed(() => filterByTitle(concerts.value, searchValue.value))
</script>
<template>
  <StatsHeader :items="concerts" :loading="isPending" />
  <div class="container mx-auto px-2 sm:px-4 lg:px-6">
    <div v-if="isPending" class="p-6 flex items-center justify-center">
      <i class="fad fa-spinner-third fa-spin fa-2x" style="color: blue;"></i>
    </div>
    <div v-else-if="isError" class="p-6 flex justify-center">
      <div class="text-red-600 font-bold">Failed to load concerts</div>
    </div>
    <div v-else>
      <FilterBar v-model="searchValue" add-to="/concerts/new" add-label="Add Concert" />
      <div class="py-6 grid grid-cols-1 md:grid-cols-3 lg:grid-cols-4 gap-4">
        <Concert v-for="concert in filteredConcerts" :key="concert.id" :concert="concert"/>
      </div>
    </div>
  </div>
</template>
