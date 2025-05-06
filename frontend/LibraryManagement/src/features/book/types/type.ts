export interface BookRequest {
  name: string;
  author: string;
  categoryId: string;
  amount: number;
}

export interface BookResponse {
  id: string;
  name: string;
  author: string;
  categoryName: string;
  categoryId: string;
  amount: number;
  availableAmount: number;
}
