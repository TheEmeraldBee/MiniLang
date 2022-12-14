using MiniLang.Core;

namespace MiniLang.Internal;

public class RandomModule : IModule
{
    public Result HandleCommand(Engine engine)
    {
        switch (engine.CurrentCommand)
        {
            case '?':
                engine.Set(
                    new Random(DateTime.Now.Millisecond * DateTime.Now.Minute)
                        .Next(0, (engine.GetNumAfter() ?? 5) + 1));
                break;
        }

        return new Result(true);
    }

    public Result HandleSkip(Engine engine)
    {
        return new Result(true);
    }
    
}