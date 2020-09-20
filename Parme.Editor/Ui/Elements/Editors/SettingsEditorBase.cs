using System;
using System.Linq;
using System.Reflection;
using ImGuiHandler;
using ImGuiHandler.MonoGame;
using Parme.Core;
using Parme.Editor.AppOperations;
using Parme.MonoGame;

namespace Parme.Editor.Ui.Elements.Editors
{
    public abstract class SettingsEditorBase : ImGuiElement, IDisposable
    {
        public SettingsCommandHandler CommandHandler { get; set; }
        public AppOperationQueue AppOperationQueue { get; set; }
        public ApplicationState ApplicationState { get; set; }
        public ITextureFileLoader TextureFileLoader { get; set; }
        public MonoGameImGuiRenderer MonoGameImGuiRenderer { get; set; }
        
        public EmitterSettings EmitterSettings
        {
            get => Get<EmitterSettings>();
            set => Set(value);
        }

        public virtual void Dispose()
        {
        }

        protected SettingsEditorBase()
        {
            var selfManagedProperties = GetType()
                .GetProperties(BindingFlags.Public | BindingFlags.Instance | BindingFlags.NonPublic)
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