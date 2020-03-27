using Insane3D;
using Insane3D.HelperFunctions;
using System;
using System.Collections.Generic;
using System.IO;
using System.Xml;

namespace HlslShaderCompiler.Code
{
    public class HlslConfiguration
    { 
        
    }
    public class ConfigParserXML
    {
        //============================================================================================================================//

        public void ReadStatsConfigurationFile(string configurationfilename)
        {
            try
            {
                if (File.Exists(configurationfilename))
                {
                    using (Stream stream = Helper.GetFileStream(configurationfilename))
                    {
                        using (XmlReader reader = XmlReader.Create(stream))
                        {
                            reader.Read();

                            if (reader.IsStartElement("CompilerConfiguration"))
                            {

                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorHandler.DoErrorHandling(ex, ErrorHandler.GetCurrentMethod(ex));
            }

        }
    }
}
