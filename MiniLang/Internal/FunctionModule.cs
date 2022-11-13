using MiniLang.Core;

namespace MiniLang.Internal;

public class FunctionModule : IModule
{
    private Dictionary<string, string> _functions = new();
    public Result HandleCommand(Engine engine)
    {
        switch (engine.CurrentCommand)
        {
            case 'F':
                var functionName = "";

                if (!engine.MoveReader())
                {
                    return new Result(false, "ERROR 'F' Expected a '\"' pair after it.");
                }

                if (engine.CurrentCommand != '"')
                {
                    return new Result(false, "ERROR 'F' Expected a '\"' pair after it.");
                }

                while (true)
                {
                    if (!engine.MoveReader())
                    {
                        return new Result(false, "ERROR: 'F' Expected a '\"' pair and only found one.");
                    }
                    if (engine.CurrentCommand == '"') break;

                    functionName += engine.CurrentCommand;
                }

                while (engine.CurrentCommand != '(')
                {
                    if (!engine.MoveReader()) { return new Result(false, "ERROR: Expected '(' to start function body but was not found for 'F'"); }
                }

                if (!engine.MoveReader())
                {
                    return new Result(false, "ERROR: Expected ')' to end function but was not found for 'F'");
                }

                var functionBody = "";
                while (engine.CurrentCommand != ')')
                {
                    var startPos = engine.GetCodeIdx();
                    
                    var res = engine.HandleSkip();
                    if (!res.QuerySuccess())
                    {
                        return res;
                    }

                    var endPos = engine.GetCodeIdx();

                    if (startPos == endPos)
                    {
                        if (!engine.MoveReader())
                        {
                            return new Result(false, "ERROR: Expected ')' to end function body but was not found for 'F'");
                        }
                        functionBody += engine.CurrentCommand;
                    }
                    else
                    {
                        for (var i = startPos + 1; i <= endPos; i++)
                        {
                            engine.SetReader(i);
                            functionBody += engine.CurrentCommand;
                        }
                    }

                }
                engine.MoveReader();

                functionBody = functionBody.Remove(functionBody.Length - 1);

                if (!_functions.TryAdd(functionName, functionBody))
                {
                    return new Result(false, $"ERROR: Function Already Defined With Name {functionName}");
                }

                break;
            case 'R':
                functionName = "";

                if (!engine.MoveReader())
                {
                    return new Result(false, "ERROR 'R' Expected a '\"' pair after it.");
                }

                if (engine.CurrentCommand != '"')
                {
                    return new Result(false, "ERROR 'R' Expected a '\"' pair after it.");
                }

                while (true)
                {
                    if (!engine.MoveReader())
                    {
                        return new Result(false, "ERROR: 'R' Expected a '\"' pair and only found one.");
                    }
                    if (engine.CurrentCommand == '"') break;

                    functionName += engine.CurrentCommand;
                }

                if (_functions.TryGetValue(functionName, out var code))
                {
                    var contextLoc = engine.GetCodeIdx();
                    var contextCode = engine.GetCodeString();
                    var res = engine.Run(code);
                    if (!res.QuerySuccess())
                    {
                        return res;
                    }
                    
                    engine.SetContext(contextCode, contextLoc);
                }
                else
                {
                    return new Result(false, $"ERROR: 'R' could not find function with name {functionName}");
                }

                break;
        }

        return new Result(true);
    }

    public Result HandleSkip(Engine engine)
    {
        switch (engine.CurrentCommand)
        {
            case 'F':
                
                break;
        }

        return new Result(true);
    }
}