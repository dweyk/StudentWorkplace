namespace StudentWorkplace.ViewModels.Questions;

using Data.Entities;

using Services;

using Views.MainMenu;

public class QuestionsTestPageViewModel : BindableBase
{
	private readonly QuestionService _questionService;

	private readonly PageService _pageService;

	private readonly UserService _userService;

	private readonly PassedQuestionService _passedQuestionService;

	private string _enteredAnswer;

	private int _questionCount;

	private int _totalQuestionsCount;

	private ICollection<PassedQuestion> _passedQuestions = new List<PassedQuestion>();

	private Question _selectedQuestion;

	public Question SelectedQuestion
	{
		get { return _selectedQuestion; }
		set { _selectedQuestion = value; }
	}

	public int QuestionCount
	{
		get { return _questionCount; }
		set
		{
			_questionCount = value;
			UpdatePageCounter();
		}
	}

	public int TotalQuestionsCount
	{
		get { return _totalQuestionsCount; }
		set { _totalQuestionsCount = value; }
	}

	public string EnteredAnswer
	{
		get { return _enteredAnswer; }
		set { _enteredAnswer = value; }
	}

	private ObservableCollection<Question> _questions;

	public ObservableCollection<Question> Questions
	{
		get { return _questions; }
		set { _questions = value; }
	}

	private string _pageCounter;

	public string PageCounter
	{
		get { return _pageCounter; }
		set { _pageCounter = value; }
	}

	public QuestionsTestPageViewModel(
		QuestionService questionService,
		PageService pageService,
		UserService userService,
		PassedQuestionService passedQuestionService)
	{
		_questionService = questionService;
		_pageService = pageService;
		_userService = userService;
		_passedQuestionService = passedQuestionService;
	}

	public void LoadTopicQuestions(string topic)
	{
		Questions = _questionService.GetQuestions(topic).ToObservableCollection();
		SelectedQuestion = Questions.FirstOrDefault()!;
		TotalQuestionsCount = Questions.Count;
		QuestionCount = 1;
	}

	public DelegateCommand AnswerCommand => new(() =>
	{
		if (string.IsNullOrWhiteSpace(EnteredAnswer))
		{
			MessageBox.Show("Пожалуйста, введите ответ");

			return;
		}

		var answer = new PassedQuestion
		{
			IsCorrectAnswer = EnteredAnswer.Equals(SelectedQuestion.Answer),
			Question = SelectedQuestion,
			User = _userService.GetCurrentUser(),
		};

		_passedQuestions.Add(answer);

		Questions.Remove(SelectedQuestion);

		var nextQuestion = Questions.FirstOrDefault()!;

		if (nextQuestion != null!)
		{
			EnteredAnswer = null!;
			SelectedQuestion = nextQuestion;
			QuestionCount += 1;

			return;
		}

		CompleteTest();
	});

	public DelegateCommand CompleteCommand => new(() =>
	{
		if (Questions.Count != 0)
		{
			var result = MessageBox.Show(
				"Вы не ответили на все вопросы. Вы точно хотите завершить тестирование?",
				"Внимание!",
				MessageBoxButton.YesNo);

			if (result == MessageBoxResult.Yes)
			{
				CompleteTest();
			}
		}
	});

	private void CompleteTest()
	{
		var result = _passedQuestionService.SavePassedQuestionsWithSave(_passedQuestions.ToReadOnlyCollection());
		if (!result.IsSuccess())
		{
			MessageBox.Show(string.Join(", ", result.Messages));

			return;
		}

		var countOfCorrectAnswers = _passedQuestions.Count(passedQuestion => passedQuestion.IsCorrectAnswer);

		_pageService.ChangePage(new MainMenuPage());

		MessageBox.Show($"Вы прошли тест. Количество верных ответов: {countOfCorrectAnswers} из {TotalQuestionsCount}");
	}

	private void UpdatePageCounter()
	{
		PageCounter = $"Вопрос {QuestionCount} из {TotalQuestionsCount}";
	}
}