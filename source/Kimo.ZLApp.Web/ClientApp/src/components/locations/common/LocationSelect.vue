<script lang="ts" setup>
import {onMounted, ref} from "vue";
import {useI18n} from "vue-i18n";
import {Select} from "primevue";
import type {Location} from "@/models/locations/location";
import LocationService from "@/services/locationService";

const {t} = useI18n();

const props = defineProps<{
  modelValue: string | number | null;
  error?: string;
}>();

const emit = defineEmits(["update:modelValue"]);

const locations = ref<Location[]>([]);

async function loadLocations(): Promise<void> {
  const locationsResult = await LocationService.getAll();
  if (!locationsResult.success || !locationsResult.data) {
    return;
  }

  locations.value = locationsResult.data;
}

onMounted(loadLocations);

function updateValue(value: string | number | null) {
  emit("update:modelValue", value);
}
</script>

<template>
  <Select
    :invalid="!!props.error"
    :modelValue="props.modelValue"
    :options="locations"
    :placeholder="t('locations.selectLocation')"
    fluid
    option-label="name"
    option-value="id"
    @update:modelValue="updateValue"/>
</template>
