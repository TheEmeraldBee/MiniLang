using MiniLang.Core;

namespace MiniLang.Internal;

public class LoopModule : IModule
{
    public Result HandleCommand(Engine engine)
    {
        switch (engine.CurrentCommand)
        {
            case '[':
                var loopStart = engine.GetCodeIdx();
                while (true)
                {
                    if (engine.Program[engine.Idx] == 0)
                    {
                        while (engine.CurrentCommand != ']')
                        {
                            if (!engine.MoveReader())
                            {
                                return new Result(true, "ERROR: Expected ] but didn't receive it for loop.");
                            }
                        }

                        break;
                    }
                    
                    while (engine.CurrentCommand != ']')
                    {
                        if (!engine.MoveReader())
                        {
                            return new Result(true, "ERROR: Expected ] but didn't receive it for loop.");
                        }
                        
                        var res = engine.HandleCommand();
                        if (!res.QuerySuccess())
                        {
                            return res;
                        }
                    }

                    engine.SetReader(loopStart);
                }

                break;
        }

        return new Result(true);
    }
}