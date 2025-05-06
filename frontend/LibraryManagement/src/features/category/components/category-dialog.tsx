// src/features/category/components/CategoryDialogComponent.tsx
import React from "react";
import {
  Dialog,
  DialogContent,
  DialogHeader,
  DialogTitle,
  DialogDescription,
} from "@/components/ui/dialog";
import { CategoryForm } from "./category-form";
import {
  CategoryRequest,
  CategoryResponse,
} from "@/features/category/types/type";

interface CategoryDialogComponentProps {
  isOpen: boolean;
  onOpenChange: (open: boolean) => void;
  editingCategory: CategoryResponse | null;
  onSubmit: (data: CategoryRequest) => void;
}

export const CategoryDialogComponent: React.FC<
  CategoryDialogComponentProps
> = ({ isOpen, onOpenChange, editingCategory, onSubmit }) => {
  const initialFormData = editingCategory
    ? { name: editingCategory.name }
    : undefined;

  const dialogTitleText = editingCategory ? "Edit Category" : "Add Category";
  const dialogDescriptionText = editingCategory
    ? "Update the name for this category. Click 'Save Changes' when you're done."
    : "Enter the name for the new category. Click 'Create' when you're done.";

  return (
    <Dialog open={isOpen} onOpenChange={onOpenChange}>
      <DialogContent className="max-w-md">
        {" "}
        {/* Adjusted max-width */}
        <DialogHeader>
          <DialogTitle className="text-lg font-semibold">
            {dialogTitleText}
          </DialogTitle>
          <DialogDescription>{dialogDescriptionText}</DialogDescription>
        </DialogHeader>
        <CategoryForm
          initialData={initialFormData}
          onSubmit={onSubmit}
          onCancel={() => onOpenChange(false)}
        />
      </DialogContent>
    </Dialog>
  );
};
