namespace StudentWorkplace.ViewModels.VideoLectures;

using Data.Entities;

using Microsoft.Win32;

using Services;

public class VideoLectureAddOrUpdateWindowViewModel : BindableBase
{
	private readonly VideoLectureService _videoLectureService;

	private string selectedVideoPath = null!;

	public string SelectedVideoPath
	{
		get { return selectedVideoPath; }
		set { selectedVideoPath = value; }
	}

	public VideoLectureAddOrUpdateWindowViewModel(VideoLectureService videoLectureService)
	{
		_videoLectureService = videoLectureService;
	}

	public DelegateCommand SelectVideoCommand => new(() =>
	{
		// Открыть диалоговое окно для выбора видеофайла
		var openFileDialog = new OpenFileDialog
		{
			Filter = "Video files (*.mp4, *.avi)|*.mp4;*.avi|All files (*.*)|*.*"
		};

		if (openFileDialog.ShowDialog() == true)
		{
			// Отобразить путь к выбранному видеофайлу
			SelectedVideoPath = openFileDialog.FileName;
		}
	});

	public DelegateCommand UploadVideoCommand => new(() =>
	{
		// Проверить, выбран ли видеофайл
		if (string.IsNullOrEmpty(SelectedVideoPath))
		{
			MessageBox.Show("Пожалуйста выберите файл для загрузки.");
			return;
		}

		try
		{
			// Считать содержимое видеофайла в байтовый массив
			byte[] videoBytes = File.ReadAllBytes(SelectedVideoPath);

			// Создать экземпляр Video и сохранить его в базе данных
			var video = new VideoLecture
			{
				Title = Path.GetFileName(SelectedVideoPath),
				Data = videoBytes,
				UploadDate = DateTime.Now
			};

			// Сохранить видео в базу данных
			_videoLectureService.AddOrUpdateVideoLectureWithSave(video);

			MessageBox.Show("Видео успешно загружено.");
		}
		catch (Exception ex)
		{
			MessageBox.Show($"Ошибка при загрузке видео: {ex.Message}");
		}
	});

	public DelegateCommand CancelCommand => new(CloseWindow);

	private void CloseWindow()
	{
		// Закрытие окна редактирования
		Application.Current.Windows[1]!.Close();
	}
}