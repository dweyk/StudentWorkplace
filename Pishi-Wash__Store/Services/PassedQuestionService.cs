namespace StudentWorkplace.Services;

using System.Collections.Immutable;

using Data;
using Data.Entities;

using Models;
using Models.Result;

public class PassedQuestionService
{
	private readonly ApplicationDbContext _applicationDbContext;

	private readonly UserService _userService;

	public PassedQuestionService(ApplicationDbContext applicationDbContext,
		UserService userService)
	{
		_applicationDbContext = applicationDbContext;
		_userService = userService;
	}

	public bool CheckUserPassedTest(string topicName)
		=> _applicationDbContext.PassedQuestions.Any(passedQuestion
			//=> EF.Functions.Like(passedQuestion.Question.TestTopic, topicName));
			=> passedQuestion.Question.TestTopic == topicName);

	public void RemovePassedTests(string topicName)
	{
		var userId = UserSetting.Default.UserId;

		var passedTests = _applicationDbContext.PassedQuestions
			.Where(passedQuestion => passedQuestion.UserId == userId)
			//.Where(passedQuestion => EF.Functions.Like(passedQuestion.Question.TestTopic, topicName));
			.Where(passedQuestion => passedQuestion.Question.TestTopic == topicName);

		_applicationDbContext.PassedQuestions.RemoveRange(passedTests);
	}

	public IReadOnlyCollection<PassedQuestionForLoadDto> GetPassedQuestionsForLoad()
	{
		var resultData = new List<PassedQuestionForLoadDto>();
		var users = _userService.GetAllUsers(asNoTracking: true);
		foreach (var user in users)
		{
			resultData.Add(new PassedQuestionForLoadDto
			{
				UserName = $"{user.UserName} {user.UserSurname} {user.UserPatronymic}",
				PassedQuestions = GetPassedQuestionsForUser(user.UserId),
			});
		}

		return resultData.ToImmutableArray();
	}

	public IEnumerable<PassedQuestionDto> GetPassedQuestionsForCurrentUser()
	{
		return _applicationDbContext.PassedQuestions.AsNoTracking()
			.Where(passedQuestion => passedQuestion.UserId == UserSetting.Default.UserId)
			.Include(passedQuestion => passedQuestion.User)
			.Include(passedQuestion => passedQuestion.Question)
			.GroupBy(passedQuestion => passedQuestion.Question.TestTopic)
			.Select(group => new PassedQuestionDto
			{
				TestTopic = group.Key,
				NumberCorrectAnswers =
					$"{group.Count(passedQuestion => passedQuestion.IsCorrectAnswer)} из {group.Count()}",
			});
	}

	public Result SavePassedQuestionsWithSave(IReadOnlyCollection<PassedQuestion> passedQuestions)
	{
		if (!passedQuestions.Any())
		{
			return Result.Fail(
				"Ошибка сохранения пройденных тестов, ответьте хотя бы на один вопрос!");
		}

		_applicationDbContext.PassedQuestions.AddRange(passedQuestions);
		_applicationDbContext.SaveChanges();

		return Result.Success();
	}

	public void AddOrUpdatePassedQuestionWithSave(PassedQuestion passedQuestion)
	{
		if (passedQuestion.QuestionId > 0)
		{
			_applicationDbContext.PassedQuestions.Update(passedQuestion);
			_applicationDbContext.SaveChanges();

			return;
		}

		_applicationDbContext.PassedQuestions.Add(passedQuestion);
		_applicationDbContext.SaveChanges();
	}

	public void UpdatePassedQuestionWithSave(PassedQuestion passedQuestion)
	{
		_applicationDbContext.PassedQuestions.Update(passedQuestion);
		_applicationDbContext.SaveChanges();
	}

	public void DeletePassedQuestion(PassedQuestion passedQuestion)
	{
		_applicationDbContext.PassedQuestions.Remove(passedQuestion);
		_applicationDbContext.SaveChanges();
	}

	private IReadOnlyCollection<PassedQuestionDto> GetPassedQuestionsForUser(int userId)
	{
		return _applicationDbContext.PassedQuestions.AsNoTracking()
			.Where(passedQuestion => passedQuestion.UserId == userId)
			.Include(passedQuestion => passedQuestion.User)
			.Include(passedQuestion => passedQuestion.Question)
			.GroupBy(passedQuestion => passedQuestion.Question.TestTopic)
			.Select(group => new PassedQuestionDto
			{
				TestTopic = group.Key,
				NumberCorrectAnswers =
					$"{group.Count(passedQuestion => passedQuestion.IsCorrectAnswer)} из {group.Count()}",
			})
			.ToImmutableArray();
	}
}