namespace StudentWorkplace.Data.Entities;

public class VideoLecture
{
	public int VideoLectureId { get; set; }

	public string Title { get; set; }

	public byte[] Data { get; set; }

	public DateTime UploadDate { get; set; }
}