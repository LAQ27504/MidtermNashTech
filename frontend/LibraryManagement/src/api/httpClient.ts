import axios from "axios";
import { ROOT_API } from "./config";

const axiosInstance = axios.create(ROOT_API);

axiosInstance.interceptors.request.use(
  (config) => {
    const token = localStorage.getItem("token");

    const isAuthRoute =
      config.url?.includes("Account/login") ||
      config.url?.includes("Account/register");

    if (token && !isAuthRoute) {
      config.headers["Authorization"] = `Bearer ${token}`;
    }

    return config;
  },
  (error) => Promise.reject(error)
);

axiosInstance.interceptors.response.use(
  (response) => {
    return response;
  },
  (error) => {
    if (error.response) {
      // Handle specific HTTP errors
      if (error.response.status === 401) {
        console.error("Unauthorized access - possibly invalid token");
        // Optionally, redirect to login or clear token
      } else if (error.response.status === 403) {
        console.error(
          "Forbidden - you don't have permission to access this resource"
        );
      } else if (error.response.status === 404) {
        console.error("Resource not found");
      } else {
        console.error(`HTTP error: ${error.response.status}`);
      }
    } else if (error.request) {
      console.error("No response received from server");
    } else {
      console.error("Error setting up request:", error.message);
    }
    return Promise.reject(error);
  }
);

export const httpClient = axiosInstance;
