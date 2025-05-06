import { Pagination } from "@/components/app/pagination";
import { PaginationResponse } from "@/constants/Common";

interface TableFooterProps<T> {
  fetchData: (
    pageNumber: number,
    pageSize: number
  ) => Promise<PaginationResponse<T>>;
  onDataFetched: (items: T[], totalCount: number) => void;
  defaultPageSize?: number;
}

export function TableFooter<T>({
  fetchData,
  onDataFetched,
  defaultPageSize = 10,
}: TableFooterProps<T>) {
  return (
    <div className="flex flex-col md:flex-row justify-between items-center gap-4">
      <Pagination
        fetchData={fetchData}
        onDataFetched={onDataFetched}
        defaultPageSize={defaultPageSize}
      />
    </div>
  );
}
