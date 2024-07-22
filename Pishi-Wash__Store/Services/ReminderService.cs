namespace StudentWorkplace.Services;

using Data;
using Data.Entities;



public class ReminderService
{
	private readonly ApplicationDbContext _applicationDbContext;

	private readonly UserService _userService;

	public ReminderService(ApplicationDbContext applicationDbContext,
		UserService userService)
	{
		_applicationDbContext = applicationDbContext;
		_userService = userService;
	}

	public IEnumerable<Reminder> GetReminders()
	{
		return _applicationDbContext.Reminders
			.Include(reminder => reminder.Schedule)
			.ThenInclude(schedule => schedule.Users)
			.AsTracking();
	}

	public IEnumerable<Reminder> GetRemindersByCurrentUser()
	{
		var user = _userService.GetCurrentUser();

		var reminders = GetReminders().ToReadOnlyCollection();

		return reminders.Where(reminder => reminder.Schedule.Users.Contains(user));
	}

	public Reminder GetReminder(int id)
	{
		return GetReminders()
			.FirstOrDefault(reminder => reminder.ReminderId == id)!;
	}

	public void DeletePreviousReminder(int sheduleId)
	{
		var reminders = GetReminders()
				.Where(remind => remind.ScheduleId == sheduleId);

		_applicationDbContext.Reminders.RemoveRange(reminders);
	}

	public void AddOrUpdateReminderWithSave(Reminder reminder)
	{
		if (reminder.ReminderId > 0)
		{
			UpdateReminderWithSave(reminder);

			return;
		}

		_applicationDbContext.Reminders.Add(reminder);
		_applicationDbContext.SaveChanges();
	}

	public void UpdateReminderWithSave(Reminder reminder)
	{
		_applicationDbContext.Reminders.Update(reminder);
		_applicationDbContext.SaveChanges();
	}

	public void DeleteReminder(Reminder reminder)
	{
		_applicationDbContext.Reminders.Remove(reminder);
		_applicationDbContext.SaveChanges();
	}
}