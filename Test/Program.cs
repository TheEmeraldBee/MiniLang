using MiniLang.Core;
using MiniLang.Internal;

var engine = new Engine(new DefaultWriter());
engine.AddLibrary(new DefaultLibrary());

var codeText = File.ReadAllText("C:/Users/brigh/Documents/C# Projects/MiniLang/Examples/numberGuesser.minilang");

engine.Run(codeText);

