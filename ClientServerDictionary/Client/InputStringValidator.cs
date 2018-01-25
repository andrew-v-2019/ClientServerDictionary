using FluentValidation;
using System;
using System.Linq;
using System.Net;

namespace Client
{
    public class InputStringValidator : AbstractValidator<string>
    {
        private readonly string[] commands = { "add", "get", "delete" };

        public InputStringValidator()
        {
            RuleFor(s => s.Trim()).NotEmpty().WithMessage("Empty command");
            RuleFor(s => s.Trim()).Must(ContainsValidIp).WithMessage("Address is not valid");
            RuleFor(s => s.Trim()).Must(ContainsValidCommand).WithMessage("Command is not recognized");
            RuleFor(s => s.Trim()).Must(ContainsValidArguments).WithMessage("Wrong count of arguments");
        }

        private bool ContainsValidArguments(string inputString)
        {
            try
            {
                var split = inputString.Split(' ');
                var command = inputString.GetInputStringItem(1);
                if (command.Equals(commands[0]) || command.Equals(commands[2])) //add or delete command
                {
                    return split.Length >= 4;
                }
                else
                {
                    if (command.Equals(commands[1])) //get command
                    {
                        return split.Length == 3;
                    }
                }
                return false;
            }
            catch
            {
                return false;
            }
        }

        private bool ContainsValidCommand(string inputString)
        {
            try
            {
                var command = inputString.GetInputStringItem(1);
                return commands.Contains(command);
            }
            catch
            {
                return false;
            }
        }

        private bool ContainsValidIp(string inputString)
        {
            var ipAndPort = inputString.GetInputStringItem(0);
            if (string.IsNullOrWhiteSpace(ipAndPort)) return false;
            try
            {
                var ip = IPAddress.Parse(ipAndPort.Split(':')[0]);
                var port = Int32.Parse(ipAndPort.Split(':')[1]);
                return true;
            }
            catch
            {
                return false;
            }
        }

    }
}
