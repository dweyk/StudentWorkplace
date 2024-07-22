namespace StudentWorkplace.Models.Result;

using Constants;

/// <summary>Бизнес сообщение.</summary>
public class BusinessMessage
{
	/// <summary>Тип бизнес сообщения.</summary>
	public BusinessMessageType Type { get; set; } = BusinessMessageType.Error;

	/// <summary>Текст бизнес-сообщения.</summary>
	public string Message { get; set; }

	/// <summary>HttpCode бизнес-сообщения.</summary>
	public int? HttpCode { get; set; }

	/// <summary>Переопределение ToString.</summary>
	/// <returns>Текст бизнес-сообщения.</returns>
	public override string ToString() => Message;

	/// <summary>Оператор приведения string к бизнес-сообщению.</summary>
	/// <param name="errorMessage">Текст бизнес-сообщения.</param>
	public static implicit operator BusinessMessage(string errorMessage)
	{
		return new BusinessMessage
		{
			Message = errorMessage,
			Type = BusinessMessageType.Error,
			HttpCode = 477
		};
	}
}