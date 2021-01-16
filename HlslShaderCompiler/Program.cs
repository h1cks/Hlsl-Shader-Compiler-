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
        static void Main(string[] args)
        {
            if (args.Length < 2)
            {
                Application.EnableVisualStyles();
                Application.SetCompatibleTextRenderingDefault(false);
                Application.Run(new CompilerForm());
            }
            else
            {
                Console.WriteLine("================================ Reading Config ================================  ");

                CommandLineConfiguration commandLine_ = new CommandLineConfiguration(args);
                ConfigParserXML xmlconfigfile_ = new ConfigParserXML();
                HlslConfiguration config_ = new HlslConfiguration();

                commandLine_.CreateConfiguration();

                config_.SetDebugMode(commandLine_[Parameters.Mode].ToLower() == "Debug".ToLower());

                xmlconfigfile_.ReadConfigurationFile(commandLine_[Parameters.ConfigFile], config_);

                int successCount_ = 0;
                int failCount_ = 0;

                List<string> failList_ = new List<string>();

                Console.WriteLine("================================  Compile Started ================================  ");

                for (int i = 0; i < config_.FileConfiguration.Count; i++ )
                {
                    Console.WriteLine("Compiling: " + commandLine_[Parameters.ProjectDir] + config_.FileConfiguration[i].FileNameRelative);

                    if (ShaderCompiler.CompileShader(commandLine_[Parameters.ProjectDir], commandLine_[Parameters.TargetDir], "GPUCode\\", config_, config_.FileConfiguration[i]))
                    {
                        successCount_++;
                        Console.WriteLine("Compiled to: " + config_.FileConfiguration[i].OutputName);
                    }
                    else
                    {
                        failCount_++;
                        Console.WriteLine("Failed:" + config_.FileConfiguration[i].FileNameRelative);

                        failList_.Add(config_.FileConfiguration[i].FileNameRelative);
                    }

                    if (i -1 < config_.FileConfiguration.Count)
                    {
                        Console.WriteLine("-------------------------------------------------------------------------------");
                    }
                }                

                Console.WriteLine("================================  Compile Completed ================================  ");

                Console.WriteLine("Total Files to compile: " + config_.FileConfiguration.Count);
                Console.WriteLine("Total Files Compiled Successfully: " + successCount_);
                Console.WriteLine("Total Files Failed to Compile/Load: " + failCount_);

                for (int i = 0; i < failList_.Count; i++)
                {
                    Console.WriteLine("Failed: " + failList_[i]);
                }


                Console.WriteLine("==================================================================================== ");
            }
        }
    }
}
