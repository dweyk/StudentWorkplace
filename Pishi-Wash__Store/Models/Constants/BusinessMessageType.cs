namespace StudentWorkplace.Models.Constants;

/// <summary>Типы бизнес сообщений.</summary>
public enum BusinessMessageType
{
	/// <summary>Информационное сообщение.</summary>
	Info = 0,

	/// <summary>Сообщение.</summary>
	Message = 10, // 0x0000000A

	/// <summary>Предупреждение.</summary>
	Warning = 20, // 0x00000014

	/// <summary>Ошибка.</summary>
	Error = 30, // 0x0000001E
}