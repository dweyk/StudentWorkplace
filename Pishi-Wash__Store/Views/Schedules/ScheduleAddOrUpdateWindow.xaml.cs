namespace StudentWorkplace.Views.Schedules;

using Data.Entities;

using ViewModels.Schedules;

public partial class ScheduleAddOrUpdateWindow : Window
{
	public ScheduleAddOrUpdateWindow(Schedule? schedule, ScheduleCrudPageViewModel schedulesCrudPageViewMode = null!)
	{
		InitializeComponent();

		var context = (ScheduleAddOrUpdateWindowViewModel)DataContext;

		context.UpdateExistSchedule(schedule!);

		context.ScheduleCrudPageViewModel = schedulesCrudPageViewMode;
	}

	private void CheckBox_Checked(object sender, RoutedEventArgs e)
	{
		if (sender is CheckBox checkBox && checkBox.DataContext is User user)
		{
			var viewModel = (ScheduleAddOrUpdateWindowViewModel)DataContext;
			viewModel.OnStudentChecked(user);
		}
	}

	private void CheckBox_Unchecked(object sender, RoutedEventArgs e)
	{
		if (sender is CheckBox checkBox && checkBox.DataContext is User user)
		{
			var viewModel = (ScheduleAddOrUpdateWindowViewModel)DataContext;
			viewModel.OnStudentUnchecked(user);
		}
	}
}