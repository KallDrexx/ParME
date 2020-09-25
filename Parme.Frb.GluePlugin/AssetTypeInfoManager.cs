using System.Collections.Generic;
using System.Text;
using FlatRedBall.Glue.Elements;
using FlatRedBall.Glue.SaveClasses;

namespace Parme.Frb.GluePlugin
{
    public class AssetTypeInfoManager
    {
        public AssetTypeInfo LogicAssetTypeInfo { get; }
        public AssetTypeInfo FileAssetTypeInfo { get; }
        public List<string> EmitterLogicTypes { get; } = new List<string>();

        public AssetTypeInfoManager()
        {
            FileAssetTypeInfo = new AssetTypeInfo
            {
                Extension = "emitter",
                CanBeObject = false,
            };
            
            LogicAssetTypeInfo = new AssetTypeInfo
            {
                CanBeObject = true,
                QualifiedRuntimeTypeName = new PlatformSpecificType
                {
                    QualifiedType = "Parme.Frb.EmitterDrawableBatch",
                },
                FriendlyName = "Parme Particle Emitter",
                ConstructorFunc = ConstructorFunc,
                AddToManagersFunc = (element, save, arg3, arg4) => $"FlatRedBall.SpriteManager.AddDrawableBatch({save.FieldName});",
                VariableDefinitions =
                {
                    new VariableDefinition
                    {
                        Name = "Emitter Type",
                        Type = "string",
                        UsesCustomCodeGeneration = true,
                        ForcedOptions = EmitterLogicTypes,
                    },
                    
                    new VariableDefinition
                    {
                        Name = "IsEmitting",
                        Type = "bool",
                        DefaultValue = "true",
                    }, 
                    
                    new VariableDefinition
                    {
                        Name = "X",
                        Type = "float",
                    }, 
                    
                    new VariableDefinition
                    {
                        Name = "Y",
                        Type = "float",
                    }, 
                    
                    new VariableDefinition
                    {
                        Name = "Z",
                        Type = "float",
                    },
                }
            };
        }

        private string ConstructorFunc(IElement element, NamedObjectSave namedObject, ReferencedFileSave referencedFile)
        {
            var emitterTypeSelected = namedObject.GetCustomVariable("Emitter Type")?.Value as string;
            
            var result = new StringBuilder();
            result.AppendLine($"{namedObject.FieldName} = new EmitterDrawableBatch();");

            if (emitterTypeSelected != null)
            {
                result.AppendLine($"            {namedObject.FieldName}.EmitterLogic = new {emitterTypeSelected}();");

                if (element is EntitySave)
                {
                    result.AppendLine($"            {namedObject.FieldName}.Parent = this;");
                }
            }

            return result.ToString();
        }
    }
}