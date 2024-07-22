namespace StudentWorkplace.ViewModels.VideoLectures;

using System.ComponentModel;

using Models;

using Services;

using Views.VideoLectures;

public class VideoLecturesCrudPageViewModel : BindableBase
{
	private readonly VideoLectureService _videoLectureService;

	public VideoLectureMetadataDto SelectedVideoLectureMetadata { get; set; }

	public ObservableCollection<VideoLectureMetadataDto> VideoLectureMetadatas { get; set; }

	public VideoLecturesCrudPageViewModel(VideoLectureService videoLectureService)
	{
		_videoLectureService = videoLectureService;

		// Загрузка метаданных видео-лекций из базы данных
		VideoLectureMetadatas =
			new ObservableCollection<VideoLectureMetadataDto>(_videoLectureService.GetVideoLectureMetadatas());
	}

	public DelegateCommand AddVideoLectureCommand => new(() =>
	{
		var lectureAddWindow = new VideoLectureAddOrUpdateWindow();
		lectureAddWindow.Closing += AddOrUpdateVideoLectureWindowClosingHandler!;

		lectureAddWindow.ShowDialog();
	});

	public DelegateCommand DeleteVideoLectureCommand => new(() =>
	{
		// Удаление видео-лекции из базы данных
		if (SelectedVideoLectureMetadata == null!)
		{
			MessageBox.Show("Пожалуйста, выберите видео-лекцию для удаления.");

			return;
		}

		var selectedLectureForDeleted =
			_videoLectureService.GetVideoLecture(SelectedVideoLectureMetadata.VideoLectureId);
		if (selectedLectureForDeleted == null!)
		{
			MessageBox.Show("Видео-лекция не найдена или была удалена.");

			return;
		}

		_videoLectureService.DeleteVideoLecture(selectedLectureForDeleted);

		ReloadData();

		MessageBox.Show("Видео-лекция успешно удалена!");
	});

	public void ReloadData()
	{
		VideoLectureMetadatas.Clear();

		var videoLectures = _videoLectureService.GetVideoLectureMetadatas();

		videoLectures.ForEach(videoLecture => { VideoLectureMetadatas.Add(videoLecture); });
	}

	private void AddOrUpdateVideoLectureWindowClosingHandler(object sender, CancelEventArgs e)
	{
		ReloadData();
	}
}