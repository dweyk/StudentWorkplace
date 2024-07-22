namespace StudentWorkplace
{
    using Services.HostedServices;

    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private ReminderBackgroundService _reminderService = null!;

        protected override void OnStartup(StartupEventArgs e)
        {
            ViewModelLocator.Init();
            base.OnStartup(e);

            _reminderService = new ReminderBackgroundService();
        }

        protected override void OnExit(ExitEventArgs e)
        {
            _reminderService?.Dispose();
            base.OnExit(e);
        }
    }
}
