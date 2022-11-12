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
                    if (engine.Get() == 0)
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
            case 'L':
                var times = engine.GetNumAfter();
                if (times == null) return new Result(false, "ERROR: 'L' expected a number after it.");

                while (engine.CurrentCommand != '[')
                {
                    if (!engine.MoveReader())
                        return new Result(false, "ERROR: 'L' Expected a loop but did not find it.");
                }

                loopStart = engine.GetCodeIdx();
                
                for (var time=0; time < times; time++)
                {
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

                engine.SetReader(loopStart);
                while (engine.CurrentCommand != ']')
                {
                    if (!engine.MoveReader())
                    {
                        return new Result(true, "ERROR: Expected ] but didn't receive it for loop.");
                    }
                }
                break;
            
            case 'W':
                
                int type;
                
                while (engine.CurrentCommand != '=')
                {
                    if (!engine.MoveReader())
                    {
                        return new Result(false, "ERROR: Expected = but didn't receive it for 'W'.");
                    }

                    engine.HandleCommand();
                }
                var start = engine.Get();
                
                if (!engine.MoveReader())
                {
                    return new Result(false, "ERROR: While Loop Incomplete");
                }
                if (engine.CurrentCommand == '>')
                {
                    type = 1;
                    if (!engine.MoveReader())
                    {
                        return new Result(false, "ERROR: While Loop Incomplete");
                    }
                } else if (engine.CurrentCommand == '<')
                {
                    type = 2;
                    if (!engine.MoveReader())
                    {
                        return new Result(false, "ERROR: While Loop Incomplete");
                    }
                } else if (engine.CurrentCommand == '!')
                {
                    type = 3;
                    if (!engine.MoveReader())
                    {
                        return new Result(false, "ERROR: While Loop Incomplete");
                    }
                } else
                {
                    type = 0;
                    engine.MoveReader(-1);
                }
                
                // End of IF
                while (engine.CurrentCommand != '[')
                {
                    var res = engine.HandleCommand();
                    if (!res.QuerySuccess()) return res;
                    
                    if (!engine.MoveReader())
                    {
                        return new Result(false, "ERROR: Expected [ but didn't receive it for while loop.");
                    }
                }

                loopStart = engine.GetCodeIdx();

                while (true)
                {
                    while (engine.CurrentCommand != ']')
                    {
                        if (!engine.MoveReader())
                        {
                            return new Result(true, "ERROR: Expected ] but didn't receive it for while loop.");
                        }
                        
                        var res = engine.HandleCommand();
                        if (!res.QuerySuccess())
                        {
                            return res;
                        }
                    }
                    
                    var isTrue = type switch
                    {
                        1 => start > engine.Get(),
                        2 => start < engine.Get(),
                        3 => start != engine.Get(),
                        _ => start == engine.Get()
                    };

                    if (!isTrue)
                    {
                        while (engine.CurrentCommand != ']')
                        {
                            if (!engine.MoveReader())
                            {
                                return new Result(true, "ERROR: Expected ] but didn't receive it for while loop.");
                            }
                        }

                        break;
                    }
                    
                    engine.SetReader(loopStart);
                }
                
                break;
        }

        return new Result(true);
    }
}