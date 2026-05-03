import type { SortingData } from "@/models/common/sortingData";

function getUriString(sorting: SortingData): string {
  const direction = sorting.order === 1 ? "asc" : "desc";
  return `sort=${encodeURIComponent(sorting.field)}&order=${direction}`;
}

export const SortingUtils = {
  getUriString
};
