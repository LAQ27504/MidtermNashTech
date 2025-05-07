export interface BorrowRequest {
  requestorId: string;
  bookIds: string[];
}

export interface ReturnRequest {
  requestId: string;
  bookIds: string[];
}

export interface ApprovalRequest {
  requestId: string;
  approverId: string;
  isApproved: boolean;
}

export interface BorrowResponse {
  id: string; // Guid as string
  requestorID: string;
  requestorName: string;
  borrowDate: string; // DateTime as ISO string
  approverID: string;
  approverName: string;
  status: RequestStatus;
  // returnDate?: string; // Uncomment if needed
  details: BookBorrowingRequestDetailsResponse[];
}

export interface BookBorrowingRequestDetailsResponse {
  id: string;
  bookId: string;
  bookName: string;
  bookAuthor: string;
  status: BorrowBookStatus;
}

export enum RequestStatus {
  Approved = 0,
  Rejected = 1,
  Waiting = 2,
}

export enum BorrowBookStatus {
  Waiting = 0,
  Approved = 1,
  Rejected = 2,
  Returned = 3,
}
