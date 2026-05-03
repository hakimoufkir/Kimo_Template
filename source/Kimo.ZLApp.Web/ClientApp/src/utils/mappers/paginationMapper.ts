import type { DataTablePageEvent } from "primevue";
import type { PaginationData } from "@/models/common/paginationData";

export function mapToPagination(e: DataTablePageEvent): PaginationData {
  return {
    skip: e.first,
    take: e.rows
  };
}

export const PaginationMapper = {
  mapToPagination
};
