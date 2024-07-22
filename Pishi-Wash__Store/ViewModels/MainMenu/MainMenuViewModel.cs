namespace StudentWorkplace.ViewModels.MainMenu;

using Services;

using Views.LaboratoryWorkPage;
using Views.Lectures;
using Views.Questions;
using Views.Reminders;
using Views.Schedules;
using Views.Statistic;
using Views.VideoLectures;

public class MainMenuViewModel : BindableBase
{
	private readonly PageService _pageService;

	private readonly StatisticService _statisticService;

	public string UserRole { get; set; } = UserSetting.Default.UserRole;

	public MainMenuViewModel(PageService pageService,
		StatisticService statisticService)
	{
		_pageService = pageService;
		_statisticService = statisticService;
	}

	public DelegateCommand OpenScheduleReadPageCommand
		=> new(() => _pageService.ChangePage(new ScheduleViewingPage()));

	public DelegateCommand OpenReminderReadPageCommand
		=> new(() => _pageService.ChangePage(new ReminderViewingPage()));

	public DelegateCommand OpenVideoLectureViewingPageCommand
		=> new(() => _pageService.ChangePage(new VideoLecturesViewingPage()));

	public DelegateCommand OpenLectureReadPageCommand
		=> new(() => _pageService.ChangePage(new LecturesViewingPage()));

	public DelegateCommand OpenQuestionReadPageCommand
		=> new(() => _pageService.ChangePage(new QuestionsViewingPage()));

	public DelegateCommand OpenLaboratoryWorkPageCommand
		=> new(() => _pageService.ChangePage(new LaboratoryWorkPage()));

	public DelegateCommand OpenUserStatisticPageCommand
		=> new(() => _pageService.ChangePage(new StatisticPage()));

	public DelegateCommand OpenLecturesCrudPageCommand
		=> new(() => _pageService.ChangePage(new LecturesCrudPage()));

	public DelegateCommand OpenScheduleCrudPageCommand
		=> new(() => _pageService.ChangePage(new ScheduleCrudPage()));

	public DelegateCommand OpenQuestionCrudPageCommand
		=> new(() => _pageService.ChangePage(new QuestionsCrudPage()));

	public DelegateCommand OpenVideoLecturesCrudPageCommand
		=> new(() => _pageService.ChangePage(new VideoLecturesCrudPage()));

	public DelegateCommand LoadStatisticCommand
		=> new(() =>
		{
			var loadStatisticTests = _statisticService.LoadStatistics();
			if (loadStatisticTests)
			{
				MessageBox.Show(
					"Статистика пользователей по пройденным тестам успешна выгружена на рабочий стол.");

				return;
			}

			MessageBox.Show(
				"Произошла ошибка при выгрузке статистики пользователей по пройденным тестам. " +
				"Попробуйте повторить выгрузку статистики еще раз.");
		});
}