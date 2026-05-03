<script lang="ts" setup>
import {Button, DatePicker, Dialog, InputNumber} from "primevue";
import {onMounted} from "vue";
import {useI18n} from "vue-i18n";
import FormField from "@/components/common/FormField.vue";
import LocationSelect from "@/components/locations/common/LocationSelect.vue";
import {useInsertForecastForm} from "@/composables/forecasts/insertForecastForm";
import {DateUtils} from "@/utils/dateUtils";

const {t} = useI18n();

defineProps<{ visible: boolean }>();
const emit = defineEmits(["update:visible", "insert"]);

function changeVisibility(value: boolean): void {
  emit("update:visible", value);
}

const {
  precipitation,
  temperature,
  timestamp,
  location,
  errors,
  meta,
  resetForm,
  createForecast,
} = useInsertForecastForm();

onMounted(() => {
  resetForm();
});

async function submitNewForecast(): Promise<void> {
  const forecast = await createForecast();
  if (!forecast) return;

  emit("insert", forecast);
  changeVisibility(false);
}

function hideDialog(): void {
  resetForm();
}
</script>

<template>
  <Dialog
    :draggable="false"
    :header="t('weatherForecasts.insert')"
    :visible="visible"
    maximizable
    modal
    @hide="hideDialog"
    @update:visible="changeVisibility($event)">
    <div class="flex flex-col gap-4">
      <FormField
        :error="errors.location"
        :label="t('locations.location')"
        required>
        <LocationSelect v-model="location" :error="errors.location"/>
      </FormField>

      <FormField
        :error="errors.timestamp"
        :label="t('weatherForecasts.date')"
        required>
        <DatePicker
          v-model="timestamp"
          :invalid="!!errors.timestamp"
          :min-date="DateUtils.tomorrow()"
          :placeholder="t('weatherForecasts.selectFutureDate')"
          fluid
          hourFormat="24"
          iconDisplay="input"
          showTime/>
      </FormField>

      <FormField
        :error="errors.temperature"
        :label="t('weatherForecasts.temperature')">
        <InputNumber
          v-model="temperature"
          :invalid="!!errors.temperature"
          :max="100"
          :maxFractionDigits="1"
          :min="-100"
          :minFractionDigits="1"
          fluid
          placeholder="- °C"
          showButtons
          suffix=" °C"/>
      </FormField>

      <FormField
        :error="errors.precipitation"
        :label="t('weatherForecasts.precipitation')">
        <InputNumber
          v-model="precipitation"
          :invalid="!!errors.precipitation"
          :max="100"
          :maxFractionDigits="1"
          :min="0"
          :minFractionDigits="1"
          fluid
          placeholder="- %"
          showButtons
          suffix=" %"/>
      </FormField>
    </div>

    <div class="flex justify-end gap-4 mt-6">
      <Button
        :disabled="!meta.valid"
        :label="t('common.save')"
        icon="pi pi-save"
        @click="submitNewForecast"/>
    </div>
  </Dialog>
</template>

<style scoped></style>
