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
        Task<Result<List<ReviewDTO>, ErrorDTO>> GetFiltered(ReviewFilterDTO filter);
        Task<Result<ReviewDTO, ErrorDTO>> CreateReview(ReviewCreateDTO dto);
        Task<ReviewDTO?> GetById(int id);
    }

    public class ReviewService : IReviewService
    {
        private readonly ServiCarApiContext _context;
        public ReviewService(ServiCarApiContext context)
        {
            _context = context;
        }
        public async Task<Result<List<ReviewDTO>, ErrorDTO>> GetFiltered(ReviewFilterDTO filter)
        {
            try
            {
                if (filter.SenderId > 0 || filter.ReceiverId > 0) 
                {
                    var reviews = await _context.Reviews
                    .Where(x => filter.SenderId == x.UserId ||
                                filter.ReceiverId == x.PointId)
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

        public async Task<Result<ReviewDTO, ErrorDTO>> CreateReview(ReviewCreateDTO dto)
        {
            try
            {
                var newReview = new Review
                {
                    Comment = dto.Comment,
                    UserId = dto.UserId,
                    PointId = dto.PointId,
                    AppointmentId = dto.AppointmentId,
                };

                _context.Reviews.Add(newReview);
                await _context.SaveChangesAsync();


                var review = await GetById(newReview.Id);
                
                if (review == null) 
                {
                    var error = new ErrorDTO
                    {
                        StatusCode = HttpStatusCode.Created,
                        Message = "Review was created but failed to retrieve the newly created record."
                    };

                    return Result<ReviewDTO, ErrorDTO>.Fail(error);
                }

                return Result<ReviewDTO, ErrorDTO>.Success(review);
            }
            catch (Exception ex) 
            {
                var error = new ErrorDTO
                {
                    StatusCode = HttpStatusCode.InternalServerError,
                    Message = "Failed to create review."
                };

                return Result<ReviewDTO, ErrorDTO>.Fail(error);
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
    }
}
