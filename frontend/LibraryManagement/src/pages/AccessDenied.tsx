import React from "react";
import { useNavigate } from "react-router-dom";
import { Button } from "@/components/ui/button"; // Import Shadcn Button

const AccessDenied: React.FC = () => {
  const navigate = useNavigate();

  const handleGoHome = () => {
    navigate("/home");
  };

  return (
    <div style={{ textAlign: "center", marginTop: "20vh" }}>
      <h1>🚫 Access Denied</h1>
      <p>You do not have permission to view this page.</p>
      <Button
        onClick={handleGoHome}
        size="lg" // optional, to adjust the button size
        style={{ marginTop: "20px" }}>
        Go Home
      </Button>
    </div>
  );
};

export default AccessDenied;
