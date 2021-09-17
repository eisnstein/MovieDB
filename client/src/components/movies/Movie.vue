<script setup lang="ts">
import { TMovie } from '../../types/movie'

const { movie } = defineProps<{
  movie: TMovie
}>()
</script>
<template>
  <h1>{{ movie.id }}</h1>
</template>