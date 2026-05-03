<script lang="ts" setup>
import {Button} from "primevue";
import {useI18n} from "vue-i18n";
import {computed, ref, watchEffect} from "vue";
import type {Forecast} from "@/models/forecasts/forecast";

const {t} = useI18n();

const props = defineProps<{ selectedForecasts: Forecast[] }>();
const emit = defineEmits(["insert", "delete"]);

const canCreate = ref(true);
const canDelete = ref(true);


const hasSelectedForecasts = computed(() => {
  return props.selectedForecasts && props.selectedForecasts.length;
});
</script>

<template>
  <div v-if="canCreate || canDelete" class="flex flex-wrap gap-4">
    <Button
      v-if="canCreate"
      :label="t('weatherForecasts.insert')"
      icon="pi pi-plus"
      @click="emit('insert')"/>

    <Button
      v-if="canDelete"
      :disabled="!hasSelectedForecasts"
      :label="t('common.delete')"
      icon="pi pi-trash"
      severity="danger"
      @click="emit('delete', selectedForecasts)"/>
  </div>
</template>

<style scoped></style>
