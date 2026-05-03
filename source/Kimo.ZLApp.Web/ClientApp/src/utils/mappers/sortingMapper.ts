import type { DataTableSortEvent } from "primevue";
import type { SortingData } from "@/models/common/sortingData";

export function mapToSorting(e: DataTableSortEvent): SortingData {
  return {
    field: typeof e.sortField === "string"
      ? e.sortField
      : e.sortField?.toString() ?? "",
    order: e.sortOrder ?? 1
  };
}

export const SortingMapper = {
  mapToSorting
};
