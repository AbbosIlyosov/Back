using MediatR;
using ServiCar.Domain.DTOs;
using ServiCar.Domain.Generics;
using ServiCar.Infrastructure.Services;

namespace Servicar.Application.Features.Appointment.Commands
{
    public record DeleteAppointmentCommand(int Id): IRequest<Result<bool, ErrorDTO>>;

    public class DeleteAppointmentCommandHandler : IRequestHandler<DeleteAppointmentCommand, Result<bool, ErrorDTO>>
    {
        private readonly IAppointmentService _appointmentService;
        public DeleteAppointmentCommandHandler(IAppointmentService appointmentService)
        {
            _appointmentService = appointmentService;
        }
        public async Task<Result<bool, ErrorDTO>> Handle(DeleteAppointmentCommand request, CancellationToken cancellationToken)
        {
            return await _appointmentService.DeleteAppointment(request.Id);
        }
    }
}
