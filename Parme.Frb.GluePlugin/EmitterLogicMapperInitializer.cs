using FlatRedBall.Glue.CodeGeneration.CodeBuilder;
using FlatRedBall.Glue.CodeGeneration.Game1;

namespace Parme.Frb.GluePlugin
{
    public class EmitterLogicMapperInitializer : Game1CodeGenerator
    {
        public string ProjectNamespace { get; set; }
        
        public override void GenerateInitialize(ICodeBlock codeBlock)
        {
            codeBlock.Line($"var emitterNameMapper = new {ProjectNamespace}.Particles.ParmeEmitterLogicGenerator();");
            codeBlock.Line("Parme.Frb.ParmeEmitterManager.Instance.EmitterLogicMapper = emitterNameMapper;");
            codeBlock.Line("FlatRedBall.Screens.ScreenManager.AfterScreenDestroyed += (screen) => " +
                           "Parme.Frb.ParmeEmitterManager.Instance.DestroyActiveEmitterGroups();");
        }
    }
}