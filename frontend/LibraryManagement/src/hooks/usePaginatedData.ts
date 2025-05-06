import { useEffect, useState } from "react";
import { PaginationRequest, PaginationResponse } from "@/constants/Common";

interface UsePaginatedDataProps<T> {
  fetchData: (params: PaginationRequest) => Promise<PaginationResponse<T>>;
  initialPageSize?: number;
}

export function usePaginatedData<T>({
  fetchData,
  initialPageSize = 10,
}: UsePaginatedDataProps<T>) {
  const [data, setData] = useState<T[]>([]);
  const [totalCount, setTotalCount] = useState(0);
  const [pageNumber, setPageNumber] = useState(1);
  const [pageSize, setPageSize] = useState(initialPageSize);
  const totalPages = Math.ceil(totalCount / pageSize);

  useEffect(() => {
    const load = async () => {
      const response = await fetchData({ pageNumber, pageSize });
      setData(response.items);
      setTotalCount(response.totalCount);
    };
    load();
  }, [pageNumber, pageSize]);

  return {
    data,
    pageNumber,
    pageSize,
    totalPages,
    setPageNumber,
    setPageSize,
    setData,
  };
}
