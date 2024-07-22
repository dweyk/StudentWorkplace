namespace StudentWorkplace.Views.VideoLectures;

using System.Windows.Threading;

using ViewModels.VideoLectures;

public partial class VideoLectureViewingWindow : Window
{
	private bool _isFullScreen;

	private WindowState _previousWindowState;

	private WindowStyle _previousWindowStyle;

	public VideoLectureViewingWindow()
	{
		InitializeComponent();

		DispatcherTimer timer = new DispatcherTimer();
		timer.Interval = TimeSpan.FromSeconds(1);
		timer.Tick += timer_Tick;
		timer.Start();
	}

	void timer_Tick(object sender, EventArgs e)
	{
		if (mePlayer.Source == null)
		{
			mePlayer.Source = new Uri(VideoLectureViewingWindowViewModel._tempFilePath);
			mePlayer.Play();
			mePlayer.Pause();
		}

		if (mePlayer.NaturalDuration.HasTimeSpan)
		{
			lblStatus.Content = $"{mePlayer.Position:mm\\:ss} / {mePlayer.NaturalDuration.TimeSpan:mm\\:ss}";
		}
	}

	private void btnPlay_Click(object sender, RoutedEventArgs e)
	{
		mePlayer.Play();
	}

	private void btnPause_Click(object sender, RoutedEventArgs e)
	{
		mePlayer.Pause();
	}

	private void btnStop_Click(object sender, RoutedEventArgs e)
	{
		mePlayer.Stop();
	}

	private void btnFullScreen_Click(object sender, RoutedEventArgs e)
	{
		if (_isFullScreen)
		{
			ExitFullScreen();
		}
		else
		{
			EnterFullScreen();
		}
	}

	private void btnClose_Click(object sender, RoutedEventArgs e)
	{
		Close();
	}

	private void EnterFullScreen()
	{
		_previousWindowState = WindowState;
		_previousWindowStyle = WindowStyle;
		WindowState = WindowState.Maximized;
		WindowStyle = WindowStyle.None;
		_isFullScreen = true;
	}

	private void ExitFullScreen()
	{
		WindowState = _previousWindowState;
		WindowStyle = _previousWindowStyle;
		_isFullScreen = false;
	}
}