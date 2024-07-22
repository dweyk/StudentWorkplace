namespace StudentWorkplace.Views.Lectures;

using Data.Entities;

using ViewModels.Lectures;

public partial class LectureReadPage : Page
{
	public LectureReadPage(Lecture lecture)
	{
		InitializeComponent();

		var context = (LectureReadPageViewModel)DataContext;

		context.Lecture = lecture;
	}
}