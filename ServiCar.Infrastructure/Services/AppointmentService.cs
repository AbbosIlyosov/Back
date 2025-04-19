using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using ServiCar.Domain.DTOs;
using ServiCar.Domain.Entities;
using ServiCar.Domain.Enums;
using ServiCar.Domain.Generics;
using ServiCar.Infrastructure.Persistence;
using System.Net;

namespace ServiCar.Infrastructure.Services
{
    public interface IAppointmentService
    {
        Task<Result<List<AppointmentDTO>, ErrorDTO>> GetAppointments(AppointmentFilterDTO filter);
        Task<Result<AppointmentDTO, ErrorDTO>> CreateAppointment(AppointmentCreateDTO dto);
        Task<Result<bool, ErrorDTO>> UpdateAppointment(AppointmentUpdateDTO dto);
        Task<Result<bool, ErrorDTO>> DeleteAppointment(int id);
        Task<Result<AppointmentDTO, ErrorDTO>> GetMyAppointment(AppointmentFilterDTO filter);
        Task<Result<IEnumerable<AppointmentDTO>, ErrorDTO>> GetMyAllAppointments();
    }
    public class AppointmentService : IAppointmentService
    {
        private readonly ServiCarApiContext _context;
        private readonly ICurrentUserService _currentUserService;
        private readonly UserManager<User> _userManager;
        public AppointmentService(ServiCarApiContext context, ICurrentUserService currentUserService , UserManager<User> userManager)
        {
            _context = context;
            _currentUserService = currentUserService;
            _userManager = userManager;
        }

        public async Task<Result<List<AppointmentDTO>, ErrorDTO>> GetAppointments(AppointmentFilterDTO filter)
        {
            try
            {
                var appointments = await _context.Appointments
                .Where(x => x.AppointmentTime.Date == filter.Date.Date &&
                            filter.PointId == x.PointId)
                .Include(x => x.Point)
                    .ThenInclude(x => x.Location)
                .Include(x => x.User)
                .Select(appt => new AppointmentDTO
                {
                    Id = appt.Id,
                    AppointmentTime = appt.AppointmentTime.ToLocalTime(),
                    StatusId = appt.AppointmentStatusId,
                    OrderNumber = appt.OrderNumber,
                    UserId = appt.UserId,
                    PointId = appt.PointId,
                    FirstName = appt.User.FirstName,
                    LastName = appt.User.LastName,
                    Point = appt.Point.PointName,
                    Address = $"{appt.Point.Location.City}, {appt.Point.Location.District}"
                })
                .ToListAsync();

                return Result<List<AppointmentDTO>, ErrorDTO>.Success(appointments);
            }
            catch (Exception ex)
            {
                var error = new ErrorDTO { StatusCode = HttpStatusCode.BadRequest, Message = "Internal server error occured." };
                return Result<List<AppointmentDTO>, ErrorDTO>.Fail(error);
            }
        }

        public async Task<Result<AppointmentDTO, ErrorDTO>> CreateAppointment(AppointmentCreateDTO dto)
        {
            try
            {
                var orderNumber = await _context.Appointments
                    .Where(a => a.PointId == dto.PointId)
                    .OrderByDescending(a => a.OrderNumber)
                    .Select(a => a.OrderNumber)
                    .FirstOrDefaultAsync();

                int.TryParse(_currentUserService.UserId, out int currentUserId);

                var appointment = new Appointment
                {
                    AppointmentTime = dto.AppointmentTime.Date,
                    AppointmentStatusId = AppointmentStatus.Created,
                    OrderNumber = orderNumber + 1,
                    UserId = currentUserId,
                    PointId = dto.PointId,
                };

                _context.Appointments.Add(appointment);
                await _context.SaveChangesAsync();

                var newAppt = new AppointmentDTO
                {
                    Id = appointment.Id,
                    AppointmentTime = appointment.AppointmentTime,
                    StatusId = AppointmentStatus.Created,
                    OrderNumber = appointment.OrderNumber,
                    UserId = currentUserId,
                    PointId = appointment.PointId,
                };

                return Result<AppointmentDTO, ErrorDTO>.Success(newAppt);
            }
            catch (Exception ex)
            {
                var error = new ErrorDTO
                {
                    StatusCode = HttpStatusCode.BadRequest,
                    Message = "Appointment creation failed.",
                    Details = ex.Message
                };

                return Result<AppointmentDTO, ErrorDTO>.Fail(error);
            }
        }

        public async Task<Result<bool, ErrorDTO>> UpdateAppointment(AppointmentUpdateDTO dto)
        {
            try
            {
                var appt = await _context.Appointments
                    .Where(x => x.Id == dto.Id)
                    .FirstOrDefaultAsync();

                if (appt is null)
                {
                    return Result<bool, ErrorDTO>.Fail(
                        new ErrorDTO
                        {
                            StatusCode = HttpStatusCode.NotFound,
                            Message = "Appointment not found."
                        });
                }

                if (dto.AppointmentTime is DateTime appointmentTime)
                {
                    appt.AppointmentTime = appointmentTime;
                }

                if (dto.StatusId is AppointmentStatus statusId)
                {
                    appt.AppointmentStatusId = statusId;
                }

                if (dto.PointId is int pointId)
                {
                    appt.PointId = pointId;
                }



                _context.Appointments.Update(appt);
                await _context.SaveChangesAsync();

                return Result<bool, ErrorDTO>.Success(true);
            }
            catch (Exception ex)
            {
                var error = new ErrorDTO { StatusCode = HttpStatusCode.BadRequest, Message = "Appointment creation failed." };
                return Result<bool, ErrorDTO>.Fail(error);
            }
        }

