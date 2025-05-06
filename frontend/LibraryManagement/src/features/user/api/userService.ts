import { UserLoginResponse } from "@/features/user/types/type";
import { ENDPOINTS_API } from "../../../api/config";
import { httpClient } from "../../../api/httpClient";

export const login = (payload: { name: string; password: string }) => {
  return httpClient.post<{ data: UserLoginResponse }>(
    ENDPOINTS_API.account.login,
    payload
  );
};

export const register = (id = "") => {
  return httpClient.get(ENDPOINTS_API.account.register);
};
