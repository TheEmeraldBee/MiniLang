using MiniLang.Core;

namespace MiniLang.Internal;

public class IfModule : IModule
{
    public Result HandleCommand(Engine engine)
    {
        switch (engine.CurrentCommand)
        {
             // If Statement
            case '(':
                int type;
                // Start of IF
                while (engine.CurrentCommand != '=')
                {
                    if (!engine.MoveReader())
                    {
                        return new Result(false, "ERROR: Expected = but didn't receive it for if.");
                    }

                    engine.HandleCommand();
                }

                var start = engine.Program[engine.Idx];

                if (!engine.MoveReader())
                {
                    return new Result(false, "ERROR: If statement incomplete expected '>', other part of if, or '|'");
                }
                if (engine.CurrentCommand == '>')
                {
                    type = 1;
                    if (!engine.MoveReader())
                    {
                        return new Result(false, "ERROR: If statement incomplete expected other part of if or '|'");
                    }
                } else if (engine.CurrentCommand == '<')
                {
                    type = 2;
                    if (!engine.MoveReader())
                    {
                        return new Result(false, "ERROR: If statement incomplete expected other part of if or '|'");
                    }
                } else
                {
                    type = 0;
                    engine.MoveReader(-1);
                }

                // End of IF
                while (engine.CurrentCommand != '|')
                {
                    if (!engine.MoveReader())
                    {
                        return new Result(false, "ERROR: Expected | but didn't receive it for if.");
                    }

                    engine.HandleCommand();
                }
                
                var end = engine.Program[engine.Idx];

                // The Rest Of The If
                var canRun = type switch
                {
                    1 => start > end,
                    2 => start < end,
                    _ => start == end
                };

                while (engine.CurrentCommand != ')')
                {
                    if (!engine.MoveReader())
                    {
                        return new Result(false, "ERROR: Expected ) but didn't receive it for if.");
                    }

                    if (canRun)
                    {
                        engine.HandleCommand();
                    }
                }

                break;
        }

        return new Result(true);
    }
}