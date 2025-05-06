// src/layouts/layout.tsx
import { Outlet } from "react-router-dom";
import { AppSidebar } from "@/components/app/app-sidebar"; // Adjust path if needed

export default function Layout() {
  return (
    <div className="flex min-h-screen">
      {/* Sidebar */}
      <AppSidebar />

      <main className="flex-1 p-6 ml-64">
        <Outlet />
      </main>
    </div>
  );
}
