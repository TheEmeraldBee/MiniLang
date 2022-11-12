namespace MiniLang.Core;

//TODO: Importing Files!

public class Engine
{
    private int[] _program;
    private int _idx;
    public readonly IWriter Writer;
    private List<IModule> _modules = new ();

    private int _value;

    private int _codeIdx;
    private string _code;
    public char CurrentCommand = ' ';

    public Engine(IWriter writer, int programSize = 4096)
    {
        _program = new int[programSize];
        Writer = writer;
        _code = "";
    }

    public void Set(int value, bool overwrite = true)
    {
        _value = value;
        
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

    private void Update()
    {
        _value = _program[_idx];
    }

    public int Get()
    {
        return _value;
    }

    public int GetIdx()
    {
        return _idx;
    }

    public void AddModule(IModule module)
    {
        _modules.Add(module);
    }

    public void AddLibrary(ILibrary lib)
    {
        foreach (var module in lib.GetLibraryModules())
        {
            _modules.Add(module);
        }
    }

    public bool MoveReader(int value=1)
    {
        _codeIdx += value;
        
        if (_codeIdx >= _code.Length)
        {
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

        // Used To Set Up The Reader
        SetReader(0);

        while (true)
        {
            var res = HandleCommand();
            if (!res.QuerySuccess())
            {
                return res;
            }

            if (!MoveReader()) { break; }
        }

        return new Result(true);
    }
    
    public Result HandleCommand()
    {
        if (Constants.IgnoreChars.Contains(CurrentCommand)) { return new Result(true); }
        
        foreach (var module in _modules)
        {
            var res = module.HandleCommand(this);
            if (!res.QuerySuccess())
            {
                return res;
            }
        }

        return new Result(true);
    }

    public Result HandleSkip()
    {
        if (Constants.IgnoreChars.Contains(CurrentCommand)) return new Result(true);

        foreach (var module in _modules)
        {
            var res = module.HandleSkip(this);
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

    public int GetCodeIdx()
    {
        return _codeIdx;
    }

    public string GetCodeString()
    {
        return _code;
    }

    public void SetContext(string code, int readerLoc)
    {
        _code = code;
        _codeIdx = readerLoc;
    }
}