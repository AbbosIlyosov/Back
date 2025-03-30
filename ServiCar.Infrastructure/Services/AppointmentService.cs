using Microsoft.EntityFrameworkCore;
using ServiCar.Domain.DTOs;
using ServiCar.Domain.Entities;
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

    }
    public class AppointmentService : IAppointmentService
    {
        private readonly ServiCarApiContext _context;
        public AppointmentService(ServiCarApiContext context)
        {
            _context = context;
        }

        public async Task<Result<List<AppointmentDTO>, ErrorDTO>> GetAppointments(AppointmentFilterDTO filter)
        {
            try
            {
                var appointments = await _context.Appointments
                .Where(x => x.AppointmentTime.Date == filter.Date.Date && 
                            filter.PointId == x.PointId)
                .Select(appt => new AppointmentDTO
                {
                    Id = appt.Id,
                    AppointmentTime = appt.AppointmentTime,
                    StatusId = appt.AppointmentStatusId,
                    OrderNumber = appt.OrderNumber,
                    UserId = appt.UserId,
                    PointId = appt.PointId,
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

                var appointment = new Appointment
                {
                    AppointmentTime = dto.AppointmentTime,
                    AppointmentStatusId = dto.StatusId,
                    OrderNumber = orderNumber + 1,
                    UserId = dto.UserId,
                    PointId = dto.PointId,
                };

                _context.Appointments.Add(appointment);
                await _context.SaveChangesAsync();

                var newAppt = new AppointmentDTO
                {
                    Id = appointment.Id,
                    AppointmentTime = appointment.AppointmentTime,
                    StatusId = appointment.AppointmentStatusId,
                    OrderNumber = appointment.OrderNumber,
                    UserId = appointment.UserId,
                    PointId = appointment.PointId,
                };

                return Result<AppointmentDTO, ErrorDTO>.Success(newAppt);
            }
            catch (Exception ex)
            {
                var error = new ErrorDTO {
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
                        new ErrorDTO {
                            StatusCode = HttpStatusCode.NotFound,
                            Message = "Appointment not found."
                        });
                }

                appt.AppointmentTime = dto.AppointmentTime;
                appt.AppointmentStatusId = dto.StatusId;
                appt.UserId = dto.UserId;
                appt.PointId = dto.PointId;

                _context.Appointments.Update(appt);
                await _context.SaveChangesAsync();

                return Result<bool, ErrorDTO>.Success(true);
            }
            catch (Exception ex)
            {
                var error = new ErrorDTO { StatusCode = HttpStatusCode.BadRequest, Message = "Appointment creation failed."};
                return Result<bool, ErrorDTO>.Fail(error);
            }
        }

        public async Task<Result<bool, ErrorDTO>> DeleteAppointment(int id)
        {
            try
            {
                var apptToRemove = await _context.Appointments.FirstOrDefaultAsync(a => a.Id == id);

                if(apptToRemove is null)
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
    }
}
