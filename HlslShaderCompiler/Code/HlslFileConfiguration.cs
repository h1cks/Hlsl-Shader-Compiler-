using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace HlslShaderCompiler.Code
{
    public class HlslFileConfiguration
    {
        protected string _pixelShaderVersion;
        protected string _entryPoint;
        protected string _fileNameRelative;
        protected string _outputName;

        public string EntryPoint => _entryPoint;
        public string PixelShaderVersion => _pixelShaderVersion;
        public string FileNameRelative => _fileNameRelative;
        public string OutputName { get => _outputName; set => _outputName = value; }


        public static HlslFileConfiguration ReadGlobal(XmlReader reader)
        {
            HlslFileConfiguration config_ = new HlslFileConfiguration();

            config_.ReadGlobalLocal(reader);

            return config_;
        }

        void ReadGlobalLocal(XmlReader reader)
        {
            _fileNameRelative = reader.GetAttribute("filename");
            
            reader.ReadToFollowing("ShaderModel");

            _pixelShaderVersion = reader.ReadElementContentAsString();

            reader.ReadToFollowing("EntryPoint");

            _entryPoint = reader.ReadElementContentAsString();

            reader.Read();

            reader.ReadEndElement();
            
        }
    }
}
