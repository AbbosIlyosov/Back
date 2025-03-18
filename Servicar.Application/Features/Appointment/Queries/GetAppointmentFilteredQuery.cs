using MediatR;
using Servicar.Domain.DTOs;
using ServiCar.Domain.DTOs;

namespace Servicar.Application.Features.Appointment.Queries
{
    public record GetAppointmentFilteredQuery(AppointmentFilterDTO filter) : IRequest<ResponseDTO>;

    public class GetAppointmentFilteredQueryHandler : IRequestHandler<GetAppointmentFilteredQuery, ResponseDTO>
    {
        public GetAppointmentFilteredQueryHandler()
        {

        }

        public Task<ResponseDTO> Handle(GetAppointmentFilteredQuery request, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }

}
