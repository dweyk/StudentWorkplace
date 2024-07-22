namespace StudentWorkplace.Services;

using System.Collections.Immutable;

using Data;
using Data.Entities;

public class UserService
{
	private readonly ApplicationDbContext _applicationDbContext;

	public UserService(ApplicationDbContext applicationDbContext)
	{
		_applicationDbContext = applicationDbContext;
	}

	public async Task<bool> AuthorizationAsync(string username, string password)
	{
		var user = await _applicationDbContext.Users.SingleOrDefaultAsync(u => u.UserLogin == username);
		if (user == null)
		{
			return false;
		}

		if (user.UserPassword.Equals(password))
		{
			await _applicationDbContext.Roles.ToListAsync();
			UserSetting.Default.UserName = user.UserName;
			UserSetting.Default.UserSurname = user.UserSurname;
			UserSetting.Default.UserPatronymic = user.UserPatronymic;
			UserSetting.Default.UserRole = user.UserRoleNavigation.RoleName;
			UserSetting.Default.UserId = user.UserId;

			return true;
		}

		return false;
	}

	public async Task<List<string>> GetAllUserLogin()
	{
		return await _applicationDbContext.Users.Select(u => u.UserLogin).ToListAsync();
	}

	public IEnumerable<User> GetAllUsers(bool asNoTracking = false)
	{
		var users = _applicationDbContext.Users;

		return asNoTracking
			? users.AsNoTracking()
			: users;
	}

	public async Task AddNewUser(
		string UserName,
		string UserSurname,
		string UserPatronymic,
		string UserLogin,
		string UserPassword)
	{
		await _applicationDbContext.Users.AddAsync(new User
		{
			UserName = UserName,
			UserSurname = UserSurname,
			UserPatronymic = UserPatronymic,
			UserLogin = UserLogin,
			UserPassword = UserPassword,
			UserRole = 2,
		});

		await _applicationDbContext.SaveChangesAsync();
	}

	public User GetCurrentUser()
	{
		return _applicationDbContext.Users.FirstOrDefault(user => user.UserId == UserSetting.Default.UserId)!;
	}
}