// src/features/category/api/categoryApi.ts

import {
  CategoryRequest,
  CategoryResponse,
} from "@/features/category/types/type";
import { PaginationRequest, PaginationResponse } from "@/constants/Common";
import { ENDPOINTS_API } from "@/api/config";
import { httpClient } from "@/api/httpClient";

export const CreateCategory = (payload: CategoryRequest) =>
  httpClient.post<{ data: CategoryResponse; message: string }>(
    ENDPOINTS_API.category.create,
    payload
  );

export const CreateManyCategories = (payload: CategoryRequest[]) =>
  httpClient.post<{ data: CategoryResponse[]; message: string }>(
    ENDPOINTS_API.category.createMany,
    payload
  );

export const GetAllCategories = () =>
  httpClient.get<{ data: CategoryResponse[]; message: string }>(
    ENDPOINTS_API.category.getAll
  );

export const GetCategoryById = (id: string) =>
  httpClient.get<{ data: CategoryResponse; message: string }>(
    ENDPOINTS_API.category.getById(id)
  );

export const UpdateCategory = (id: string, payload: CategoryRequest) =>
  httpClient.put<{ data: CategoryResponse; message: string }>(
    ENDPOINTS_API.category.update(id),
    payload
  );

export const DeleteCategory = (id: string) =>
  httpClient.delete<{ message: string }>(ENDPOINTS_API.category.delete(id));

export const GetCategoryPaginated = (params: PaginationRequest) =>
  httpClient.get<{
    data: PaginationResponse<CategoryResponse>;
    message: string;
  }>(ENDPOINTS_API.category.getPaginated(params.pageNumber, params.pageSize));
