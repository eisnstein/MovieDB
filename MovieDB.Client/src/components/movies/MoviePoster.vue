<script setup lang="ts">
import { computed, ref, watch } from 'vue'

const { url } = defineProps<{
  url: string | null | undefined
}>()

const defaultUrl = "https://m.media-amazon.com/images/M/MV5BMDljNTQ5ODItZmQwMy00M2ExLTljOTQtZTVjNGE2NTg0NGIxXkEyXkFqcGdeQXVyODkzNTgxMDg@._V1_SX300.jpg"

const failed = ref(false)
watch(() => url, () => failed.value = false)

const src = computed(() => {
  // OMDb returns "N/A" when there is no poster
  if (failed.value || !url || url === 'N/A') {
    return defaultUrl
  }

  return url
})
</script>
<template>
  <img :src="src" @error="failed = true" />
</template>
