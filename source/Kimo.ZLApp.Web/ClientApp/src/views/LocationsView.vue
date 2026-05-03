<script lang="ts" setup>
import { onMounted, ref } from "vue";
import { Card } from "primevue";
import { useI18n } from "vue-i18n";
import { useToast } from "primevue/usetoast";
import type { Location } from "@/models/locations/location";
import LocationService from "@/services/locationService";

const { t } = useI18n();
const toast = useToast();

const isLoading = ref(false);
const locations = ref<Location[]>([]);
const errorMessage = ref<string | null>(null);

async function loadLocations(): Promise<void> {
  isLoading.value = true;
  errorMessage.value = null;

  const result = await LocationService.getAll();

  if (!result.success || !result.data) {
    errorMessage.value = result.error?.message ?? t("locations.loadingFailed");
    toast.add({
      severity: "error",
      summary: t("common.errorOccurred"),
      detail: errorMessage.value,
      life: 3000
    });
    isLoading.value = false;
    return;
  }

  locations.value = result.data;
  isLoading.value = false;
}

onMounted(async () => {
  await loadLocations();
});
</script>

<template>
  <Card>
    <template #title>
      {{ t("locations.title") }}
    </template>
    <template #content>
      <div class="flex justify-content-between align-items-center mb-4">
        <span>{{ t("locations.description") }}</span>
        <button class="p-button p-component" type="button" @click="loadLocations">
          <span class="p-button-label">{{ t("common.refresh") }}</span>
        </button>
      </div>

      <div v-if="isLoading">{{ t("common.loading") }}</div>
      <div v-else-if="errorMessage">{{ errorMessage }}</div>
      <div v-else-if="!locations.length">{{ t("common.noEntries") }}</div>
      <ul v-else class="locations-list">
        <li v-for="location in locations" :key="location.id" class="locations-list__item">
          <span class="locations-list__id">#{{ location.id }}</span>
          <span>{{ location.name }}</span>
        </li>
      </ul>
    </template>
  </Card>
</template>

<style scoped>
.locations-list {
  list-style: none;
  margin: 0;
  padding: 0;
  display: flex;
  flex-direction: column;
  gap: 0.75rem;
}

.locations-list__item {
  display: flex;
  gap: 0.75rem;
  align-items: center;
  padding: 0.75rem 1rem;
  border: 1px solid #e5e7eb;
  border-radius: 0.5rem;
  background: #fff;
}

.locations-list__id {
  color: #6b7280;
  min-width: 3rem;
}
</style>

