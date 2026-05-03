<script lang="ts" setup>
import {formatDate, normalizeDateOnly} from "@/utils/format/dateFormatter";
import {Button, Column, DataTable, type DataTablePageEvent, type DataTableSortEvent} from "primevue";
import type {Forecast} from "@/models/forecasts/forecast";
import type {Location} from "@/models/locations/location";
import {useI18n} from "vue-i18n";
import {onMounted, ref} from "vue";
import {useForecastsStore} from "@/stores/forecastsStore";
import ForecastTableActions from "@/components/forecasts/table/ForecastTableActions.vue";
import ForecastTableFiltering from "@/components/forecasts/table/ForecastTableFiltering.vue";
import {PaginationUtils} from "@/utils/dataTables/paginationUtils";
import {SortingMapper} from "@/utils/mappers/sortingMapper";
import {PaginationMapper} from "@/utils/mappers/paginationMapper";

const {t} = useI18n();
const forecastsStore = useForecastsStore();

const selectedForecasts = ref([]);

const emit = defineEmits(["details", "insert", "delete"]);

onMounted(async () => {
  forecastsStore.pagination.skip = 0;
  forecastsStore.pagination.take = PaginationUtils.pageSizes[0];
  forecastsStore.sorting.field = "location";
  forecastsStore.sorting.order = 1;

  await forecastsStore.loadData();
});

function showInsertDialog(): void {
  emit("insert");
}

function showDetailsDialog(forecast: Forecast): void {
  emit("details", forecast);
}

function showDeleteDialog(forecasts: Forecast[]): void {
  emit("delete", forecasts);
}

async function onSortChange(e: DataTableSortEvent): Promise<void> {
  forecastsStore.sorting = SortingMapper.mapToSorting(e);
  await forecastsStore.loadData();
}

async function onPageChange(e: DataTablePageEvent): Promise<void> {
  forecastsStore.pagination = PaginationMapper.mapToPagination(e);
  await forecastsStore.loadData();
}

async function onSelectedLocationsChange(locations: Location[]): Promise<void> {
  forecastsStore.filtering.filters.set(
    "locationId",
    locations.map((i) => i.id).join(",")
  );

  await forecastsStore.loadData();
}

async function onFilterDateChange(dates: Date[]): Promise<void> {
  forecastsStore.filtering.filters.set(
    "date",
    dates.map((i) => normalizeDateOnly(i)).join(",")
  );

  await forecastsStore.loadData();
}
</script>

<template>
  <DataTable
    v-model:selection="selectedForecasts"
    :first="forecastsStore.pagination.skip"
    :lazy="true"
    :loading="forecastsStore.isLoading"
    :rows="forecastsStore.pagination.take"
    :rows-per-page-options="PaginationUtils.pageSizes"
    :sort-field="forecastsStore.sorting.field"
    :sort-order="forecastsStore.sorting.order"
    :total-records="forecastsStore.data.totalRecords"
    :value="forecastsStore.data.data"
    currentPageReportTemplate="{first} bis {last} von {totalRecords}"
    paginator
    paginatorTemplate="FirstPageLink PrevPageLink CurrentPageReport NextPageLink LastPageLink RowsPerPageDropdown"
    show-gridlines
    striped-rows
    @page="onPageChange($event)"
    @sort="onSortChange($event)">
    <template #header>
      <div class="flex flex-col flex-wrap gap-4 justify-between">
        <ForecastTableFiltering
          @dateChange="onFilterDateChange"
          @locationChange="onSelectedLocationsChange"/>

        <ForecastTableActions
          :selectedForecasts="selectedForecasts"
          @delete="showDeleteDialog"
          @insert="showInsertDialog"/>
      </div>
    </template>

    <template #empty>{{ t("common.noEntries") }}</template>

    <Column headerStyle="width: 3rem" selectionMode="multiple"></Column>

    <Column :header="t('locations.location')" :sortable="true" field="location">
      <template #body="slotProps">
        {{ slotProps.data.location.name }}
      </template>
    </Column>

    <Column selectionMode="single">
      <template #body="slotProps">
        <Button
          :tooltip="t('common.details')"
          icon="pi pi-info-circle"
          rounded
          text
          @click="showDetailsDialog(slotProps.data)"/>

        <Button
          :tooltip="t('common.delete')"
          icon="pi pi-trash"
          rounded
          severity="danger"
          text
          @click="showDeleteDialog([slotProps.data])"/>
      </template>
    </Column>

    <Column :header="t('weatherForecasts.date')" :sortable="true" field="date">
      <template #body="slotProps">
        {{ formatDate(slotProps.data.date) }}
      </template>
    </Column>
  </DataTable>
</template>

<style scoped></style>
