﻿using HlslShaderCompiler.Code;
using Insane3D.HelperFunctions;
using SharpDX;
using System;
using System.Collections.Generic;
using System.IO;
using D3D11 = SharpDX.Direct3D11;
using D3DCompiler = SharpDX.D3DCompiler;


namespace Insane3D
{
    public static class ShaderCompiler 
    {
       
 
        //================================================================================================================//

        public static bool CompileShader(  string baseSourceDirectory, string outputDirectory, string includeDirectory, HlslConfiguration global_, HlslFileConfiguration file)
        {
            try
            {
                string _path = Path.GetDirectoryName(file.FileNameRelative);

                IncludeFX _include = new IncludeFX(baseSourceDirectory + _path + "\\");

                if (File.Exists(baseSourceDirectory + file.FileNameRelative))
                {
                    D3DCompiler.CompilationResult errors_ = D3DCompiler.ShaderBytecode.CompileFromFile(baseSourceDirectory + file.FileNameRelative, 
                        file.EntryPoint, 
                        file.PixelShaderVersion, 
                        global_.GetShaderFlags(), 
                        D3DCompiler.EffectFlags.None, 
                        null, 
                        _include);

                    D3DCompiler.ShaderBytecode shaderByteCode = errors_.Bytecode;

                    if (shaderByteCode != null)
                    {
                        string fileName_ = Path.GetFileNameWithoutExtension(Path.GetFileName(file.FileNameRelative));

                        string path_ = outputDirectory + _path + "\\";

                        file.OutputName  = path_ + fileName_ + ".obj";

                        System.IO.Directory.CreateDirectory(path_ + "\\");

                        FileStream write_ = new FileStream(file.OutputName, FileMode.Create, FileAccess.Write);

                        shaderByteCode.Save(write_);

                        
                    }
                    else
                    {
                        throw new Exception(errors_.Message);
                    }

                    return true;
                }
            }
            catch 
            {
                return false;
            }

            return false;
        }


    }
}