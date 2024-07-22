namespace StudentWorkplace.Views.LaboratoryWorkPage;

using System.Reflection;

using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.Emit;

public partial class LaboratoryWorkPage : Page
{
	public LaboratoryWorkPage()
	{
		InitializeComponent();
	}

	private void AnalyzeCodeButton_Click(object sender, RoutedEventArgs e)
	{
		var code = CodeTextBox.Text;

		AnalyzeAndExecuteCode(code);
	}

	private void AnalyzeAndExecuteCode(string code)
	{
		var syntaxTree = CSharpSyntaxTree.ParseText(code);
		var assemblyName = Path.GetRandomFileName();

		var references = new MetadataReference[]
		{
			MetadataReference.CreateFromFile(typeof(object).Assembly.Location),
			MetadataReference.CreateFromFile(typeof(Console).Assembly.Location)
		};

		var compilation = CSharpCompilation.Create(
			assemblyName,
			syntaxTrees: new[] { syntaxTree },
			references: references,
			options: new CSharpCompilationOptions(OutputKind.ConsoleApplication));

		using MemoryStream ms = new MemoryStream();

		var result = compilation.Emit(ms);

		if (!result.Success)
		{
			var errors = "Код содержит ошибки компиляции:\n";

			foreach (Diagnostic diagnostic in result.Diagnostics)
			{
				errors += $"{diagnostic.Id}: {diagnostic.GetMessage()}\n";
			}

			MessageBox.Show(errors);

			return;
		}

		ms.Seek(0, SeekOrigin.Begin);

		var assembly = Assembly.Load(ms.ToArray());
		var entryPoint = assembly.EntryPoint;
		var obj = assembly.CreateInstance(entryPoint.Name);

		using var writer = new StringWriter();

		Console.SetOut(writer);

		entryPoint.Invoke(obj, null);

		MessageBox.Show("Код успешно скомпилирован и выполнен.\n" + writer);
	}
}