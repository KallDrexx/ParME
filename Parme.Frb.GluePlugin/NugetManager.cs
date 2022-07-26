using System;
using FlatRedBall.Glue.Plugins.ExportedImplementations;
using FlatRedBall.Glue.SaveClasses;
using FlatRedBall.Glue.VSHelpers.Projects;

namespace Parme.Frb.GluePlugin
{
    public static class NugetManager
    {
        private const string PackageName = "Parme.Frb";
        
        public static void SetNugetVersion()
        {
            if (GlueState.Self.CurrentGlueProject == null)
            {
                return;
            }
            
            var version = PluginVersion.GetRawVersion();
            var hasNugetsEmbeddedInCsproj = GlueState.Self.CurrentGlueProject.FileVersion >= 
                                            (int)GlueProjectSave.GluxVersions.NugetPackageInCsproj;

            if (!hasNugetsEmbeddedInCsproj)
            {
                return; // Nugets aren't supported inside csproj, so just let compile errors happen
            }

            var mainProject = GlueState.Self.CurrentMainProject;
            if (mainProject == null)
            {
                throw new NullReferenceException("mainProject");
            }
            
            var codeProject = (VisualStudioProject) mainProject.CodeProject;
            if (codeProject.HasProjectReference("Parme.Frb.csproj"))
            {
                GlueCommands.Self.PrintOutput("Parme.Frb.csproj directly referenced.  Not adding nuget");
            }
            else
            {
                var existingVersion = codeProject.GetNugetPackageVersion(PackageName);
                GlueCommands.Self.PrintOutput(string.IsNullOrWhiteSpace(existingVersion)
                    ? "Project does not currently reference Parme.Frb library"
                    : $"Project currently references Parme.Frb version {existingVersion}");

                if (existingVersion != version)
                {
                    GlueCommands.Self.PrintOutput($"Adding Parme.Frb nuget version {version}");
                    codeProject.RemoveNugetPackage(PackageName);
                    codeProject.AddNugetPackage(PackageName, version);
                }
            }
        }
    }
}