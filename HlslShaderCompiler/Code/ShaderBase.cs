using Insane3D.HelperFunctions;
using SharpDX;
using System;
using System.Collections.Generic;
using System.IO;
using D3D11 = SharpDX.Direct3D11;
using D3DCompiler = SharpDX.D3DCompiler;


namespace Insane3D
{
    public enum TextureFilter { None, Bilinear, Trilinear, AnsiTropic }
    public class ShaderBase : IDisposable
    {
        protected delegate void ShaderMode(D3D11.DeviceContext1 deviceContext);

        protected const D3D11.Filter c_defaultFilter = D3D11.Filter.MinMagMipLinear;
        protected const int c_defaultAnsioLevel = 2;
        protected const float c_defaultLODBias = -0.1f;

#if DEBUG
        protected const D3DCompiler.ShaderFlags c_compileflags = SharpDX.D3DCompiler.ShaderFlags.OptimizationLevel3;//
#else
        protected const D3DCompiler.ShaderFlags c_compileflags = SharpDX.D3DCompiler.ShaderFlags.OptimizationLevel3;//
#endif

        //================================================================================================================//

        protected D3D11.SamplerState _samplerState;
        protected int _ansiotropicLevel;
        protected float _mipLODBias;
        protected TextureFilter _textureSamplingLevel;
        protected Dictionary<TextureFilter, D3D11.Filter> _filterMappings;

        //================================================================================================================//

        public D3D11.SamplerState BaseSampler { get => _samplerState; }
        public float MipLODBias { get => _mipLODBias; set => _mipLODBias = value; }
        public int AnsiotropicLevel { get => _ansiotropicLevel; set => _ansiotropicLevel = value; }
        public TextureFilter TextureSamplingLevel { get => _textureSamplingLevel; }

        //================================================================================================================//

        public ShaderBase()
        {
            _ansiotropicLevel = c_defaultAnsioLevel;
            _textureSamplingLevel = TextureFilter.Trilinear;
            _mipLODBias = c_defaultLODBias;
        }

        //================================================================================================================//

        protected void InitialiseShader(D3D11.Device1 d3dDevice)
        {
            _filterMappings = new Dictionary<TextureFilter, D3D11.Filter>
            {
                { TextureFilter.None, D3D11.Filter.MinMagMipPoint },
                { TextureFilter.Bilinear, D3D11.Filter.MinMagLinearMipPoint },
                { TextureFilter.Trilinear, D3D11.Filter.MinMagMipLinear },
                { TextureFilter.AnsiTropic, D3D11.Filter.Anisotropic }
            };

            CreateSampler(d3dDevice, c_defaultFilter, _ansiotropicLevel, _mipLODBias);
        }

        //================================================================================================================//

        public void SetAnsioTropicLevel(D3D11.Device1 d3dDevice, int level)
        {
            if (_ansiotropicLevel != level)
            {
                _ansiotropicLevel = level;

                if (_textureSamplingLevel == TextureFilter.AnsiTropic)
                {
                    CreateSampler(d3dDevice, D3D11.Filter.Anisotropic, _ansiotropicLevel, _mipLODBias);
                }
            }
        }

        //===========================================================================================================================

        public void SetFilter(D3D11.Device1 d3dDevice, TextureFilter filter)
        {
            _textureSamplingLevel = filter;

            if (_filterMappings.TryGetValue(filter, out var _filter))
            {
                CreateSampler(d3dDevice, _filter, _ansiotropicLevel, _mipLODBias);
            }
        }

        //===========================================================================================================================

        protected static bool CompileVertexShader(D3D11.Device1 d3dDevice, string codePath, string function, IncludeFX a_includeFX, out D3D11.VertexShader shaderObject, out D3DCompiler.ShaderBytecode shaderByteCode)
        {
            D3DCompiler.CompilationResult errors_ = D3DCompiler.ShaderBytecode.CompileFromFile(codePath, function, "vs_5_0", c_compileflags, D3DCompiler.EffectFlags.None, null, a_includeFX);

            shaderByteCode = errors_.Bytecode;

            if (shaderByteCode != null)
            {
                shaderObject = new D3D11.VertexShader(d3dDevice, shaderByteCode)
                {
                    DebugName = "fileName"
                };
            }
            else
            {
                throw new Exception(errors_.Message);
            }

            return true;
        }


        //===========================================================================================================================

