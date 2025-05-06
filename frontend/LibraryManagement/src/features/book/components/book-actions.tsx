// src/features/book/components/BookActions.tsx
import React from "react";
import { Button } from "@/components/ui/button";
import { BookResponse } from "@/features/book/types/type";

interface BookActionsProps {
  book: BookResponse;
  onEdit: (book: BookResponse) => void;
  onDelete: (id: string) => void;
}

export const BookActions: React.FC<BookActionsProps> = ({
  book,
  onEdit,
  onDelete,
}) => {
  return (
    <div className="flex gap-2">
      <Button size="sm" variant="outline" onClick={() => onEdit(book)}>
        Edit
      </Button>
      <Button size="sm" variant="destructive" onClick={() => onDelete(book.id)}>
        Delete
      </Button>
    </div>
  );
};
