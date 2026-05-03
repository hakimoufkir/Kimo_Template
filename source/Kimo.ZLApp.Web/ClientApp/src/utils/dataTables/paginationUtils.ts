import type { PaginationData } from "@/models/common/paginationData";

const pageSizes = [10, 25, 50, 100];

function getUriString(pagination: PaginationData): string {
  return `skip=${pagination.skip}&take=${pagination.take}`;
}

export const PaginationUtils = {
  getUriString,
  pageSizes
};
