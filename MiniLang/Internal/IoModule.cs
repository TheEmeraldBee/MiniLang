using MiniLang.Core;

namespace MiniLang.Internal;

public class IoModule : IModule
{
    public Result HandleCommand(Engine engine)
    {
        switch (engine.CurrentCommand)
        {
            // Output
            case '/':
                engine.Writer.NewLine(engine);
                break;
            case '.':
                engine.Writer.Print(engine);
                break;
            case '^':
                engine.Writer.PrintAscii(engine);
                break;
            
            // Input
            case ',':
                engine.Writer.ReadAsciiCharacter(engine);
                break;
            case '_':
                return engine.Writer.ReadNumber(engine);
            case '&':
                return engine.Writer.ReadAsciiString(engine);
        }

        return new Result(true);
    }
}