using MediatR;
using ServiCar.Domain.DTOs;
using ServiCar.Domain.Generics;
using ServiCar.Infrastructure.Services;

namespace Servicar.Application.Features.WorkingTime
{
    public record GetWorkingTimeQuery() : IRequest<Result<IEnumerable<WorkingTimeDTO>, ErrorDTO>>;

    public class GetWorkingTimeQueryHandler : IRequestHandler<GetWorkingTimeQuery, Result<IEnumerable<WorkingTimeDTO>, ErrorDTO>>
    {
        private readonly IWorkingTimeService _workingTimeService;
        public GetWorkingTimeQueryHandler(IWorkingTimeService workingTimeService)
        {
            _workingTimeService = workingTimeService;
        }
        public async Task<Result<IEnumerable<WorkingTimeDTO>, ErrorDTO>> Handle(GetWorkingTimeQuery request, CancellationToken cancellationToken)
        {
            return await _workingTimeService.GetAll();
        }
    }
}
