namespace StudentWorkplace.ViewModels.VideoLectures;

using System.ComponentModel;
using System.Windows.Media;

public class VideoLectureViewingWindowViewModel : BindableBase
{
	public static readonly string _tempFilePath = Path.Combine(Path.GetTempPath(), "tempvideo.mp4");

	public MediaPlayer VideoPlayer { get; } = new MediaPlayer();

	public async Task PlayVideoLecture(byte[] videoBytes)
	{
		// Записать видео во временный файл
		await File.WriteAllBytesAsync(_tempFilePath, videoBytes);

		// Открыть временный файл в MediaPlayer
		VideoPlayer.Open(new Uri(_tempFilePath, UriKind.Relative));
	}

	public void DeleteTemporaryFile(object sender, CancelEventArgs e)
	{
		DeleteTemporaryFile();
	}

	private void DeleteTemporaryFile()
	{
		Task.Run(async () =>
		{
			if (File.Exists(_tempFilePath))
			{
				File.Delete(_tempFilePath);
			}
		});
	}
}