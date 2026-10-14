<script setup lang="ts">
import { computed } from 'vue'

const { items, loading } = defineProps<{
  items: ReadonlyArray<{ seenAt: string }>
  loading: boolean
}>()

const now = new Date()
const year = now.getFullYear()
const month = now.getMonth()

const seenDates = computed(() => items.map((item) => new Date(item.seenAt)))

const stats = computed(() => [
  { label: 'Total', count: items.length },
  { label: 'This Year', count: seenDates.value.filter((d) => d.getFullYear() === year).length },
  { label: 'This Month', count: seenDates.value.filter((d) => d.getFullYear() === year && d.getMonth() === month).length },
])
</script>
<template>
  <header class="bg-gray-100">
    <div class="container mx-auto py-6 px-2 sm:px-4 lg:px-6">
      <div class="flex justify-center gap-4">
        <div v-for="stat in stats" :key="stat.label" class="w-1/3 rounded-sm bg-white p-2 md:p-4 border">
          <div class="text-gray-600 text-sm">{{ stat.label }}</div>
          <i v-if="loading" class="mt-2 fad fa-spinner-third fa-spin fa-lg" style="color: blue;"></i>
          <div v-else class="text-4xl font-bold leading-tight">
            {{ stat.count }}
          </div>
        </div>
      </div>
    </div>
  </header>
</template>
