import axios from "axios";
import type { NewForecast } from "@/models/forecasts/newForecast";
import type { SortingData } from "@/models/common/sortingData";
import type { PaginationData } from "@/models/common/paginationData";
import { PaginationUtils } from "@/utils/dataTables/paginationUtils";
import { SortingUtils } from "@/utils/dataTables/sortingUtils";
import type { Forecast } from "@/models/forecasts/forecast";
import type { FilterData } from "@/models/common/filterData";
import { FilteringUtils } from "@/utils/dataTables/filteringUtils";
import type { DeletionResult } from "@/models/common/deletionResult";
import { Result } from "@/services/common/Result";
import { ErrorMapper } from "@/utils/mappers/errorMapper";
import type { PaginatedResult } from "@/models/common/paginatedResult";

class ForecastService {
  private static readonly BASE_ROUTE = "v1/forecasts";

  static async insert(
    forecastToSubmit: NewForecast
  ): Promise<Result> {
    try {
      await axios.post(this.BASE_ROUTE, forecastToSubmit);
      return Result.ok();
    } catch (e: unknown) {
      console.error(e);
      return Result.error(ErrorMapper.mapToError(e));
    }
  }

  static async getAll(
    pagination: PaginationData,
    sorting: SortingData,
    filtering: FilterData
  ): Promise<Result<PaginatedResult<Forecast>>> {
    try {
      const queryParams = [
        PaginationUtils.getUriString(pagination),
        SortingUtils.getUriString(sorting),
        FilteringUtils.getUriString(filtering)
      ].join("&");

      const url = `${this.BASE_ROUTE}?${queryParams}`;
      const response = await axios.get(url);

      return Result.ok(response.data);
    } catch (e: unknown) {
      console.error(e);
      return Result.error(ErrorMapper.mapToError(e));
    }
  }

  static async delete(
    forecasts: Forecast[]
  ): Promise<Result<DeletionResult>> {
    try {
      const idsQuery = forecasts
        .map((f) => `id=${encodeURIComponent(f.id)}`)
        .join("&");

      const url = `${this.BASE_ROUTE}?${idsQuery}`;

      const response = await axios.delete(url);
      return Result.ok(response.data);
    } catch (e: unknown) {
      console.error(e);
      return Result.error(ErrorMapper.mapToError(e));
    }
  }
}

export default ForecastService;
