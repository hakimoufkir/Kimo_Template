<script lang="ts" setup>
import {computed} from "vue";

const props = defineProps<{
  label: string;
  error?: string;
  required?: boolean;
}>();

const hasError = computed(() => !!props.error);
const fieldClass = computed(() => ({"text-red-500": hasError.value}));
</script>

<template>
  <div :class="fieldClass" class="flex flex-col gap-2">
    <label :class="{ 'mandatory-field': required }">{{ label }}</label>
    <slot></slot>
    <small>{{ error }}</small>
  </div>
</template>

<style scoped></style>
