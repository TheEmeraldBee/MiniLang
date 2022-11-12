using MiniLang.Core;
using MiniLang.Internal;

var engine = new Engine(new DefaultWriter());
engine.AddLibrary(new DefaultLibrary());
engine.AddModule(new DebugModule());

while (true)
{
    Console.Write("MiniLang ::: ");
    var res = engine.Run(Console.ReadLine() ?? string.Empty);
    if (!res.QuerySuccess())
    {
        engine.Writer.Message(res.GetError());
    }
    Console.WriteLine();
}
