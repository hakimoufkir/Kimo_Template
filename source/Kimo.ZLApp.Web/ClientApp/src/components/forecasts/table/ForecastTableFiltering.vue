<script lang="ts" setup>
import {DatePicker} from "primevue";
import LocationMultiSelect from "@/components/locations/common/LocationMultiSelect.vue";
import {useI18n} from "vue-i18n";

const {t} = useI18n();

const emit = defineEmits(["locationChange", "dateChange"]);
</script>

<template>
  <div class="flex flex-wrap gap-4">
    <LocationMultiSelect
      class="w-[18rem]"
      @change="emit('locationChange', $event)"/>

    <DatePicker
      :manualInput="false"
      :placeholder="t('weatherForecasts.selectDates')"
      class="w-[18rem]"
      icon-display="input"
      selectionMode="multiple"
      show-button-bar
      showIcon
      @valueChange="emit('dateChange', $event ?? [])"/>
  </div>
</template>

<style scoped></style>
