using Application.Users.Responses;
using MediatR;

namespace Application.Users.Commands.ListTournamentJoinRequestsByUserId
{
    public record ListTournamentJoinRequestsByUserIdCommand(Guid UserId) : IRequest<List<UserTournamentJoinRequestResponse>>;
}
