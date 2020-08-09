using ImGuiHandler;
using ImGuiNET;

namespace Parme.Editor.Ui.Elements.Initializers.ParticleCount
{
    public class ParticleCountSelector : ImGuiElement
    {
        public enum InitializerTypes {Static = 1, Random = 2};

        private readonly string[] _typeNames = {"<None>", "Static", "Random"};
        
        public ImGuiElement ChildElement { get; set; }

        public InitializerTypes? SelectedInitializerType
        {
            get => Get<InitializerTypes?>();
            set => Set(value);
        }
        
        protected override void CustomRender()
        {
            var selectedIndex = SelectedInitializerType != null ? (int) SelectedInitializerType : 0;
            if (ImGui.Combo("Type", ref selectedIndex, _typeNames, _typeNames.Length))
            {
                SelectedInitializerType = selectedIndex == 0
                    ? (InitializerTypes?) null
                    : (InitializerTypes) selectedIndex;
            }
            
            ChildElement?.Render();
        }
    }
}