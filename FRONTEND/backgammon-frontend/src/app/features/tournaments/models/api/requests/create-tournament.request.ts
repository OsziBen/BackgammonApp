export interface CreateTournamentRequest {
  name: string;
  description?: string;
  type: string;
  visibility: string;
  maxParticipants: number;
  rulesTemplateId: string; // Guid → string
  startDate: string; // DateTimeOffset → ISO string
  endDate: string;
  deadline: string;
}
