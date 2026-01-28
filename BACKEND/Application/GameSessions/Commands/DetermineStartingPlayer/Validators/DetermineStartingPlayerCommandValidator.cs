using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.GameSessions.Commands.DetermineStartingPlayer.Validators
{
    public class DetermineStartingPlayerCommandValidator : AbstractValidator<DetermineStartingPlayerCommand>
    {
        public DetermineStartingPlayerCommandValidator()
        {
            
        }
    }
}
