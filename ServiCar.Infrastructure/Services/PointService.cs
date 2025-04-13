using Microsoft.EntityFrameworkCore;
using ServiCar.Domain.DTOs;
using ServiCar.Domain.Entities;
using ServiCar.Domain.Enums;
using ServiCar.Domain.Generics;
using ServiCar.Infrastructure.Persistence;
using System.Net;

namespace ServiCar.Infrastructure.Services
{
    public interface IPointService
    {
        Task<Result<PointDTO, ErrorDTO>> GetById(int pointId);
        Task<Result<List<PointGridInfoDTO>, ErrorDTO>> GetPoints(PointFilterDTO filter);
        Task<Result<PointDTO, ErrorDTO>> CreatePoint(CreatePointDTO dto);
        Task<Result<string, ErrorDTO>> UpdatePoint(UpdatePointDTO dto);
        Task<Result<string, ErrorDTO>> DeletePoint(int id);
    }

    public class PointService : IPointService
    {
        private readonly ServiCarApiContext _context;
        private readonly ICurrentUserService _currentUserService;
        public PointService(ServiCarApiContext context, ICurrentUserService currentUserService)
        {
            _context = context;
            _currentUserService = currentUserService;
        }

        public async Task<Result<PointDTO, ErrorDTO>> GetById(int pointId)
        {
            try
            {
                var point = await _context.Points
                    .Where(p => p.Id == pointId &&
                                p.PointStatusId != PointStatus.Hide)
                    .Include(p => p.Categories)
                    .Include(p => p.Location)
                    .Include(p => p.Business)
                    .Include(p => p.WorkingTime)
                    .Include(p => p.User)
                    .Select(p => new PointDTO
                    {
                        Id = p.Id,
                        PointName = p.PointName,
                        IsAppointmentAvailable = p.IsAppointmentAvailable,
                        PointStatusId = p.PointStatusId,
                        Categories = p.Categories.Select(c => new CategoryDTO { Id = c.Id, Name = c.Name }).ToList(),
                        Location = new LocationDTO { Id = p.LocationId, District = p.Location.District, City = p.Location.City, Longitude = p.Location.Longitude, Latitude = p.Location.Latitude },
                        Business = new BusinessDTO { Id = p.BusinessId, Name = p.Business.Name, AboutUs = p.Business.AboutUs, PointsCount = p.Business.PointsCount, StatusId = p.Business.BusinessStatusId },
                        WorkingTime = new WorkingTimeDTO { Id = p.WorkingTime.Id, Name = p.WorkingTime.Name, StartTime = p.WorkingTime.StartTime, EndTime = p.WorkingTime.EndTime },
                        User = new UserDTO { Id = p.UserId, Email = p.User.Email, FirstName = p.User.FirstName, LastName = p.User.LastName },
                    })
                    .FirstOrDefaultAsync();

                if (point is null)
                {
                    var error = new ErrorDTO { StatusCode = HttpStatusCode.NotFound, Message = "Point not found." };
                    return Result<PointDTO, ErrorDTO>.Fail(error);
                }

                return Result<PointDTO, ErrorDTO>.Success(point);
            }
            catch (Exception ex)
            {
                var error = new ErrorDTO { StatusCode = HttpStatusCode.InternalServerError, Message = "Failed to get Point." };
                return Result<PointDTO, ErrorDTO>.Fail(error);
            }
        }

        public async Task<Result<List<PointGridInfoDTO>, ErrorDTO>> GetPoints(PointFilterDTO filter)
        {
            try
            {
                var query = _context.Points
                    .Include(p => p.Categories)
                    .Include(p => p.Location)
                    .Include(p => p.Business)
                        .ThenInclude(b => b.Image)
                    .Include(p => p.Reviews)
                    .Include(p => p.WorkingTime)
                    .Include(p => p.User)
                    .AsQueryable();

                // Apply filters conditionally
                if (filter.CategoryId > 0)
                {
                    query = query.Where(p => p.Categories.Any(c => c.Id == filter.CategoryId));
                }

                if (filter.BusinessId > 0)
                {
                    query = query.Where(p => p.BusinessId == filter.BusinessId);
                }

                if (filter.LocationId > 0)
                {
                    query = query.Where(p => p.LocationId == filter.LocationId);
                }


                var points = await query
                            .Select(p => new PointGridInfoDTO
                            {
                                Id = p.Id,
                                PointName = p.PointName,
                                Rating = p.Reviews.Any() 
                                        ? p.Reviews.Sum(r => r.Rating)/p.Reviews.Count()
                                        : 0,
                                Image = p.Business.Image.FileData,
                                WorkingTimeStart = p.WorkingTime.StartTime.ToShortTimeString(),
                                WorkingTimeEnd = p.WorkingTime.EndTime.ToShortTimeString()
                            })
                            .ToListAsync();

                return Result<List<PointGridInfoDTO>, ErrorDTO>.Success(points);
            }
            catch (Exception ex)
            {
                var error = new ErrorDTO { StatusCode = HttpStatusCode.BadRequest, Message = "Could not get points." };
                return Result<List<PointGridInfoDTO>, ErrorDTO>.Fail(error);
            }
        }

