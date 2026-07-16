export const LeadSource = {
  Website: 1,
  Phone: 2,
  Email: 3,
  Referral: 4,
  SocialMedia: 5,
  Advertisement: 6,
  Other: 7,
} as const;

export type LeadSource =
  (typeof LeadSource)[keyof typeof LeadSource];