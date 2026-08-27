using Microsoft.EntityFrameworkCore;
using Maxgram.Backend.Data;
using Maxgram.Backend.Dto;
using Maxgram.Backend.Services.Interfaces;

namespace Maxgram.Backend.Services;

public class UserService : IUserService
{
    private readonly MaxgramDbContext _db;
    public UserService(MaxgramDbContext db)
    {
        _db = db;
    }

    public async Task<List<UserSearchDto>> SearchAsync(int currentUserId, string? search)
    {
        var query = _db.Users.Where(u => u.Id != currentUserId);

        if (!string.IsNullOrWhiteSpace(search))
        {
            query = query.Where(u =>
                u.Username.Contains(search) || u.DisplayName.Contains(search));
        }

        return await query
            .Select(u => new UserSearchDto(u.Id, u.Username, u.DisplayName))
            .ToListAsync();
    }
}
