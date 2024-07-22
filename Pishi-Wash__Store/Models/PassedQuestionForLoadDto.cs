namespace StudentWorkplace.Models;

public class PassedQuestionForLoadDto
{
	public string UserName { get; init; } = null!;

	public IReadOnlyCollection<PassedQuestionDto> PassedQuestions { get; set; } = null!;
}