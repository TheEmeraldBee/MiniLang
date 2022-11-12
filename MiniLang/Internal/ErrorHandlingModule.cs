using MiniLang.Core;

namespace MiniLang.Internal;

public class ErrorHandlingModule : IModule
{
    public Result HandleCommand(Engine engine)
    {
        switch (engine.CurrentCommand)
        {
            case ':':
                var except = false;
                while (engine.CurrentCommand != '|')
                {
                    if (!engine.MoveReader())
                    {
                        return new Result(false, "ERROR: Expected | to end try");
                    }

                    var res = engine.HandleCommand();
                    if (!res.QuerySuccess())
                    {
                        except = true;
                        break;
                    }
                }

                while (engine.CurrentCommand != ';')
                {
                    if (!engine.MoveReader())
                    {
                        return new Result(false, "ERROR: Expected ; to end try except block.");
                    }

                    if (except)
                    {
                        var res = engine.HandleCommand();

                        if (!res.QuerySuccess())
                        {
                            return res;
                        }
                    }
                }
                break;
        }

        return new Result(true);
    }

    public Result HandleSkip(Engine engine)
    {
        switch (engine.CurrentCommand)
        {
            case ':':
                while (engine.CurrentCommand != ';')
                {
                    if (!engine.MoveReader())
                    {
                        return new Result(false, "ERROR: Expected ; to try and except block");
                    }
                }

                break;
        }

        return new Result(true);
    }
}