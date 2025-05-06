// src/features/bookBorrowRequest/pages/NormalBookRequestPage.tsx
import React, { useState, Fragment, useEffect } from "react";
import {
  Table,
  TableBody,
  TableCell,
  TableHead,
  TableHeader,
  TableRow,
} from "@/components/ui/table";
import { Button } from "@/components/ui/button";
import {
  GetRequestPaginatedUserId,
  ReturnBooks, // Import the ReturnBooks API function
} from "@/features/bookBorrowRequest/api/bookBorrowRequestApi";
import {
  BorrowResponse,
  ReturnRequest, // Import the ReturnRequest type
  RequestStatus,
  BorrowBookStatus,
} from "@/features/bookBorrowRequest/types/type";
import { PaginationRequest, PaginationResponse } from "@/constants/Common";
import { TableFooter } from "@/components/app/table-footer";
import { RequestStatusBadge } from "@/components/app/request-status-badge";
import { BorrowRequestDetailsRow } from "../components/borrow-detail-request-row";
import { ChevronDown, ChevronRight, Undo2 } from "lucide-react"; // Added Undo2 for return icon
import { useUserContext } from "@/context/userContext";

const formatDate = (dateString: string) => {
  // ... (formatDate function remains the same) ...
  if (!dateString) return "N/A";
  try {
    return new Date(dateString).toLocaleDateString(undefined, {
      year: "numeric",
      month: "short",
      day: "numeric",
      hour: "2-digit",
      minute: "2-digit",
    });
  } catch (e) {
    return "Invalid Date";
  }
};

