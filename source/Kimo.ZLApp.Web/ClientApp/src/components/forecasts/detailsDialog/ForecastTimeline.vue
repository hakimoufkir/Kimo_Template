<script lang="ts" setup>
import {Timeline} from "primevue";
import {computed} from "vue";
import type {Forecast} from "@/models/forecasts/forecast";

const props = defineProps<{ forecast: Forecast }>();

type MergedData = {
  hour: number;
  value: number;
  type: "temperature" | "precipitation";
};

const mergedForecastData = computed<MergedData[]>(() => {
  const temperatureItems: MergedData[] = props.forecast.temperatureData.map(
    (t) => ({
      hour: t.hour,
      value: t.value,
      type: "temperature",
    })
  );

  const precipitationItems: MergedData[] = props.forecast.precipitationData.map(
    (p) => ({
      hour: p.hour,
      value: p.value,
      type: "precipitation",
    })
  );

  return [...temperatureItems, ...precipitationItems].sort(
    (a, b) => a.hour - b.hour
  );
});
</script>

<template>
  <Timeline :value="mergedForecastData" class="w-100 mt-5">
    <template #marker="slotProps">
      <span
        :class="{
          'bg-yellow-500': slotProps.item.type === 'temperature',
          'bg-cyan-500': slotProps.item.type === 'precipitation',
        }"
        class="flex w-8 h-8 items-center justify-center text-white rounded-full z-10 shadow text-surface-0">
        <i
          :class="
            slotProps.item.type === 'temperature' ? 'pi pi-sun' : 'pi pi-cloud'
          "></i>
      </span>
    </template>
    <template #opposite="slotProps">
      <span class="p-text-secondary"> {{ slotProps.item.hour }}:00 </span>
    </template>
    <template #content="slotProps">
      <span>
        {{ slotProps.item.value }}
        {{ slotProps.item.type === "temperature" ? "°C" : "%" }}
      </span>
    </template>
    <template #connector>
      <div class="p-timeline-event-connector"></div>
    </template>
  </Timeline>
</template>

<style scoped></style>
