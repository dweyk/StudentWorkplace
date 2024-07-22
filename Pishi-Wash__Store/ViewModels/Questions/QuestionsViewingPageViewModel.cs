namespace StudentWorkplace.ViewModels.Questions;

using System.ComponentModel;
using System.Runtime.CompilerServices;

using Models;

using Services;

using Views.Questions;

public class QuestionsViewingPageViewModel : BindableBase
{
	private readonly QuestionService _questionService;

	private readonly PageService _pageService;

	private readonly PassedQuestionService _passedQuestionService;

	public event PropertyChangedEventHandler PropertyChanged;

	private QuestionTopicDto selectedTest;
	public QuestionTopicDto SelectedTest
	{
		get { return selectedTest; }
		set
		{
			selectedTest = value;
			OnPropertyChanged();
		}
	}

	private ObservableCollection<QuestionTopicDto> _questionTopics { get; set; }
	public ObservableCollection<QuestionTopicDto> QuestionTopics
	{
		get { return _questionTopics; }
		set
		{
			_questionTopics = value;
			OnPropertyChanged();
		}
	}

	public QuestionsViewingPageViewModel(QuestionService QuestionService,
		PageService pageService,
		PassedQuestionService passedQuestionService)
	{
		_questionService = QuestionService;
		_pageService = pageService;
		_passedQuestionService = passedQuestionService;

		QuestionTopics = new ObservableCollection<QuestionTopicDto>(_questionService.GetTestTopics());
	}
	
	public DelegateCommand PassTestCommand => new(() =>
	{
		if (selectedTest == null!)
		{
			MessageBox.Show("Выберете тест для прохождения!");

			return;
		}

		if (_passedQuestionService.CheckUserPassedTest(selectedTest!.QuestionTopic))
		{
			var result = MessageBox.Show("Вы проходили данный тест. " +
				"Вы хотите стереть все данные по прохождению теста и пройти его снова?", "Внимание",
				MessageBoxButton.YesNo);

			if (result == MessageBoxResult.No)
			{
				return;
			}
		}

		_passedQuestionService.RemovePassedTests(selectedTest.QuestionTopic);

		var firstQuestionOfTopic = _questionService.GetFirstQuestionByTopic(selectedTest.QuestionTopic);
		_pageService.ChangePage(new QuestionsTestPage(firstQuestionOfTopic));
	});

	protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
	{
		PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
	}
}