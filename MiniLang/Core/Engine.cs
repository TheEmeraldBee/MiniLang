using System.Reflection;
using MiniLang.Internal;

namespace MiniLang.Core;

/*
    * The Language:
    * Mini Lang Uses 20 characters some with multiple uses.
    * > moves one location to the right >num moves num to the right
    * < moves one location to the left <num moves num to the left
    * - removes one value from that location -num removes num from value
    * + adds one value to that location +num adds num to value
    * ! sets number to negative version of self
    * * sets number to next number typed (Ex: *123 Can't: * 123 or *1 23)
    * $ sets position to next number typed ($123 goes to pos
    *                                       123 remember 0 is start) and if no number typed, goes to 0
    * ? sets number to random int from 0 to number after it
    *              Default 5
    * . outputs number at that location
    * ^ outputs ASCII char at that location
    * , sets location unicode number input
    * & sets location and locations after it to unicode numbers
    * _ sets location to number input
    * / new line
    * [ opens loop
    * ] ends loop
    * ( start if
    * = check equal
    * => check greater
    * =< check less
    * ) end if
    * | if set location
    *             pos 2 == pos 4 result prints 2 if true
    * if example (>> = >> | > ++ .)
    *
    * comments that need any of the lang chars can be made with {
    * and will end at }
    *
    * Lang 2.0:
    * Module System. Every character handle is inside of a module interface.
    * Program is a int[]
    * Single Characters as functions and expressions.
    * Program can handle errors through E function.
    * Comments are still { and } and MUST be used for the code, any unrecognized values throw errors.
*/

public class Engine
{
    public int[] Program;
    public readonly IWriter Writer;
    public List<IModule> Modules = new ();

    public int Idx;

    private int _codeIdx;
    private string _code;
    public char CurrentCommand = ' ';

    private bool _running;

    public Engine(IWriter writer, int programSize = 4096)
    {
        Program = new int[programSize];
        Writer = writer;
        _code = "";
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

    public bool SetReader(int location=0)
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
        _code = code;
        _running = true;
        
        // Used To Set Up The Reader
        SetReader();
        
        while (_running)
        {
            var res = HandleCommand();
            if (!res.QuerySuccess())
            {
                Writer.Message(res.GetError());
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