using MediatR;
using ServiCar.Domain.DTOs;
using ServiCar.Domain.Generics;
using ServiCar.Infrastructure.Services;

namespace Servicar.Application.Features.Appointment.Commands
{
    public record UpdateAppointmentCommand(AppointmentUpdateDTO Model) : IRequest<Result<bool, ErrorDTO>>;

    public class UpdateAppointmentCommandHandler : IRequestHandler<UpdateAppointmentCommand, Result<bool, ErrorDTO>>
    {
        private readonly IAppointmentService _appointmentService;
        public UpdateAppointmentCommandHandler(IAppointmentService appointmentService)
        {
            _appointmentService = appointmentService;
        }
        public async Task<Result<bool, ErrorDTO>> Handle(UpdateAppointmentCommand request, CancellationToken cancellationToken)
        {
            return await _appointmentService.UpdateAppointment(request.Model);
        }
    }
}