        protected static bool LoadVertexShader(D3D11.Device1 d3dDevice, string filePath, string fileName, out D3D11.VertexShader shaderObject, out D3DCompiler.ShaderBytecode shaderByteCode)
        {
            using (Stream fileStream = Helper.GetFileStream(fileName, filePath))
            {
                shaderByteCode = new D3DCompiler.ShaderBytecode(fileStream);

                if (shaderByteCode != null)
                {
                    shaderObject = new D3D11.VertexShader(d3dDevice, shaderByteCode)
                    {
                        DebugName = "fileName"
                    };
                }
                else
                {
                    throw new Exception("Unable to load file: " + fileName);
                }

            }


            return true;
        }
        //===========================================================================================================================

        protected static bool LoadComputeShader(D3D11.Device1 d3dDevice, string filePath, string fileName, out D3D11.ComputeShader shaderObject, out D3DCompiler.ShaderBytecode shaderByteCode)
        {
            using (Stream fileStream = Helper.GetFileStream(fileName, filePath))
            {
                shaderByteCode = new D3DCompiler.ShaderBytecode(fileStream);

                if (shaderByteCode != null)
                {
                    shaderObject = new D3D11.ComputeShader(d3dDevice, shaderByteCode)
                    {
                        DebugName = "fileName"
                    };
                }
                else
                {
                    throw new Exception("Unable to load file: " + fileName);
                }

            }


            return true;
        }

        //===========================================================================================================================

        protected static bool CompilePixelShader(D3D11.Device1 d3dDevice, string codePath, string function, IncludeFX includeFX, out D3D11.PixelShader shaderObject, out D3DCompiler.ShaderBytecode shaderByteCode)
        {
            D3DCompiler.CompilationResult errors_ = D3DCompiler.ShaderBytecode.CompileFromFile(codePath, function, "ps_5_0", c_compileflags, D3DCompiler.EffectFlags.None, null, includeFX);

            shaderByteCode = errors_.Bytecode;

            if (shaderByteCode != null)
            {
                shaderObject = new D3D11.PixelShader(d3dDevice, shaderByteCode)
                {
                    DebugName = "fileName"
                };
            }
            else
            {
                throw new Exception(errors_.Message);
            }

            return true;
        }

        //===========================================================================================================================

        protected static bool LoadPixelShader(D3D11.Device1 d3dDevice, string filePath, string fileName, out D3D11.PixelShader shaderObject, out D3DCompiler.ShaderBytecode shaderByteCode)
        {
            using (Stream fileStream = Helper.GetFileStream(fileName, filePath))
            {
                shaderByteCode = new D3DCompiler.ShaderBytecode(fileStream);

                if (shaderByteCode != null)
                {
                    shaderObject = new D3D11.PixelShader(d3dDevice, shaderByteCode)
                    {
                        DebugName = fileName
                    };
                }
                else
                {
                    throw new Exception("Unable to load file: " + fileName);
                }
            }

            return true;
        }

        //===========================================================================================================================

        protected static bool CompileHullShader(D3D11.Device1 d3dDevice, string codePath, string function, IncludeFX includeFX, out D3D11.HullShader shaderObject, out D3DCompiler.ShaderBytecode shaderByteCode)
        {
            D3DCompiler.CompilationResult errors_ = D3DCompiler.ShaderBytecode.CompileFromFile(codePath, function, "hs_5_0", c_compileflags, D3DCompiler.EffectFlags.None, null, includeFX);

            shaderByteCode = errors_.Bytecode;

            if (shaderByteCode != null)
            {
                shaderObject = new D3D11.HullShader(d3dDevice, shaderByteCode)
                {
                    DebugName = function
                };
            }
            else
            {
                throw new Exception(errors_.Message);
            }

            return true;
        }

        //===========================================================================================================================

        protected static bool LoadHullShader(D3D11.Device1 d3dDevice, string filePath, string fileName, out D3D11.HullShader shaderObject, out D3DCompiler.ShaderBytecode shaderByteCode)
        {
            using (Stream fileStream = Helper.GetFileStream(fileName, filePath))
            {
                shaderByteCode = new D3DCompiler.ShaderBytecode(fileStream);

                if (shaderByteCode != null)
                {
                    shaderObject = new D3D11.HullShader(d3dDevice, shaderByteCode)
                    {
                        DebugName = fileName
                    };
                }
                else
                {
                    throw new Exception("Unable to load file: " + fileName);
                }
            }

            return true;
        }

