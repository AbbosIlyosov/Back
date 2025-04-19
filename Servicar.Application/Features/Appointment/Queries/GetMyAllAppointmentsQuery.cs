using MediatR;
using ServiCar.Domain.DTOs;
using ServiCar.Domain.Generics;
using ServiCar.Infrastructure.Services;

namespace Servicar.Application.Features.Appointment.Queries
{
    public record GetMyAllAppointmentsQuery() : IRequest<Result<IEnumerable<AppointmentDTO>, ErrorDTO>>;

    public class GetMyAllAppointmentsHandler : IRequestHandler<GetMyAllAppointmentsQuery, Result<IEnumerable<AppointmentDTO>, ErrorDTO>>
    {
        private readonly IAppointmentService _appointmentService;
        public GetMyAllAppointmentsHandler(IAppointmentService appointmentService)
        {
            _appointmentService = appointmentService;
        }
        public async Task<Result<IEnumerable<AppointmentDTO>, ErrorDTO>> Handle(GetMyAllAppointmentsQuery request, CancellationToken cancellationToken)
        {
            return await _appointmentService.GetMyAllAppointments();
        }
    }
}