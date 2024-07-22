namespace StudentWorkplace.ViewModels.Statistic;

using System.ComponentModel;
using System.Runtime.CompilerServices;

using Models;

using Services;

public class StatisticViewingPageViewModel : BindableBase
{
	private readonly PassedQuestionService _passedQuestionService;

	public event PropertyChangedEventHandler PropertyChanged;

	private ObservableCollection<PassedQuestionDto> _passedQuestion { get; set; }
	public ObservableCollection<PassedQuestionDto> PassedQuestions
	{
		get { return _passedQuestion; }
		set
		{
			_passedQuestion = value;
			OnPropertyChanged();
		}
	}

	public StatisticViewingPageViewModel(PassedQuestionService passedQuestionService)
	{
		_passedQuestionService = passedQuestionService;

		PassedQuestions = _passedQuestionService.GetPassedQuestionsForCurrentUser().ToObservableCollection();
	}

	protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
	{
		PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
	}
}