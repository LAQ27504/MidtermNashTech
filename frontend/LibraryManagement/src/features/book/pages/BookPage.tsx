// src/pages/BooksPage.tsx (or your main page file)
import React, { useState } from "react";
import { Button } from "@/components/ui/button";
import {
  CreateBook,
  GetBookPaginated,
  UpdateBook,
  DeleteBook,
} from "@/features/book/api/bookApi";
import { BookRequest, BookResponse } from "@/features/book/types/type";
import { PaginationRequest, PaginationResponse } from "@/constants/Common";
import { TableFooter } from "@/components/app/table-footer";

// Import new components (adjust paths as necessary)
import { BooksTable } from "@/features/book/components/book-admin-table";
import { BookDialogComponent } from "@/features/book/components/book-dialog";

export default function BooksPage() {
  const [books, setBooks] = useState<BookResponse[]>([]);
  const [isDialogOpen, setIsDialogOpen] = useState(false);
  const [editingBook, setEditingBook] = useState<BookResponse | null>(null);

  const defaultPageSize = 10;

  const handleSaveBook = async (formData: BookRequest) => {
    if (editingBook) {
      await UpdateBook(editingBook.id, formData);
    } else {
      await CreateBook(formData);
    }
    setIsDialogOpen(false);
    setEditingBook(null);
  };

  const handleDeleteBook = async (id: string) => {
    await DeleteBook(id);
  };

  const openAddDialog = () => {
    setEditingBook(null); // Clear any editing state
    setIsDialogOpen(true);
  };

  const openEditDialog = (book: BookResponse) => {
    setEditingBook(book);
    setIsDialogOpen(true);
  };

  const bookTableHeaders = [
    "Name",
    "Author",
    "Category",
    "Amount",
    "Available",
    "Actions",
  ];

  // Passed to BookTableFooter for data fetching
  const fetchBooksData = async (
    pageNumber: number,
    pageSize: number
  ): Promise<PaginationResponse<BookResponse>> => {
    const req: PaginationRequest = { pageNumber, pageSize };
    const res = await GetBookPaginated(req);
    return res.data.data; // Assuming BookTableFooter expects this shape
  };

  // Called by BookTableFooter after data is fetched
  const handleDataFetchedFromFooter = (
    fetchedBooks: BookResponse[],
    totalItems: number
  ) => {
    setBooks(fetchedBooks);
    // totalItems can be used if needed elsewhere, e.g. for displaying "X of Y items"
  };

  return (
    <div className="p-6 space-y-6">
      {/* Header: Title and Add Book Button */}
      <div className="flex justify-between items-center">
        <h1 className="text-2xl font-semibold">Books</h1>
        <Button onClick={openAddDialog}>Add Book</Button>
      </div>

      {/* Dialog for Adding/Editing Books */}
      <BookDialogComponent
        isOpen={isDialogOpen}
        onOpenChange={setIsDialogOpen}
        editingBook={editingBook}
        onSubmit={handleSaveBook}
      />

      {/* Books Table Display */}
      <BooksTable
        books={books}
        headers={bookTableHeaders}
        onEditBook={openEditDialog}
        onDeleteBook={handleDeleteBook}
      />

      {/* Table Footer with Pagination */}
      <TableFooter
        fetchData={fetchBooksData}
        onDataFetched={handleDataFetchedFromFooter}
        defaultPageSize={defaultPageSize}
      />
    </div>
  );
}
