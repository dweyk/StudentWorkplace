namespace StudentWorkplace.Services;

using Data;
using Data.Entities;

using Models;



public class VideoLectureService
{
	private readonly ApplicationDbContext _applicationDbContext;

	public VideoLectureService(ApplicationDbContext applicationDbContext)
	{
		_applicationDbContext = applicationDbContext;
	}

	public IEnumerable<VideoLecture> GetVideoLectures()
	{
		return _applicationDbContext.VideoLectures.AsTracking();
	}

	public IEnumerable<VideoLectureMetadataDto> GetVideoLectureMetadatas()
	{
		return _applicationDbContext.VideoLectures
			.Select(videoLecture => new VideoLectureMetadataDto
			{
				VideoLectureId = videoLecture.VideoLectureId,
				Title = videoLecture.Title,
				UploadDate = videoLecture.UploadDate,
			})
			.ToArray();
	}

	public VideoLecture GetVideoLecture(int id)
	{
		return _applicationDbContext.VideoLectures
			.AsTracking()
			.FirstOrDefault(videoLecture => videoLecture.VideoLectureId == id)!;
	}

	public void AddOrUpdateVideoLectureWithSave(VideoLecture videoLecture)
	{
		if (videoLecture.VideoLectureId > 0)
		{
			UpdateVideoLectureWithSave(videoLecture);

			return;
		}

		_applicationDbContext.VideoLectures.Add(videoLecture);
		_applicationDbContext.SaveChanges();
	}

	public void UpdateVideoLectureWithSave(VideoLecture videoLecture)
	{
		_applicationDbContext.VideoLectures.Update(videoLecture);
		_applicationDbContext.SaveChanges();
	}

	public void DeleteVideoLecture(VideoLecture videoLecture)
	{
		_applicationDbContext.VideoLectures.Remove(videoLecture);
		_applicationDbContext.SaveChanges();
	}
}