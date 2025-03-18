using MediatR;

namespace Servicar.Application.Features.Appointment.Commands
{
    public record CreateAppointmentCommand(string Name) : IRequest<int>;

    public class CreateAppointmentCommandHandler : IRequestHandler<CreateAppointmentCommand, int>
    {
        public CreateAppointmentCommandHandler()
        {
        }

        public async Task<int> Handle(CreateAppointmentCommand request, CancellationToken cancellationToken)
        {
            return 1;
        }
    }
}
