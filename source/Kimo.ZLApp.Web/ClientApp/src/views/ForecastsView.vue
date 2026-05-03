<script lang="ts" setup>
import {computed, type ComputedRef, ref} from "vue";
import {Card} from "primevue";
import {useI18n} from "vue-i18n";
import InsertForecastDialog from "@/components/forecasts/InsertForecastDialog.vue";
import ForecastsTable from "@/components/forecasts/ForecastsTable.vue";
import type {Forecast} from "@/models/forecasts/forecast";
import ForecastDetailsDialog from "@/components/forecasts/ForecastDetailsDialog.vue";
import DeletionDialog from "@/components/common/DeletionDialog.vue";
import ForecastService from "@/services/forecastService";
import type {NewForecast} from "@/models/forecasts/newForecast";
import {useToast} from "primevue/usetoast";
import {useForecastsStore} from "@/stores/forecastsStore";

const {t} = useI18n();
const toast = useToast();
const forecastsStore = useForecastsStore();

const showInsertForecastDialog = ref<boolean>(false);
const showForecastDetailsDialog = ref<boolean>(false);
const showDeleteForecastDialog = ref<boolean>(false);
const activeForecasts = ref<Forecast[]>([]);

const activeForecastSelectionText: ComputedRef<string> = computed(() => {
  if (!activeForecasts.value || !activeForecasts.value.length) {
    return `0 ${t("weatherForecasts.forecasts")}`;
  }

  if (activeForecasts.value.length == 1) {
    return `1 ${t("weatherForecasts.forecast")}`;
  }

  return `${activeForecasts.value.length} ${t("weatherForecasts.forecasts")}`;
});

function showInsertDialog(): void {
  showInsertForecastDialog.value = true;
}

function showDetailsDialog(forecast: Forecast): void {
  showForecastDetailsDialog.value = true;
  activeForecasts.value = [forecast];
}

function showDeletionDialog(forecasts: Forecast[]): void {
  showDeleteForecastDialog.value = true;
  activeForecasts.value = forecasts;
}

async function createForecast(forecast: NewForecast): Promise<void> {
  const createResult = await ForecastService.insert(forecast);

  if (!createResult.success) {
    toast.add({
      severity: "error",
      summary: t("common.errorOccurred"),
      detail: t("weatherForecasts.insertionFailed"),
      life: 3000,
    });

    return;
  }

  showInsertForecastDialog.value = false;
  toast.add({
    severity: "success",
    summary: t("common.successfullySaved"),
    detail: t("common.successfullySavedInfo", {
      type: t("weatherForecasts.forecast"),
    }),
    life: 3000,
  });

  await forecastsStore.loadData();
}

async function deleteForecasts(): Promise<void> {
  const deleteResult = await ForecastService.delete(activeForecasts.value);

  if (!deleteResult.success) {
    toast.add({
      severity: "error",
      summary: t("common.errorOccurred"),
      detail: t("weatherForecasts.deletionFailed"),
      life: 3000,
    });

    return;
  }

  showDeleteForecastDialog.value = false;
  toast.add({
    severity: "success",
    summary: t("common.successfullyDeleted"),
    detail: t("common.successfullyDeletedInfo", {
      successful: deleteResult.data?.successful,
    }),
    life: 3000,
  });

  await forecastsStore.loadData();
}
</script>

<template>
  <Card>
    <template #title>
      {{ t("weatherForecasts.title") }}
    </template>
    <template #content>
      <ForecastsTable
        @delete="showDeletionDialog"
        @details="showDetailsDialog"
        @insert="showInsertDialog"/>
    </template>
  </Card>

  <InsertForecastDialog
    v-model:visible="showInsertForecastDialog"
    @insert="createForecast"/>

  <ForecastDetailsDialog
    v-model:visible="showForecastDetailsDialog"
    :forecast="activeForecasts?.[0]"/>

  <DeletionDialog
    v-model:visible="showDeleteForecastDialog"
    :content="activeForecastSelectionText"
    @delete="deleteForecasts"/>
</template>

<style scoped></style>
