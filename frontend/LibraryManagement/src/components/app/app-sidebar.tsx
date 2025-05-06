// src/components/app-sidebar.tsx
import { Link, useLocation, useNavigate } from "react-router-dom"; // Import useNavigate
import { cn } from "@/lib/utils";
import { Button, buttonVariants } from "@/components/ui/button";
import {
  BookText,
  ListOrdered,
  BookMarked,
  Folder,
  ListChecks,
  LogOut,
} from "lucide-react"; // Import LogOut icon
import { useAuthContext } from "@/context/authContext";
import { useUserContext } from "@/context/userContext";

// Define your navigation items
const navNormalItems = [
  { href: "/home", label: "Borrow Book", icon: BookText },
  { href: "/normal-book-request", label: "Borrow Request", icon: ListOrdered },
  // Add more pages as needed
];

const navItemsSuperUser = [
  { href: "/home", label: "Borrow Book", icon: BookText },
  { href: "/books", label: "Books", icon: BookMarked },
  { href: "/categories", label: "Categoies", icon: Folder },
  { href: "/super-borrow-request", label: "Request Approve", icon: ListChecks },
  // Add more pages as needed
];

export function AppSidebar() {
  const location = useLocation();
  const navigate = useNavigate(); // Initialize useNavigate
  const { isSuperUser } = useUserContext(); // Using context for role
  const navItems = isSuperUser ? navItemsSuperUser : navNormalItems;

  const { isAuthenticated, setIsAuthenticated } = useAuthContext();

  const handleLogout = () => {
    if (!isAuthenticated) navigate("/login");

    localStorage.removeItem("token");
    localStorage.removeItem("tokenExpiry");
    setIsAuthenticated(false);
    console.log("User logged out");
  };

  return (
    <aside
      className="w-64 border-r bg-background p-4 flex flex-col fixed h-full"
      key={isSuperUser ? "super" : "normal"}>
      {/* Optional: Add a logo or title */}
      <div className="mb-4 px-3 py-2">
        <h2 className="mb-2 px-4 text-lg font-semibold tracking-tight">
          Library Management
        </h2>
      </div>

      <nav className="flex flex-col space-y-1 flex-grow">
        {" "}
        {/* Added flex-grow here */}
        {navItems.map((item) => (
          <Link
            key={item.href}
            to={item.href}
            className={cn(
              buttonVariants({ variant: "ghost" }),
              location.pathname === item.href
                ? "bg-muted hover:bg-muted"
                : "hover:bg-transparent hover:underline",
              "justify-start"
            )}>
            <item.icon className="mr-2 h-4 w-4" />
            {item.label}
          </Link>
        ))}
      </nav>

      {/* Logout Button Section */}
      <div className="mt-auto pt-4">
        {" "}
        {/* Added pt-4 for some spacing */}
        <Button
          variant="ghost"
          className="w-full justify-start"
          onClick={handleLogout}>
          <LogOut className="mr-2 h-4 w-4" />
          Logout
        </Button>
      </div>
    </aside>
  );
}
