// src/features/category/components/CategoryForm.tsx
import React, { useEffect, useState } from "react";
import { Input } from "@/components/ui/input";
import { Button } from "@/components/ui/button";
import { CategoryRequest } from "@/features/category/types/type";

interface CategoryFormProps {
  onSubmit: (data: CategoryRequest) => void;
  onCancel?: () => void;
  initialData?: CategoryRequest; // For editing
}

export const CategoryForm: React.FC<CategoryFormProps> = ({
  onSubmit,
  onCancel,
  initialData,
}) => {
  const defaultFormState: CategoryRequest = {
    name: "",
  };
  const [form, setForm] = useState<CategoryRequest>(
    initialData || defaultFormState
  );

  useEffect(() => {
    // Update form when initialData changes
    setForm(initialData || defaultFormState);
  }, [initialData]);

  const handleChange = (e: React.ChangeEvent<HTMLInputElement>) => {
    const { name, value } = e.target;
    setForm((f) => ({
      ...f,
      [name]: value,
    }));
  };

  const handleSubmit = (e: React.FormEvent) => {
    e.preventDefault();
    if (!form.name.trim()) {
      // Basic validation, can be expanded
      alert("Category name cannot be empty.");
      return;
    }
    onSubmit(form);
  };

  const buttonText = initialData ? "Save Changes" : "Create";

  return (
    <form onSubmit={handleSubmit} className="space-y-4 pt-2 pb-4 px-1">
      {" "}
      {/* Adjusted padding */}
      <Input
        name="name"
        placeholder="Category Name"
        value={form.name}
        onChange={handleChange}
        required
        autoFocus
      />
      <div className="flex justify-end gap-2 pt-4">
        {onCancel && (
          <Button variant="secondary" onClick={onCancel} type="button">
            Cancel
          </Button>
        )}
        <Button type="submit">{buttonText}</Button>
      </div>
    </form>
  );
};