        //===========================================================================================================================

        protected static bool CompileDomainShader(D3D11.Device1 d3dDevice, string codePath, string function, IncludeFX includeFX, out D3D11.DomainShader shaderObject, out D3DCompiler.ShaderBytecode shaderByteCode)
        {
            D3DCompiler.CompilationResult errors_ = D3DCompiler.ShaderBytecode.CompileFromFile(codePath, function, "ds_5_0", c_compileflags, D3DCompiler.EffectFlags.None, null, includeFX);

            shaderByteCode = errors_.Bytecode;

            if (shaderByteCode != null)
            {
                shaderObject = new SharpDX.Direct3D11.DomainShader(d3dDevice, shaderByteCode)
                {
                    DebugName = function
                };
            }
            else
            {
                throw new Exception(errors_.Message);
            }

            return true;
        }

        //===========================================================================================================================

        protected static bool LoadDomainShader(D3D11.Device1 d3dDevice, string filePath, string fileName, out D3D11.DomainShader shaderObject, out D3DCompiler.ShaderBytecode shaderByteCode)
        {
            using (Stream fileStream = Helper.GetFileStream(fileName, filePath))
            {
                shaderByteCode = new D3DCompiler.ShaderBytecode(fileStream);

                if (shaderByteCode != null)
                {
                    shaderObject = new D3D11.DomainShader(d3dDevice, shaderByteCode)
                    {
                        DebugName = fileName
                    };
                }
                else
                {
                    throw new Exception("Unable to load file: " + fileName);
                }
            }

            return true;
        }

        //===========================================================================================================================

        protected static bool CompileGeometryShader(D3D11.Device1 d3dDevice, string codePath, string function, IncludeFX includeFX, out D3D11.GeometryShader shaderObject, out D3DCompiler.ShaderBytecode shaderByteCode)
        {

            D3DCompiler.CompilationResult errors_ = D3DCompiler.ShaderBytecode.CompileFromFile(codePath, function, "gs_5_0", c_compileflags, D3DCompiler.EffectFlags.None, null, includeFX);

            shaderByteCode = errors_.Bytecode;

            if (shaderByteCode != null)
            {
                shaderObject = new D3D11.GeometryShader(d3dDevice, shaderByteCode)
                {
                    DebugName = function
                };
            }
            else
            {
                throw new Exception(errors_.Message);
            }

            return true;
        }

        //===========================================================================================================================

        protected static bool LoadGeometryShader(D3D11.Device1 d3dDevice, string filePath, string fileName, out D3D11.GeometryShader shaderObject, out D3DCompiler.ShaderBytecode shaderByteCode)
        {
            using (Stream fileStream = Helper.GetFileStream(fileName, filePath))
            {
                shaderByteCode = new D3DCompiler.ShaderBytecode(fileStream);

                if (shaderByteCode != null)
                {
                    shaderObject = new D3D11.GeometryShader(d3dDevice, shaderByteCode)
                    {
                        DebugName = fileName
                    };
                }
                else
                {
                    throw new Exception("Unable to load file: " + fileName);
                }
            }

            return true;
        }


        //===========================================================================================================================

        void CreateSampler(D3D11.Device1 d3dDevice, D3D11.Filter filter, int level, float mipLODBias)
        {
            if (_samplerState != null)
                _samplerState.Dispose();

            _samplerState = new SharpDX.Direct3D11.SamplerState(d3dDevice, new D3D11.SamplerStateDescription()
            {
                Filter = filter, // trilini
                AddressU = D3D11.TextureAddressMode.Wrap,
                AddressV = D3D11.TextureAddressMode.Wrap,
                AddressW = D3D11.TextureAddressMode.Wrap,
                MaximumAnisotropy = level,
                MipLodBias = mipLODBias,
                MinimumLod = -float.MaxValue,
                MaximumLod = float.MaxValue,

            });
        }

        //===========================================================================================================================

        #region IDisposable Support
        private bool disposedValue = false; // To detect redundant calls

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    if (_samplerState != null)
                        _samplerState.Dispose();
                }

                disposedValue = true;
            }
        }


        // This code added to correctly implement the disposable pattern.
        public void Dispose()
        {
            // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
            Dispose(true);
            GC.SuppressFinalize(this);
        }
        #endregion
    }
}
