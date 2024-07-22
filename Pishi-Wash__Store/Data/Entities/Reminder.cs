namespace StudentWorkplace.Data.Entities;

public class Reminder
{
	public int ReminderId { get; set; }

	public string ReminderMessage { get; set; }

	public int ScheduleId { get; set; }

	public Schedule Schedule { get; set; }
}