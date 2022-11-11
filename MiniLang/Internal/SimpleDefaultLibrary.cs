using MiniLang.Core;

namespace MiniLang.Internal;


// The Simplest Library Required For The Engine to Function in more than one way
public class SimpleDefaultLibrary : ILibrary
{
    public List<IModule> GetLibraryModules()
    {
        return new List<IModule>() { new BasicsModule(), new IoModule(), new LoopModule() };
    }
}