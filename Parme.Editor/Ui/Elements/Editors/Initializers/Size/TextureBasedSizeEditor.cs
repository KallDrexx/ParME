using System.Linq;
using ImGuiNET;
using Parme.Core.Initializers;
using Parme.Editor.Commands;

namespace Parme.Editor.Ui.Elements.Editors.Initializers.Size
{
    [EditorForType(typeof(TextureBasedSizeInitializer))]
    public class TextureBasedSizeEditor : SettingsEditorBase
    {
        [SelfManagedProperty]
        public int Percentage
        {
            get => Get<int>();
            set => Set(value);
        }
        
        protected override void CustomRender()
        {
            ImGui.TextWrapped("Sets the initial particle size based on the actual size of the texture, modified " +
                              "by the specified percentage. \n\nWarning: If no texture sections have been defined then " +
                              "the particle won't show up");
            
            InputInt(nameof(Percentage), "%");
        }

        protected override void OnNewSettingsLoaded()
        {
            var initializer = EmitterSettings.Initializers
                .OfType<TextureBasedSizeInitializer>()
                .First();

            Percentage = initializer.Percentage;
        }

        protected override void OnSelfManagedPropertyChanged(string propertyName)
        {
            var initializer = new TextureBasedSizeInitializer
            {
                Percentage = Percentage,
            };
            
            CommandHandler.Execute(new UpdateInitializerCommand(InitializerType.Size, initializer));
        }
    }
}