namespace StudentWorkplace.Views.Lectures;

using Data.Entities;

using ViewModels.Lectures;

public partial class LecturesAddOrUpdateWindow : Window
{
	public LecturesAddOrUpdateWindow(Lecture? lecture, LecturesCrudPageViewModel lecturesCrudPageViewMode = null!)
	{
		InitializeComponent();
		var context = (LectureAddOrUpdateWindowViewModel)DataContext;
		context.Lecture = lecture ?? new Lecture();
		context.LecturesCrudPageViewModel = lecturesCrudPageViewMode;
	}
}