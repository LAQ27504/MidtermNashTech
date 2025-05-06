// src/features/book/components/BooksTable.tsx
import React from "react";
import { Table, TableBody, TableRow, TableCell } from "@/components/ui/table";
import { BookTableHeader } from "@/components/app/table-header"; // Assuming this path is correct
import { BookResponse } from "@/features/book/types/type";
import { BookActions } from "./book-actions"; // Or adjust path as needed

interface BooksTableProps {
  books: BookResponse[];
  headers: string[];
  onEditBook: (book: BookResponse) => void;
  onDeleteBook: (id: string) => void;
}

export const BooksTable: React.FC<BooksTableProps> = ({
  books,
  headers,
  onEditBook,
  onDeleteBook,
}) => {
  return (
    <div className="rounded-md border overflow-auto">
      <Table className="table-fixed w-full">
        <BookTableHeader headers={headers} />
        <TableBody>
          {books.length === 0 && (
            <TableRow>
              <TableCell colSpan={headers.length} className="text-center">
                No books found.
              </TableCell>
            </TableRow>
          )}
          {books.map((b) => (
            <TableRow key={b.id} className="hover:bg-gray-50">
              <TableCell className="w-40 whitespace-nowrap overflow-hidden text-ellipsis">
                {b.name}
              </TableCell>
              <TableCell className="w-32 whitespace-nowrap overflow-hidden text-ellipsis">
                {b.author}
              </TableCell>
              <TableCell className="w-32 whitespace-nowrap overflow-hidden text-ellipsis">
                {b.categoryName}
              </TableCell>
              <TableCell className="w-16 text-center">{b.amount}</TableCell>
              <TableCell className="w-16 text-center">
                {b.availableAmount}
              </TableCell>
              <TableCell className="w-36">
                <BookActions
                  book={b}
                  onEdit={onEditBook}
                  onDelete={onDeleteBook}
                />
              </TableCell>
            </TableRow>
          ))}
        </TableBody>
      </Table>
    </div>
  );
};
