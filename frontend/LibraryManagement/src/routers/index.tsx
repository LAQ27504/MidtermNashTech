import BookRequest from "@/pages/BookRequest";
import Home from "@/pages/Home";
import Layout from "@/pages/Layout";
import LoginPage from "@/pages/Login";
import NotFound from "@/pages/NotFound";
import Register from "@/pages/Register";
import Test from "@/pages/Test";

export const pathName = {
  home: "",
  login: "/login",
  register: "/register",
  bookRequest: "/book-request",
  test: "/test",
};

export const AppRoutes = [
  { path: "*", element: <NotFound /> },
  { path: pathName.login, element: <LoginPage /> },
  { path: pathName.register, element: <Register /> },
  { path: pathName.test, element: <Test /> },
  {
    element: <Layout />,
    children: [
      { path: pathName.home, element: <Home /> },
      { path: pathName.bookRequest, element: <BookRequest /> },
    ],
  },
];
