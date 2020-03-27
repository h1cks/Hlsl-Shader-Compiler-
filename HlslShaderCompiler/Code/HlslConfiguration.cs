using SharpDX.D3DCompiler;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace HlslShaderCompiler.Code
{
    public class HlslConfiguration
    {
        protected List<ShaderFlags> _debugShaderFlags;
        protected List<ShaderFlags> _releaseShaderFlags;
        protected bool _debugMode;
        protected List<HlslFileConfiguration> _fileConfiguration;

        public List<HlslFileConfiguration> FileConfiguration => _fileConfiguration;

        public HlslConfiguration()
        {
            _debugMode = true;
            _releaseShaderFlags = new List<ShaderFlags>();
            _debugShaderFlags = new List<ShaderFlags>();
            _fileConfiguration = new List<HlslFileConfiguration>();
        }

        internal void AddGlobalDebugFlag(ShaderFlags value_)
        {
            _debugShaderFlags.Add(value_);

            Console.WriteLine("Debug Shader Flag:" + value_.ToString());
        }

        internal void AddGlobalReleaseFlag(ShaderFlags value_)
        {
            _releaseShaderFlags.Add(value_);

            Console.WriteLine("Release Shader Flag:" + value_.ToString());
        }

        public void SetDebugMode(bool mode)
        {
            _debugMode = mode;

            Console.WriteLine("Debug Mode:" + _debugMode.ToString());
        }

        ShaderFlags GetFlags(List<ShaderFlags> flagsList)
        {
            ShaderFlags flags_ = ShaderFlags.None;

            for (int i = 0; i < flagsList.Count; i++)
            {
                flags_ |= flagsList[i];
            }

            return flags_;
        }

        public ShaderFlags GetShaderFlags()
        {
            if (_debugMode)
            {
                return GetFlags(_debugShaderFlags);
            }
            else
            {
                return GetFlags(_releaseShaderFlags);
            }

        }

        public void ReadFileList(XmlReader reader)
        {
            while (reader.Name.ToLower() == "ShaderFile".ToLower())
            {
                _fileConfiguration.Add(HlslFileConfiguration.ReadGlobal(reader));

                reader.Read();
            }
        }
    }
}
