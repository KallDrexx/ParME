---
title: Parme Cli Tool
permalink: general_cli.html
---

The Parme CLI tool provides the ability to generate code for `.emlogic` files.  This tool requires the [.net core 3.1 runtime](https://dotnet.microsoft.com/download/dotnet-core/3.1).

```
.\Parme.Cli.exe codegen

codegen:
  Generate code for emitter logic

Usage:
  Parme.Cli codegen [options] <inputFile> <outputFile> <language>

Arguments:
  <inputFile>     Input emitter logic definition file
  <outputFile>    Name of the file to generate
  <language>      language for the generated output (valid values: "csharp")

Options:
  --className <classname>    Name to give the generated class
  --namespace <namespace>    Namespace to put generated code in
  -?, -h, --help             Show help and usage information
```