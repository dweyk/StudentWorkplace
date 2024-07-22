namespace StudentWorkplace.Services.HostedServices;

using System.Threading;

using Data.Entities;

public class ReminderBackgroundService : IDisposable
{
	private readonly Timer _timer;

	private readonly IServiceProvider _serviceProvider;

	public ReminderBackgroundService()
	{
		_serviceProvider = ViewModelLocator._provider!;
		_timer = new Timer(CheckSchedules, null, TimeSpan.Zero, TimeSpan.FromMinutes(5));
	}

	private void CheckSchedules(object? state)
	{
		using var scope = _serviceProvider.CreateScope();

		var _scheduleService = scope.ServiceProvider.GetRequiredService<ScheduleService>();
		var _reminderService = scope.ServiceProvider.GetRequiredService<ReminderService>();

		var now = DateTime.UtcNow;
		var endOfTomorrow = now.Date.AddDays(2).AddTicks(-1);

		var schedules = _scheduleService.GetSchedulesByPeriod(now, endOfTomorrow);

		foreach (var schedule in schedules)
		{
			var timeBeforeEvent = (schedule.DateTime - now);
			var reminderMessage =
				$"Событие {schedule.Title} произойдет через {timeBeforeEvent.Days} дней, " +
				$"{timeBeforeEvent.Hours} часов, {timeBeforeEvent.Minutes} минут.";

			var reminder = new Reminder
			{
				ReminderMessage = reminderMessage,
				Schedule = schedule,
				ScheduleId = schedule.SсheduleId,
			};

			_reminderService.DeletePreviousReminder(reminder.ScheduleId);

			_reminderService.AddOrUpdateReminderWithSave(reminder);
		}
	}

	public void Dispose()
	{
		_timer.Dispose();
	}
}