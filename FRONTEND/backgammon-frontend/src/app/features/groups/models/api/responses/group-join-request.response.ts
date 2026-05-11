export interface GroupJoinRequestResponse {
  id: string;

  userName: string;

  status: string;

  createdAt: string;

  reviewedAt?: string | null;

  reviewedByUser?: string | null;
}
