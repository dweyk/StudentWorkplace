namespace StudentWorkplace.ViewModels.Lectures;

using System.ComponentModel;
using System.Runtime.CompilerServices;

using Data.Entities;

using Services;

using Views.Lectures;

public class LecturesCrudPageViewModel : BindableBase
{
	private readonly LectureService _lectureService;

	public event PropertyChangedEventHandler PropertyChanged;

	private Lecture _selectedLecture;

	private ObservableCollection<Lecture> _lectures { get; set; }

	public Lecture SelectedLecture
	{
		get { return _selectedLecture; }
		set
		{
			_selectedLecture = value;
			OnPropertyChanged();
		}
	}

	public ObservableCollection<Lecture> Lectures
	{
		get { return _lectures; }
		set
		{
			_lectures = value;
			OnPropertyChanged();
		}
	}

	public LecturesCrudPageViewModel(LectureService lectureService)
	{
		_lectureService = lectureService;

		// Загрузка лекций из базы данных
		Lectures = new ObservableCollection<Lecture>(_lectureService.GetLectures());
	}

	public DelegateCommand AddLectureCommand => new(() =>
	{
		var lectureAddWindow = new LecturesAddOrUpdateWindow(null!, this);
		lectureAddWindow.Closing += AddOrUpdateLectureWindowClosingHandler!;

		lectureAddWindow.ShowDialog();
	});

	public DelegateCommand UpdateLectureCommand => new(() =>
	{
		// Обновление лекции в базе данных
		if (SelectedLecture == null!)
		{
			MessageBox.Show("Пожалуйста, выберите лекцию для редактирования.");

			return;
		}

		var lectureAddWindow = new LecturesAddOrUpdateWindow(_selectedLecture, this);

		lectureAddWindow.Closing += AddOrUpdateLectureWindowClosingHandler!;

		lectureAddWindow.ShowDialog();
	});
	
	public DelegateCommand DeleteLectureCommand => new(() =>
	{
		// Удаление лекции из базы данных
		if (SelectedLecture == null!)
		{
			MessageBox.Show("Пожалуйста, выберите лекцию для удаления.");

			return;
		}

		_lectureService.DeleteLecture(SelectedLecture);
		ReloadData();

		MessageBox.Show("Лекция успешно удалена!");
	});

	protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
	{
		PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
	}

	public void ReloadData()
	{
		Lectures.Clear();

		var lectures = _lectureService.GetLectures();

		lectures.ForEach(lecture =>
		{
			Lectures.Add(lecture);
		});
	}

	private void AddOrUpdateLectureWindowClosingHandler(object sender, CancelEventArgs e)
	{
		ReloadData();
	}
}