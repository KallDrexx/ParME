using System;
using ImGuiHandler;
using ImGuiNET;

namespace Parme.Editor.Ui.Elements.Triggers
{
    public class TriggerParentSection : ImGuiElement
    {
        public enum TriggerTypes { OneShot = 1, TimeElapsed = 2 }
        private readonly string[] _triggerTypeNames = {"<None>", "One Shot", "Time Elapsed"};
        private ImGuiElement _child;

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

                _child = SelectedTriggerType switch
                {
                    null => null,
                    TriggerTypes.OneShot => new OneShotTriggerEditor {IsVisible = true},
                    TriggerTypes.TimeElapsed => new TimeElapsedTriggerEditor {IsVisible = true},
                    _ => throw new NotSupportedException($"No editor for trigger type of {SelectedTriggerType}"),
                };
            }
            
            _child?.Render();
        }
    }
}