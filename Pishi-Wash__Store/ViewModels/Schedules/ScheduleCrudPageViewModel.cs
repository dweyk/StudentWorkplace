namespace StudentWorkplace.ViewModels.Schedules;

using System.ComponentModel;

using Data.Entities;

using Services;

using Views.Schedules;

public class ScheduleCrudPageViewModel : BindableBase
{
	private readonly ScheduleService _scheduleService;

	public Schedule SelectedSchedule { get; set; } = null!;

	public ObservableCollection<Schedule> Schedules { get; set; }

	public ScheduleCrudPageViewModel(ScheduleService scheduleService)
	{
		_scheduleService = scheduleService;

		// Загрузка расписаний из базы данных
		Schedules = new ObservableCollection<Schedule>(_scheduleService.GetSchedules());
	}

	public DelegateCommand AddScheduleCommand => new(() =>
	{
		var scheduleAddWindow = new ScheduleAddOrUpdateWindow(null!, this);
		scheduleAddWindow.Closing += AddOrUpdateScheduleWindowClosingHandler!;

		scheduleAddWindow.ShowDialog();
	});

	public DelegateCommand UpdateScheduleCommand => new(() =>
	{
		// Обновление лекции в базе данных
		if (SelectedSchedule == null!)
		{
			MessageBox.Show("Пожалуйста, выберите расписание для редактирования.");

			return;
		}

		var scheduleAddWindow = new ScheduleAddOrUpdateWindow(SelectedSchedule, this);

		scheduleAddWindow.Closing += AddOrUpdateScheduleWindowClosingHandler!;

		scheduleAddWindow.ShowDialog();
	});
	
	public DelegateCommand DeleteScheduleCommand => new(() =>
	{
		// Удаление лекции из базы данных
		if (SelectedSchedule == null!)
		{
			MessageBox.Show("Пожалуйста, выберите расписание для удаления.");

			return;
		}

		_scheduleService.DeleteSchedule(SelectedSchedule);
		ReloadData();

		MessageBox.Show("Расписание успешно удалено!");
	});

	private void ReloadData()
	{
		Schedules.Clear();

		var schedules = _scheduleService.GetSchedules();

		schedules.ForEach(schedule =>
		{
			Schedules.Add(schedule);
		});
	}

	private void AddOrUpdateScheduleWindowClosingHandler(object sender, CancelEventArgs e)
	{
		ReloadData();
	}
}