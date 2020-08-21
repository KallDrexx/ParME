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

        public WorkbenchItem? SelectedItem
        {
            get => Get<WorkbenchItem?>();
            set => Set(value);
        }

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
            if (ImGui.Begin("Workbench", ImGuiWindowFlags.NoMove | ImGuiWindowFlags.NoResize | ImGuiWindowFlags.NoDecoration))
            {
                ImGui.Text("Initializers"); 
                
                ImGui.SameLine(Size.X / 2 + 25);
                ImGui.Text("Modifiers");
                
                RenderInitializersSection();

                ImGui.SameLine();
                
                RenderModifiersSection();
            }
            
            ImGui.End();
        }

        private static string EditorObjectNameAndValue(IEditorObject initializer)
        {
            return initializer == null 
                ? "<None>" 
                : $"{initializer.EditorShortName} {initializer.EditorShortValue}";
        }

        private void RenderInitializersSection()
        {
            const int firstColumnWidth = 225;
            
            void RightAlignText(string text)
            {
                var textWidth = ImGui.CalcTextSize(text).X;
                var position = ImGui.GetCursorPosX() + firstColumnWidth - textWidth - 10;
                ImGui.SetCursorPosX(position);
                ImGui.Text(text);
            }
            
            ImGui.BeginChild("Initializers", new Vector2((Size.X / 2), Size.Y - 60), true);
            
            ImGui.Columns(2, "initializercolumns", false);
            ImGui.SetColumnWidth(0, firstColumnWidth);
            
            RightAlignText("Texture File:"); 
            
            ImGui.NextColumn();
            var fileName = !string.IsNullOrWhiteSpace(TextureFilename) ? TextureFilename : "<None>"; 
            Selectable(fileName, new WorkbenchItem(WorkbenchItemType.TextureFileName, null, null));
            
            ImGui.NextColumn();
            RightAlignText("Texture Sections Defined:");
            
            ImGui.NextColumn();
            Selectable(TextureSections.Count.ToString(), new WorkbenchItem(WorkbenchItemType.TextureSection, null, null));

            ImGui.NextColumn();
            RightAlignText("Max Particle Lifetime:"); 
            
            ImGui.NextColumn();
            Selectable($"{ParticleLifeTime} Seconds", new WorkbenchItem(WorkbenchItemType.Lifetime, null, null));

            ImGui.NextColumn();
            RightAlignText("Particles Emitted:");
            
            ImGui.NextColumn();
            Selectable(EditorObjectNameAndValue(ParticleCountInitializer), 
                new WorkbenchItem(WorkbenchItemType.Initializer, InitializerType.ParticleCount, ParticleCountInitializer));
            
            ImGui.NextColumn();
            RightAlignText("Texture:");
            
            ImGui.NextColumn();
            Selectable(EditorObjectNameAndValue(TextureSectionInitializer),
                new WorkbenchItem(WorkbenchItemType.Initializer, InitializerType.TextureSectionIndex, TextureSectionInitializer));
            
            ImGui.NextColumn();
            RightAlignText("Color Multiplier:");
            
            ImGui.NextColumn();
            Selectable(EditorObjectNameAndValue(ColorMultiplierInitializer),
                new WorkbenchItem(WorkbenchItemType.Initializer, InitializerType.ColorMultiplier, ColorMultiplierInitializer));
            
            ImGui.NextColumn();
            RightAlignText("Position:");
            
            ImGui.NextColumn();
            Selectable(EditorObjectNameAndValue(PositionInitializer), 
                new WorkbenchItem(WorkbenchItemType.Initializer, InitializerType.Position, PositionInitializer));
            
            ImGui.NextColumn();
            RightAlignText("Size:");
            
            ImGui.NextColumn();
            Selectable(EditorObjectNameAndValue(SizeInitializer),
                new WorkbenchItem(WorkbenchItemType.Initializer, InitializerType.Size, SizeInitializer));
            
            ImGui.NextColumn();
            RightAlignText("Velocity:");
            
            ImGui.NextColumn();
            Selectable(EditorObjectNameAndValue(VelocityInitializer),
                new WorkbenchItem(WorkbenchItemType.Initializer, InitializerType.Velocity, VelocityInitializer));
            
            ImGui.Columns(1);
            ImGui.EndChild();
        }

        private void RenderModifiersSection()
        {
            ImGui.BeginChild("Modifiers", new Vector2((Size.X / 2) - 30, Size.Y - 60), true);

            foreach (var modifier in Modifiers.Where(x => x != null))
            {
                Selectable($"{EditorObjectNameAndValue(modifier)}",
                    new WorkbenchItem(WorkbenchItemType.Modifier, null, modifier));
            }
            
            ImGui.EndChild();
        }

        private void TextureSectionsOnCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
        }

        private void ModifiersOnCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
        }
        
        private void Selectable(string text, WorkbenchItem item)
        {
            var isSelected = item.Equals(SelectedItem);
            ImGui.Selectable(text, ref isSelected);

            if (isSelected && !item.Equals(SelectedItem))
            {
                SelectedItem = item;
            }
        }
    }
}