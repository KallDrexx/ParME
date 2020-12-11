using FlatRedBall.Glue.CodeGeneration;
using FlatRedBall.Glue.CodeGeneration.CodeBuilder;
using FlatRedBall.Glue.SaveClasses;

namespace Parme.Frb.GluePlugin
{
    public class ParmeScreenCodeGenerator : ElementComponentCodeGenerator
    {
        public override ICodeBlock GenerateDestroy(ICodeBlock codeBlock, IElement element)
        {
            if (element is ScreenSave)
            {
                codeBlock.Line("Parme.Frb.ParmeEmitterManager.Instance.DestroyActiveEmitterGroups();");
            }

            return codeBlock;
        }
    }
}