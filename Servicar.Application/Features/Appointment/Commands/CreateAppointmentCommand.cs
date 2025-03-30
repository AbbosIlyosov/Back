using MediatR;
using ServiCar.Domain.DTOs;
using ServiCar.Domain.Generics;
using ServiCar.Infrastructure.Services;

namespace Servicar.Application.Features.Appointment.Commands
{
    public record CreateAppointmentCommand(AppointmentCreateDTO Model) : IRequest<Result<AppointmentDTO, ErrorDTO>>;

    public class CreateAppointmentCommandHandler : IRequestHandler<CreateAppointmentCommand, Result<AppointmentDTO, ErrorDTO>>
    {
        private readonly IAppointmentService _appointmentService;
        public CreateAppointmentCommandHandler(IAppointmentService appointmentService)
        {
            _appointmentService = appointmentService;
        }

        public async Task<Result<AppointmentDTO, ErrorDTO>> Handle(CreateAppointmentCommand request, CancellationToken cancellationToken)
        {
            return await _appointmentService.CreateAppointment(request.Model);
        }
    }
}
