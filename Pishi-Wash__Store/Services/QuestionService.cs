namespace StudentWorkplace.Services;

using Data;
using Data.Entities;

using Models;

public class QuestionService
{
	private readonly ApplicationDbContext _applicationDbContext;

	public QuestionService(ApplicationDbContext applicationDbContext)
	{
		_applicationDbContext = applicationDbContext;
	}

	public IEnumerable<QuestionTopicDto> GetTestTopics()
	=> _applicationDbContext.Questions
		.Select(question => new QuestionTopicDto
		{
			QuestionTopic = question.TestTopic
		})
		.Distinct();

	public IEnumerable<Question> GetQuestions()
	{
		return _applicationDbContext.Questions
			.Include(question => question.PassedQuestions)
			.AsTracking()
			.OrderBy(question => question.TestTopic);
	}

	public Question GetFirstQuestionByTopic(string topicName)
	{
		return GetQuestions()
			//.FirstOrDefault(question => EF.Functions.Like(question.TestTopic, topicName))!;
			.FirstOrDefault(question => question.TestTopic == topicName)!;
	}

	public IEnumerable<Question> GetQuestions(string topic)
	{
		return GetQuestions()
			//.Where(question => EF.Functions.Like(question.TestTopic, topic));
			.Where(question => question.TestTopic == topic);
	}

	public void AddOrUpdateQuestionWithSave(Question Question)
	{
		if (Question.QuestionId > 0)
		{
			_applicationDbContext.Questions.Update(Question);
			_applicationDbContext.SaveChanges();

			return;
		}

		_applicationDbContext.Questions.Add(Question);
		_applicationDbContext.SaveChanges();
	}

	public void UpdateQuestionWithSave(Question Question)
	{
		_applicationDbContext.Questions.Update(Question);
		_applicationDbContext.SaveChanges();
	}

	public void DeleteQuestion(Question Question)
	{
		_applicationDbContext.Questions.Remove(Question);
		_applicationDbContext.SaveChanges();
	}
}