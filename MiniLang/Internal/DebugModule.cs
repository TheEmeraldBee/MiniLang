using MiniLang.Core;

namespace MiniLang.Internal;

public class DebugModule : IModule
{
    public Result HandleCommand(Engine engine)
    {
        switch (engine.CurrentCommand)
        {
            case 'I':
                engine.Writer.Message(engine.GetIdx().ToString());
                break;
            case 'V':
                engine.Writer.Message("MiniLang Version: " + Constants.Version);
                break;
        }

        return new Result(true);
    }
}