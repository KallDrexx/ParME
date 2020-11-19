using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FlatRedBall.Glue.Elements;
using FlatRedBall.Glue.SaveClasses;

namespace Parme.Frb.GluePlugin
{
    public class AssetTypeInfoManager
    {
        private const string LogicVarName = "Emitter Logic";
        private const string GroupVarName = "Emitter Grouping";
        private readonly List<string> _emitterLogicTypes = new List<string>();
        
        public AssetTypeInfo LogicAssetTypeInfo { get; }
        public AssetTypeInfo FileAssetTypeInfo { get; }

        public AssetTypeInfoManager()
        {
            FileAssetTypeInfo = new AssetTypeInfo
            {
                FriendlyName = "Parme Emlogic ATI",
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
                        ForcedOptions = _emitterLogicTypes,
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
                        Name = "StopsOnScreenPause",
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

        public void AddEmitterLogicTypeName(string name)
        {
            if (_emitterLogicTypes.All(x => !x.Equals(name, StringComparison.OrdinalIgnoreCase)))
            {
                _emitterLogicTypes.Add(name);
            }
            
            _emitterLogicTypes.Sort();
        }

        public void RemoveEmitterLogicTypeName(string name)
        {
            _emitterLogicTypes.RemoveAll(x => x.Equals(name, StringComparison.OrdinalIgnoreCase));
        }

        public void ClearEmitterLogicTypes()
        {
            _emitterLogicTypes.Clear();
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
            result.AppendLine($"{namedObject.FieldName} = Parme.Frb.ParmeEmitterManager.Instance");
            result.AppendLine($"                .CreateEmitter(new {emitterTypeSelected}(), {parentString}, {groupNameString});");

            return result.ToString();
        }
    }
}