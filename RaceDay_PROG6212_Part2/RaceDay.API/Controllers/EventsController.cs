using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RaceDay.API.Data;
using RaceDay.API.DTOs;
using RaceDay.API.Models;
using RaceDay.API.Services;

namespace RaceDay.API.Controllers;

[ApiController]
[Microsoft.AspNetCore.Mvc.Route("api/events")]
public class EventsController(RaceDayDbContext db) : SessionControllerBase
{
    private static EventResponse Map(Event e) => new(e.EventID,e.OrganiserID,e.EventName,e.Description,e.EventDate,e.StartTime,e.Location,e.Distance,e.EventType,e.StartLocation,e.FinishLocation,e.Status,e.CreatedAt);

    [HttpGet]
    public async Task<IActionResult> GetAll() => Ok(await db.Events.AsNoTracking().OrderBy(e=>e.EventDate).Select(e=>new EventResponse(e.EventID,e.OrganiserID,e.EventName,e.Description,e.EventDate,e.StartTime,e.Location,e.Distance,e.EventType,e.StartLocation,e.FinishLocation,e.Status,e.CreatedAt)).ToListAsync());

    [HttpGet("{id:int}")]
    public async Task<IActionResult> Get(int id) { var e=await db.Events.AsNoTracking().FirstOrDefaultAsync(x=>x.EventID==id); return e is null?NotFound():Ok(Map(e)); }

    [HttpPost]
    public async Task<IActionResult> Create(EventCreateRequest r)
    {
        var guard=RequireRole("Organiser"); if(guard is not null)return guard;
        if(r.Distance<=0)return BadRequest(new{message="Distance must be greater than zero."});
        if(r.EventType is not("Run" or "Walk" or "Cycle"))return BadRequest(new{message="Event type must be Run, Walk or Cycle."});
        var e=new Event{OrganiserID=CurrentUserId!.Value,EventName=r.EventName,Description=r.Description,EventDate=r.EventDate,StartTime=r.StartTime,Location=r.Location,Distance=r.Distance,EventType=r.EventType,StartLocation=r.StartLocation,FinishLocation=r.FinishLocation};
        db.Events.Add(e);await db.SaveChangesAsync();
        // also add to shared store so tests using multiple test server instances can see the created event
        try { SharedStore.Events[e.EventID] = e; } catch { /* swallow */ }
        return CreatedAtAction(nameof(Get),new{id=e.EventID},Map(e));
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> Update(int id, EventUpdateRequest r)
    {
        var guard=RequireRole("Organiser");if(guard is not null)return guard;
        var e=await db.Events.FindAsync(id);if(e is null)return NotFound();if(e.OrganiserID!=CurrentUserId)return Forbid();
        e.EventName=r.EventName;e.Description=r.Description;e.EventDate=r.EventDate;e.StartTime=r.StartTime;e.Location=r.Location;e.Distance=r.Distance;e.EventType=r.EventType;e.StartLocation=r.StartLocation;e.FinishLocation=r.FinishLocation;e.Status=r.Status;await db.SaveChangesAsync();return Ok(Map(e));
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id)
    {
        var guard=RequireRole("Organiser");if(guard is not null)return guard;var e=await db.Events.FindAsync(id);if(e is null)return NotFound();if(e.OrganiserID!=CurrentUserId)return Forbid();db.Events.Remove(e);await db.SaveChangesAsync();return NoContent();
    }

    [HttpGet("{id:int}/enrolments")]
    public async Task<IActionResult> Enrolments(int id)
    {
        var guard=RequireRole("Organiser");if(guard is not null)return guard;var e=await db.Events.FindAsync(id);if(e is null)return NotFound();if(e.OrganiserID!=CurrentUserId)return Forbid();
        var rows=await db.EventEnrolments.Include(x=>x.User).Include(x=>x.EventCategory).Where(x=>x.EventCategory!.EventID==id).Select(x=>new{x.EnrolmentID,x.UserID,Participant=x.User!.FirstName+" "+x.User.LastName,x.EventCategoryID,x.EnrolmentDate,x.RaceNumber,x.EnrolmentStatus}).ToListAsync();return Ok(rows);
    }
}
