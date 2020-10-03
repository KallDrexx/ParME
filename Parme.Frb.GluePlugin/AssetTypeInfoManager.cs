using System.Collections.Generic;
using System.Text;
using FlatRedBall.Glue.Elements;
using FlatRedBall.Glue.SaveClasses;

namespace Parme.Frb.GluePlugin
{
    public class AssetTypeInfoManager
    {
        private const string LogicVarName = "Emitter Logic";
        private const string GroupVarName = "Emitter Grouping";
        
        public AssetTypeInfo LogicAssetTypeInfo { get; }
        public AssetTypeInfo FileAssetTypeInfo { get; }
        public List<string> EmitterLogicTypes { get; } = new List<string>();

        public AssetTypeInfoManager()
        {
            FileAssetTypeInfo = new AssetTypeInfo
            {
                Extension = MainParmePlugin.Extension,
                CanBeObject = false,
            };
            
            LogicAssetTypeInfo = new AssetTypeInfo
            {
                CanBeObject = true,
                QualifiedRuntimeTypeName = new PlatformSpecificType
                {
                    QualifiedType = "Parme.Frb.ParmeFrbEmitter",
                },
                FriendlyName = "Parme Particle Emitter",
                ConstructorFunc = ConstructorFunc,
                DestroyMethod = "this.Destroy()",
                VariableDefinitions =
                {
                    new VariableDefinition
                    {
                        Name = LogicVarName,
                        Type = "string",
                        UsesCustomCodeGeneration = true,
                        ForcedOptions = EmitterLogicTypes,
                    },
                    
                    new VariableDefinition
                    {
                        Name = GroupVarName,
                        Type = "string",
                        DefaultValue = ParmeEmitterManager.DefaultGroupName,
                        UsesCustomCodeGeneration = true,
                    },
                    
                    new VariableDefinition
                    {
                        Name = "IsEmitting",
                        Type = "bool",
                        DefaultValue = "true",
                    }, 
                    
                    new VariableDefinition
                    {
                        Name = "XOffset",
                        Type = "float",
                        DefaultValue = "0",
                    }, 
                    
                    new VariableDefinition
                    {
                        Name = "YOffset",
                        Type = "float",
                        DefaultValue = "0",
                    },
                }
            };
        }

        private static string ConstructorFunc(IElement element, NamedObjectSave namedObject, ReferencedFileSave referencedFile)
        {
            var emitterTypeSelected = namedObject.GetCustomVariable(LogicVarName)?.Value as string;
            var emitterGroupName = namedObject.GetCustomVariable(GroupVarName)?.Value as string;

            if (string.IsNullOrWhiteSpace(emitterTypeSelected))
            {
                // No behavior selected, so we can't actually create the emitter
                return string.Empty;
            }

            var parentString = element is EntitySave ? "this" : "null";
            var groupNameString = $"\"{emitterGroupName}\"";
            
            var result = new StringBuilder();
            result.AppendLine($"            var emitterLogic = new {emitterTypeSelected}();");
            result.AppendLine($"            {namedObject.FieldName} = Parme.Frb.ParmeEmitterManager.Instance");
            result.AppendLine($"                .CreateEmitter(emitterLogic, {parentString}, {groupNameString});");
            result.AppendLine();
            
            return result.ToString();
        }
    }
}