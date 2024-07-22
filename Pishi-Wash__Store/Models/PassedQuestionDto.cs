namespace StudentWorkplace.Models;

public class PassedQuestionDto
{
	public string TestTopic { get; init; } = null!;

	public string NumberCorrectAnswers { get; init; } = null!;
}