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
    var projectPath = Path.GetDirectoryName(project);
    var folderToZip = Path.Combine(projectPath, "bin", "Release", "netcoreapp3.1");
    var resultingZip = Path.Combine(ArtifactsFolder, $"Parme.Editor.{version}.zip");
    
    if (File.Exists(resultingZip)) {
        File.Delete(resultingZip);
    }
    
    ZipFile.CreateFromDirectory(folderToZip, resultingZip, CompressionLevel.Optimal, false);
}

static void RemoveCurrentArtifacts() {
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

RemoveCurrentArtifacts();

Console.WriteLine("Building Glue Plugin");
ReplaceVersion(gluePluginProject, version);
Directory.SetCurrentDirectory(Path.GetDirectoryName(gluePluginProject));
BuildProject(gluePluginProject);

Console.WriteLine("Building Editor");
ReplaceVersion(editorProject, version);
Directory.SetCurrentDirectory(Path.GetDirectoryName(editorProject));
BuildProject(editorProject);
CreateZipArchive(editorProject, version);

foreach (var project in nugetProjects)
{
    Console.WriteLine("Processing " + project);
    Directory.SetCurrentDirectory(Path.GetDirectoryName(project));
    ReplaceVersion(project, version);
    BuildNugetPackages(project);
    MoveNugetArtifacts(project);
}