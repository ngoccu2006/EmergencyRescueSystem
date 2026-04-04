using Microsoft.AspNetCore.Identity;
using RescueSystem.Application.DTOs.User;
using RescueSystem.Domain.Entities;

namespace RescueSystem.Application.Services
{
    public interface IUserService
    {
        Task<(bool Success, string Message)> CreateUserAsync(CreateUserDto createUserDto);
        Task<UserDto?> GetUserByIdAsync(Guid userId);
        Task<List<UserDto>> GetAllUsersAsync();
        Task<(bool Success, string Message)> UpdateUserAsync(Guid userId, UpdateUserDto updateUserDto);
        Task<(bool Success, string Message)> DeleteUserAsync(Guid userId);
        Task<(bool Success, string Message)> ChangePasswordAsync(Guid userId, string currentPassword, string newPassword);
    }

    public class UserService : IUserService
    {
        private readonly UserManager<User> _userManager;
        private readonly RoleManager<Role> _roleManager;

        public UserService(UserManager<User> userManager, RoleManager<Role> roleManager)
        {
            _userManager = userManager;
            _roleManager = roleManager;
        }

        public async Task<(bool Success, string Message)> CreateUserAsync(CreateUserDto createUserDto)
        {
            try
            {
                var user = new User
                {
                    Id = Guid.NewGuid(),
                    UserName = createUserDto.UserName,
                    Email = createUserDto.Email,
                    FullName = createUserDto.FullName,
                    PhoneNumber = createUserDto.PhoneNumber,
                    Address = createUserDto.Address,
                    DateOfBirth = createUserDto.DateOfBirth,
                    Avatar = createUserDto.Avatar,
                    IsActive = true,
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow
                };

                var result = await _userManager.CreateAsync(user, createUserDto.Password);

                if (!result.Succeeded)
                {
                    var errors = string.Join(", ", result.Errors.Select(e => e.Description));
                    return (false, $"Failed to create user: {errors}");
                }

                // Add roles if provided
                if (createUserDto.Roles.Any())
                {
                    var roleResult = await _userManager.AddToRolesAsync(user, createUserDto.Roles);
                    if (!roleResult.Succeeded)
                    {
                        var errors = string.Join(", ", roleResult.Errors.Select(e => e.Description));
                        return (false, $"User created but failed to assign roles: {errors}");
                    }
                }

                return (true, "User created successfully");
            }
            catch (Exception ex)
            {
                return (false, $"Error creating user: {ex.Message}");
            }
        }

        public async Task<UserDto?> GetUserByIdAsync(Guid userId)
        {
            try
            {
                var user = await _userManager.FindByIdAsync(userId.ToString());
                if (user == null)
                    return null;

                var roles = await _userManager.GetRolesAsync(user);

                return new UserDto
                {
                    Id = user.Id,
                    FullName = user.FullName,
                    Email = user.Email ?? string.Empty,
                    UserName = user.UserName ?? string.Empty,
                    PhoneNumber = user.PhoneNumber,
                    Address = user.Address,
                    DateOfBirth = user.DateOfBirth,
                    Avatar = user.Avatar,
                    IsActive = user.IsActive,
                    CreatedAt = user.CreatedAt,
                    UpdatedAt = user.UpdatedAt,
                    Roles = roles.ToList()
                };
            }
            catch (Exception)
            {
                return null;
            }
        }

        public async Task<List<UserDto>> GetAllUsersAsync()
        {
            try
            {
                var users = _userManager.Users.ToList();
                var userDtos = new List<UserDto>();

                foreach (var user in users)
                {
                    var roles = await _userManager.GetRolesAsync(user);
                    userDtos.Add(new UserDto
                    {
                        Id = user.Id,
                        FullName = user.FullName,
                        Email = user.Email ?? string.Empty,
                        UserName = user.UserName ?? string.Empty,
                        PhoneNumber = user.PhoneNumber,
                        Address = user.Address,
                        DateOfBirth = user.DateOfBirth,
                        Avatar = user.Avatar,
                        IsActive = user.IsActive,
                        CreatedAt = user.CreatedAt,
                        UpdatedAt = user.UpdatedAt,
                        Roles = roles.ToList()
                    });
                }

                return userDtos;
            }
            catch (Exception)
            {
                return new List<UserDto>();
            }
        }

        public async Task<(bool Success, string Message)> UpdateUserAsync(Guid userId, UpdateUserDto updateUserDto)
        {
            try
            {
                var user = await _userManager.FindByIdAsync(userId.ToString());
                if (user == null)
                    return (false, "User not found");

                user.FullName = updateUserDto.FullName;
                user.PhoneNumber = updateUserDto.PhoneNumber;
                user.Address = updateUserDto.Address;
                user.DateOfBirth = updateUserDto.DateOfBirth;
                user.Avatar = updateUserDto.Avatar;
                user.IsActive = updateUserDto.IsActive;
                user.UpdatedAt = DateTime.UtcNow;

                var result = await _userManager.UpdateAsync(user);
                if (!result.Succeeded)
                {
                    var errors = string.Join(", ", result.Errors.Select(e => e.Description));
                    return (false, $"Failed to update user: {errors}");
                }

                // Update roles if provided
                if (updateUserDto.Roles.Any())
                {
                    var currentRoles = await _userManager.GetRolesAsync(user);
                    await _userManager.RemoveFromRolesAsync(user, currentRoles);
                    var roleResult = await _userManager.AddToRolesAsync(user, updateUserDto.Roles);
                    if (!roleResult.Succeeded)
                    {
                        var errors = string.Join(", ", roleResult.Errors.Select(e => e.Description));
                        return (false, $"User updated but failed to assign roles: {errors}");
                    }
                }

                return (true, "User updated successfully");
            }
            catch (Exception ex)
            {
                return (false, $"Error updating user: {ex.Message}");
            }
        }

        public async Task<(bool Success, string Message)> DeleteUserAsync(Guid userId)
        {
            try
            {
                var user = await _userManager.FindByIdAsync(userId.ToString());
                if (user == null)
                    return (false, "User not found");

                var result = await _userManager.DeleteAsync(user);
                if (!result.Succeeded)
                {
                    var errors = string.Join(", ", result.Errors.Select(e => e.Description));
                    return (false, $"Failed to delete user: {errors}");
                }

                return (true, "User deleted successfully");
            }
            catch (Exception ex)
            {
                return (false, $"Error deleting user: {ex.Message}");
            }
        }

        public async Task<(bool Success, string Message)> ChangePasswordAsync(Guid userId, string currentPassword, string newPassword)
        {
            try
            {
                var user = await _userManager.FindByIdAsync(userId.ToString());
                if (user == null)
                    return (false, "User not found");

                var result = await _userManager.ChangePasswordAsync(user, currentPassword, newPassword);
                if (!result.Succeeded)
                {
                    var errors = string.Join(", ", result.Errors.Select(e => e.Description));
                    return (false, $"Failed to change password: {errors}");
                }

                return (true, "Password changed successfully");
            }
            catch (Exception ex)
            {
                return (false, $"Error changing password: {ex.Message}");
            }
        }
    }
}
