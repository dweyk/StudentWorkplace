namespace StudentWorkplace.ViewModels.VideoLectures;

using Models;

using Services;

using Views.VideoLectures;

public class VideoLecturesViewingPageViewModel : BindableBase
{
	private readonly VideoLectureService _videoLectureService;

	public VideoLectureMetadataDto SelectedVideoLectureMetadata { get; set; }

	public ObservableCollection<VideoLectureMetadataDto> VideoLectureMetadatas { get; set; }

	public VideoLecturesViewingPageViewModel(VideoLectureService videoLectureService)
	{
		_videoLectureService = videoLectureService;

		// Загрузка метаданных видео-лекций из базы данных
		VideoLectureMetadatas =
			new ObservableCollection<VideoLectureMetadataDto>(_videoLectureService.GetVideoLectureMetadatas());
	}

	public DelegateCommand ViewVideoLectureCommand => new(async () =>
	{
		if (SelectedVideoLectureMetadata == null!)
		{
			MessageBox.Show("Пожалуйста, выберите видео-лекцию для просмотра.");

			return;
		}

		var selectedVideoLecture =
			_videoLectureService.GetVideoLecture(SelectedVideoLectureMetadata.VideoLectureId);
		if (selectedVideoLecture == null!)
		{
			MessageBox.Show("Видео-лекция не найдена или была удалена.");

			return;
		}

		var videoLectureViewingWindow = new VideoLectureViewingWindow();
		var context = (VideoLectureViewingWindowViewModel)videoLectureViewingWindow.DataContext;

		videoLectureViewingWindow.Closing += context.DeleteTemporaryFile!;
		await context.PlayVideoLecture(selectedVideoLecture.Data);

		videoLectureViewingWindow.ShowDialog();
	});
}