using System.ComponentModel;
using ImGuiHandler;
using Parme.Core;

namespace Parme.Editor.Ui.Elements.Editors
{
    public abstract class SettingsEditorBase : ImGuiElement
    {
        protected EmitterSettings EmitterSettings;

        protected SettingsEditorBase()
        {
            PropertyChanged += OnSelfPropertyChanged;
        }

        public void LoadNewSettings(EmitterSettings settings)
        {
            using (DisablePropertyChangedNotifications())
            {
                EmitterSettings = settings;
                OnNewSettingsLoaded();
            }
        }

        protected abstract void OnNewSettingsLoaded();

        protected abstract void OnSelfPropertyChanged(object sender, PropertyChangedEventArgs e);
    }
}