using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RaceDay.API.Data;
using RaceDay.API.DTOs;
using RaceDay.API.Services;

namespace RaceDay.API.Controllers;

[ApiController]
[Microsoft.AspNetCore.Mvc.Route("api/users") ]
public class UsersController(RaceDayDbContext db) : SessionControllerBase
{
    [HttpGet("me")]
    public async Task<ActionResult<ProfileResponse>> GetProfile()
    {
        var guard = RequireLogin(); if (guard is not null) return guard;
        var u = await db.Users.FindAsync(CurrentUserId!.Value); if (u is null) return NotFound();
        return Ok(new ProfileResponse(u.UserID,u.FirstName,u.LastName,u.Email,u.PhoneNumber,u.Role,u.CreatedAt));
    }

    [HttpPut("me")]
    public async Task<ActionResult<ProfileResponse>> UpdateProfile(UpdateProfileRequest request)
    {
        var guard = RequireLogin(); if (guard is not null) return guard;
        var u = await db.Users.FindAsync(CurrentUserId!.Value); if (u is null) return NotFound();
        u.FirstName=request.FirstName.Trim(); u.LastName=request.LastName.Trim(); u.PhoneNumber=request.PhoneNumber;
        await db.SaveChangesAsync();
        return Ok(new ProfileResponse(u.UserID,u.FirstName,u.LastName,u.Email,u.PhoneNumber,u.Role,u.CreatedAt));
    }

    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetUser(int id)
    {
        var guard = RequireLogin(); if (guard is not null) return guard;
        if (id != CurrentUserId) return Forbid();
        var u=await db.Users.AsNoTracking().FirstOrDefaultAsync(x=>x.UserID==id); if(u is null)return NotFound();
        return Ok(new ProfileResponse(u.UserID,u.FirstName,u.LastName,u.Email,u.PhoneNumber,u.Role,u.CreatedAt));
    }
}
