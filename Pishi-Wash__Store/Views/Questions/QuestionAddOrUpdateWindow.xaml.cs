namespace StudentWorkplace.Views.Questions;

using Data.Entities;

using ViewModels.Questions;

public partial class QuestionAddOrUpdateWindow : Window
{
	public QuestionAddOrUpdateWindow(Question? question, QuestionsCrudPageViewModel questionsCrudPageViewModel = null!)
	{
		InitializeComponent();
		var context = (QuestionAddOrUpdateWindowViewModel)DataContext;
		context.Question = question ?? new Question();
		context.QuestionsCrudPageViewModel = questionsCrudPageViewModel;
	}
}