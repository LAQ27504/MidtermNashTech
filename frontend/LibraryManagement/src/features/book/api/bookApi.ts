// src/features/book/api/bookApi.ts

import { BookRequest, BookResponse } from "@/features/book/types/type";
import { PaginationRequest, PaginationResponse } from "@/constants/Common";
import { ENDPOINTS_API } from "@/api/config";
import { httpClient } from "@/api/httpClient";

export const CreateBook = (book: BookRequest) => {
  return httpClient.post<{ data: BookResponse; message: string }>(
    ENDPOINTS_API.book.create,
    book
  );
};

export const GetBookList = () => {
  return httpClient.get<{ data: BookResponse[]; message: string }>(
    ENDPOINTS_API.book.getAll
  );
};

export const GetBookById = (id: string) => {
  return httpClient.get<{ data: BookResponse; message: string }>(
    ENDPOINTS_API.book.getById(id)
  );
};

export const UpdateBook = (id: string, book: BookRequest) => {
  return httpClient.put<{ data: BookResponse; message: string }>(
    ENDPOINTS_API.book.update(id),
    book
  );
};

export const DeleteBook = (id: string) => {
  return httpClient.delete<{ message: string }>(ENDPOINTS_API.book.delete(id));
};

export const GetBookPaginated = (pagination: PaginationRequest) => {
  return httpClient.get<{
    data: PaginationResponse<BookResponse>;
    message: string;
  }>(
    ENDPOINTS_API.book.getPaginated(pagination.pageNumber, pagination.pageSize)
  );
};
