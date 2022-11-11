using MiniLang.Core;

namespace MiniLang.Internal;

public class DefaultWriter : IWriter
{
    public void Print(Engine engine)
    {
        Console.Write(engine.Program[engine.Idx]);
    }

    public void PrintAscii(Engine engine)
    {
        Console.Write((char) engine.Program[engine.Idx]);
    }

    public void NewLine(Engine engine)
    {
        Console.WriteLine("");
    }

    // Where errors are logged
    public void Message(string message)
    {
        Console.WriteLine(message);
    }

    public Result ReadNumber(Engine engine)
    {
        var toRead = Console.ReadLine();
        if (int.TryParse(toRead, out var result))
        {
            engine.Program[engine.Idx] = result;
        }
        else
        {
            return new Result(false, "ERROR: Reading Input Could Not Parse Input.");
        }

        return new Result(true);
    }

    public void ReadAsciiCharacter(Engine engine)
    {
        engine.Program[engine.Idx] = (Console.ReadLine() ?? "0")[0];
    }

    public Result ReadAsciiString(Engine engine)
    {
        var toRead = Console.ReadLine();
        foreach (var read in toRead)
        {
            if (!engine.MoveReader())
            {
                return new Result(false, "ERROR: Reading Input Went Past Maximum Program Size.");
            }

            engine.Program[engine.Idx] = read;
        }

        return new Result(true);
    }
}