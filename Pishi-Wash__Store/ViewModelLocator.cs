namespace StudentWorkplace
{
	using Data;

	using Services;

	using ViewModels;
	using ViewModels.Lectures;
	using ViewModels.MainMenu;
	using ViewModels.Questions;
	using ViewModels.Reminder;
	using ViewModels.Schedules;
	using ViewModels.Sign;
	using ViewModels.Statistic;
	using ViewModels.VideoLectures;

	internal class ViewModelLocator
	{
		public static ServiceProvider? _provider;

		private static IConfiguration? _configuration;

		public static void Init()
		{
			_configuration = new ConfigurationBuilder()
				.SetBasePath(Directory.GetCurrentDirectory())
				.AddJsonFile("appsettings.json")
				.Build();

			var services = new ServiceCollection();

			#region ViewModel

			services.AddTransient<mWindowViewModel>();
			services.AddTransient<SignInViewModel>();
			services.AddTransient<SignUpViewModel>();
			services.AddTransient<MainMenuViewModel>();

			services.AddTransient<LecturesViewingPageViewModel>();
			services.AddTransient<LecturesCrudPageViewModel>();
			services.AddTransient<LectureAddOrUpdateWindowViewModel>();
			services.AddTransient<LectureReadPageViewModel>();

			services.AddTransient<QuestionAddOrUpdateWindowViewModel>();
			services.AddTransient<QuestionsCrudPageViewModel>();
			services.AddTransient<QuestionsViewingPageViewModel>();
			services.AddTransient<QuestionsTestPageViewModel>();

			services.AddTransient<StatisticViewingPageViewModel>();

			services.AddTransient<VideoLectureAddOrUpdateWindowViewModel>();
			services.AddTransient<VideoLecturesCrudPageViewModel>();
			services.AddTransient<VideoLectureViewingWindowViewModel>();
			services.AddTransient<VideoLecturesViewingPageViewModel>();

			services.AddTransient<ScheduleAddOrUpdateWindowViewModel>();
			services.AddTransient<ScheduleCrudPageViewModel>();
			services.AddTransient<ScheduleViewingPageViewModel>();

			services.AddTransient<ReminderViewingPageViewModel>();

			#endregion

			#region Connection

			services.AddDbContext<ApplicationDbContext>(
				options =>
				{
					// options.UseNpgsql("Host=localhost;Port=5432;Database=mydatabase;Username=postgres;Password=postgres");
					options.UseSqlite("Data Source=testbd.db");
				},
				ServiceLifetime.Singleton);

			#endregion

			#region Services

			services.AddSingleton<PageService>();
			services.AddSingleton<UserService>();
			services.AddSingleton<LectureService>();
			services.AddSingleton<QuestionService>();
			services.AddSingleton<PassedQuestionService>();
			services.AddSingleton<StatisticService>();
			services.AddSingleton<VideoLectureService>();
			services.AddSingleton<ScheduleService>();
			services.AddSingleton<ReminderService>();

			#endregion

			_provider = services.BuildServiceProvider();
		}

		public mWindowViewModel? mWindowViewModel
			=> _provider?.GetRequiredService<mWindowViewModel>();

		public SignInViewModel? SignInViewModel
			=> _provider?.GetRequiredService<SignInViewModel>();

		public VideoLectureAddOrUpdateWindowViewModel? VideoLectureAddOrUpdateWindowViewModel
			=> _provider?.GetRequiredService<VideoLectureAddOrUpdateWindowViewModel>();

		public VideoLecturesCrudPageViewModel? VideoLecturesCrudPageViewModel
			=> _provider?.GetRequiredService<VideoLecturesCrudPageViewModel>();

		public VideoLectureViewingWindowViewModel? VideoLectureViewingWindowViewModel
			=> _provider?.GetRequiredService<VideoLectureViewingWindowViewModel>();

		public VideoLecturesViewingPageViewModel? VideoLecturesViewingPageViewModel
			=> _provider?.GetRequiredService<VideoLecturesViewingPageViewModel>();

		public MainMenuViewModel? MainMenuViewModel
			=> _provider?.GetRequiredService<MainMenuViewModel>();

		public LectureAddOrUpdateWindowViewModel? LectureAddOrUpdateWindowViewModel
			=> _provider?.GetRequiredService<LectureAddOrUpdateWindowViewModel>();

		public QuestionAddOrUpdateWindowViewModel? QuestionAddOrUpdateWindowViewModel
			=> _provider?.GetRequiredService<QuestionAddOrUpdateWindowViewModel>();

		public QuestionsCrudPageViewModel? QuestionsCrudPageViewModel
			=> _provider?.GetRequiredService<QuestionsCrudPageViewModel>();

		public QuestionsViewingPageViewModel? QuestionsViewingPageViewModel
			=> _provider?.GetRequiredService<QuestionsViewingPageViewModel>();

		public QuestionsTestPageViewModel? QuestionsTestPageViewModel
			=> _provider?.GetRequiredService<QuestionsTestPageViewModel>();

		public StatisticViewingPageViewModel? StatisticViewingPageViewModel
			=> _provider?.GetRequiredService<StatisticViewingPageViewModel>();

		public LecturesViewingPageViewModel? LecturesViewingPageViewModel
			=> _provider?.GetRequiredService<LecturesViewingPageViewModel>();

		public LecturesCrudPageViewModel? LecturesCrudViewModel
			=> _provider?.GetRequiredService<LecturesCrudPageViewModel>();

		public LectureReadPageViewModel? LectureReadPageViewModel
			=> _provider?.GetRequiredService<LectureReadPageViewModel>();

		public SignUpViewModel? SignUpViewModel
			=> _provider?.GetRequiredService<SignUpViewModel>();

		public ScheduleAddOrUpdateWindowViewModel? ScheduleAddOrUpdateWindowViewModel
			=> _provider?.GetRequiredService<ScheduleAddOrUpdateWindowViewModel>();

		public ScheduleViewingPageViewModel? ScheduleViewingPageViewModel
			=> _provider?.GetRequiredService<ScheduleViewingPageViewModel>();

		public ReminderViewingPageViewModel? ReminderViewingPageViewModel
			=> _provider?.GetRequiredService<ReminderViewingPageViewModel>();

		public ScheduleCrudPageViewModel? ScheduleCrudPageViewModel
			=> _provider?.GetRequiredService<ScheduleCrudPageViewModel>();
	}
}