export default function NormalBookRequestPage() {
  const { userId, isLoading: userLoading } = useUserContext();
  const [requests, setRequests] = useState<BorrowResponse[]>([]);
  const [expandedRowId, setExpandedRowId] = useState<string | null>(null);
  const [refreshKey, setRefreshKey] = useState(0);

  const defaultPageSize = 10;
  // Added "Actions" header
  const tableHeaders = ["Request Date", "Status", "Processed By", "Actions"];

  useEffect(() => {
    if (userId) {
      setRefreshKey((prev) => prev + 1);
      setRequests([]);
    }
  }, [userId]);

  const fetchUserRequests = async (
    pageNumber: number,
    pageSize: number
  ): Promise<PaginationResponse<BorrowResponse>> => {
    // ... (fetchUserRequests function remains the same) ...
    if (!userId) {
      return { items: [], pageNumber, pageSize, totalCount: 0 };
    }
    const req: PaginationRequest = { pageNumber, pageSize };
    try {
      const response = await GetRequestPaginatedUserId(req, userId);
      if (response && response.data && response.data.data) {
        return response.data.data;
      }
    } catch (error) {
      console.error("Error fetching user's borrow requests:", error);
    }
    return { items: [], pageNumber, pageSize, totalCount: 0 };
  };

  const handleDataFetched = (
    fetchedRequests: BorrowResponse[],
    totalCount: number
  ) => {
    setRequests(fetchedRequests);
  };

  const toggleRowExpansion = (requestId: string) => {
    setExpandedRowId(expandedRowId === requestId ? null : requestId);
  };

  // Function to handle returning all borrowed books in a request
  const handleReturnAllBooks = async (request: BorrowResponse) => {
    if (!userId) {
      alert("User not identified. Please log in again.");
      return;
    }

    const borrowedBookIds = request.details
      .filter((detail) => detail.status === BorrowBookStatus.Borrowed)
      .map((detail) => detail.bookId);

    if (borrowedBookIds.length === 0) {
      alert(
        "No books in this request are currently marked as 'Borrowed' and eligible for return."
      );
      return;
    }

    const returnPayload: ReturnRequest = {
      // Your ReturnRequest DTO uses 'id' for the requestId and 'bookId' for the array of book IDs
      id: request.id, // This is the BookBorrowingRequestId
      bookId: borrowedBookIds, // Array of specific Book IDs to be returned
    };

    if (
      !window.confirm(
        `Are you sure you want to return ${borrowedBookIds.length} book(s) for this request?`
      )
    ) {
      return;
    }

    try {
      const response = await ReturnBooks(returnPayload);

      let success = false;
      let message = "Failed to process return request.";

      if (response && response.data) {
        const operationResult = response.data as any;
        if (typeof operationResult.succeeded === "boolean") {
          if (operationResult.succeeded) {
            success = true;
            message = operationResult.message || "Books returned successfully!";
            console.log(
              "Return successful, updated request:",
              operationResult.data
            );
          } else {
            message = operationResult.message || "Failed to return books.";
          }
        } else if (response.data.data) {
          success = true; // Assume success if this structure
          message = "Books returned successfully!";
          console.log(
            "Return successful, updated request:",
            response.data.data
          );
        }
      }

      alert(message);
      if (success) {
        setRefreshKey((prev) => prev + 1); // Refresh the list of requests
        setExpandedRowId(null); // Collapse row
      }
    } catch (error: any) {
      const errorMessage =
        error.response?.data?.message ||
        error.message ||
        "An unexpected error occurred during return.";
      console.error("Error returning books:", error);
      alert(`Error: ${errorMessage}`);
    }
  };

  if (userLoading) {
    return <div className="p-6 text-center">Loading user information...</div>;
  }
  if (!userId && !userLoading) {
    return (
      <div className="p-6 text-center">
        Please log in to view your requests.
      </div>
    );
  }

  return (
    <div className="p-6 space-y-6">
      <div className="flex justify-between items-center">
        <h1 className="text-2xl font-semibold">My Book Borrowing Requests</h1>
      </div>

      <div className="rounded-md border overflow-x-auto">
        <Table>
          <TableHeader>
            <TableRow>
              <TableHead className="w-[50px]"></TableHead> {/* Expand icon */}
              {tableHeaders.map((header) => (
                <TableHead key={header}>{header}</TableHead>
              ))}
            </TableRow>
          </TableHeader>
          <TableBody>
            {requests.length === 0 && (
              <TableRow>
                <TableCell
                  colSpan={tableHeaders.length + 1}
                  className="text-center h-24">
                  You have no book borrowing requests.
                </TableCell>
              </TableRow>
            )}
            {requests.map((request) => {
              const canReturn =
                request.status === RequestStatus.Approved &&
                request.details.some(
                  (detail) => detail.status === BorrowBookStatus.Borrowed
                );
              return (
                <Fragment key={request.id}>
                  <TableRow className="hover:bg-muted/50">
                    <TableCell className="px-2">
                      <Button
                        variant="ghost"
                        size="icon"
                        onClick={() => toggleRowExpansion(request.id)}
                        aria-expanded={expandedRowId === request.id}>
                        {expandedRowId === request.id ? (
                          <ChevronDown className="h-4 w-4" />
                        ) : (
                          <ChevronRight className="h-4 w-4" />
                        )}
                      </Button>
                    </TableCell>
                    <TableCell>{formatDate(request.borrowDate)}</TableCell>
                    <TableCell>
                      <RequestStatusBadge status={request.status} />
                    </TableCell>
                    <TableCell>
                      {request.approverName || "Pending Review"}
                    </TableCell>
                    <TableCell>
                      {canReturn && (
                        <Button
                          variant="outline"
                          size="sm"
                          onClick={() => handleReturnAllBooks(request)}
                          className="text-blue-600 border-blue-600 hover:bg-blue-50 hover:text-blue-700">
                          <Undo2 className="mr-1 h-4 w-4" /> Return Books
                        </Button>
                      )}
                    </TableCell>
                  </TableRow>
                  {expandedRowId === request.id && (
                    <BorrowRequestDetailsRow
                      details={request.details}
                      colSpan={tableHeaders.length + 1}
                    />
                  )}
                </Fragment>
              );
            })}
          </TableBody>
        </Table>
      </div>

      {userId && (
        <TableFooter
          key={refreshKey}
          fetchData={fetchUserRequests}
          onDataFetched={handleDataFetched}
          defaultPageSize={defaultPageSize}
        />
      )}
    </div>
  );
}
