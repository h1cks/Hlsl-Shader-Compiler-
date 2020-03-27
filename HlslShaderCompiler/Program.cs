using HlslShaderCompiler.Code;
using Insane3D;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace HlslShaderCompiler
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            string[] args = Environment.GetCommandLineArgs();

            if (args.Length < 2)
            {
                Application.EnableVisualStyles();
                Application.SetCompatibleTextRenderingDefault(false);
                Application.Run(new CompilerForm());
            }
            else
            {
                CommandLineConfiguration _commandLine = new CommandLineConfiguration(args);
                ConfigParserXML _xmlconfigfile = new ConfigParserXML();
                HlslConfiguration _config = new HlslConfiguration();

                _commandLine.CreateConfiguration();

                _config.SetDebugMode(_commandLine[Parameters.Mode].ToLower() == "Debug".ToLower());

                _xmlconfigfile.ReadConfigurationFile(_commandLine[Parameters.ConfigFile], _config);

                for (int i = 0; i < _config.FileConfiguration.Count; i++ )
                {
                    Console.WriteLine("Compiling:" + _commandLine[Parameters.ProjectDir] + _config.FileConfiguration[i].FileNameRelative);

                    if (ShaderCompiler.CompileShader(_commandLine[Parameters.ProjectDir], _commandLine[Parameters.TargetDir], "GPUCode\\", _config, _config.FileConfiguration[i]))
                    {

                        Console.WriteLine("Compiled to: " + _config.FileConfiguration[i].OutputName);
                    }
                    else
                    {
                        Console.WriteLine("Failed:" + _config.FileConfiguration[i].FileNameRelative);
                    }
                }
            }
        }
    }
}
