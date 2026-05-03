<script lang="ts" setup>
import {Dialog} from "primevue";
import {useI18n} from "vue-i18n";
import type {Forecast} from "@/models/forecasts/forecast";
import {formatDate} from "@/utils/format/dateFormatter";
import ForecastTimeline from "@/components/forecasts/detailsDialog/ForecastTimeline.vue";

const {t} = useI18n();

defineProps<{ visible: boolean; forecast: Forecast }>();
const emit = defineEmits(["update:visible"]);

function changeVisibility(value: boolean): void {
  emit("update:visible", value);
}
</script>

<template>
  <Dialog
    :draggable="false"
    :header="t('weatherForecasts.details')"
    :visible="visible"
    maximizable
    modal
    @update:visible="changeVisibility($event)">
    <h3>{{ forecast.location.name }}</h3>
    <p>
      {{ formatDate(forecast.date) }}
    </p>
    <ForecastTimeline :forecast="forecast"/>
  </Dialog>
</template>

<style scoped></style>
