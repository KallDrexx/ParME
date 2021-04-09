using ImGuiNET;
using Parme.Core.PositionModifier;
using Parme.Editor.Commands;

namespace Parme.Editor.Ui.Elements.Editors.PositionModifiers
{
    [EditorForType(typeof(AltitudeBouncePositionModifier))]
    public class AltitudeBounceEditor : SettingsEditorBase
    {
        [SelfManagedProperty]
        public float MinAcceleration
        {
            get => Get<float>();
            set => Set(value);
        }
        
        [SelfManagedProperty]
        public float MaxAcceleration
        {
            get => Get<float>();
            set => Set(value);
        }

        [SelfManagedProperty]
        public float Gravity
        {
            get => Get<float>();
            set => Set(value);
        }

        [SelfManagedProperty]
        public float Elasticity
        {
            get => Get<float>();
            set => Set(value);
        }
        
        protected override void CustomRender()
        {
            ImGui.Text("Altitude:");
            InputFloat(nameof(MinAcceleration), "Min Bounce");
            InputFloat(nameof(MaxAcceleration), "Max Bounce");
            InputFloat(nameof(Gravity), "Gravity");
            InputFloat(nameof(Elasticity), "Elasticity");
        }

        protected override void OnNewSettingsLoaded()
        {
            var modifier = (AltitudeBouncePositionModifier) EmitterSettings.PositionModifier;

            MinAcceleration = modifier.MinBounceAcceleration;
            MaxAcceleration = modifier.MaxBounceAcceleration;
            Gravity = modifier.Gravity;
            Elasticity = modifier.Elasticity;
        }

        protected override void OnSelfManagedPropertyChanged(string propertyName)
        {
            var modifier = new AltitudeBouncePositionModifier
            {
                MinBounceAcceleration = MinAcceleration,
                MaxBounceAcceleration = MaxAcceleration,
                Gravity = Gravity,
                Elasticity = Elasticity,
            };
            
            CommandHandler.Execute(new UpdatePositionModifierCommand(modifier));
        }
    }
}