        public async Task<Result<bool, ErrorDTO>> DeleteAppointment(int id)
        {
            try
            {
                var apptToRemove = await _context.Appointments.FirstOrDefaultAsync(a => a.Id == id);

                if (apptToRemove is null)
                {
                    var error = new ErrorDTO { StatusCode = HttpStatusCode.NotFound, Message = "Appointment not found." };
                    return Result<bool, ErrorDTO>.Fail(error);
                }

                _context.Appointments.Remove(apptToRemove);
                var result = await _context.SaveChangesAsync();

                return Result<bool, ErrorDTO>.Success(result > 0);
            }
            catch (Exception ex)
            {
                var error = new ErrorDTO { StatusCode = HttpStatusCode.BadRequest, Message = "Appointment deletion failed." };
                return Result<bool, ErrorDTO>.Fail(error);
            }

        }

        public async Task<Result<AppointmentDTO, ErrorDTO>> GetMyAppointment(AppointmentFilterDTO filter)
        {
            try
            {
                int.TryParse(_currentUserService.UserId, out int currentUserId);

                var appointments = await _context.Appointments
                .Where(x => x.UserId == currentUserId
                            && filter.PointId == x.PointId
                            && x.AppointmentTime.Date == filter.Date.Date)
                .Include(x => x.User)
                .Include(x => x.Point)
                    .ThenInclude(p => p.Categories)
                .Include(x => x.Point)
                    .ThenInclude(p => p.Location)
                .Select(appt => new AppointmentDTO
                {
                    Id = appt.Id,
                    AppointmentTime = appt.AppointmentTime,
                    StatusId = appt.AppointmentStatusId,
                    OrderNumber = appt.OrderNumber,
                    UserId = appt.UserId,
                    PointId = appt.PointId,
                    FirstName = appt.User.FirstName,
                    LastName = appt.User.LastName,
                    Point = appt.Point.PointName,
                    Address = $"{appt.Point.Location.City}, {appt.Point.Location.District}",
                    ServiceType = string.Join(", ", appt.Point.Categories.Select(c => c.Name))
                })
                .FirstOrDefaultAsync();

                if (appointments is null)
                {
                    var error = new ErrorDTO { StatusCode = HttpStatusCode.NoContent, Message = "Appointment not found." };
                    return Result<AppointmentDTO, ErrorDTO>.Fail(error);
                }

                return Result<AppointmentDTO, ErrorDTO>.Success(appointments);
            }
            catch (Exception ex)
            {
                var error = new ErrorDTO { StatusCode = HttpStatusCode.BadRequest, Message = "Internal server error occured." };
                return Result<AppointmentDTO, ErrorDTO>.Fail(error);
            }
        }

        public async Task<Result<IEnumerable<AppointmentDTO>, ErrorDTO>> GetMyAllAppointments()
        {
            try
            {
                int.TryParse(_currentUserService.UserId, out int currentUserId);


                if(currentUserId == 0)
                {
                    var error = new ErrorDTO { StatusCode = HttpStatusCode.BadRequest, Message = "User not found." };
                    return Result<IEnumerable<AppointmentDTO>, ErrorDTO>.Fail(error);
                }

                var query = _context.Appointments
                .Include(x => x.User)
                .Include(x => x.Point)
                    .ThenInclude(p => p.Categories)
                .Include(x => x.Point)
                    .ThenInclude(p => p.Location)
                .Include(x => x.Reviews)
                .AsQueryable();

                // if current user is worker
                if (!string.IsNullOrEmpty(_currentUserService.UserRole) && _currentUserService.UserRole?.ToLower() == "worker")
                {
                    // return all appointments of his business

                    var points = await _userManager.Users
                        .Include(x => x.Business)
                        .ThenInclude(x => x.Points)
                        .Where(x => x.Id == currentUserId)
                        .SelectMany(x => x.Business.Points.Select(p => p.Id))
                        .ToListAsync();

                    query = query.Where(x => points.Contains(x.PointId));
                }
                else
                {
                    // return all appointments of the user
                    query = query.Where(x => x.UserId == currentUserId);
                }

                    var appointments = await query
                    .Select(appt => new AppointmentDTO
                    {
                        Id = appt.Id,
                        AppointmentTime = appt.AppointmentTime,
                        StatusId = appt.AppointmentStatusId,
                        OrderNumber = appt.OrderNumber,
                        UserId = appt.UserId,
                        PointId = appt.PointId,
                        FirstName = appt.User.FirstName,
                        LastName = appt.User.LastName,
                        Point = appt.Point.PointName,
                        Address = $"{appt.Point.Location.City}, {appt.Point.Location.District}",
                        ServiceType = string.Join(", ", appt.Point.Categories.Select(c => c.Name)),
                        HasReview = appt.Reviews.Any()
                    })
                    .AsSplitQuery()
                    .ToListAsync();


                return Result<IEnumerable<AppointmentDTO>, ErrorDTO>.Success(appointments);
            }
            catch (Exception ex)
            {
                var error = new ErrorDTO { StatusCode = HttpStatusCode.BadRequest, Message = "Internal server error occured." };
                return Result<IEnumerable<AppointmentDTO>, ErrorDTO>.Fail(error);
            }
        }
    }
}
