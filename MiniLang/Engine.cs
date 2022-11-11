namespace MiniLang;

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
 *                                       123 remember 0 is start)
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
*/

public class Engine
{
    private readonly int[] _program;
    private int _idx;
    private int _codeIdx;
    private string _code;

    private bool _errored;

    private static readonly char[] Nums = { '0', '1', '2', '3', '4', '5', '6', '7', '8', '9' };

    public Engine(int programSize=4096)
    {
        _code = "";
        _program = new int[programSize];
    }

    public void HandleCodeFile(string filepath)
    {
        var codeString = File.ReadAllText(filepath);
        HandleCodeString(codeString);
    }

    public void HandleCodeString(string code)
    {
        _code = code;
        for (_codeIdx = 0; _codeIdx < code.Length; _codeIdx++)
        {
            RunCommand(_code[_codeIdx]);

            if (_errored)
            {
                return;
            }

            if (_idx < _program.Length && _idx >= 0) continue;
            
            _idx = 0;
            Console.Error.WriteLine($"ERROR: Program Out Of Bounds at location {_codeIdx} with character {_code[_codeIdx]}.");
            return;
        }
    }

    private void RunCommand(char command)
    {
        switch (command)
        {
            case '<':
                _idx -= GetNumAfter() ?? 1;
                break;
            case '>':
                _idx += GetNumAfter() ?? 1;
                break;
            case '-':
                _program[_idx] -= GetNumAfter() ?? 1;
                break;
            case '+':
                _program[_idx] += GetNumAfter() ?? 1;
                break;
            case '?': 
                _program[_idx] = new Random(DateTime.Now.Millisecond).Next(0, GetNumAfter() ?? 10);
                break;
            case '!':
                _program[_idx] *= -1;
                break;
            case '*':
                SetNumber();
                break;
            case '$':
                SetIdx();
                break;
            case '.':
                PrintNum();
                break;
            case '^':
                Print();
                break;
            case '/':
                NewLine();
                break;
            case '&':
                ReadString(Console.ReadLine() ?? string.Empty);
                break;
            case ',':
                var text = Console.ReadLine() ?? "0";
                _program[_idx] = text[0];
                break;
            case '_':
                ReadNumber(Console.ReadLine() ?? string.Empty);
                break;
            
            // Loop
            case '[':
                var loopStart = _codeIdx;
                while (true)
                {
                    if (_program[_idx] == 0)
                    {
                        while (_code[_codeIdx] != ']')
                        {
                            _codeIdx++;

                            if (_codeIdx > _code.Length)
                            {
                                _errored = true;
                                Console.Error.WriteLine($"ERROR: Expected ] but didn't receive it for loop.");
                                return;
                            }
                        }

                        break;
                    }
                    
                    while (_code[_codeIdx] != ']')
                    {
                        _codeIdx++;
    
                        if (_codeIdx > _code.Length)
                        {
                            _errored = true;
                            Console.Error.WriteLine($"ERROR: Expected ] but didn't receive it for loop.");
                            return;
                        }
                        
                        RunCommand(_code[_codeIdx]);
                    }

                    _codeIdx = loopStart;
                }

                break;
            
            // If Statement
            case '(':
                int type;
                // Start of IF
                while (_code[_codeIdx] != '=')
                {
                    _codeIdx++;

                    if (_codeIdx > _code.Length)
                    {
                        _errored = true;
                        Console.Error.WriteLine($"ERROR: Expected = but didn't receive it for if."); 
                        return;
                    }
                    
                    RunCommand(_code[_codeIdx]);

                }
                
                var start = _program[_idx];

                _codeIdx++;
                if (_code[_codeIdx] == '>')
                {
                    type = 1;
                    _codeIdx++;
                } else if (_code[_codeIdx] == '<')
                {
                    type = 2;
                    _codeIdx++;
                } else
                {
                    type = 0;
                    _codeIdx--;
                }

                // End of IF
                while (_code[_codeIdx] != '|')
                {
                    _codeIdx++;
                    
                    if (_codeIdx > _code.Length)
                    {
                        _errored = true;
                        Console.Error.WriteLine($"ERROR: Expected | but didn't receive it for if."); 
                        return;
                    }
                    
                    RunCommand(_code[_codeIdx]);
                }
                
                var end = _program[_idx];

                // The Rest Of The If
                var canRun = type switch
                {
                    1 => start > end,
                    2 => start < end,
                    _ => start == end
                };

                while (_code[_codeIdx] != ')')
                {
                    _codeIdx++;

                    if (_codeIdx > _code.Length)
                    {
                        _errored = true;
                        Console.Error.WriteLine($"ERROR: Expected ) but didn't receive it for if."); 
                        return;
                    }

                    if (canRun)
                    {
                        RunCommand(_code[_codeIdx]);
                    }
                }

                break;
            
            // Comment
            case '{':
                while (_code[_codeIdx] != '}')
                {
                    _codeIdx++;
                    if (_codeIdx > _code.Length)
                    {
                        return;
                    }
                }
                break;
        }
    }

    private void PrintNum()
    {
        Console.Write(_program[_idx]);
    }

    private void Print()
    {
        Console.Write(Convert.ToChar(_program[_idx]));
    }

    private void NewLine()
    {
        Console.Write('\n');
    }

    private void SetNumber()
    {
        var number = "";
        while (true)
        {
            _codeIdx++;
            if (!Nums.Contains(_code[_codeIdx]))
            {
                break;
            }

            number += _code[_codeIdx];
        }
        _codeIdx--;

        if (number.Length != 0)
        {
            _program[_idx] = int.Parse(number);
        }
    }

    private void SetIdx()
    {
        var number = "";
        while (true)
        {
            _codeIdx++;
            if (!Nums.Contains(_code[_codeIdx]))
            {
                break;
            }

            number += _code[_codeIdx];
        }
        _codeIdx--;
        
        _idx = int.Parse(number);
        if (_idx > _program.Length)
        {
            Console.Error.WriteLine("ERROR: Went over the program limit with $ operator.");
            _errored = true;
        }
    }

    private int? GetNumAfter()
    {
        var number = "";
        while (true)
        {
            _codeIdx++;
            if (_codeIdx >= _code.Length)
            {
                break;
            }
            if (!Nums.Contains(_code[_codeIdx]))
            {
                break;
            }

            number += _code[_codeIdx];
        }

        _codeIdx--;
        
        if (number.Length != 0)
        {
            return int.Parse(number);
        }

        return null;
    }

    private void ReadNumber(string toRead)
    {
        if (int.TryParse(toRead, out var result))
        {
            _program[_idx] = result;
        }
        else
        {
            _errored = true;
            Console.Error.WriteLine("ERROR: Reading Input Could Not Parse Input.");
        }
    }

    private void ReadString(string toRead)
    {
        foreach (var read in toRead)
        {
            _idx++;
            if (_idx > _program.Length)
            {
                _errored = true;
                Console.Error.WriteLine("ERROR: Reading Input Went Past Maximum Program Size.");
                return;
            }

            _program[_idx] = read;
        }
    }
}
