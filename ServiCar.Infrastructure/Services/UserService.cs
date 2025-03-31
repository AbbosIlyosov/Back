using Microsoft.AspNetCore.Identity;
using Servicar.Domain.DTOs;
using ServiCar.Domain.DTOs;
using ServiCar.Domain.Entities;
using ServiCar.Domain.Generics;
using System.Net;

namespace ServiCar.Infrastructure.Services
{
    public interface IUserService
    {
        Task<Result<string, ErrorDTO>> UpdateUserProfile(UserUpdateDTO dto);
        Task<Result<string, ErrorDTO>> DeleteUser(int userId);
        Task<Result<string, ErrorDTO>> AssignRole(AssignRoleDTO dto);
    }

    public class UserService : IUserService
    {
        private readonly UserManager<User> _userManager;
        public UserService(UserManager<User> userManager)
        {
            _userManager = userManager;
        }
        public async Task<Result<string, ErrorDTO>> DeleteUser(int userId)
        {
            try
            {
                var user = await _userManager.FindByIdAsync(userId.ToString());
                
                if (user == null)
                {
                    var error = new ErrorDTO
                    {
                        StatusCode = HttpStatusCode.BadRequest,
                        Message = "User not found."
                    };

                    return Result<string, ErrorDTO>.Fail(error);
                }

                var deleteResult = await _userManager.DeleteAsync(user);

                if (deleteResult != null)
                {
                    var error = new ErrorDTO
                    {
                        StatusCode = HttpStatusCode.BadRequest,
                        Message = "User could not be deleted."
                    };

                    return Result<string, ErrorDTO>.Fail(error);
                }

                return Result<string, ErrorDTO>.Success("User deleted successfully.");
            }
            catch (Exception ex)
            {
                var error = new ErrorDTO
                {
                    StatusCode = HttpStatusCode.InternalServerError,
                    Message = "Something went wrong."
                };

                return Result<string, ErrorDTO>.Fail(error);
            }
        }

        public Task<Result<string, ErrorDTO>> UpdateUserProfile(UserUpdateDTO dto)
        {
            throw new NotImplementedException();
        }

        public async Task<Result<string, ErrorDTO>> AssignRole(AssignRoleDTO dto)
        {
            try
            {
                var user = await _userManager.FindByIdAsync(dto.UserId.ToString());

                if (user == null)
                {
                    var error = new ErrorDTO
                    {
                        StatusCode = HttpStatusCode.BadRequest,
                        Message = "User not found."
                    };

                    return Result<string, ErrorDTO>.Fail(error);
                }

                var result = await _userManager.AddToRoleAsync(user, dto.Role);

                if (!result.Succeeded)
                {
                    var error = new ErrorDTO
                    {
                        StatusCode = HttpStatusCode.BadRequest,
                        Message = result.Errors.FirstOrDefault()?.Description ?? "Failed to assign role."
                    };

                    return Result<string, ErrorDTO>.Fail(error);
                }

                return Result<string, ErrorDTO>.Success("Role assigned sucessfully");
            }
            catch (Exception ex)
            {
                var error = new ErrorDTO
                {
                    StatusCode = HttpStatusCode.InternalServerError,
                    Message = "Something went wrong. Please check the information and try again."
                };

                return Result<string, ErrorDTO>.Fail(error);
            }
        }
    }
}
