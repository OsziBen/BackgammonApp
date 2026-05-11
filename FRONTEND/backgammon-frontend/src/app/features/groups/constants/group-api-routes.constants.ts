export const GROUP_API_ROUTES = {
  BASE: 'groups',

  BY_ID: (groupId: string): string => `${groupId}`,

  JOIN: (groupId: string): string => `${groupId}/join`,

  REQUESTS: (groupId: string): string => `${groupId}/requests`,

  APPROVE_JOIN_REQUEST: (groupId: string, requestId: string): string =>
    `${groupId}/requests/${requestId}/approve`,

  REJECT_JOIN_REQUEST: (groupId: string, requestId: string): string =>
    `${groupId}/requests/${requestId}/reject`,

  LEAVE: (groupId: string): string => `${groupId}/leave`,

  MEMBERS: (groupId: string): string => `${groupId}/members`,

  MEMBER: (groupId: string, userId: string): string =>
    `${groupId}/members/${userId}`,

  MODERATORS: (groupId: string): string => `${groupId}/moderators`,

  MODERATOR: (groupId: string, userId: string): string =>
    `${groupId}/moderators/${userId}`,
} as const;
