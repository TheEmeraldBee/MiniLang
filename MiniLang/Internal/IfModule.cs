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

                var start = engine.Get();

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
                } else if (engine.CurrentCommand == '!')
                {
                    type = 3;
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
                
                var end = engine.Get();

                // The Rest Of The If
                var canRun = type switch
                {
                    1 => start > end,
                    2 => start < end,
                    3 => start != end,
                    _ => start == end
                };

                // If
                while (engine.CurrentCommand != ')')
                {
                    if (!engine.MoveReader())
                    {
                        return new Result(false, "ERROR: Expected ) but didn't receive it for if.");
                    }

                    if (engine.CurrentCommand == '|')
                    {
                        break;
                    }

                    if (canRun)
                    {
                        var res = engine.HandleCommand();
                        if (!res.QuerySuccess())
                        {
                            return res;
                        }
                    }
                    else
                    {
                        // Fix Nesting
                        var skipRes = engine.HandleSkip();
                        if (!skipRes.QuerySuccess())
                        {
                            return skipRes;
                        }
                    }
                }

                // Else
                if (engine.CurrentCommand == '|')
                {
                    while (engine.CurrentCommand != ')')
                    {
                        if (!engine.MoveReader())
                        {
                            return new Result(false, "ERROR: Expected ) but didn't receive it for if.");
                        }

                        if (!canRun)
                        {
                            engine.HandleCommand();
                        }
                    }
                }

                engine.MoveReader();

                break;
        }

        return new Result(true);
    }

    public Result HandleSkip(Engine engine)
    {
        switch (engine.CurrentCommand)
        {
            // If Statement
            case '(':
                while (engine.CurrentCommand != ')')
                {
                    if (!engine.MoveReader())
                        return new Result(false, "ERROR: Expected ) but didn't receive it for if.");
                }

                engine.MoveReader();

                break;
        }

        return new Result(true);
    }
}