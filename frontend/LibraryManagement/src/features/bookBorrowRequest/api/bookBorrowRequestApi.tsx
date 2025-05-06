import {
  BorrowRequest,
  ReturnRequest,
  ApprovalRequest,
  BorrowResponse,
} from "@/features/bookBorrowRequest/types/type";
import { ENDPOINTS_API } from "@/api/config";
import { httpClient } from "@/api/httpClient";
import { PaginationRequest, PaginationResponse } from "@/constants/Common";

export const CreateBorrowRequest = (payload: BorrowRequest) => {
  return httpClient.post<{ data: BorrowResponse; message: string }>(
    ENDPOINTS_API.bookBorrowingRequest.create,
    payload // Send the adapted payload
  );
};

export const GetAllBorrowRequests = () => {
  return httpClient.get<{ data: BorrowResponse[]; message: string }>(
    ENDPOINTS_API.bookBorrowingRequest.getAll
  );
};

export const GetBorrowRequestById = (id: string) => {
  return httpClient.get<{ data: BorrowResponse; message: string }>(
    ENDPOINTS_API.bookBorrowingRequest.getById(id)
  );
};

export const GetBorrowRequestsByUser = (requestorId: string) => {
  return httpClient.get<{ data: BorrowResponse[]; message: string }>(
    ENDPOINTS_API.bookBorrowingRequest.getByUser(requestorId)
  );
};

export const ReturnBooks = (payload: ReturnRequest) => {
  return httpClient.post<{ data: BorrowResponse; message: string }>(
    ENDPOINTS_API.bookBorrowingRequest.return,
    payload
  );
};

export const SetRequestApproval = (payload: ApprovalRequest) => {
  return httpClient.post<any>(
    ENDPOINTS_API.bookBorrowingRequest.approve,
    payload
  );
};

export const GetRequestPaginatedUserId = (
  pagination: PaginationRequest,
  requestorId: string
) => {
  return httpClient.get<{ data: PaginationResponse<BorrowResponse> }>(
    ENDPOINTS_API.bookBorrowingRequest.getPaginatedUserId(
      pagination.pageNumber,
      pagination.pageSize,
      requestorId
    )
  );
};

export const GetRequestPaginated = (pagination: PaginationRequest) => {
  return httpClient.get<{ data: PaginationResponse<BorrowResponse> }>(
    ENDPOINTS_API.bookBorrowingRequest.getPaginated(
      pagination.pageNumber,
      pagination.pageSize
    )
  );
};
