using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Maxgram.Backend.Data;
using Maxgram.Backend.Entities;
using Maxgram.Backend.Dto;
using Maxgram.Backend.Common;
using Maxgram.Backend.Services.Interfaces;

namespace Maxgram.Backend.Services;

public class AuthService : IAuthService
{
    private readonly MaxgramDbContext _db;
    private readonly PasswordHasher<User> _hasher = new();

    public AuthService(MaxgramDbContext db)
    {
        _db = db;
    }

    public async Task<ServiceResult<UserDto>> RegisterAsync(RegisterRequest request)
    {
        if (request.Password != request.ConfirmPassword)
            return ServiceResult<UserDto>.Fail(ErrorCode.BadRequest, "Пароли не совпадают");

        var usernameTaken = await _db.Users.AnyAsync(u => u.Username == request.Username);
        if (usernameTaken)
            return ServiceResult<UserDto>.Fail(ErrorCode.Conflict, "Username уже занят");

        var emailTaken = await _db.Users.AnyAsync(u => u.Email == request.Email);
        if (emailTaken)
            return ServiceResult<UserDto>.Fail(ErrorCode.Conflict, "Email уже занят");

        var user = new User
        {
            Username = request.Username,
            DisplayName = request.DisplayName,
            Email = request.Email
        };
        user.PasswordHash = _hasher.HashPassword(user, request.Password);

        _db.Users.Add(user);
        await _db.SaveChangesAsync();

        return ServiceResult<UserDto>.Ok(new UserDto(user.Id, user.Username, user.DisplayName, user.Email));
    }

    public async Task<User?> ValidateLoginAsync(LoginRequest request)
    {
        var user = await _db.Users.FirstOrDefaultAsync(u =>
            u.Username == request.UsernameOrEmail || u.Email == request.UsernameOrEmail);

        if (user == null) return null;

        var result = _hasher.VerifyHashedPassword(user, user.PasswordHash, request.Password);
        return result == PasswordVerificationResult.Failed ? null : user;
    }

    public async Task<UserDto?> GetByIdAsync(int id)
    {
        var user = await _db.Users.FindAsync(id);
        return user == null ? null : new UserDto(user.Id, user.Username, user.DisplayName, user.Email);
    }
}
