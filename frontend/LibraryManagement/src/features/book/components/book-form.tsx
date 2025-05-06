// src/features/book/pages/BookForm.tsx (or src/components/BookForm.tsx as per original)
import React, { useEffect, useState } from "react";
import { Input } from "@/components/ui/input";
import { Button } from "@/components/ui/button";
import { BookRequest } from "@/features/book/types/type";
import { GetAllCategories } from "@/features/category/api/categoryApi";

interface Category {
  id: string;
  name: string;
}

interface BookFormProps {
  onSubmit: (data: BookRequest) => void;
  onCancel?: () => void;
  initialData?: BookRequest; // For editing
}

export const BookForm: React.FC<BookFormProps> = ({
  onSubmit,
  onCancel,
  initialData,
}) => {
  const [categories, setCategories] = useState<Category[]>([]);
  const defaultFormState: BookRequest = {
    name: "",
    author: "",
    categoryId: "",
    amount: 0,
  };
  const [form, setForm] = useState<BookRequest>(
    initialData || defaultFormState
  );

  useEffect(() => {
    GetAllCategories()
      .then((res) => setCategories(res.data.data))
      .catch(() => setCategories([]));
  }, []);

  useEffect(() => {
    // Update form when initialData changes (e.g., opening dialog for edit or add)
    setForm(initialData || defaultFormState);
  }, [initialData]);

  const handleChange = (
    e: React.ChangeEvent<HTMLInputElement | HTMLSelectElement>
  ) => {
    const { name, value } = e.target;
    setForm((f) => ({
      ...f,
      [name]: name === "amount" ? Number(value) : value,
    }));
  };

  const handleSubmit = (e: React.FormEvent) => {
    e.preventDefault();
    onSubmit(form);
    // Form reset is handled by initialData effect or dialog close
  };

  const buttonText = initialData ? "Save Changes" : "Create";

  return (
    <form onSubmit={handleSubmit} className="space-y-4 p-6">
      <div className="flex flex-col gap-4">
        <Input
          name="name"
          placeholder="Book Name"
          value={form.name}
          onChange={handleChange}
          required
        />
        <Input
          name="author"
          placeholder="Author"
          value={form.author}
          onChange={handleChange}
          required
        />
        <select
          name="categoryId"
          value={form.categoryId}
          onChange={handleChange}
          className="border rounded px-3 py-2 w-full"
          required>
          <option value="" disabled>
            — Select Category —
          </option>
          {categories.map((c) => (
            <option key={c.id} value={c.id}>
              {c.name}
            </option>
          ))}
        </select>
        <Input
          name="amount"
          type="number"
          min="0" // Good practice for amounts
          placeholder="Amount"
          value={form.amount}
          onChange={handleChange}
          required
        />
      </div>

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
