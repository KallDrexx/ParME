using System.Linq;
using System.Reflection;
using ImGuiHandler;
using Parme.Core;

namespace Parme.Editor.Ui.Elements.Editors
{
    public abstract class SettingsEditorBase : ImGuiElement
    {
        public SettingsCommandHandler CommandHandler { get; set; }
        
        public EmitterSettings EmitterSettings
        {
            get => Get<EmitterSettings>();
            set => Set(value);
        }

        protected SettingsEditorBase()
        {
            var selfManagedProperties = GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance)
                .Select(x => (Property: x, Attribute: x.GetCustomAttribute<SelfManagedPropertyAttribute>()))
                .Where(x => x.Attribute != null)
                .Select(x => x.Property.Name)
                .ToHashSet();

            PropertyChanged += (sender, args) =>
            {
                if (args.PropertyName == nameof(EmitterSettings))
                {
                    using (DisablePropertyChangedNotifications())
                    {
                        OnNewSettingsLoaded();
                    }
                }
                
                else if (selfManagedProperties.Contains(args.PropertyName))
                {
                    OnSelfManagedPropertyChanged(args.PropertyName);
                }
            };
        }

        protected abstract void OnNewSettingsLoaded();

        protected abstract void OnSelfManagedPropertyChanged(string propertyName);
    }
}