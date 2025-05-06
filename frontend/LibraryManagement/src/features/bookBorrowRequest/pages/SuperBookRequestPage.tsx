// src/features/bookBorrowRequest/pages/SuperBookBorrowRequestPage.tsx
import React, { useState, Fragment } from "react";
import { Navigate, useNavigate, useSearchParams } from "react-router-dom";
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
  GetRequestPaginated,
  SetRequestApproval,
} from "@/features/bookBorrowRequest/api/bookBorrowRequestApi";
import {
  BorrowResponse,
  ApprovalRequest,
} from "@/features/bookBorrowRequest/types/type";
import { PaginationRequest, PaginationResponse } from "@/constants/Common";
import { TableFooter } from "@/components/app/table-footer";
import { RequestStatusBadge } from "@/components/app/request-status-badge";
import { BorrowRequestDetailsRow } from "../components/borrow-detail-request-row";
import { ChevronDown, ChevronRight, CheckCircle, XCircle } from "lucide-react";
import { useUserContext } from "@/context/userContext";
import { pathName } from "@/routers";

const formatDate = (dateString: string) => {
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

export default function SuperBookBorrowRequestPage() {
  const [requests, setRequests] = useState<BorrowResponse[]>([]);
  const [expandedRowId, setExpandedRowId] = useState<string | null>(null);
  const { userId, isLoading: userLoading } = useUserContext();
  const [searchParams, setSearchParams] = useSearchParams();

  const navigate = useNavigate();
  const defaultPageSize = 10;
  const tableHeaders = ["Requestor", "Request Date", "Actions"];

  const fetchWaitingRequests = async (
    pageNumber: number,
    pageSize: number
  ): Promise<PaginationResponse<BorrowResponse>> => {
    const req: PaginationRequest = { pageNumber, pageSize };
    try {
      const response = await GetRequestPaginated(req);
      if (response && response.data && response.data.data) {
        return response.data.data;
      }
    } catch (error) {
      console.error("Error fetching waiting requests:", error);
    }
    return {
      items: [],
      pageNumber: pageNumber,
      pageSize: pageSize,
      totalCount: 0,
    };
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

  const handleApproval = async (requestId: string, isApproved: boolean) => {
    if (!userId) {
      alert("Admin user not identified. Please log in again.");
      return;
    }
    const payload: ApprovalRequest = {
      requestId: requestId,
      approverId: userId,
      isApproved: isApproved,
    };

    try {
      await SetRequestApproval(payload);
      alert(`Request ${isApproved ? "approved" : "rejected"} successfully.`);
      setExpandedRowId(null);
      navigate(pathName.superBorrowRequest);
      const currentPageSize =
        searchParams.get("pageSize") || defaultPageSize.toString();
      setSearchParams(
        { page: "1", pageSize: currentPageSize },
        { replace: true }
      );
      window.location.reload();
    } catch (error: any) {
      console.error("Failed to update request status:", error);
      const errorMessage =
        error.response?.data?.message ||
        error.message ||
        "An unexpected error occurred.";
      alert(`Failed to update request status: ${errorMessage}`);
    }
  };

  if (userLoading) {
    return <div className="p-6 text-center">Loading admin information...</div>;
  }
  if (!userId && !userLoading) {
    return <div className="p-6 text-center">Admin not logged in.</div>;
  }

  return (
    <div className="p-6 space-y-6">
      <div className="flex justify-between items-center">
        <h1 className="text-2xl font-semibold">
          Pending Book Borrowing Requests
        </h1>
      </div>

      <div className="rounded-md border overflow-x-auto">
        <Table>
          <TableHeader>
            <TableRow>
              <TableHead className="w-[50px]"></TableHead>
              {tableHeaders.map((header) => (
                <TableHead key={header}>{header}</TableHead>
              ))}
              <TableHead>Status</TableHead>
            </TableRow>
          </TableHeader>
          <TableBody>
            {requests.length === 0 && !userLoading && (
              <TableRow>
                <TableCell
                  colSpan={tableHeaders.length + 2}
                  className="text-center h-24">
                  No pending requests found.
                </TableCell>
              </TableRow>
            )}
            {requests.map((request) => (
              <Fragment key={request.id}>
                <TableRow className="hover:bg-muted/50">
                  <TableCell className="px-2">
                    <Button
                      variant="ghost"
                      size="sm"
                      onClick={() => toggleRowExpansion(request.id)}
                      aria-expanded={expandedRowId === request.id}>
                      {expandedRowId === request.id ? (
                        <ChevronDown className="h-4 w-4" />
                      ) : (
                        <ChevronRight className="h-4 w-4" />
                      )}
                    </Button>
                  </TableCell>
                  <TableCell>{request.requestorName || "N/A"}</TableCell>
                  <TableCell>{formatDate(request.borrowDate)}</TableCell>
                  <TableCell>
                    <div className="flex gap-2">
                      <Button
                        variant="outline"
                        size="sm"
                        className="text-green-600 border-green-600 hover:bg-green-50 hover:text-green-700"
                        onClick={() => handleApproval(request.id, true)}>
                        <CheckCircle className="mr-1 h-4 w-4" /> Approve
                      </Button>
                      <Button
                        variant="outline"
                        size="sm"
                        className="text-red-600 border-red-600 hover:bg-red-50 hover:text-red-700"
                        onClick={() => handleApproval(request.id, false)}>
                        <XCircle className="mr-1 h-4 w-4" /> Reject
                      </Button>
                    </div>
                  </TableCell>
                  <TableCell>
                    <RequestStatusBadge status={request.status} />
                  </TableCell>
                </TableRow>
                {expandedRowId === request.id && (
                  <BorrowRequestDetailsRow
                    details={request.details}
                    colSpan={tableHeaders.length + 2}
                  />
                )}
              </Fragment>
            ))}
          </TableBody>
        </Table>
      </div>

      {userId && (
        <TableFooter
          fetchData={fetchWaitingRequests}
          onDataFetched={handleDataFetched}
          defaultPageSize={defaultPageSize}
        />
      )}
    </div>
  );
}
