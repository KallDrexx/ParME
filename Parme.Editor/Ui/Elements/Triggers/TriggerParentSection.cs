using ImGuiHandler;
using ImGuiNET;

namespace Parme.Editor.Ui.Elements.Triggers
{
    public class TriggerParentSection : ImGuiElement
    {
        public enum TriggerTypes { OneShot = 1, TimeElapsed = 2 }
        private readonly string[] _triggerTypeNames = {"<None>", "One Shot", "Time Elapsed"};
        
        public ImGuiElement TriggerDisplaySection { get; set; }

        public TriggerTypes? SelectedTriggerType
        {
            get => Get<TriggerTypes?>();
            set => Set(value);
        }
        
        protected override void CustomRender()
        {
            var selectedIndex = SelectedTriggerType != null ? (int) SelectedTriggerType : 0;
            if (ImGui.Combo("Type", ref selectedIndex, _triggerTypeNames, _triggerTypeNames.Length))
            {
                SelectedTriggerType = selectedIndex == 0
                    ? (TriggerTypes?) null
                    : (TriggerTypes) selectedIndex;
            }
            
            TriggerDisplaySection?.Render();
        }
    }
}