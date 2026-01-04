export interface MarkingResult {
  raw: string;
  gradeBand: string;
  timestamp: Date;
  question?: string;
  skillId: string;
  skillName: string;
}
