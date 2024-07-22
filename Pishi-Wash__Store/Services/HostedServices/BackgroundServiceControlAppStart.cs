namespace StudentWorkplace.Services.HostedServices;

using System.Threading;

using Microsoft.Extensions.Hosting;

/// <summary>
/// Базовый класс для создания Background сервис.
/// </summary>
internal abstract class BackgroundServiceControlAppStart : BackgroundService
{
	/// <summary>Ожидает загрузку приложения.</summary>
	/// <param name="lifetime">IHostApplicationLifetime.</param>
	/// <param name="stoppingToken">CancellationToken.</param>
	/// <returns>Загрузилось ли приложение.</returns>
	protected async Task<bool> WaitForAppStartup(
		IHostApplicationLifetime lifetime,
		CancellationToken stoppingToken)
	{
		var startedSource = new TaskCompletionSource();

		lifetime.ApplicationStarted.Register((() => startedSource.SetResult()));

		var cancelledSource = new TaskCompletionSource();

		stoppingToken.Register((() => cancelledSource.SetResult()));

		return await Task.WhenAny(startedSource.Task, cancelledSource.Task) == startedSource.Task;
	}
}