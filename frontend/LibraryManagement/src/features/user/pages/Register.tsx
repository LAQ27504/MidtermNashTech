import React, { useState } from "react";
import { Input } from "@/components/ui/input";
import { Button } from "@/components/ui/button";
import { Label } from "@/components/ui/label";
import {
  Card,
  CardContent,
  CardFooter,
  CardHeader,
  CardTitle,
} from "@/components/ui/card";
import clsx from "clsx";

const Register: React.FC = () => {
  const [formData, setFormData] = useState({
    name: "",
    email: "",
    password: "",
    confirmPassword: "",
  });

  const [errors, setErrors] = useState({
    name: "",
    email: "",
    password: "",
    confirmPassword: "",
  });

  const validateField = (id: string, value: string) => {
    let error = "";

    switch (id) {
      case "name":
        if (!value) error = "Name is required";
        break;
      case "email":
        if (!value) error = "Email is required";
        else if (!/\S+@\S+\.\S+/.test(value)) error = "Invalid email address";
        break;
      case "password":
        if (!value) error = "Password is required";
        else if (value.length < 6)
          error = "Password must be at least 6 characters";
        break;
      case "confirmPassword":
        if (!value) error = "Confirm Password is required";
        else if (value !== formData.password) error = "Passwords do not match";
        break;
    }

    setErrors((prev) => ({ ...prev, [id]: error }));
  };

  const handleChange = (e: React.ChangeEvent<HTMLInputElement>) => {
    const { id, value } = e.target;
    setFormData((prev) => ({ ...prev, [id]: value }));
    validateField(id, value);
  };

  const validateForm = () => {
    Object.entries(formData).forEach(([id, value]) => {
      validateField(id, value);
    });

    return Object.values(errors).every((error) => !error);
  };

  const handleSubmit = (e: React.FormEvent) => {
    e.preventDefault();
    if (validateForm()) {
      console.log("Form Data:", formData);
    }
  };

  const inputClass = (error: string) =>
    clsx("mt-1", {
      "border-red-500 ring-red-500 focus:ring-red-500": error,
    });

  return (
    <div className="flex justify-center items-center h-screen bg-gray-50 px-4">
      <Card className="w-full max-w-md shadow-xl rounded-2xl">
        <CardHeader>
          <CardTitle className="text-2xl text-center">Register</CardTitle>
        </CardHeader>
        <form onSubmit={handleSubmit} noValidate>
          <CardContent className="space-y-4">
            {["name", "email", "password", "confirmPassword"].map((field) => (
              <div key={field}>
                <Label htmlFor={field} className="capitalize">
                  {field === "confirmPassword" ? "Confirm Password" : field}
                </Label>
                <Input
                  id={field}
                  type={field.includes("password") ? "password" : "text"}
                  value={(formData as any)[field]}
                  onChange={handleChange}
                  className={inputClass((errors as any)[field])}
                />
                {(errors as any)[field] && (
                  <p className="text-red-500 text-sm animate-fade-in">
                    {(errors as any)[field]}
                  </p>
                )}
              </div>
            ))}
          </CardContent>
          <CardFooter>
            <Button type="submit" className="w-full">
              Register
            </Button>
          </CardFooter>
        </form>
      </Card>
    </div>
  );
};

export default Register;
