namespace StudentWorkplace.Services;

using Data;
using Data.Entities;



public class ScheduleService
{
	private readonly ApplicationDbContext _applicationDbContext;

	private readonly UserService _userService;

	public ScheduleService(ApplicationDbContext applicationDbContext,
		UserService userService)
	{
		_applicationDbContext = applicationDbContext;
		_userService = userService;
	}

	public IEnumerable<Schedule> GetSchedules(bool asNoTracking = false)
	{
		var schedules = _applicationDbContext.Schedules
			.Include(schedule => schedule.Users)
			.Include(schedule => schedule.Reminders);

		return asNoTracking
			? schedules.AsNoTracking()
			: schedules;
	}

	public IEnumerable<Schedule> GetSchedulesByCurrentUser()
	{
		var user = _userService.GetCurrentUser();

		var schedules =  GetSchedules().ToReadOnlyCollection();

		return schedules.Where(schedule => schedule.Users != null! && schedule.Users.Contains(user));
	}

	public IEnumerable<Schedule> GetSchedulesByPeriod(DateTime startDateTime, DateTime endDateTime)
	{
		return GetSchedules(asNoTracking: false)
			.Where(s => s.DateTime >= startDateTime && s.DateTime <= endDateTime);
	}

	public Schedule GetSchedule(int id)
	{
		return GetSchedules()
			.FirstOrDefault(schedule => schedule.SсheduleId == id)!;
	}

	public void AddOrUpdateScheduleWithSave(Schedule schedule)
	{
		if (schedule.SсheduleId > 0)
		{
			UpdateScheduleWithSave(schedule);

			return;
		}

		_applicationDbContext.Schedules.Add(schedule);
		_applicationDbContext.SaveChanges();
	}

	public void UpdateScheduleWithSave(Schedule schedule)
	{
		_applicationDbContext.Schedules.Update(schedule);
		_applicationDbContext.SaveChanges();
	}

	public void DeleteSchedule(Schedule schedule)
	{
		_applicationDbContext.Remove(schedule);
		_applicationDbContext.SaveChanges();
	}
}