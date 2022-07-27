// Requires `dotnet script` tool.  `dotnet tool install -g dotnet-script`

using System.Runtime.CompilerServices;
using System.IO;
using System.Xml;
using System.Text.RegularExpressions;
using System.Diagnostics;
using System.IO.Compression;

static string GetScriptFolder([CallerFilePath] string path = null) => Path.GetDirectoryName(path);
static readonly string ArtifactsFolder = Path.Combine(GetScriptFolder(), ".artifacts");

static void ReplaceVersion(string fileName, string version) {
    const string pattern = "<Version>.*</Version>";

    // Use regex as xml document will change formatting and empty lines.
    var content = File.ReadAllText(fileName);
    var match = Regex.Match(content, pattern);
    if (!match.Success)
    {
        throw new InvalidOperationException($"File {fileName} does not have a version tag");
    }

    content = Regex.Replace(content, pattern, $"<Version>{version}</Version>");
    File.WriteAllText(fileName, content);
}

static void BuildNugetPackages(string project) {
    var packProcess = Process.Start($"dotnet", $"pack -c Release {project}");
    packProcess.WaitForExit();
}

static void BuildProject(string project) {
    var process = Process.Start("dotnet", $"build -c Release {project}");
    process.WaitForExit();
}

static void MoveNugetArtifacts(string project) {
    if (!Directory.Exists(ArtifactsFolder)) {
        Directory.CreateDirectory(ArtifactsFolder);
    }

    var projectPath = Path.GetDirectoryName(project);
    var nugetPath = Path.Combine(projectPath, "bin", "Release");
    var nugetFiles = Directory.GetFiles(nugetPath, "*.nupkg");
    if (nugetFiles.Length == 0) {
        throw new InvalidOperationException($"No nuget packages found in '{nugetPath}'");
    }
    
    foreach (var file in nugetFiles) {
        File.Move(file, Path.Combine(ArtifactsFolder, Path.GetFileName(file)), true); 
    }
}

static void CreateZipArchive(string project, string version) {
    if (!Directory.Exists(ArtifactsFolder)) {
        Directory.CreateDirectory(ArtifactsFolder);
    }
    
    var projectName = Path.GetFileNameWithoutExtension(project);
    var projectPath = Path.GetDirectoryName(project);
    var folderToZip = Path.Combine(projectPath, "bin", "Release", "netcoreapp3.1");
    var resultingZip = Path.Combine(ArtifactsFolder, $"{projectName}.{version}.zip");
    
    if (File.Exists(resultingZip)) {
        File.Delete(resultingZip);
    }
    
    ZipFile.CreateFromDirectory(folderToZip, resultingZip, CompressionLevel.Optimal, false);
}

static void CreateGluePluginArchive(string project, string version) {
    if (!Directory.Exists(ArtifactsFolder)) {
        Directory.CreateDirectory(ArtifactsFolder);
    }
    
    var projectName = Path.GetFileNameWithoutExtension(project);
    var projectPath = Path.GetDirectoryName(project);
    var binaryFolder = Path.Combine(projectPath, "bin", "Release", "netcoreapp3.1");
    var pluginFolder = Path.Combine(binaryFolder, projectName);
    var innerFolder = Path.Combine(pluginFolder, projectName);
    var resultingZip = Path.Combine(ArtifactsFolder, $"{projectName}.{version}.zip");
    
    if (File.Exists(resultingZip)) {
        File.Delete(resultingZip);
    }
   
    if (Directory.Exists(pluginFolder))
    {
        Directory.Delete(pluginFolder, true);
    } 
    
    Directory.CreateDirectory(innerFolder);
   
    var includeList = new string[] {
        "Parme.Core.dll", "Parme.Core.pdb", "Parme.CSharp.dll", "Parme.CSharp.pdb", "Parme.Frb.GluePlugin.dll",
        "Parme.Frb.GluePlugin.pdb"
    };
    
    foreach (var file in includeList)
    {
        var path = Path.Combine(binaryFolder, file);
        if (!File.Exists(path))
        {
            throw new InvalidOperationException($"File '{path}' does not exist but is expected to");
        }
        
        File.Copy(path, Path.Combine(innerFolder, file));
    }
    
    ZipFile.CreateFromDirectory(pluginFolder, resultingZip, CompressionLevel.Optimal, false);
}

static void RemoveCurrentArtifacts() {
    if (!Directory.Exists(ArtifactsFolder)) {
        return;
    }

    var files = Directory.GetFiles(ArtifactsFolder);
    foreach (var file in files) {
        File.Delete(file);
    }
}

if (Args.Count < 1) {
    Console.WriteLine("No version passed in");
    return;
}

var version = Args[0];
Console.WriteLine($"Version: {version}");

var nugetProjects = new[] {
    Path.Combine(GetScriptFolder(), "Parme.Core\\Parme.Core.csproj"),
    Path.Combine(GetScriptFolder(), "Parme.CSharp\\Parme.CSharp.csproj"),
    Path.Combine(GetScriptFolder(), "Parme.MonoGame\\Parme.MonoGame.csproj"),
    Path.Combine(GetScriptFolder(), "Parme.Frb\\Parme.Frb.csproj"),
};

var gluePluginProject = Path.Combine(GetScriptFolder(), "Parme.Frb.GluePlugin\\Parme.Frb.GluePlugin.csproj");
var editorProject = Path.Combine(GetScriptFolder(), "Parme.Editor\\Parme.Editor.csproj");
var cliProject = Path.Combine(GetScriptFolder(), "Parme.Cli\\Parme.Cli.csproj");

RemoveCurrentArtifacts();

Console.WriteLine("Building Glue Plugin");
ReplaceVersion(gluePluginProject, version);
Directory.SetCurrentDirectory(Path.GetDirectoryName(gluePluginProject));
BuildProject(gluePluginProject);
CreateGluePluginArchive(gluePluginProject, version);

Console.WriteLine("Building Editor");
ReplaceVersion(editorProject, version);
Directory.SetCurrentDirectory(Path.GetDirectoryName(editorProject));
BuildProject(editorProject);
CreateZipArchive(editorProject, version);

Console.WriteLine("Building Cli Tool");
ReplaceVersion(cliProject, version);
Directory.SetCurrentDirectory(Path.GetDirectoryName(cliProject));
BuildProject(cliProject);
CreateZipArchive(cliProject, version);

foreach (var project in nugetProjects)
{
    Console.WriteLine("Processing " + project);
    Directory.SetCurrentDirectory(Path.GetDirectoryName(project));
    ReplaceVersion(project, version);
    BuildNugetPackages(project);
    MoveNugetArtifacts(project);
}