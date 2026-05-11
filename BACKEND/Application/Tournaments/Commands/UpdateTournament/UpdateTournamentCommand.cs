using Application.Tournaments.Responses;
using Common.Enums.Tournament;
using MediatR;

namespace Application.Tournaments.Commands.UpdateTournament
{
    public record UpdateTournamentCommand(
        Guid TournamentId,
        string Name,
        string Description,
        TournamentType Type,
        TournamentVisibility Visibility,
        TournamentStatus Status,
        int MaxParticipants,
        DateTimeOffset StartDate,
        DateTimeOffset EndDate,
        DateTimeOffset Deadline,
        Guid RulesTemplateId) : IRequest<TournamentBaseResponse>;
}
