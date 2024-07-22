namespace StudentWorkplace.ViewModels.Schedules;

using Data.Entities;

using Services;

public class ScheduleViewingPageViewModel : BindableBase
{
	public ObservableCollection<Schedule> Schedules { get; set; }

	public ScheduleViewingPageViewModel(ScheduleService scheduleService)
	{
		// Загрузка расписаний из базы данных для конкретного пользователя.
		Schedules = new ObservableCollection<Schedule>(scheduleService.GetSchedulesByCurrentUser());
	}
}