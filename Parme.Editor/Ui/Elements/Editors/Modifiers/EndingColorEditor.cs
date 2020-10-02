using System.Linq;
using ImGuiNET;
using Parme.Core.Modifiers;
using Parme.Editor.Commands;

namespace Parme.Editor.Ui.Elements.Editors.Modifiers
{
    [EditorForType(typeof(EndingColorModifier))]
    public class EndingColorEditor : SettingsEditorBase
    {
        [SelfManagedProperty]
        public int RedMultiplier
        {
            get => Get<int>();
            set => Set(value);
        }
        
        [SelfManagedProperty]
        public int GreenMultiplier
        {
            get => Get<int>();
            set => Set(value);
        }
        
        [SelfManagedProperty]
        public int BlueMultiplier
        {
            get => Get<int>();
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
            ImGui.TextWrapped("The final color multiplier the particle should be at when the particle reaches" +
                              "it's maximum lifetime.  The color will change with a linear interpolations.");
            
            ImGui.NewLine();
            
            var red = RedMultiplier;
            var green = GreenMultiplier;
            var blue = BlueMultiplier;
            var alpha = AlphaMultiplier;

            if (ImGui.SliderInt("Red", ref red, 0, 255))
            {
                RedMultiplier = red;
            }
            
            if (ImGui.SliderInt("Green", ref green, 0, 255))
            {
                GreenMultiplier = green;
            }
            
            if (ImGui.SliderInt("Blue", ref blue, 0, 255))
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
            var modifier = EmitterSettings.Modifiers
                .OfType<EndingColorModifier>()
                .First();

            RedMultiplier = modifier.Red;
            GreenMultiplier = modifier.Green;
            BlueMultiplier = modifier.Blue;
            AlphaMultiplier = modifier.Alpha;
        }

        protected override void OnSelfManagedPropertyChanged(string propertyName)
        {
            var modifier = new EndingColorModifier
            {
                Red = (byte) RedMultiplier,
                Green = (byte) GreenMultiplier,
                Blue = (byte) BlueMultiplier,
                Alpha = (byte) AlphaMultiplier,
            };
            
            CommandHandler.Execute(new UpdateModifierCommand(modifier));
        }
    }
}