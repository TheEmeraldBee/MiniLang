using MiniLang.Core;

namespace MiniLang.Internal;

public class StringModule : IModule
{
    public Result HandleCommand(Engine engine)
    {
        switch (engine.CurrentCommand)
        {
            case 'C':
                if (!engine.MoveReader())
                {
                    return new Result(false, "ERROR: 'C' expected a character after it.");
                }
                engine.Set(engine.CurrentCommand);
                break;
            case 'P':
                var output = "";

                if (!engine.MoveReader())
                {
                    return new Result(false, "ERROR 'P' Expected a '\"' pair after it.");
                }

                if (engine.CurrentCommand != '"')
                {
                    return new Result(false, "ERROR 'P' Expected a '\"' pair after it.");
                }

                while (true)
                {
                    if (!engine.MoveReader())
                    {
                        return new Result(false, "ERROR: 'P' Expected a '\"' pair and only found one.");
                    }
                    if (engine.CurrentCommand == '"') break;

                    output += engine.CurrentCommand;
                }
                
                engine.Writer.WriteString(output);

                break;
            case 'S':
                var stringSet = "";
                
                if (!engine.MoveReader())
                {
                    return new Result(false, "ERROR 'S' Expected a '\"' pair after it.");
                }

                if (engine.CurrentCommand != '"')
                {
                    return new Result(false, "ERROR 'S' Expected a '\"' pair after it.");
                }

                while (true)
                {
                    if (!engine.MoveReader())
                    {
                        return new Result(false, "ERROR: 'S' Expected a '\"' pair and only found one.");
                    }
                    if (engine.CurrentCommand == '"') break;

                    stringSet += engine.CurrentCommand;
                }

                var initPos = engine.GetIdx();
                foreach (var character in stringSet)
                {
                    engine.Set(character);
                    engine.MoveIdx(1);
                }
                engine.SetIdx(initPos);
                
                break;
        }

        return new Result(true);
    }

    public Result HandleSkip(Engine engine)
    {
        return new Result(true);
    }
}