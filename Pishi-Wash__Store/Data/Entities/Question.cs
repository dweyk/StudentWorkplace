namespace StudentWorkplace.Data.Entities;

public class Question
{
	public int QuestionId { get; set; }

	public string Text { get; set; } = null!;

	public string Answer { get; set; } = null!;

	public string TestTopic { get; set; } = null!;

	public ICollection<PassedQuestion> PassedQuestions { get; set; }
}