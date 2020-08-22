using System;
using System.Linq;
using ImGuiNET;
using Parme.Core.Initializers;
using Parme.Editor.Commands;

namespace Parme.Editor.Ui.Elements.Editors.Initializers.ColorMultiplier
{
    public class StaticColorMultiplierEditor : SettingsEditorBase
    {
        [SelfManagedProperty]
        public float RedMultiplier
        {
            get => Get<float>();
            set => Set(value);
        }
        
        [SelfManagedProperty]
        public float GreenMultiplier
        {
            get => Get<float>();
            set => Set(value);
        }
        
        [SelfManagedProperty]
        public float BlueMultiplier
        {
            get => Get<float>();
            set => Set(value);
        }
        
        [SelfManagedProperty]
        public float AlphaMultiplier
        {
            get => Get<float>();
            set => Set(value);
        }
        
        protected override void CustomRender()
        {
            var red = RedMultiplier;
            var green = GreenMultiplier;
            var blue = BlueMultiplier;
            var alpha = AlphaMultiplier;

            if (ImGui.SliderFloat("Red", ref red, 0, 1))
            {
                RedMultiplier = red;
            }
            
            if (ImGui.SliderFloat("Green", ref green, 0, 1))
            {
                GreenMultiplier = green;
            }
            
            if (ImGui.SliderFloat("Blue", ref blue, 0, 1))
            {
                BlueMultiplier = blue;
            }
            
            if (ImGui.SliderFloat("Alpha", ref alpha, 0, 1))
            {
                AlphaMultiplier = alpha;
            }
        }

        protected override void OnNewSettingsLoaded()
        {
            var initializer = (EmitterSettings.Initializers ?? Array.Empty<IParticleInitializer>())
                .FirstOrDefault(x => x.InitializerType == InitializerType.ColorMultiplier);

            if (initializer == null)
            {
                RedMultiplier = 1;
                GreenMultiplier = 1;
                BlueMultiplier = 1;
                AlphaMultiplier = 1;
            }
            else
            {
                var colorInitializer = (StaticColorInitializer) initializer;
                RedMultiplier = colorInitializer.RedMultiplier;
                GreenMultiplier = colorInitializer.GreenMultiplier;
                BlueMultiplier = colorInitializer.BlueMultiplier;
                AlphaMultiplier = colorInitializer.AlphaMultiplier;
            }
        }

        protected override void OnSelfManagedPropertyChanged(string propertyName)
        {
            var initializer = new StaticColorInitializer
            {
                RedMultiplier = RedMultiplier,
                GreenMultiplier = GreenMultiplier,
                BlueMultiplier = BlueMultiplier,
                AlphaMultiplier = AlphaMultiplier,
            };
            
            CommandHandler.Execute(new UpdateInitializerCommand(InitializerType.ColorMultiplier, initializer));
        }
    }
}