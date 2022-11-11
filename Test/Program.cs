using MiniLang;

var projects = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + "/MiniLangProjects/";
if (!Directory.Exists(projects))
{
    Console.Error.WriteLine("ERROR: Could Not Find MiniLangProjects folder in Documents. Please Create It!");
    return;
}

Console.WriteLine("Please Enter Your Project Name\n(Should be a .minilang file in your documents/MiniLangProjects folder.): ");
var file = Console.ReadLine();
if (!File.Exists(projects + file + ".minilang"))
{
    Console.Error.WriteLine($"ERROR: Could Not Find Project {file}.minilang in your projects. Please Create It or Fix The Name!");
    return;
}

var engine = new Engine();
engine.HandleCodeFile(projects + file + ".minilang");
// engine.HandleCodeFile();

// while (true)
// {
//     Console.Write("Mini Lang::: ");
//     var res = Console.ReadLine() ?? string.Empty;
//     
//     engine.HandleCodeString(res);
//     
//     Console.WriteLine();
// }