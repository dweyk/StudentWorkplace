namespace StudentWorkplace.ViewModels.Lectures;

using Data.Entities;

public class LectureReadPageViewModel : BindableBase
{
	public Lecture Lecture { get; set; } = null!;
}