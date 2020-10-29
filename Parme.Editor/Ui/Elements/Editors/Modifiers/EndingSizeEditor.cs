using System.Linq;
using ImGuiNET;
using Parme.Core.Modifiers;
using Parme.Editor.Commands;

namespace Parme.Editor.Ui.Elements.Editors.Modifiers
{
    [EditorForType(typeof(EndingSizeModifier))]
    public class EndingSizeEditor : SettingsEditorBase
    {
        [SelfManagedProperty]
        public int Width
        {
            get => Get<int>();
            set => Set(value);
        }

        [SelfManagedProperty]
        public int Height
        {
            get => Get<int>();
            set => Set(value);
        }
        
        protected override void CustomRender()
        {
            ImGui.TextWrapped("The final size of the particle when it reaches the end of its life.  The size " +
                              "will change with a linear interpolation to get from its initial to final size.");
            
            ImGui.NewLine();
            InputInt(nameof(Width), "Width");
            InputInt(nameof(Height), "Height");
        }

        protected override void OnNewSettingsLoaded()
        {
            var modifier = EmitterSettings.Modifiers
                .OfType<EndingSizeModifier>()
                .First();

            Width = modifier.Width;
            Height = modifier.Height;
        }

        protected override void OnSelfManagedPropertyChanged(string propertyName)
        {
            var modifier = new EndingSizeModifier
            {
                Width = Width,
                Height = Height,
            };
            
            CommandHandler.Execute(new UpdateModifierCommand(modifier));
        }
    }
}