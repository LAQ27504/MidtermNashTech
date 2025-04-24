import { useState } from "react";
import { books, Book } from "@/data/books";
import {
  Table,
  TableHeader,
  TableRow,
  TableHead,
  TableBody,
  TableCell,
} from "@/components/ui/table";
import { Checkbox } from "@/components/ui/checkbox";

export default function Home() {
  // track selected IDs (optional)
  const [selected, setSelected] = useState<Set<string>>(new Set());

  const toggle = (book: Book, checked: boolean) => {
    // log the book detail
    console.log("Book toggled:", book);
    // update local selection state
    setSelected((prev) => {
      const next = new Set(prev);
      checked ? next.add(book.id) : next.delete(book.id);
      return next;
    });
  };

  return (
    <div className="p-6">
      <h1 className="text-2xl font-semibold mb-4">Books</h1>

      <div className="rounded-md border">
        <Table>
          <TableHeader>
            <TableRow>
              {/* First column: select-all checkbox */}
              <TableHead className="w-12"></TableHead>
              <TableHead>Title</TableHead>
              <TableHead>Author</TableHead>
              <TableHead>Category</TableHead>
              <TableHead>Available</TableHead>
            </TableRow>
          </TableHeader>

          <TableBody>
            {books.map((book) => (
              <TableRow key={book.id}>
                {/* Row-level checkbox */}
                <TableCell className="w-12">
                  <Checkbox
                    checked={selected.has(book.id)}
                    onCheckedChange={(val) => toggle(book, val === true)}
                  />
                </TableCell>
                <TableCell>{book.title}</TableCell>
                <TableCell>{book.author}</TableCell>
                <TableCell>{book.category}</TableCell>
                <TableCell>{book.available}</TableCell>
              </TableRow>
            ))}
          </TableBody>
        </Table>
      </div>
    </div>
  );
}
