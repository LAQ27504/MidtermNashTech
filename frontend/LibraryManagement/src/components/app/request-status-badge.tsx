// src/components/app/RequestStatusBadge.tsx
import React from "react";
import { Badge } from "@/components/ui/badge"; // Assuming shadcn/ui Badge component
import {
  BorrowBookStatus,
  RequestStatus,
} from "@/features/bookBorrowRequest/types/type";

interface RequestStatusBadgeProps {
  status: RequestStatus;
}

export const RequestStatusBadge: React.FC<RequestStatusBadgeProps> = ({
  status,
}) => {
  let variant: "default" | "secondary" | "destructive" | "outline" =
    "secondary";
  let text = "Unknown";

  switch (status) {
    case RequestStatus.Approved:
      variant = "default"; // Consider a "success" variant if your Badge supports it (e.g., green)
      text = "Approved";
      break;
    case RequestStatus.Rejected:
      variant = "destructive";
      text = "Rejected";
      break;
    case RequestStatus.Waiting:
      variant = "outline"; // Consider a "warning" variant (e.g., yellow/orange)
      text = "Waiting";
      break;
    default:
      text = `Unknown (${status})`; // Handle unexpected status values
      break;
  }

  return (
    <Badge variant={variant} className="capitalize">
      {text}
    </Badge>
  );
};

interface BorrowBookStatusBadgeProps {
  status: BorrowBookStatus;
}

export const BorrowBookStatusBadge: React.FC<BorrowBookStatusBadgeProps> = ({
  status,
}) => {
  let variant: "default" | "secondary" | "destructive" | "outline" =
    "secondary";
  let text = "Unknown";

  switch (status) {
    case BorrowBookStatus.Waiting:
      text = "Waiting";
      variant = "outline"; // Yellow/Orange
      break;
    case BorrowBookStatus.Approved: // Individual book approved (might be part of a larger request)
      text = "Approved";
      variant = "default"; // Green
      break;
    case BorrowBookStatus.Rejected: // Individual book rejected
      text = "Rejected";
      variant = "destructive"; // Red
      break;
    case BorrowBookStatus.Borrowed: // Book is currently borrowed
      text = "Borrowed";
      variant = "default"; // Blue or primary color
      break;
    case BorrowBookStatus.Returned: // Book has been returned
      text = "secondary"; // Grey or a less prominent color
      break;
    default:
      text = `Unknown (${status})`;
      break;
  }
  return (
    <Badge variant={variant} className="capitalize">
      {text}
    </Badge>
  );
};
