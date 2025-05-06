import { get } from "http";

export const ROOT_API = {
  baseURL: "http://localhost:5213/",
  headers: {
    "Content-Type": "application/json",
  },
};

export const ENDPOINTS_API = {
  account: {
    login: "Account/login",
    register: "Account/register",
  },
  book: {
    create: "Book",
    getAll: "Book",
    createMany: "Book/multi",
    getById: (id: any) => `Book/${id}`,
    update: (id: any) => `Book/${id}`,
    delete: (id: any) => `Book/${id}`,
    getPaginated: (page: any, size: any) => `Book/${page}/${size}`,
  },
  bookBorrowingRequest: {
    create: "BookBorrowingRequest",
    getAll: "BookBorrowingRequest",
    getById: (id: string) => `BookBorrowingRequest/${id}`,
    getByUser: (requestorId: string) =>
      `BookBorrowingRequest/user/${requestorId}`,
    return: "BookBorrowingRequest/return",
    approve: "BookBorrowingRequest/approve",
    getPaginatedUserId: (page: number, size: number, requestorId: string) =>
      `BookBorrowingRequest/${page}/${size}/${requestorId}`,
    getPaginated: (page: number, size: number) =>
      `BookBorrowingRequest/waiting/${page}/${size}`,
  },
  category: {
    create: "Category",
    getAll: "Category",
    createMany: "Category/multi",
    getById: (id: string) => `Category/${id}`,
    update: (id: string) => `Category/${id}`,
    delete: (id: string) => `Category/${id}`,
    getPaginated: (page: number, size: number) => `Category/${page}/${size}`,
  },
};
