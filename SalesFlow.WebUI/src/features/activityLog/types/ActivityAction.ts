export type ActivityAction =
  | 1
  | 2
  | 3
  | 4
  | 5
  | 6;

export const ActivityActions = {
  Create: 1,
  Update: 2,
  Delete: 3,
  Convert: 4,
  Login: 5,
  Logout: 6,
} as const;