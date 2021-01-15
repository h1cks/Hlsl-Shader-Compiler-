# Hlsl-Shader-Compiler-
Tool for compiling HLSL Code.  WIP

Command line

-Mode release -targetdir D:\Development\Strafe\Strafe\bin\Release\net46\GPUCode\  -projectdir D:\Development\Strafe\Strafe\ -configfile  D:\Development\Strafe\Strafe\GPUCode\Config.xml

mode release/debug - applies global flags based upon XML config

Example Config.XML


```
<?xml version="1.0" encoding="utf-8" ?>
<CompilerConfiguration>
  <Global>
    <DebugOptions>
      <ShaderFlag>Debug</ShaderFlag>
    </DebugOptions>
    <ReleaseOptions>
      <ShaderFlag>OptimizationLevel3</ShaderFlag>
    </ReleaseOptions>
  </Global>
  <ShaderFile filename="GpuCode\DecalShadervs.hlsl">
    <ShaderModel>vs_5_0</ShaderModel>
    <EntryPoint>VSMain</EntryPoint>
  </ShaderFile>
  <ShaderFile filename="GpuCode\Terrainvs.hlsl">
    <ShaderModel>vs_5_0</ShaderModel>
    <EntryPoint>VSMain</EntryPoint>
  </ShaderFile>
  <ShaderFile filename="GpuCode\Terrainps.hlsl">
    <ShaderModel>ps_5_0</ShaderModel>
    <EntryPoint>PSMain</EntryPoint>
  </ShaderFile>
  <ShaderFile filename="GpuCode\instanceshadervs.hlsl">
    <ShaderModel>vs_5_0</ShaderModel>
    <EntryPoint>VSMain</EntryPoint>
  </ShaderFile>
  <ShaderFile filename="GpuCode\instanceshaderps.hlsl">
    <ShaderModel>ps_5_0</ShaderModel>
    <EntryPoint>PSMain</EntryPoint>
  </ShaderFile>
  <ShaderFile filename="GpuCode\instanceshadershadowvs.hlsl">
    <ShaderModel>vs_5_0</ShaderModel>
    <EntryPoint>VSMain_Shadow</EntryPoint>
  </ShaderFile>
  <ShaderFile filename="GpuCode\instanceshadershadowps.hlsl">
    <ShaderModel>ps_5_0</ShaderModel>
    <EntryPoint>PsShadowMain</EntryPoint>
  </ShaderFile>
  <ShaderFile filename="GpuCode\watervs.hlsl">
    <ShaderModel>vs_5_0</ShaderModel>
    <EntryPoint>VSMain</EntryPoint>
  </ShaderFile>
  <ShaderFile filename="GpuCode\waterps.hlsl">
    <ShaderModel>ps_5_0</ShaderModel>
    <EntryPoint>PSMain</EntryPoint>
  </ShaderFile>
  <ShaderFile filename="GpuCode\waterds.hlsl">
    <ShaderModel>ds_5_0</ShaderModel>
    <EntryPoint>DSMain</EntryPoint>
  </ShaderFile>
  <ShaderFile filename="GpuCode\waterhs.hlsl">
    <ShaderModel>hs_5_0</ShaderModel>
    <EntryPoint>HSMain</EntryPoint>
  </ShaderFile>
</CompilerConfiguration>
```


