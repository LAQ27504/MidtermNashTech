// src/features/book/components/BookDialogComponent.tsx
import React from "react";
import { Dialog, DialogContent, DialogTitle } from "@/components/ui/dialog";
import { BookForm } from "@/features/book/components/book-form"; // Adjust path if BookForm is elsewhere
import { BookRequest, BookResponse } from "@/features/book/types/type";

interface BookDialogComponentProps {
  isOpen: boolean;
  onOpenChange: (open: boolean) => void;
  editingBook: BookResponse | null;
  onSubmit: (data: BookRequest) => void;
}

export const BookDialogComponent: React.FC<BookDialogComponentProps> = ({
  isOpen,
  onOpenChange,
  editingBook,
  onSubmit,
}) => {
  const initialFormData = editingBook
    ? {
        name: editingBook.name,
        author: editingBook.author,
        categoryId: editingBook.categoryId,
        amount: editingBook.amount,
      }
    : undefined; // BookForm will use its default empty state

  return (
    <Dialog open={isOpen} onOpenChange={onOpenChange}>
      <DialogContent className="max-w-lg">
        <DialogTitle className="text-lg font-semibold">
          {editingBook ? "Edit Book" : "Add Book"}
        </DialogTitle>
        <BookForm
          initialData={initialFormData}
          onSubmit={onSubmit}
          onCancel={() => onOpenChange(false)}
        />
      </DialogContent>
    </Dialog>
  );
};
