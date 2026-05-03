export interface PaginatedResult<TData> {
  data: TData[];
  totalRecords: number;
}
