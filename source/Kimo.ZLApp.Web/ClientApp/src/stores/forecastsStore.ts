import { defineStore } from "pinia";
import { ref } from "vue";
import ForecastService from "@/services/forecastService";
import type { PaginatedResult } from "@/models/common/paginatedResult";
import type { PaginationData } from "@/models/common/paginationData";
import type { SortingData } from "@/models/common/sortingData";
import type { FilterData } from "@/models/common/filterData";
import type { Forecast } from "@/models/forecasts/forecast";

export const useForecastsStore = defineStore("forecasts", () => {
  const data = ref<PaginatedResult<Forecast>>({
    data: [],
    totalRecords: 0
  });
  const isLoading = ref(false);
  const error = ref<Error | undefined>();
  const pagination = ref<PaginationData>({
    skip: 0,
    take: 0
  });
  const sorting = ref<SortingData>({
    field: "",
    order: 0
  });
  const filtering = ref<FilterData>({
    filters: new Map()
  });

  async function loadData(): Promise<void> {
    isLoading.value = true;
    error.value = undefined;

    const forecastsResult = await ForecastService.getAll(
      pagination.value,
      sorting.value,
      filtering.value);

    if (forecastsResult.success && forecastsResult.data) {
      data.value = forecastsResult.data;
    }
    else {
      data.value = {
        data: [],
        totalRecords: 0
      };

      error.value = forecastsResult.error;
    }

    isLoading.value = false;
  }

  return {
    data,
    isLoading,
    error,
    pagination,
    sorting,
    filtering,
    loadData
  };
});
