using MiniLang.Core;
using MiniLang.Internal;

var engine = new Engine(new DefaultWriter());
engine.AddLibrary(new DefaultLibrary());

while (true)
{
    Console.Write("MiniLang ::: ");
    engine.Run(Console.ReadLine() ?? string.Empty);
    Console.WriteLine();
}

