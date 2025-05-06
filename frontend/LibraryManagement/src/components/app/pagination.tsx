import { useEffect, useState } from "react";
import { Button } from "@/components/ui/button";
import { PaginationResponse } from "@/constants/Common";

interface PaginationProps<T> {
  fetchData: (
    pageNumber: number,
    pageSize: number
  ) => Promise<PaginationResponse<T>>;
  onDataFetched: (items: T[], totalCount: number) => void;
  defaultPageSize?: number;
}

export function Pagination<T>({
  fetchData,
  onDataFetched,
  defaultPageSize = 10,
}: PaginationProps<T>) {
  const [currentPage, setCurrentPage] = useState(1);
  const [recordsPerPage, setRecordsPerPage] = useState(defaultPageSize);
  const [totalPages, setTotalPages] = useState(1);

  const loadData = async (page: number, size: number) => {
    const data = await fetchData(page, size);
    onDataFetched(data.items, data.totalCount);
    setTotalPages(Math.ceil(data.totalCount / size));
  };

  useEffect(() => {
    loadData(currentPage, recordsPerPage);
  }, [currentPage, recordsPerPage]);

  return (
    <div className="flex items-center justify-between w-full gap-4">
      {/* Records per page */}
      <div className="flex items-center gap-2">
        <span>Show</span>
        <select
          className="border rounded px-2 py-1"
          value={recordsPerPage}
          onChange={(e) => {
            const newSize = Number(e.target.value);
            setRecordsPerPage(newSize);
            setCurrentPage(1); // Reset to first page
          }}>
          {[1, 5, 10, 20, 50].map((n) => (
            <option key={n} value={n}>
              {n}
            </option>
          ))}
        </select>
        <span>records per page</span>
      </div>

      {/* Page controls */}
      <div className="flex items-center gap-2">
        <Button
          variant="ghost"
          disabled={currentPage === 1}
          onClick={() => setCurrentPage(currentPage - 1)}>
          Previous
        </Button>
        <span>
          Page {currentPage} of {totalPages}
        </span>
        <Button
          variant="ghost"
          disabled={currentPage === totalPages}
          onClick={() => setCurrentPage(currentPage + 1)}>
          Next
        </Button>
      </div>
    </div>
  );
}
