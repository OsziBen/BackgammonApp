using Application.Tournaments.Responses;
using Common.Enums.Tournament;
using MediatR;

namespace Application.Tournaments.Commands.CreateTournament
{
    public record CreateTournamentCommand(
        Guid UserId,
        string Name,
        string? Description,
        TournamentType Type,
        TournamentVisibility Visibility,
        int MaxParticipants,
        Guid RulesTemplateId,
        DateTimeOffset StartDate,
        DateTimeOffset EndDate,
        DateTimeOffset Deadline) : IRequest<TournamentBaseResponse>;
}
