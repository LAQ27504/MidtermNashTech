import { Table, TableBody } from "@/components/ui/table";
import { BookResponse } from "../types/type";
import { BookTableHeader } from "@/components/app/table-header";
import { BookTableRow } from "./book-table-row";

interface BookTableProps {
  books: BookResponse[];
  selected: Set<string>;
  maxSelection: number;
  headers: string[];
  onToggle: (book: BookResponse, checked: boolean) => void;
}

export const BookTable: React.FC<BookTableProps> = ({
  books,
  selected,
  maxSelection,
  headers,
  onToggle,
}) => {
  return (
    <div className="rounded-md border">
      <Table>
        <BookTableHeader headers={headers} />
        <TableBody>
          {books.map((book) => (
            <BookTableRow
              key={book.id}
              book={book}
              isChecked={selected.has(book.id)}
              isDisabled={
                !selected.has(book.id) && selected.size >= maxSelection
              }
              onCheck={(checked) => onToggle(book, checked)}
            />
          ))}
        </TableBody>
      </Table>
    </div>
  );
};
