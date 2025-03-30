using Microsoft.EntityFrameworkCore;
using ServiCar.Domain.DTOs;
using ServiCar.Domain.Entities;
using ServiCar.Domain.Generics;
using ServiCar.Infrastructure.Persistence;
using System.Net;

namespace ServiCar.Infrastructure.Services
{
    public interface IBusinessService
    {
        Task<Result<List<BusinessDTO>, ErrorDTO>> GetAllBusinesses();
        Task<Result<BusinessDTO, ErrorDTO>> CreateBusiness(BusinessCreateDTO dto);
        Task<Result<string, ErrorDTO>> UpdateBusiness(BusinessUpdateDTO dto);
        Task<Result<string, ErrorDTO>> UpdateBusinessStatus(BusinessStatusUpdateDTO dto);
    }
    public class BusinessService : IBusinessService
    {
        private readonly ServiCarApiContext _context;
        public BusinessService(ServiCarApiContext context)
        {
            _context = context;
        }

        public async Task<Result<List<BusinessDTO>, ErrorDTO>> GetAllBusinesses()
        {
            try
            {
                var businesses = await _context.Businesses.Select(x => new BusinessDTO
                {
                    Id = x.Id,
                    Name = x.Name,
                    AboutUs = x.AboutUs,
                    Image = x.Image.FileData,
                    PointsCount = x.PointsCount,
                    StatusId = x.BusinessStatusId
                }).ToListAsync();

                return Result<List<BusinessDTO>, ErrorDTO>.Success(businesses);
            }
            catch (Exception ex) 
            {
                var error = new ErrorDTO { StatusCode = HttpStatusCode.BadRequest, Message = "Could not get businesses."};
                return Result<List<BusinessDTO>, ErrorDTO>.Fail(error);
            }
        }

        public async Task<Result<BusinessDTO, ErrorDTO>> CreateBusiness(BusinessCreateDTO dto)
        {
            try
            {
                var business = new Business
                {
                    Name = dto.Name,
                    ImageId = dto.ImageId,
                    AboutUs = dto.AboutUs,
                    Categories = dto.Categories.Select(c => new Category { Name = c.Name }).ToList()
                };

                _context.Businesses.Add(business);
                await _context.SaveChangesAsync();

                var businessDTO = new BusinessDTO
                {
                    Id = business.Id,
                    Name = business.Name,
                    Image = business.Image.FileData,
                    AboutUs = business.AboutUs,
                    PointsCount = business.PointsCount,
                    StatusId = business.BusinessStatusId
                };  

                return Result<BusinessDTO, ErrorDTO>.Success(businessDTO);
            }
            catch(Exception ex)
            {
                var error = new ErrorDTO { StatusCode = HttpStatusCode.BadRequest, Message = "Business could not be created." };
                return Result<BusinessDTO, ErrorDTO>.Fail(error);
            }
        }

        public async Task<Result<string, ErrorDTO>> UpdateBusiness(BusinessUpdateDTO dto)
        {
            try
            {
                var business = await _context.Businesses.FirstOrDefaultAsync(x => x.Id == dto.Id);

                if (business is null)
                {
                    var error = new ErrorDTO { StatusCode = HttpStatusCode.NotFound, Message = "Business not found." };
                    return Result<string, ErrorDTO>.Fail(error);
                }

                if (dto.Name is not null) { business.Name = dto.Name; }
                if (dto.ImageId is not null) { business.ImageId = dto.ImageId; }
                if (dto.AboutUs is not null) { business.AboutUs = dto.AboutUs; }

                _context.Businesses.Update(business);
                await _context.SaveChangesAsync();

                return Result<string, ErrorDTO>.Success("Business updated successfully.");
            }
            catch (Exception ex)
            {
                var error = new ErrorDTO { StatusCode = HttpStatusCode.BadRequest, Message = "Business could not be updated." };
                return Result<string, ErrorDTO>.Fail(error);
            }
        }

        public async Task<Result<string, ErrorDTO>> UpdateBusinessStatus(BusinessStatusUpdateDTO dto)
        {
            try
            {
                var business = await _context.Businesses.FirstOrDefaultAsync(x => x.Id == dto.Id);

                if (business is null)
                {
                    var error = new ErrorDTO { StatusCode = HttpStatusCode.NotFound, Message = "Business not found." };
                    return Result<string, ErrorDTO>.Fail(error);
                }

                business.BusinessStatusId = dto.BusinessStatusId;
                _context.Businesses.Update(business);
                await _context.SaveChangesAsync();

                return Result<string, ErrorDTO>.Success("Status updated successfully.");
            }
            catch (Exception ex)
            {
                var error = new ErrorDTO { StatusCode = HttpStatusCode.BadRequest, Message = "Status could not be updated." };
                return Result<string, ErrorDTO>.Fail(error);
            }
        }
    }
}
