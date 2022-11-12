namespace MiniLang.Core;

//TODO: Error Handling For Inside Program
//TODO: Loop That Runs Specific Num Of Times
//TODO: END statement that ends the game

public class Engine
{
    private int[] _program;
    private int _idx;
    public readonly IWriter Writer;
    public List<IModule> Modules = new ();

    public int Value;

    private int _codeIdx;
    private string _code;
    public char CurrentCommand = ' ';

    private bool _running;

    public Engine(IWriter writer, int programSize = 4096)
    {
        _program = new int[programSize];
        Writer = writer;
        _code = "";
    }

    public void Set(int value, bool overwrite = true)
    {
        Value = value;
        
        if (!overwrite) return;
        
        _program[_idx] = value;
    }
    
    public void MoveIdx(int dist)
    {
        _idx += dist;
        Update();
    }

    public void SetIdx(int idx)
    {
        _idx = idx;
        Update();
    }

    public void Update()
    {
        Value = _program[_idx];
    }

    public int Get()
    {
        return Value;
    }

    public int GetIdx()
    {
        return _idx;
    }

    public void AddModule(IModule module)
    {
        Modules.Add(module);
    }

    public void AddLibrary(ILibrary lib)
    {
        foreach (var module in lib.GetLibraryModules())
        {
            Modules.Add(module);
        }
    }

    public bool MoveReader(int value=1)
    {
        _codeIdx += value;
        
        if (_codeIdx >= _code.Length)
        {
            _running = false;
            return false;
        }

        CurrentCommand = _code[_codeIdx];
        return true;
    }

    public bool SetReader(int location)
    {
        _codeIdx = location;
        
        if (_codeIdx >= _code.Length)
        {
            return false;
        }

        CurrentCommand = _code[_codeIdx];
        return true;
    }

    public Result Run(string code)
    {
        _code = code + "\n";
        _running = true;
        
        // Used To Set Up The Reader
        SetReader(0);

        while (_running)
        {
            var res = HandleCommand();
            if (!res.QuerySuccess())
            {
                Writer.Message(res.GetError());
                break;
            }

            if (!MoveReader()) { _running = false; }
        }

        return new Result(true);
    }
    
    public Result HandleCommand()
    {
        if (Constants.IgnoreChars.Contains(CurrentCommand)) { return new Result(true); }
        
        foreach (var module in Modules)
        {
            var res = module.HandleCommand(this);
            if (!res.QuerySuccess())
            {
                return res;
            }
        }

        return new Result(true);
    }

    public int? GetNumAfter()
    {
        var number = "";

        while (true)
        {
            if (!MoveReader())
            {
                break;
            }
            if (!Constants.NumberChars.Contains(CurrentCommand))
            {
                break;
            }

            number += _code[_codeIdx];
        }

        MoveReader(-1);

        if (number.Length != 0)
        {
            return int.Parse(number);
        }

        return null;
    }

    public void Close()
    {
        _running = false;
    }

    public int GetCodeIdx()
    {
        return _codeIdx;
    }
}