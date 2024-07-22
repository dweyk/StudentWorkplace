namespace StudentWorkplace.Models.Result;

using Constants;

public interface IResult
{
	/// <summary>Список бизнес сообщений.</summary>
	IReadOnlyCollection<BusinessMessage> Messages { get; set; }

	/// <summary>Позитивен ли Result.</summary>
	/// <returns>Содержит ли Result бизнес сообщения &gt;= BusinessMessageType.Error.</returns>
	bool IsSuccess();

	/// <summary>Позитивен ли Result.</summary>
	/// <param name="minimalFailLevel">Минимальный тип ошибки для успешного Result.</param>
	/// <returns>Содержит ли Result бизнес сообщения &gt;= minimalFailLevel.</returns>
	bool IsSuccess(BusinessMessageType minimalFailLevel);
}

/// <summary>Generic монада Result.</summary>
/// <typeparam name="T">Тип данных.</typeparam>
public class Result<T> : ResultBase, IResult
{
	/// <summary>Список бизнес-ошибок.</summary>
	public IReadOnlyCollection<BusinessMessage> Messages { get; set; } = null!;

	/// <summary>Данные.</summary>
	public T Data { get; set; }

	/// <summary>Позитивен ли Result.</summary>
	/// <param name="minimalFailLevel">Минимальный тип ошибки для успешного Result.</param>
	/// <returns>Содержит ли Result бизнес сообщения &gt;= minimalFailLevel.</returns>
	public bool IsSuccess(BusinessMessageType minimalFailLevel)
	{
		return EqualSuccess(Messages, minimalFailLevel);
	}

	/// <summary>Позитивен ли Result.</summary>
	/// <returns>Содержит ли Result бизнес сообщения &gt;= BusinessMessageType.Error.</returns>
	public bool IsSuccess() => IsSuccess(BusinessMessageType.Error);

	/// <summary>Оператор приведения к данным.</summary>
	/// <param name="data">Данные.</param>
	public static implicit operator Result<T>(T data) => Success(data);

	private Result(T data) => Data = data;

	private Result(IEnumerable<BusinessMessage> messages)
	{
		Messages = messages.ToList();
	}

	/// <summary>Получение позитивного Result.</summary>
	/// <param name="data">Данные.</param>
	/// <returns>Result.</returns>
	public static Result<T> Success(T data) => new Result<T>(data);

	/// <summary>Получение негативного Result.</summary>
	/// <param name="messages">Список бизнес-сообщений.</param>
	/// <returns>Result.</returns>
	public static Result<T> Fail(IEnumerable<BusinessMessage> messages) => new Result<T>(messages);

	/// <summary>Получение негативного Result.</summary>
	/// <param name="message">Текст бизнес-сообщения.</param>
	/// <returns>Result.</returns>
	public static Result<T> Fail(string message)
	{
		return new Result<T>(new List<BusinessMessage>
		{
			new BusinessMessage { Message = message }
		});
	}

	/// <summary>Получение негативного Result.</summary>
	/// <param name="messages">Список бизнес-сообщений.</param>
	/// <returns>Result.</returns>
	public static Result<T> Forbidden(IEnumerable<BusinessMessage> messages)
	{
		foreach (BusinessMessage message in messages)
			message.HttpCode = 403;
		return new Result<T>(messages);
	}

	/// <summary>Получение негативного Result.</summary>
	/// <param name="message">Бизнес-сообщение.</param>
	/// <returns>Result.</returns>
	public static Result<T> Forbidden(string message)
	{
		return new Result<T>(new List<BusinessMessage>
		{
			new BusinessMessage
			{
				HttpCode = 403,
				Message = message
			}
		});
	}

	/// <summary>Оператор приведения Result и generic Result.</summary>
	/// <param name="result">Result.</param>
	public static implicit operator Result<T>(Result result)
	{
		return Fail(result.Messages);
	}
}

