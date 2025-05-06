import React from "react";
import { useNavigate } from "react-router-dom";
import { Button } from "@/components/ui/button"; // Import Shadcn Button

const NotFound: React.FC = () => {
  const navigate = useNavigate();

  const handleGoHome = () => {
    navigate("/home");
  };

  return (
    <div style={{ textAlign: "center", marginTop: "50px" }}>
      <h1>404 - Page Not Found</h1>
      <p>The page you are looking for does not exist.</p>
      <Button
        onClick={handleGoHome}
        size="lg" // optional, to adjust the button size
        style={{ marginTop: "20px" }}>
        Go to Home
      </Button>
    </div>
  );
};

export default NotFound;
