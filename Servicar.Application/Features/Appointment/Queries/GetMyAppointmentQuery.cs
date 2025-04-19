using MediatR;
using ServiCar.Domain.DTOs;
using ServiCar.Domain.Generics;
using ServiCar.Infrastructure.Services;

namespace Servicar.Application.Features.Appointment.Queries
{
    public record GetMyAppointmentQuery(AppointmentFilterDTO filter) : IRequest<Result<AppointmentDTO, ErrorDTO>>;

    public class GetMyAppointmentQueryHandler : IRequestHandler<GetMyAppointmentQuery, Result<AppointmentDTO, ErrorDTO>>
    {
        private readonly IAppointmentService _appointmentService;
        public GetMyAppointmentQueryHandler(IAppointmentService appointmentService)
        {
            _appointmentService = appointmentService;
        }
        public async Task<Result<AppointmentDTO, ErrorDTO>> Handle(GetMyAppointmentQuery request, CancellationToken cancellationToken)
        {
            return await _appointmentService.GetMyAppointment(request.filter);
        }
    }

}