/// <summary>Монада Result.</summary>
public class Result : ResultBase, IResult
{
	/// <summary>Список бизнес сообщений.</summary>
	public IReadOnlyCollection<BusinessMessage> Messages { get; set; } = null!;

	private Result()
	{
	}

	private Result(IEnumerable<BusinessMessage> messages)
	{
		Messages = messages.ToList();
	}

	/// <summary>Позитивен ли Result.</summary>
	/// <param name="minimalFailLevel">Минимальный тип ошибки для успешного Result.</param>
	/// <returns>Содержит ли Result бизнес сообщения &gt;= minimalFailLevel.</returns>
	public bool IsSuccess(BusinessMessageType minimalFailLevel)
	{
		return EqualSuccess(Messages, minimalFailLevel);
	}

	/// <summary>Позитивен ли Result.</summary>
	/// <returns>Содержит ли Result бизнес сообщения &gt;= BusinessMessageType.Error.</returns>
	public bool IsSuccess() => IsSuccess(BusinessMessageType.Error);

	/// <summary>You can use impicit conversion instead.</summary>
	/// <typeparam name="T">Тип данных.</typeparam>
	/// <param name="data">Данные.</param>
	/// <returns>Result.</returns>
	public static Result<T> Success<T>(T data) => Result<T>.Success(data);

	/// <summary>Получение позитивного Result.</summary>
	/// <returns>Result.</returns>
	public static Result Success() => new Result();

	/// <summary>Получение негативного Result.</summary>
	/// <param name="messages">Список бизнес сообщений.</param>
	/// <returns>Result.</returns>
	public static Result Fail(IReadOnlyCollection<BusinessMessage> messages)
	{
		return new Result(messages);
	}

	/// <summary>Получение негативного Result.</summary>
	/// <param name="message">Список бизнес сообщений.</param>
	/// <returns>Result.</returns>
	public static Result Fail(BusinessMessage message)
	{
		return Fail(new BusinessMessage[1]
		{
			message
		});
	}

	/// <summary>Получение негативного Result.</summary>
	/// <param name="message">Текст бизнес-сообщения.</param>
	/// <param name="messageType">Тип бизнес-сообщения.</param>
	/// <param name="httpCode">Код.</param>
	/// <returns>Result.</returns>
	public static Result Fail(string message,
		BusinessMessageType messageType = BusinessMessageType.Error,
		int httpCode = 477)
	{
		return Fail(new BusinessMessage
		{
			Message = message,
			HttpCode = httpCode,
			Type = messageType
		});
	}

	/// <summary>Получение негативного Result.</summary>
	/// <param name="message">Бизнес-сообщение.</param>
	/// <returns>Result.</returns>
	public static Result Forbidden(BusinessMessage message)
	{
		message.HttpCode = 403;
		return new Result(new BusinessMessage[1]
		{
			message
		});
	}

	/// <summary>Получение негативного Result.</summary>
	/// <param name="message">Бизнес-сообщение.</param>
	/// <param name="messageType">Тип бизнес-сообщения.</param>
	/// <returns>Result.</returns>
	public static Result Forbidden(string message, BusinessMessageType messageType = BusinessMessageType.Error)
	{
		return Forbidden(new BusinessMessage
		{
			Message = message,
			HttpCode = 403,
			Type = messageType
		});
	}
}

/// <summary>Базовый класс для монады Result.</summary>
public class ResultBase
{
	/// <summary>Проверка на позититвный Result.</summary>
	/// <param name="messages">Список бизнес-сообщений.</param>
	/// <param name="minimalLevel">Минимальный тип бизнес-сообщений.</param>
	/// <returns>Позитивен ли Result.</returns>
	protected static bool EqualSuccess(
		IReadOnlyCollection<BusinessMessage> messages,
		BusinessMessageType minimalLevel)
	{
		return messages == null! ||
			!messages.Any(s => s.Type >= minimalLevel);
	}
}