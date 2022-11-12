using MiniLang.Core;

namespace MiniLang.Internal;

public class DefaultWriter : IWriter
{
    public void Print(Engine engine)
    {
        Console.Write(engine.Get());
    }

    public void PrintAscii(Engine engine)
    {
        Console.Write((char)engine.Get());
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
            engine.Set(result);
        }
        else
        {
            return new Result(false, "ERROR: Reading Input Could Not Parse Input.");
        }

        return new Result(true);
    }

    public void ReadAsciiCharacter(Engine engine)
    {
        engine.Set((Console.ReadLine() ?? "0")[0]);
    }

    public Result ReadAsciiString(Engine engine)
    {
        var toRead = Console.ReadLine() ?? "";
        var startIdx = engine.GetIdx();
        foreach (var read in toRead)
        {
            if (!engine.MoveReader())
            {
                return new Result(false, "ERROR: Reading Input Went Past Maximum Program Size.");
            }

            engine.Set(read);
        }
        engine.SetIdx(startIdx);

        return new Result(true);
    }

    public void WriteString(string value)
    {
        Console.Write(value);
    }
}