namespace StudentWorkplace.ViewModels.Lectures;

using Data.Entities;

using Services;

public class LectureAddOrUpdateWindowViewModel : BindableBase
{
	public Lecture Lecture { get; set; } = null!;

	public LecturesCrudPageViewModel LecturesCrudPageViewModel { get; set; } = null!;
	
	private readonly LectureService _lectureService;

	public LectureAddOrUpdateWindowViewModel(LectureService lectureService)
	{
		_lectureService = lectureService;
	}

	public DelegateCommand SaveCommand => new(() =>
	{
		if (Lecture == null!
		    || string.IsNullOrWhiteSpace(Lecture.Title)
		    || string.IsNullOrWhiteSpace(Lecture.Content))
		{
			MessageBox.Show("Заполните все поля!");

			return;
		}

		// Добавление новой лекции в базу данных
		_lectureService.AddOrUpdateLectureWithSave(Lecture);

		MessageBox.Show("Лекция успешно сохранена!");
	});

	public DelegateCommand CancelCommand => new(CloseWindow);

	private void CloseWindow()
	{
		// Закрытие окна редактирования
		Application.Current.Windows[1]!.Close();
	}
}