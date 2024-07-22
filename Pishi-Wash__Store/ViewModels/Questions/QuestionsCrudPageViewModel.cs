namespace StudentWorkplace.ViewModels.Questions;

using System.ComponentModel;
using System.Runtime.CompilerServices;

using Data.Entities;

using Services;

using Views.Questions;

public class QuestionsCrudPageViewModel : BindableBase
{
	private readonly QuestionService _questionService;

	public event PropertyChangedEventHandler PropertyChanged;

	private Question _selectedQuestion;
	public Question SelectedQuestion
	{
		get { return _selectedQuestion; }
		set
		{
			_selectedQuestion = value;
			OnPropertyChanged();
		}
	}

	private ObservableCollection<Question> _questions { get; set; }
	public ObservableCollection<Question> Questions
	{
		get { return _questions; }
		set
		{
			_questions = value;
			OnPropertyChanged();
		}
	}

	public QuestionsCrudPageViewModel(QuestionService questionService)
	{
		_questionService = questionService;

		Questions = new ObservableCollection<Question>(_questionService.GetQuestions());
	}

	public DelegateCommand AddCommand => new(() =>
	{
		var lectureAddWindow = new QuestionAddOrUpdateWindow(null!, this);
		lectureAddWindow.Closing += AddOrUpdateQuestionWindowClosingHandler!;

		lectureAddWindow.ShowDialog();
	});

	public DelegateCommand UpdateCommand => new(() =>
	{
		if (SelectedQuestion == null!)
		{
			MessageBox.Show("Пожалуйста, выберите вопрос для редактирования.");

			return;
		}

		var questionAddWindow = new QuestionAddOrUpdateWindow(_selectedQuestion, this);

		questionAddWindow.Closing += AddOrUpdateQuestionWindowClosingHandler!;

		questionAddWindow.ShowDialog();
	});
	
	public DelegateCommand DeleteCommand => new(() =>
	{
		if (SelectedQuestion == null!)
		{
			MessageBox.Show("Пожалуйста, выберите вопрос для удаления.");

			return;
		}

		_questionService.DeleteQuestion(SelectedQuestion);
		ReloadData();

		MessageBox.Show("Вопрос успешно удален.");
	});

	protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
	{
		PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
	}

	public void ReloadData()
	{
		Questions.Clear();

		var questions = _questionService.GetQuestions();

		questions.ForEach(question =>
		{
			Questions.Add(question);
		});
	}

	private void AddOrUpdateQuestionWindowClosingHandler(object sender, CancelEventArgs e)
	{
		ReloadData();
	}
}