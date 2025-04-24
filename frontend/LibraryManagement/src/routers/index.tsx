import LoginPage from "@/pages/Login";
import NotFound from "@/pages/NotFound";

export const pathName = {
  home: "",
  login: "/login",
};

export const AppRoutes = [
  { path: "*", element: <NotFound /> },
  { path: pathName.login, element: <LoginPage /> },
];
