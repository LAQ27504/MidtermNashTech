export enum UserType {
  SuperUser = 0,
  NormalUser = 1,
}

export interface UserRegisterRequest {
  name: string;
  password: string;
  confirmPassword: string;
  type: UserType;
}

export interface UserRegisterRequest {
  name: string;
  password: string;
  confirmPassword: string;
  type: UserType;
}

export interface UserLoginResponse {
  id: string;
  name: string;
  accessToken: string;
  expiresIn: number;
}
