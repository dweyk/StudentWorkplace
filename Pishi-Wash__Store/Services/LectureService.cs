namespace StudentWorkplace.Services;

using Data;
using Data.Entities;



public class LectureService
{
	private readonly ApplicationDbContext _applicationDbContext;

	public LectureService(ApplicationDbContext applicationDbContext)
	{
		_applicationDbContext = applicationDbContext;
	}

	public IEnumerable<Lecture> GetLectures()
	{
		return _applicationDbContext.Lectures.AsTracking();
	}

	public void AddOrUpdateLectureWithSave(Lecture lecture)
	{
		if (lecture.LectureId > 0)
		{
			UpdateLectureWithSave(lecture);

			return;
		}

		_applicationDbContext.Lectures.Add(lecture);
		_applicationDbContext.SaveChanges();
	}

	public void UpdateLectureWithSave(Lecture lecture)
	{
		_applicationDbContext.Lectures.Update(lecture);
		_applicationDbContext.SaveChanges();
	}

	public void DeleteLecture(Lecture lecture)
	{
		_applicationDbContext.Lectures.Remove(lecture);
		_applicationDbContext.SaveChanges();
	}
}