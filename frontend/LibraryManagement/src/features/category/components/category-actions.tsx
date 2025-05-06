// src/features/category/components/CategoryActions.tsx
import React from "react";
import { Button } from "@/components/ui/button";
import { CategoryResponse } from "@/features/category/types/type";

interface CategoryActionsProps {
  category: CategoryResponse;
  onEdit: (category: CategoryResponse) => void;
  onDelete: (id: string) => void;
}

export const CategoryActions: React.FC<CategoryActionsProps> = ({
  category,
  onEdit,
  onDelete,
}) => {
  return (
    <div className="flex gap-2">
      <Button size="sm" variant="outline" onClick={() => onEdit(category)}>
        Edit
      </Button>
      <Button
        size="sm"
        variant="destructive"
        onClick={() => onDelete(category.id)}>
        Delete
      </Button>
    </div>
  );
};
