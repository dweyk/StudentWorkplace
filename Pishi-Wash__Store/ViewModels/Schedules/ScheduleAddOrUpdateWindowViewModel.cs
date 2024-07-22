namespace StudentWorkplace.ViewModels.Schedules;

using Data.Entities;

using Services;

public class ScheduleAddOrUpdateWindowViewModel : BindableBase
{
	public Schedule Schedule { get; set; } = null!;

	public ObservableCollection<User> Students { get; set; } = null!;

	public ObservableCollection<User> SelectedStudents { get; set; } = new();

	public ScheduleCrudPageViewModel ScheduleCrudPageViewModel { get; set; } = null!;
	
	private readonly ScheduleService _scheduleService;

	public ScheduleAddOrUpdateWindowViewModel(ScheduleService scheduleService, UserService userService)
	{
		_scheduleService = scheduleService;
		Students = userService.GetAllUsers(asNoTracking: false).ToObservableCollection();
	}

	public DateTime NewScheduleDate { get; set; } = DateTime.Now.Date;
	public string NewScheduleTime { get; set; } = DateTime.Now.ToString("HH:mm");

	public DelegateCommand SaveCommand => new(() =>
	{
		if (!DateTime.TryParse($"{NewScheduleDate.ToShortDateString()} {NewScheduleTime}", out DateTime dateTime))
		{
			MessageBox.Show("Неверно введена дата занятия.");

			return;
		}

		var utcDateTime = dateTime.ToUniversalTime();

		if (utcDateTime.Date < DateTime.UtcNow.AddDays(1).Date)
		{
			MessageBox.Show($"Занятие планирается как минимум за день: с {DateTime.UtcNow.AddDays(1).Date}");

			return;
		}

		if (SelectedStudents.Count == 0 || SelectedStudents.All(user => user.UserRole != 2))
		{
			MessageBox.Show("Не выбрано ни одного студента.");

			return;
		}

		if (string.IsNullOrWhiteSpace(Schedule.Title))
		{
			MessageBox.Show("Заполните наименование занятия.");

			return;
		}

		Schedule.DateTime = utcDateTime;

		Schedule.Users ??= new List<User>();
		Schedule.Users.Clear();
		foreach (var user in SelectedStudents)
		{
			Schedule.Users.Add(user);
		}

		// Добавление нового расписания в бд
		_scheduleService.AddOrUpdateScheduleWithSave(Schedule);

		MessageBox.Show("Расписание успешно сохранено.");
	});

	public DelegateCommand CancelCommand => new(CloseWindow);

	public void OnStudentChecked(User user)
	{
		if (!SelectedStudents.Contains(user))
		{
			SelectedStudents.Add(user);
		}
	}

	public void OnStudentUnchecked(User user)
	{
		if (SelectedStudents.Contains(user))
		{
			SelectedStudents.Remove(user);
		}
	}

	public void UpdateExistSchedule(Schedule schedule)
	{
		if (schedule != null!)
		{
			Schedule = schedule;
			NewScheduleDate = schedule.DateTime.Date;
			NewScheduleTime = schedule.DateTime.ToString("HH:mm");

			return;
		}

		Schedule = new();
	}

	private void CloseWindow()
	{
		// Закрытие окна редактирования
		Application.Current.Windows[1]!.Close();
	}
}