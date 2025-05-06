import BookRequest from "@/features/bookBorrowRequest/pages/BookRequest";
import Home from "../features/book/pages/Home";
import Layout from "@/pages/Layout";
import LoginPage from "@/features/user/pages/Login";
import NotFound from "@/pages/NotFound";
import Register from "@/features/user/pages/Register";
import Test from "@/pages/Test";
import PrivateRoute from "@/routers/PrivateRoute";
import SuperUserRoute from "@/routers/SuperUserRoute";
import NormalUserRoute from "@/routers/NormalUserRoute";
import { Navigate } from "react-router-dom";
import AccessDenied from "@/pages/AccessDenied";
import BooksPage from "@/features/book/pages/BookPage";
import CategoriesPage from "@/features/category/pages/CategoryPage";
import SuperBookBorrowRequestPage from "@/features/bookBorrowRequest/pages/SuperBookRequestPage";
import NormalBookRequestPage from "@/features/bookBorrowRequest/pages/NormalBookRequesetPage";

export const pathName = {
  home: "/home",
  login: "/login",
  register: "/register",
  accessDenied: "/access-denied",
  books: "/books",
  category: "/categories",
  superBorrowRequest: "/super-borrow-request",
  bookRequest: "/normal-book-request",
  test: "/test",
};

// Public routes accessible to everyone
export const publicRoutes = [
  { path: "*", element: <NotFound /> }, // Catch-all for 404
  { path: pathName.login, element: <LoginPage /> }, // Public route
  { path: pathName.register, element: <Register /> }, // Public route
  { path: pathName.test, element: <Test /> }, // Public route
  { path: pathName.accessDenied, element: <AccessDenied /> }, // Public route
  {
    path: "/", // Explicitly match the root
    element: <Navigate to="/login" replace />, // Redirect to login if not authenticated
  },
  {
    element: (
      <PrivateRoute>
        <Layout />
      </PrivateRoute>
    ),
    children: [
      { path: pathName.home, element: <Home /> }, // Protected route for normal users
    ],
  },
];

// Routes for normal users
export const normalRoutes = {
  element: (
    <PrivateRoute>
      <NormalUserRoute>
        <Layout />
      </NormalUserRoute>
    </PrivateRoute>
  ),
  children: [
    { path: pathName.bookRequest, element: <NormalBookRequestPage /> }, // Protected route for normal users
  ],
};

// Routes for superusers
export const superRoutes = {
  element: (
    <PrivateRoute>
      <SuperUserRoute>
        <Layout />
      </SuperUserRoute>
    </PrivateRoute>
  ),
  children: [
    { path: pathName.books, element: <BooksPage /> }, // Protected route for superusers
    { path: pathName.category, element: <CategoriesPage /> }, // Protected route for superusers
    {
      path: pathName.superBorrowRequest,
      element: <SuperBookBorrowRequestPage />,
    }, // Protected route for superusers
  ],
};

// Combined index routes
export const index = [...publicRoutes, normalRoutes, superRoutes];
