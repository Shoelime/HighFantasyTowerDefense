using System.Collections.Generic;
using System.Linq;

public class DeveloperConsole
{
    private readonly string _prefix;
    private readonly IEnumerable<IConsoleCommand> _commands;

    public DeveloperConsole(string prefix, IEnumerable<IConsoleCommand> commands)
    {
        this._prefix = prefix;
        this._commands = commands;
    }

    public void ProcessCommand(string _inputValue)
    {
        if (!_inputValue.StartsWith(_prefix)) { return; }

        _inputValue = _inputValue.Remove(0, _prefix.Length);

        string[] inputSplit = _inputValue.Split(' ');

        string commandInput = inputSplit[0];
        string[] args = inputSplit.Skip(1).ToArray();

        ProcessCommand(commandInput, args);
    }

    public void ProcessCommand(string commandInput, string[] args)
    {
        foreach (var command in _commands)
        {
            if (!commandInput.Equals(command.CommandWord, System.StringComparison.OrdinalIgnoreCase))
            {
                continue;
            }

            if (command.Process(args))
            {
                return;
            }
            else DeveloperConsoleBehaviour.Instance.DisplayErrorLog();
        }
    }
}
