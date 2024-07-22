namespace StudentWorkplace.Views.Questions;

using Data.Entities;

using ViewModels.Questions;

public partial class QuestionsTestPage : Page
{
	public QuestionsTestPage(Question question)
	{
		InitializeComponent();

		var context = (QuestionsTestPageViewModel)DataContext;

		context.SelectedQuestion = question;
		context.LoadTopicQuestions(question.TestTopic);
	}
}