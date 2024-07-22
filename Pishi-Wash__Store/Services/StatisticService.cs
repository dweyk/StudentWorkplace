namespace StudentWorkplace.Services;

using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Spreadsheet;

public class StatisticService
{
	private readonly PassedQuestionService _passedQuestionService;

	public StatisticService(PassedQuestionService passedQuestionService)
	{
		_passedQuestionService = passedQuestionService;
	}

	public bool LoadStatistics()
	{
		try
		{
			var desktopPath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);

			var filePath = Path.Combine(desktopPath, "userTests.xlsx");

			// Создать Excel-файл
			using var spreadsheetDocument = SpreadsheetDocument.Create(filePath, SpreadsheetDocumentType.Workbook);

			// Добавить лист в Excel-файл
			var workbookPart = spreadsheetDocument.AddWorkbookPart();
			workbookPart.Workbook = new Workbook();

			var worksheetPart = workbookPart.AddNewPart<WorksheetPart>();
			worksheetPart.Worksheet = new Worksheet(new SheetData());

			var sheets = spreadsheetDocument.WorkbookPart?.Workbook.AppendChild(new Sheets());

			// Append a new worksheet and associate it with the workbook.
			var sheet = new Sheet
			{
				Id = workbookPart.GetIdOfPart(worksheetPart),
				SheetId = 1,
				Name = "UserSheet",
			};

			sheets!.Append(sheet);

			// Добавить заголовки столбцов
			var row = worksheetPart.Worksheet.GetFirstChild<SheetData>()!.AppendChild(new Row());
			
			row.Append(CreateCell("Имя пользователя:"));
			row.Append(CreateCell("Наименование теста:"));
			row.Append(CreateCell("Результат теста:"));

			var userWithTests = _passedQuestionService.GetPassedQuestionsForLoad();
			
			// Добавить данные пользователей
			foreach (var user in userWithTests)
			{
				row = worksheetPart.Worksheet.GetFirstChild<SheetData>()!.AppendChild(new Row());
				row.Append(CreateCell(user.UserName));

				//var count = 0;
				foreach (var testResult in user.PassedQuestions)
				{
					row.Append(CreateCell(testResult.TestTopic));
					row.Append(CreateCell(testResult.NumberCorrectAnswers));
				}
			}

			// Сохранить изменения
			worksheetPart.Worksheet.Save();

			return true;
		}
		catch
		{
			return false;
		}
	}

	private static Cell CreateCell(string text)
	{
		var cell = new Cell
		{
			CellValue = new CellValue(text),
			DataType = new EnumValue<CellValues>(CellValues.String)
		};

		return cell;
	}
}