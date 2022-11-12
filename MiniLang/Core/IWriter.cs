namespace MiniLang.Core;

public interface IWriter
{
    public void Print(Engine engine);
    public void PrintAscii(Engine engine);
    public void NewLine(Engine engine);
    public void Message(string message);
    public Result ReadNumber(Engine engine);
    public void ReadAsciiCharacter(Engine engine);
    public Result ReadAsciiString(Engine engine);
    public void WriteString(string value);
}