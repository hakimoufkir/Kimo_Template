<script lang="ts" setup>
import {MultiSelect, type MultiSelectChangeEvent} from "primevue";
import type {Location} from "@/models/locations/location";
import {onMounted, ref} from "vue";
import LocationService from "@/services/locationService";
import {useI18n} from "vue-i18n";
import {PluralizedStringUtils} from "@/utils/format/pluralizedStringUtils";

const {t} = useI18n();

const emit = defineEmits<{
  (event: 'change', value: Location[]): void;
}>();

const model = ref<Location[]>([]);
const locations = ref<Location[]>([]);

onMounted(loadLocations);

async function loadLocations(): Promise<void> {
  const locationsResult = await LocationService.getAll();
  if (!locationsResult.success || !locationsResult.data) {
    return;
  }

  locations.value = locationsResult.data;
}

function selectionChanged(e: MultiSelectChangeEvent): void {
  emit("change", e.value);
}
</script>

<template>
  <MultiSelect
    v-model="model"
    :options="locations"
    :placeholder="t('locations.selectLocations')"
    :virtualScrollerOptions="{ itemSize: 44 }"
    auto-filter-focus
    display="chip"
    filter
    optionLabel="name"
    reset-filter-on-hide
    @change="selectionChanged($event)">
    <template #option="slotProps">
      {{ slotProps.option.name }}
    </template>
    <template #footer>
      <div class="py-2 px-4 mt-2 bg-surface-100 dark:bg-surface-700">
        <b>{{ model ? model.length : 0 }}</b>
        {{
          PluralizedStringUtils.getPluralizedStringFromCount(
            model ? model.length : 0,
            t("locations.location"),
            t("locations.locations")
          )
        }}
        {{ t("common.selected") }}
      </div>
    </template>
    <template #filtericon>
      <i
        v-if="model?.length > 0"
        class="pi pi-filter-slash cursor-pointer"
        @click="model = []"/>
    </template>
  </MultiSelect>
</template>

<style scoped></style>
