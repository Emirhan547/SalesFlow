export type CustomerType =
  | 1
  | 2;

export const CustomerTypes = {
  Individual: 1,
  Corporate: 2,
} as const;
