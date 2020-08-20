using System;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using System.Numerics;
using ImGuiHandler;
using ImGuiNET;
using Parme.Core;
using Parme.Core.Initializers;
using Parme.Core.Modifiers;

namespace Parme.Editor.Ui.Elements
{
    public class Workbench : ImGuiElement
    {
        public Vector2 Position { get; set; }
        public Vector2 Size { get; set; }

        public string TextureFilename
        {
            get => Get<string>();
            set => Set(value);
        }

        public float ParticleLifeTime
        {
            get => Get<float>();
            set => Set(value);
        }

        public IParticleInitializer ParticleCountInitializer
        {
            get => Get<IParticleInitializer>();
            set
            {
                if (value != null && value.InitializerType != InitializerType.ParticleCount)
                {
                    throw new InvalidOperationException($"Initializer type of {value.InitializerType} passed in, " +
                                                        "but only particle count initializers were expected");
                }
                
                Set(value);
            }
        }

        public IParticleInitializer ColorMultiplierInitializer
        {
            get => Get<IParticleInitializer>();
            set
            {
                if (value != null && value.InitializerType != InitializerType.ColorMultiplier)
                {
                    throw new InvalidOperationException($"Initializer type of {value.InitializerType} passed in, " +
                                                        "but only color multiplier initializers were expected");
                }
                
                Set(value);
            }
        }

        public IParticleInitializer PositionInitializer
        {
            get => Get<IParticleInitializer>();
            set
            {
                if (value != null && value.InitializerType != InitializerType.Position)
                {
                    throw new InvalidOperationException($"Initializer type of {value.InitializerType} passed in, " +
                                                        $"but only position initializers were expected");
                }
                
                Set(value);
            }
        }

        public IParticleInitializer SizeInitializer
        {
            get => Get<IParticleInitializer>();
            set
            {
                if (value != null && value.InitializerType != InitializerType.Size)
                {
                    throw new InvalidOperationException($"Initializer type of {value.InitializerType} passed in, " +
                                                        "but only size initializers were expected");
                }
                
                Set(value);
            }
        }

        public IParticleInitializer VelocityInitializer
        {
            get => Get<IParticleInitializer>();
            set
            {
                if (value != null && value.InitializerType != InitializerType.Velocity)
                {
                    throw new InvalidOperationException($"Initializer type of {value.InitializerType} passed in, " +
                                                        "but only velocity initializers were expected");
                }
                
                Set(value);
            }
        }

        public IParticleInitializer TextureSectionInitializer
        {
            get => Get<IParticleInitializer>();
            set
            {
                if (value != null && value.InitializerType != InitializerType.TextureSectionIndex)
                {
                    throw new InvalidOperationException($"Initializer type of {value.InitializerType} passed in, " +
                                                        "but only texture section initializers were expected");
                }
                
                Set(value);
            }
        }

        public ObservableCollection<IParticleModifier> Modifiers { get; }
        public ObservableCollection<TextureSectionCoords> TextureSections { get; }

        public Workbench()
        {
            Modifiers = new ObservableCollection<IParticleModifier>();
            TextureSections = new ObservableCollection<TextureSectionCoords>();
            
            Modifiers.CollectionChanged += ModifiersOnCollectionChanged;
            TextureSections.CollectionChanged += TextureSectionsOnCollectionChanged;
        }

        protected override void CustomRender()
        {
            ImGui.SetNextWindowPos(Position);
            ImGui.SetNextWindowSize(Size);
            if (ImGui.Begin("Workbench", ImGuiWindowFlags.NoMove | ImGuiWindowFlags.NoResize | ImGuiWindowFlags.NoCollapse))
            {
                ImGui.Text("Initializers"); 
                
                ImGui.SameLine(Size.X / 2 + 5);
                ImGui.Text("Modifiers");
                
                RenderInitializersSection();

                ImGui.SameLine();
                
                RenderModifiersSection();
            }
            
            ImGui.End();
        }

        private static string InitializerSummary(IParticleInitializer initializer)
        {
            if (initializer == null)
            {
                return "<None>";
            }

            switch (initializer.GetType().Name)
            {
                default:
                    return $"{initializer.GetType().Name}";
            }
        }

        private static string ModifierValueSummary(IParticleModifier modifier)
        {
            if (modifier == null)
            {
                throw new ArgumentNullException(nameof(modifier));
            }
            
            switch (modifier.GetType().Name)
            {
                default:
                    return "<Undefined>";
            }
        }

        private void RenderInitializersSection()
        {
            ImGui.BeginChild("Initializers", new Vector2((Size.X / 2) - 15, Size.Y - 60), true);
            
            ImGui.Columns(2, "initializercolumns", false);
            ImGui.Text("Texture File:"); 
            
            ImGui.NextColumn();
            ImGui.Selectable(!string.IsNullOrWhiteSpace(TextureFilename) ? TextureFilename : "<None>");
            
            ImGui.NextColumn();
            ImGui.Text("Texture Sections Defined:");
            
            ImGui.NextColumn();
            ImGui.Selectable(TextureSections.Count.ToString());
            
            ImGui.NextColumn();
            ImGui.Text("Max Particle Lifetime:"); 
            
            ImGui.NextColumn();
            InputFloat(nameof(ParticleLifeTime), "Seconds");
            
            ImGui.NextColumn();
            ImGui.Text("Particles Emitted:");
            
            ImGui.NextColumn();
            ImGui.Selectable(InitializerSummary(ParticleCountInitializer));
            
            ImGui.NextColumn();
            ImGui.Text("Texture:");
            
            ImGui.NextColumn();
            ImGui.Selectable(InitializerSummary(TextureSectionInitializer));
            
            ImGui.NextColumn();
            ImGui.Text("Color Multiplier:");
            
            ImGui.NextColumn();
            ImGui.Selectable(InitializerSummary(ColorMultiplierInitializer));
            
            ImGui.NextColumn();
            ImGui.Text("Position:");
            
            ImGui.NextColumn();
            ImGui.Selectable(InitializerSummary(PositionInitializer));
            
            ImGui.NextColumn();
            ImGui.Text("Size:");
            
            ImGui.NextColumn();
            ImGui.Selectable(InitializerSummary(SizeInitializer));
            
            ImGui.NextColumn();
            ImGui.Text("Velocity:");
            
            ImGui.NextColumn();
            ImGui.Selectable(InitializerSummary(VelocityInitializer));
            
            ImGui.Columns(1);
            ImGui.EndChild();
        }

        private void RenderModifiersSection()
        {
            ImGui.BeginChild("Modifiers", new Vector2((Size.X / 2) - 15, Size.Y - 60), true);

            foreach (var modifier in Modifiers.Where(x => x != null))
            {
                ImGui.Selectable(modifier.GetType().Name);
                ImGui.SameLine();
                ImGui.Text($"({ModifierValueSummary(modifier)})");
            }
            
            ImGui.EndChild();
        }

        private void TextureSectionsOnCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
        }

        private void ModifiersOnCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
        }
    }
}