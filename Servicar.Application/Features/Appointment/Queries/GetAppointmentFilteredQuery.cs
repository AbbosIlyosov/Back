using MediatR;
using ServiCar.Domain.DTOs;
using ServiCar.Domain.Generics;
using ServiCar.Infrastructure.Services;

namespace Servicar.Application.Features.Appointment.Queries
{
    public record GetAppointmentFilteredQuery(AppointmentFilterDTO filter) : IRequest<Result<List<AppointmentDTO>, ErrorDTO>>;

    public class GetAppointmentFilteredQueryHandler : IRequestHandler<GetAppointmentFilteredQuery, Result<List<AppointmentDTO>, ErrorDTO>>
    {
        private readonly IAppointmentService _appointmentService;
        public GetAppointmentFilteredQueryHandler(IAppointmentService appointmentService)
        {
            _appointmentService = appointmentService;
        }
        public async Task<Result<List<AppointmentDTO>, ErrorDTO>> Handle(GetAppointmentFilteredQuery request, CancellationToken cancellationToken)
        {
            return await _appointmentService.GetAppointments(request.filter);
        }
    }

}
