namespace StudentWorkplace.ViewModels.Reminder;

using Data.Entities;

using Services;

public class ReminderViewingPageViewModel : BindableBase
{
	public ObservableCollection<Reminder> Reminders { get; set; }

	public ReminderViewingPageViewModel(ReminderService reminderService)
	{
		// Загрузка расписаний из базы данных для конкретного пользователя.
		Reminders = new ObservableCollection<Reminder>(reminderService.GetRemindersByCurrentUser());
	}
}