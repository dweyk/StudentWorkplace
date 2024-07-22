namespace StudentWorkplace.Data;

using Entities;

public sealed partial class ApplicationDbContext : DbContext
{
	public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
		: base(options)
	{
		Database.EnsureCreated();
	}

	// protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
	// {
	// 	optionsBuilder.UseNpgsql("Host=localhost;Port=5432;Database=mydatabase;Username=postgres;Password=postgres");
	// }

	public DbSet<Role> Roles { get; set; } = null!;

	public DbSet<User> Users { get; set; } = null!;

	public DbSet<Lecture> Lectures { get; set; } = null!;

	public DbSet<Question> Questions { get; set; } = null!;

	public DbSet<PassedQuestion> PassedQuestions { get; set; } = null!;

	public DbSet<VideoLecture> VideoLectures { get; set; } = null!;

	public DbSet<Schedule> Schedules { get; set; } = null!;

	public DbSet<Reminder> Reminders { get; set; } = null!;

	protected override void OnModelCreating(ModelBuilder modelBuilder)
	{
		modelBuilder
			.UseCollation("utf8mb4_0900_ai_ci")
			.HasCharSet("utf8mb4");

		modelBuilder.Entity<Role>(entity =>
		{
			entity.HasKey(e => e.RoleId).HasName("PRIMARY");

			entity.ToTable("role");

			entity.Property(e => e.RoleId).HasColumnName("RoleID");
			entity.Property(e => e.RoleName).HasMaxLength(100);
		});

		modelBuilder.Entity<User>(entity =>
		{
			entity.HasKey(e => e.UserId).HasName("PRIMARY");

			entity.ToTable("user");

			entity.HasIndex(e => e.UserRole, "conn__User");

			entity.Property(e => e.UserId).HasColumnName("UserID");
			entity.Property(e => e.UserLogin).HasColumnType("text");
			entity.Property(e => e.UserName).HasMaxLength(100);
			entity.Property(e => e.UserPassword).HasColumnType("text");
			entity.Property(e => e.UserPatronymic).HasMaxLength(100);
			entity.Property(e => e.UserSurname).HasMaxLength(100);

			entity.HasOne(d => d.UserRoleNavigation)
				.WithMany(p => p.Users)
				.HasForeignKey(d => d.UserRole)
				.OnDelete(DeleteBehavior.ClientSetNull)
				.HasConstraintName("conn__User");
		});

		modelBuilder.Entity<Lecture>(entity =>
		{
			entity.HasKey(e => e.LectureId).HasName("PRIMARY");

			entity.ToTable("lecture");

			entity.Property(e => e.LectureId).HasColumnName("LectureId");
			entity.Property(e => e.Content).HasMaxLength(10000).IsRequired(false);
			entity.Property(e => e.Title).HasMaxLength(250).IsRequired(false);
		});

		modelBuilder.Entity<Schedule>(entity =>
		{
			entity.HasKey(e => e.SсheduleId).HasName("PRIMARY");

			entity.ToTable("schedule");

			entity.Property(q => q.SсheduleId).HasColumnName("SсheduleId");
			entity.Property(e => e.DateTime).IsRequired();
			entity.Property(e => e.Title).HasMaxLength(512).IsRequired();

			entity.HasMany(schedule => schedule.Users)
				.WithMany(user => user.Schedules);
		});

		modelBuilder.Entity<Reminder>(entity =>
		{
			entity.HasKey(e => e.ReminderId).HasName("PRIMARY");

			entity.ToTable("reminder");

			entity.Property(q => q.ReminderId).HasColumnName("ReminderId");
			entity.Property(e => e.ReminderMessage).HasMaxLength(512).IsRequired();

			entity.HasOne(reminder => reminder.Schedule)
				.WithMany(schedule => schedule.Reminders)
				.HasForeignKey(reminder => reminder.ScheduleId)
				.OnDelete(DeleteBehavior.Restrict)
				.HasConstraintName("schedule__reminder")
				.IsRequired();
		});

		modelBuilder.Entity<VideoLecture>(entity =>
		{
			entity.HasKey(e => e.VideoLectureId).HasName("PRIMARY");

			entity.ToTable("video_lectures");

			entity.Property(e => e.VideoLectureId).HasColumnName("VideoId");
			entity.Property(e => e.Title).HasMaxLength(250).IsRequired(false);
			entity.Property(e => e.Data).IsRequired();
			entity.Property(e => e.UploadDate).IsRequired();
		});

		modelBuilder.Entity<Question>(entity =>
		{
			entity.HasKey(e => e.QuestionId).HasName("PRIMARY");

			entity.ToTable("question");

			entity.Property(q => q.QuestionId).HasColumnName("QuestionId");
			entity.Property(q => q.Answer).HasMaxLength(500).IsRequired();
			entity.Property(q => q.TestTopic).HasMaxLength(250).IsRequired();
			entity.Property(q => q.Text).HasMaxLength(1500).IsRequired();
		});

		modelBuilder.Entity<PassedQuestion>(entity =>
		{
			entity.HasKey(e => e.PassedQuestionId).HasName("PRIMARY");

			entity.ToTable("passed_question");

			entity.Property(q => q.PassedQuestionId).HasColumnName("PassedQuestionId");
			entity.Property(q => q.IsCorrectAnswer).IsRequired();

			entity.HasOne(pq => pq.Question)
				.WithMany(q => q.PassedQuestions)
				.HasForeignKey(pq => pq.QuestionId)
				.OnDelete(DeleteBehavior.Restrict)
				.HasConstraintName("passedQuestion__question");

			entity.HasOne(pq => pq.User)
				.WithMany(q => q.PassedQuestions)
				.HasForeignKey(pq => pq.UserId)
				.OnDelete(DeleteBehavior.Restrict)
				.HasConstraintName("passedQuestion__user");
		});

		modelBuilder.Entity<Role>().HasData(
			new Role
			{
				RoleId = 1,
				RoleName = "Admin",
			},
			new Role
			{
				RoleId = 2,
				RoleName = "Student",
			}
		);

		modelBuilder.Entity<User>().HasData(
			new User
			{
				UserId = 1,
				UserSurname = "admin",
				UserName = "admin",
				UserPatronymic = "admin",
				UserLogin = "admin",
				UserPassword = "admin",
				UserRole = 1,
			}
		);

		OnModelCreatingPartial(modelBuilder);

		base.OnModelCreating(modelBuilder);
	}

	partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}