namespace MiniLang.Core;

public class Result
{
    private bool _success;
    private string _error;
    
    public Result(bool success, string error="")
    {
        _success = success;
        _error = error;
    }

    public bool QuerySuccess()
    {
        return _success;
    }

    public string GetError()
    {
        return _error;
    }
}