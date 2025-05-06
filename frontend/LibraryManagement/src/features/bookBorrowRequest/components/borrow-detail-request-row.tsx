// src/features/bookBorrowRequest/components/BorrowRequestDetailsRow.tsx
import React from "react";
import {
  Table,
  TableBody,
  TableCell,
  TableHead,
  TableHeader,
  TableRow,
} from "@/components/ui/table";
import { BookBorrowingRequestDetailsResponse } from "@/features/bookBorrowRequest/types/type"; // Adjust path
import { BorrowBookStatusBadge } from "@/components/app/request-status-badge"; // Adjust path to where your badge is

interface BorrowRequestDetailsRowProps {
  details: BookBorrowingRequestDetailsResponse[];
  colSpan: number;
}

export const BorrowRequestDetailsRow: React.FC<
  BorrowRequestDetailsRowProps
> = ({ details, colSpan }) => {
  if (!details || details.length === 0) {
    return (
      <TableRow>
        <TableCell colSpan={colSpan} className="p-0">
          {" "}
          {/* Remove default padding from cell */}
          <div className="p-4 text-sm text-muted-foreground text-center bg-muted/30 dark:bg-muted/50">
            No book details available for this request.
          </div>
        </TableCell>
      </TableRow>
    );
  }

  return (
    <TableRow className="bg-muted/30 dark:bg-muted/50 hover:bg-muted/40 dark:hover:bg-muted/60">
      {" "}
      {/* Subtle background for the whole details row */}
      <TableCell colSpan={colSpan} className="p-0 !border-none">
        {" "}
        {/* Remove padding and border from outer cell */}
        <div className="p-4 space-y-2">
          {" "}
          {/* Add padding to the inner div */}
          <h4 className="font-semibold text-sm text-foreground">
            Book Details:
          </h4>
          <div className="border rounded-md overflow-hidden">
            {" "}
            {/* Optional: border around the inner table */}
            <Table className="bg-background">
              {" "}
              {/* Inner table with standard background */}
              <TableHeader>
                <TableRow>
                  <TableHead className="h-10">Book Name</TableHead>
                  <TableHead className="h-10">Author</TableHead>
                  <TableHead className="h-10">Status</TableHead>
                </TableRow>
              </TableHeader>
              <TableBody>
                {details.map((detail) => (
                  <TableRow
                    key={detail.id}
                    className="hover:bg-muted/10 dark:hover:bg-muted/20">
                    <TableCell className="py-2">{detail.bookName}</TableCell>
                    <TableCell className="py-2">{detail.bookAuthor}</TableCell>
                    <TableCell className="py-2">
                      <BorrowBookStatusBadge status={detail.status} />
                    </TableCell>
                  </TableRow>
                ))}
              </TableBody>
            </Table>
          </div>
        </div>
      </TableCell>
    </TableRow>
  );
};
