using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using ServiCar.Domain.DTOs;
using ServiCar.Domain.Entities;
using ServiCar.Domain.Generics;
using ServiCar.Infrastructure.Persistence;
using System.Net;

namespace ServiCar.Infrastructure.Services
{
    public interface IReviewService
    {
        Task<Result<List<ReviewDTO>, ErrorDTO>> GetAll();
        Task<Result<List<ReviewDTO>, ErrorDTO>> GetFiltered(ReviewFilterDTO filter);
        Task<Result<bool, ErrorDTO>> CreateReview(ReviewCreateDTO dto);
        Task<ReviewDTO?> GetById(int id);
    }

    public class ReviewService : IReviewService
    {
        private readonly ServiCarApiContext _context;
        private readonly ICurrentUserService _currentUserService;
        private readonly UserManager<User> _userManager;
        public ReviewService(ServiCarApiContext context, ICurrentUserService currentUserService, UserManager<User> userManager)
        {
            _context = context;
            _currentUserService = currentUserService;
            _userManager = userManager;
        }
        public async Task<Result<List<ReviewDTO>, ErrorDTO>> GetFiltered(ReviewFilterDTO filter)
        {
            try
            {
                if (filter.UserId > 0 || filter.PointId > 0) 
                {
                    var reviews = await _context.Reviews
                    .Where(x => filter.UserId == x.UserId ||
                                filter.PointId == x.PointId)
                    .Include(x => x.User)
                    .Include(x => x.Point)
                    .Include(x => x.Appointment)
                    .Select(r => new ReviewDTO
                    {
                        Id = r.Id,
                        UserId = r.User.Id,
                        PointId = r.Point.Id,
                        AppointmentId = r.AppointmentId,
                        Comment = r.Comment,
                        Rating = r.Rating,

                        User = $"{r.User.FirstName} {r.User.LastName}",
                        Point = r.Point.PointName
                    })
                    .ToListAsync();

                    return Result<List<ReviewDTO>, ErrorDTO>.Success(reviews);
                }
                else
                {
                    var error = new ErrorDTO
                    {
                        StatusCode = HttpStatusCode.BadRequest,
                        Message = "The filter criteria is not valid."
                    };

                    return Result<List<ReviewDTO>, ErrorDTO>.Fail(error);
                }
            }
            catch (Exception ex)
            {
                var error = new ErrorDTO
                {
                    StatusCode = HttpStatusCode.InternalServerError,
                    Message = "Failed to get reviews."
                };
                return Result<List<ReviewDTO>, ErrorDTO>.Fail(error);
            }
        }

        public async Task<Result<bool, ErrorDTO>> CreateReview(ReviewCreateDTO dto)
        {
            try
            {
                int.TryParse(_currentUserService.UserId, out int currentUserId);

                if (currentUserId == 0) 
                {
                    var error = new ErrorDTO
                    {
                        StatusCode = HttpStatusCode.BadRequest,
                        Message = "User not found."
                    };
                    return Result<bool, ErrorDTO>.Fail(error);
                }

                var newReview = new Review
                {
                    Comment = dto.Comment,
                    Rating = dto.Rating,
                    UserId = currentUserId,
                    PointId = dto.PointId,
                    AppointmentId = dto.AppointmentId,
                };

                _context.Reviews.Add(newReview);
                await _context.SaveChangesAsync();

                return Result<bool, ErrorDTO>.Success(true);
            }
            catch (Exception ex) 
            {
                var error = new ErrorDTO
                {
                    StatusCode = HttpStatusCode.InternalServerError,
                    Message = "Failed to create review."
                };

                return Result<bool, ErrorDTO>.Fail(error);
            }
        }

        public async Task<ReviewDTO?> GetById(int id)
        {
            var review = await _context.Reviews.
                Where(x => x.Id == id)
                .Include(x => x.User)
                .Include(x => x.Point)
                .Include(x =>  x.Appointment)
                .Select(r => new ReviewDTO
                {
                    Id = r.Id,
                    UserId = r.User.Id,
                    PointId = r.Point.Id,
                    AppointmentId = r.AppointmentId,
                    Comment = r.Comment,
                    Rating = r.Rating,

                    User = $"{r.User.FirstName} {r.User.LastName}",
                    Point = r.Point.PointName
                })
                .FirstOrDefaultAsync();
            
            return review;
        }

        public async Task<Result<List<ReviewDTO>, ErrorDTO>> GetAll()
        {
            try
            {
                int.TryParse(_currentUserService.UserId, out int currentUserId);

                if(currentUserId == 0)
                {
                    var error = new ErrorDTO
                    {
                        StatusCode = HttpStatusCode.BadRequest,
                        Message = "User not found."
                    };
                    return Result<List<ReviewDTO>, ErrorDTO>.Fail(error);
                }

                var query = _context.Reviews
                    .Include(x => x.User)
                    .Include(x => x.Point)
                    .Include(x => x.Appointment)
                    .AsQueryable();

                // if current user is worker
                if (!string.IsNullOrEmpty(_currentUserService.UserRole) && _currentUserService.UserRole.ToLower() == "worker")
                {
                    // return only reviews for points that the worker is associated with 

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
                    // return reviews added by the user only
                    query = query.Where(x => x.UserId == currentUserId);
                }

                var reviews = await query
                    .Select(r => new ReviewDTO
                    {
                        Id = r.Id,
                        UserId = r.User.Id,
                        PointId = r.Point.Id,
                        AppointmentId = r.AppointmentId,
                        Comment = r.Comment,
                        Rating = r.Rating,
                        User = $"{r.User.FirstName} {r.User.LastName}",
                        Point = r.Point.PointName
                    })
                    .ToListAsync();

                return Result<List<ReviewDTO>, ErrorDTO>.Success(reviews);
            }
            catch (Exception ex)
            {
                var error = new ErrorDTO
                {
                    StatusCode = HttpStatusCode.InternalServerError,
                    Message = "Failed to get reviews."
                };
                return Result<List<ReviewDTO>, ErrorDTO>.Fail(error);
            }
        }
    }
}
