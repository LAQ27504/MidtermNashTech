import { useState } from "react";
import { BookTable } from "@/features/book/components/book-table";
import { TableFooter } from "@/components/app/table-footer";
import { GetBookPaginated } from "@/features/book/api/bookApi";
import { BookResponse } from "../types/type";
import { Button } from "@/components/ui/button";
import { useUserContext } from "@/context/userContext";
import {
  BorrowRequest,
  BorrowResponse,
} from "@/features/bookBorrowRequest/types/type";
import { CreateBorrowRequest } from "@/features/bookBorrowRequest/api/bookBorrowRequestApi";

export default function Home() {
  const MAX_SELECTION = 5;

  const [books, setBooks] = useState<BookResponse[]>([]);
  const [totalCount, setTotalCount] = useState(0);
  const [selected, setSelected] = useState<Set<string>>(new Set());
  const { userId, username } = useUserContext();

  const toggle = (book: BookResponse, checked: boolean) => {
    setSelected((prev) => {
      const next = new Set(prev);
      if (checked) {
        if (prev.size >= MAX_SELECTION) return prev;
        next.add(book.id);
      } else {
        next.delete(book.id);
      }
      return next;
    });
  };

  const handleLogSelected = async () => {
    const selectedBooks = books.filter((book) => selected.has(book.id));
    const bookIds = selectedBooks.map((book) => book.id);

    const borrowRequestPayload: BorrowRequest = {
      requestorId: userId,
      bookIds: bookIds,
    };

    try {
      const response = await CreateBorrowRequest(borrowRequestPayload);
      const result =
        (response?.data?.data || response?.data?.data) ?? response?.data;

      if (result?.id) {
        const createdRequest: BorrowResponse = result;
        console.log("Borrow request created successfully:", createdRequest);
        alert(
          `Borrow request created successfully for ${username}! Request ID: ${createdRequest.id}`
        );
        return;
      }

      const message = response?.data?.message || "Unknown error from API.";
      console.error("Failed to create borrow request:", message);
      alert(`Failed to create borrow request: ${message}`);
    } catch (error: any) {
      console.error("Error creating borrow request:", error);
      const errorMessage =
        error?.response?.data?.message ||
        error.message ||
        "An unexpected error occurred.";
      alert(`Error creating borrow request: ${errorMessage}`);
    }

    console.log("Selected books:", selectedBooks);
  };

  return (
    <div className="p-6 space-y-6">
      <h1 className="text-2xl font-semibold">Books</h1>
      <BookTable
        books={books}
        selected={selected}
        maxSelection={MAX_SELECTION}
        headers={["Select", "Name", "Author", "Category", "Available"]}
        onToggle={toggle}
      />
      <div className="flex flex-col md:flex-row justify-between items-center gap-4">
        <div>{selected.size} books selected</div>
        <TableFooter<BookResponse>
          fetchData={(pageNumber, pageSize) =>
            GetBookPaginated({ pageNumber, pageSize }).then(
              (res) => res.data.data // Assuming response structure
            )
          }
          onDataFetched={(items, totalCount) => {
            setBooks(items);
            setTotalCount(totalCount);
          }}
        />
        <Button disabled={selected.size === 0} onClick={handleLogSelected}>
          Borrow Selected Books ({selected.size}/{MAX_SELECTION})
        </Button>
      </div>
    </div>
  );
}
