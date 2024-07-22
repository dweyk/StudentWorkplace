namespace StudentWorkplace.Data.Entities;

public class Schedule
{
	public int SсheduleId { get; set; }

	public string Title { get; set; } = null!;

	public DateTime DateTime { get; set; }

	public int ReminderId { get; set; }

	public ICollection<Reminder> Reminders { get; set; }

	public ICollection<User> Users { get; set; } = null!;
}