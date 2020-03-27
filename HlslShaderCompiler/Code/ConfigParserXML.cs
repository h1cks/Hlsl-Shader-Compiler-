using Insane3D;
using Insane3D.HelperFunctions;
using SharpDX.D3DCompiler;
using System;
using System.Collections.Generic;
using System.IO;
using System.Xml;

namespace HlslShaderCompiler.Code
{



 
    public class ConfigParserXML
    {
        //============================================================================================================================//

        public void ReadConfigurationFile(string configurationfilename, HlslConfiguration configuration)
        {
            try
            {
                if (File.Exists(configurationfilename))
                {
                    using (Stream stream_ = Helper.GetFileStream(configurationfilename))
                    {
                        using (XmlReader reader_ = XmlReader.Create(stream_))
                        {
                            
                            reader_.ReadToFollowing("CompilerConfiguration");

                            if (reader_.IsStartElement("CompilerConfiguration"))
                            {
                                reader_.ReadToFollowing("Global");

                                if (reader_.IsStartElement("Global"))
                                {
                                    ReadGlobal(reader_, configuration);
                                }

                                reader_.Read();

                                reader_.ReadEndElement();
                            }

                            reader_.ReadToFollowing("ShaderFile");

                            configuration.ReadFileList(reader_);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorHandler.DoErrorHandling(ex, ErrorHandler.GetCurrentMethod(ex));
            }

        }

        //============================================================================================================================//

        void ReadGlobal(XmlReader reader, HlslConfiguration configuration)
        {
            reader.ReadToFollowing("DebugOptions");

            if (reader.IsStartElement("DebugOptions"))
            {
                reader.Read();
                reader.Read();

                while (reader.Name == "ShaderFlag")
                {
                    string flag_ = reader.ReadElementString("ShaderFlag");

                    if (Enum.TryParse(flag_, out ShaderFlags value_))
                    {
                        configuration.AddGlobalDebugFlag(value_);
                    }

                    reader.Read();
                }

                reader.ReadEndElement();
            }

            if (reader.IsStartElement("ReleaseOptions"))
            {
                reader.Read();
                reader.Read();

                while (reader.Name == "ShaderFlag")
                {
                    string flag_ = reader.ReadElementString("ShaderFlag");

                    if (Enum.TryParse(flag_, out ShaderFlags value_))
                    {
                        configuration.AddGlobalReleaseFlag(value_);
                    }

                    reader.Read();
                }

                reader.ReadEndElement();
            }
        }
    }
}
