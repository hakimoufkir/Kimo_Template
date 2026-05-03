<script lang="ts" setup>
import {ref} from "vue";
import Button from "primevue/button";

defineProps<{ msg: string }>();

const count = ref(0);
</script>

<template>
  <div class="flex flex-col align-items-center">
    <h1>{{ msg }}</h1>
    <Button
      :label="'count is: ' + count.toString()"
      class="my-4"
      @click="count++"/>
  </div>
</template>

<style scoped></style>
