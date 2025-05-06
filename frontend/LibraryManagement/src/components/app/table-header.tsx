import { TableHeader, TableRow, TableHead } from "@/components/ui/table";

interface BookTableHeaderProps {
  headers: string[];
}

export const BookTableHeader: React.FC<BookTableHeaderProps> = ({
  headers,
}) => (
  <TableHeader>
    <TableRow>
      {headers.map((header) => (
        <TableHead key={header}>{header}</TableHead>
      ))}
    </TableRow>
  </TableHeader>
);
