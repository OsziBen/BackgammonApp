export const TOURNAMENT_API_ROUTES = {
  BASE: 'tournaments',

  BY_ID: (tournamentId: string): string => `${tournamentId}`,

  JOIN: (tournamentId: string): string => `${tournamentId}/join`,

  REQUESTS: (tournamentId: string): string => `${tournamentId}/requests`,

  APPROVE_JOIN_REQUEST: (tournamentId: string, requestId: string): string =>
    `${tournamentId}/requests/${requestId}/approve`,

  REJECT_JOIN_REQUEST: (tournamentId: string, requestId: string): string =>
    `${tournamentId}/requests/${requestId}/reject`,

  WITHDRAW: (tournamentId: string): string => `${tournamentId}/withdraw`,

  PARTICIPANTS: (tournamentId: string): string =>
    `${tournamentId}/participants`,

  PARTICIPANT: (tournamentId: string, userId: string): string =>
    `${tournamentId}/participants/${userId}`,
} as const;
