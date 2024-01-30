using System.Net;
using Domain.Entities;
using Domain.Filters;
using Domain.Filters.User;
using Domain.Models.User;
using Domain.Wrappers;
using Infrastructure.Persistence.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Infrastructure.Services.UserServices;

public class UserService(ApplicationDbContext context, ILogger<UserService> logger) : IUserService
{
    public async Task<PagedResponse<List<UserDto>>> GetUsers(UserFilter filter)
    {
        var validFilter = new UserFilter(filter.PageNumber, filter.PageSize);
        
        var query = context.Users.AsNoTracking().AsQueryable();

        if (filter.Name != null)
        {
            query = query.Where(u => string.Concat(u.FirstName, u.LastName)
                .ToLower().Trim().Contains(filter.Name.ToLower().Trim()));
        }

        if (filter.PhoneNumber != null)
        {
            query = query.Where(x => x.PhoneNumber == filter.PhoneNumber);
        }
        
        var data = await (
            from user in query
            select new UserDto()
            {
                Id = user.Id,
                FirstName = user.FirstName,
                LastName = user.LastName,
                PhoneNumber = user.PhoneNumber
            })
            .Skip((validFilter.PageNumber - 1) * validFilter.PageSize)
            .Take(validFilter.PageSize)
            .AsNoTracking()
            .ToListAsync();
        var totalRecords = data.Count;
        return new PagedResponse<List<UserDto>>(data, totalRecords, validFilter.PageNumber, validFilter.PageSize);
    }

    public async Task<Response<UserDto>> GetUserById(int id)
    {
        var user = await context.Users.FirstOrDefaultAsync(u => u.Id == id);
        if (user == null)
            return new Response<UserDto>(HttpStatusCode.BadRequest, "User not found");
        
        return new Response<UserDto>(new UserDto()
        {
            Id = user.Id,
            FirstName = user.FirstName,
            LastName = user.LastName,
            PhoneNumber = user.PhoneNumber
        });
    }

    public async Task<Response<UserDto>> AddUser(UserDto userDto)
    {
        try
        {
            var user = new User()
            {
                FirstName = userDto.FirstName,
                LastName = userDto.LastName,
                PhoneNumber = userDto.PhoneNumber
            };

            await context.Users.AddAsync(user);
            await context.SaveChangesAsync();

            return new Response<UserDto>(userDto);
        }
        catch (Exception ex)
        {
            logger.LogError(ex.Message);
            return new Response<UserDto>(HttpStatusCode.InternalServerError, "Internal server error");
        }
    }

    public async Task<Response<UserDto>> UpdateUser(UserDto userDto)
    {
        try
        {
            var user = await context.Users.FirstOrDefaultAsync(u => u.Id == userDto.Id);

            if (user == null)
                return new Response<UserDto>(HttpStatusCode.BadRequest, "User not found");

            user.FirstName = userDto.FirstName;
            user.LastName = userDto.LastName;
            user.PhoneNumber = userDto.PhoneNumber;

            await context.SaveChangesAsync();

            return new Response<UserDto>(userDto);
        }
        catch (Exception ex)
        {
            logger.LogError(ex.Message);
            return new Response<UserDto>(HttpStatusCode.InternalServerError, "Internal server error");
        }
    }

    public async Task<Response<string>> DeleteUser(int id)
    {
        try
        {
            var user = await context.Users.FirstOrDefaultAsync(u => u.Id == id);

            if (user == null)
                return new Response<string>(HttpStatusCode.BadRequest, "User not found");

            context.Users.Remove(user);
            await context.SaveChangesAsync();

            return new Response<string>("User deleted successfully");
        }
        catch (Exception ex)
        {
            logger.LogError(ex.Message);
            return new Response<string>(HttpStatusCode.InternalServerError, "Internal server error");
        }
    }
}
