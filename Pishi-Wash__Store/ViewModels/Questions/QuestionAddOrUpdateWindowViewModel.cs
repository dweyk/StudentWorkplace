namespace StudentWorkplace.ViewModels.Questions;

using Data.Entities;

using Services;

public class QuestionAddOrUpdateWindowViewModel : BindableBase
{
	public Question Question { get; set; } = null!;

	public QuestionsCrudPageViewModel QuestionsCrudPageViewModel { get; set; } = null!;
	
	private readonly QuestionService _questionService;

	public QuestionAddOrUpdateWindowViewModel(QuestionService questionService)
	{
		_questionService = questionService;
	}

	public DelegateCommand SaveCommand => new(() =>
	{
		if (Question == null!
		    || string.IsNullOrWhiteSpace(Question.Text)
		    || string.IsNullOrWhiteSpace(Question.Answer)
		    || string.IsNullOrWhiteSpace(Question.TestTopic))
		{
			MessageBox.Show("Заполните все поля!");

			return;
		}

		_questionService.AddOrUpdateQuestionWithSave(Question);

		MessageBox.Show("Вопрос по тестированию успешно сохранен!");
	});

	public DelegateCommand CancelCommand => new(CloseWindow);

	private void CloseWindow()
	{
		// Закрытие окна редактирования
		Application.Current.Windows[1]!.Close();
	}
}