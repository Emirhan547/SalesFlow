export const LeadStatus = {
  New: 1,
  Contacted: 2,
  Qualified: 3,
  Lost: 4,
  Converted: 5,
} as const;

export type LeadStatus =
  (typeof LeadStatus)[keyof typeof LeadStatus];