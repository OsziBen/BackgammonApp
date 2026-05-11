export interface UserBaseResponse {
  id: string;

  userName: string;

  joinedAt: string;

  rating: number;

  experiencePoints: number;

  groupRoleName?: string | null;

  assignedAt?: string | null;

  grantedByUserName?: string | null;
}