        public async Task<Result<PointDTO, ErrorDTO>> CreatePoint(CreatePointDTO dto)
        {
            try
            {
                var existingPoint = await _context.Points
                    .Where(x => x.PointName == dto.PointName)
                    .FirstOrDefaultAsync();

                if (existingPoint is not null)
                {
                    var error = new ErrorDTO { StatusCode = HttpStatusCode.BadRequest, Message = "Point already exists." };
                    return Result<PointDTO, ErrorDTO>.Fail(error);
                }


                var selectedCategories = await _context.Categories
                    .Where(c => dto.Categories.Contains(c.Id))
                    .ToListAsync();

                int.TryParse(_currentUserService.UserId, out int currentUserId);

                var point = new Point
                {
                    PointName = dto.PointName,
                    LocationId = dto.LocationId,
                    BusinessId = dto.BusinessId,
                    WorkingTimeId = dto.WorkingTimeId,
                    Categories = selectedCategories,
                    UserId = currentUserId
                };

                _context.Points.Add(point);
                await _context.SaveChangesAsync();

                var pointDTO = new PointDTO
                {
                    Id = point.Id,
                    PointName = point.PointName,
                    IsAppointmentAvailable = point.IsAppointmentAvailable,
                    PointStatusId = point.PointStatusId,
                    //LocationId = point.LocationId,
                    //CategoryId = point.CategoryId,
                    //BusinessId = point.BusinessId,
                    //WorkingTimeId = point.WorkingTimeId,
                    //UserId = point.UserId,
                };

                return Result<PointDTO, ErrorDTO>.Success(pointDTO);
            }
            catch (Exception ex)
            {
                var error = new ErrorDTO { StatusCode = HttpStatusCode.InternalServerError, Message = "Point creation failed." };
                return Result<PointDTO, ErrorDTO>.Fail(error);
            }
        }

        public async Task<Result<string, ErrorDTO>> UpdatePoint(UpdatePointDTO dto)
        {
            try
            {
                var point = await _context.Points
                    .Where(x => x.Id == dto.Id)
                    .FirstOrDefaultAsync();
                if (point is null)
                {
                    var error = new ErrorDTO { StatusCode = HttpStatusCode.Gone, Message = "Point not found." };
                    return Result<string, ErrorDTO>.Fail(error);
                }

                if (dto.PointName is not null)
                {
                    point.PointName = dto.PointName;
                }

                if (dto.IsAppointmentAvailable is not null)
                {
                    point.IsAppointmentAvailable = dto.IsAppointmentAvailable ?? false;
                }

                if (dto.PointStatusId is not null && dto.PointStatusId > 0)
                {
                    point.PointStatusId = dto.PointStatusId ?? 0;
                }

                if (dto.LocationId is not null && dto.LocationId > 0)
                {
                    point.LocationId = dto.LocationId ?? 0;
                }

                //if (dto.CategoryId is not null && dto.CategoryId > 0)
                //{
                //    point.CategoryId = dto.CategoryId ?? 0;
                //}

                if (dto.BusinessId is not null && dto.BusinessId > 0)
                {
                    point.BusinessId = dto.BusinessId ?? 0;
                }

                if (dto.WorkingTimeId is not null && dto.WorkingTimeId > 0)
                {
                    point.WorkingTimeId = dto.WorkingTimeId ?? 0;
                }

                if (dto.UserId is not null && dto.UserId > 0)
                {
                    point.UserId = dto.UserId ?? 0;
                }

                _context.Points.Update(point);
                await _context.SaveChangesAsync();

                return Result<string, ErrorDTO>.Success("Point updated successfully.");
            }
            catch (Exception ex)
            {
                var error = new ErrorDTO { StatusCode = HttpStatusCode.InternalServerError, Message = "Point update failed." };
                return Result<string, ErrorDTO>.Fail(error);
            }
        }

        public async Task<Result<string, ErrorDTO>> DeletePoint(int id)
        {
            try
            {
                var point = await _context.Points
                    .Where(x => x.Id == id)
                    .FirstOrDefaultAsync();

                if (point is null)
                {
                    var error = new ErrorDTO { StatusCode = HttpStatusCode.Gone, Message = "Point not found." };
                    return Result<string, ErrorDTO>.Fail(error);
                }

                point.PointStatusId = PointStatus.Hide;

                _context.Points.Update(point);
                await _context.SaveChangesAsync();

                return Result<string, ErrorDTO>.Success("Point deleted successfully.");
            }
            catch
            {
                var error = new ErrorDTO { StatusCode = HttpStatusCode.InternalServerError, Message = "Point deletion failed." };
                return Result<string, ErrorDTO>.Fail(error);
            }
        }
    }
}
