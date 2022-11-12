using MiniLang.Core;

namespace MiniLang.Internal;

public class DefaultLibrary : ILibrary
{
    public List<IModule> GetLibraryModules()
    {
        return new List<IModule> { new BasicsModule(), new IoModule(), new LoopModule(), new IfModule(), 
            new AdvancedModule(), new RandomModule(), new StringModule(), new TempVariableModule(),
            new ErrorHandlingModule(), new FunctionModule()
        };
    }
}