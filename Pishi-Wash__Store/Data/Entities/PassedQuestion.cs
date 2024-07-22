namespace StudentWorkplace.Data.Entities;

public class PassedQuestion
{
	public int PassedQuestionId { get; set; }

	public bool IsCorrectAnswer { get; set; }

	public int QuestionId { get; set; }

	public Question Question { get; set; }

	public int UserId { get; set; }

	public User User { get; set; }
}