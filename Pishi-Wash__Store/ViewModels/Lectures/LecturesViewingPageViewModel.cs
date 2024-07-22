namespace StudentWorkplace.ViewModels.Lectures;

using System.ComponentModel;
using System.Runtime.CompilerServices;

using Data.Entities;

using Services;

using Views.Lectures;

public class LecturesViewingPageViewModel : BindableBase
{
	private readonly LectureService _lectureService;

	private readonly PageService _pageService;

	public event PropertyChangedEventHandler PropertyChanged;

	private Lecture selectedLecture;

	public Lecture SelectedLecture
	{
		get { return selectedLecture; }
		set
		{
			selectedLecture = value;
			OnPropertyChanged();
		}
	}

	public ObservableCollection<Lecture> Lectures { get; set; }

	public LecturesViewingPageViewModel(LectureService lectureService, PageService pageService)
	{
		_lectureService = lectureService;
		_pageService = pageService;

		// Загрузка лекций из базы данных
		Lectures = new ObservableCollection<Lecture>(_lectureService.GetLectures());
	}
	
	public DelegateCommand ReadLectureCommand => new(() =>
	{
		if (SelectedLecture == null!)
		{
			MessageBox.Show("Пожалуйста, выберите лекцию для чтения.");

			return;
		}

		_pageService.ChangePage(new LectureReadPage(SelectedLecture!));
	});

	protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
	{
		PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
	}
}