import { jwtDecode } from "jwt-decode";

interface DecodedToken {
  nameid: string;
  name: string;
  role: string;
  exp: number;
}

export function isTokenValid(): boolean {
  const token = localStorage.getItem("token");
  const expiry = localStorage.getItem("tokenExpiry");
  if (!token || !expiry) return false;

  return Date.now() < parseInt(expiry, 10);
}

export function clearAuth() {
  localStorage.removeItem("token");
  localStorage.removeItem("tokenExpiry");
}

export function getUserFromToken(token: string): DecodedToken | null {
  try {
    const decoded = jwtDecode<DecodedToken>(token);
    return decoded;
  } catch (err) {
    console.error("Invalid token", err);
    return null;
  }
}
