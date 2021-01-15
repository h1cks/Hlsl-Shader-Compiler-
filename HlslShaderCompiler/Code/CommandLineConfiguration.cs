using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HlslShaderCompiler.Code
{
    public enum Parameters { Mode, TargetDir, ProjectDir, ConfigFile, None }
    public class CommandLineConfiguration
    {
        protected Dictionary<Parameters, string> _parserArguments;
        protected string[] _args;

        //=================================================================================================================================================

        public string this[Parameters key]
        {
            get { if (_parserArguments.TryGetValue(key, out string value_))
                    return value_;
                else 
                    return ""; } 
        }

        //=================================================================================================================================================

        public CommandLineConfiguration(string[] args)
        {
            _args = new string[args.Length];

            args.CopyTo(_args, 0);

            _parserArguments = new Dictionary<Parameters, string>();
        }

        //=================================================================================================================================================

        public void CreateConfiguration()
        {
            //arg0 is always exe

            foreach (Parameters _params in Enum.GetValues(typeof(Parameters)))
            {
                for (int i = 0; i < _args.Length - 1; i += 2)
                {
                    string arg_ = _args[i].ToLower();

                    if (arg_.ToLower() == "-" + _params.ToString().ToLower())
                    {
                        _parserArguments.Add(_params, _args[i + 1].ToLower());
                        break;
                    }
                }
            }
        }
    }
}


