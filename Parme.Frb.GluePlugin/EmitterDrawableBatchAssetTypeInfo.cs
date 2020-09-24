using FlatRedBall.Glue.Elements;
using FlatRedBall.Glue.SaveClasses;

namespace Parme.Frb.GluePlugin
{
    public static class EmitterDrawableBatchAssetTypeInfo
    {
        public static AssetTypeInfo GetAti(string name)
        {
            return new AssetTypeInfo
            {
                FriendlyName = name,
                QualifiedRuntimeTypeName = new PlatformSpecificType
                {
                    QualifiedType = "Parme.Frb.EmitterDrawableBatch",
                },
                Extension = "emitter",
                CanBeObject = true,
                CustomLoadFunc = CustomLoad,
                AddToManagersFunc = AddToManagers,
                ConstructorFunc = Constructor,
                VariableDefinitions =
                {
                    new VariableDefinition
                    {
                        Name = "IsEmitting",
                        Type = "bool",
                        
                        DefaultValue = "true",
                    }
                }
            };
        }

        private static string Constructor(IElement arg1, NamedObjectSave arg2, ReferencedFileSave arg3)
        {
            return $"{arg2.FieldName} = new EmitterDrawableBatch(new FireExample());";
        }

        private static string AddToManagers(IElement arg1, NamedObjectSave arg2, ReferencedFileSave arg3, string arg4)
        {
            return $"FlatRedBall.SpriteManager.AddDrawableBatch({arg2.FieldName});";
        }

        private static string CustomLoad(IElement arg1, NamedObjectSave arg2, ReferencedFileSave arg3, string arg4)
        {
            if (arg2 == null)
            {
                return null;
            }
            
            return $"{arg2.FieldName} = new EmitterDrawableBatch(new FireExample());";
        }
    }
}