export interface PaginationRequest {
  pageNumber: number;
  pageSize: number;
}

export interface PaginationResponse<T> {
  totalCount: number;
  pageNumber: number;
  pageSize: number;
  items: T[];
}
