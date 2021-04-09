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
using Parme.Core.PositionModifier;
using Parme.Core.Triggers;

namespace Parme.Editor.Ui.Elements
{
    public class Workbench : ImGuiElement
    {
        /// <summary>
        /// Affects how much of the window size should be reserved for controls below the initializer/modifier section
        /// </summary>
        private const int ChildWindowHeightSubtractionFactor = 75;

        private readonly SettingsCommandHandler _commandHandler;
        public event EventHandler<IParticleModifier> ModifierRemovalRequested; 
        
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

        public IParticleTrigger Trigger
        {
            get => Get<IParticleTrigger>();
            set => Set(value);
        }

        public Vector2 EmitterVelocity
        {
            get => Get<Vector2>();
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

        public IParticleInitializer RotationalVelocityInitializer
        {
            get => Get<IParticleInitializer>();
            set
            {
                if (value != null && value.InitializerType != InitializerType.RotationalVelocity)
                {
                    throw new InvalidOperationException($"Initializer type of {value.InitializerType} passed in, " +
                                                        "but only rotational velocity initializers were expected");
                }
                
                Set(value);
            }
        }

        public IParticleInitializer RotationalOrientationInitializer
        {
            get => Get<IParticleInitializer>();
            set
            {
                if (value != null && value.InitializerType != InitializerType.RotationalOrientation)
                {
                    throw new InvalidOperationException($"Initializer type of {value.InitializerType} passed in, " +
                                                        "but only rotational orientation initializers were expected");
                }
                
                Set(value);
            }
        }

        public IParticlePositionModifier PositionModifier
        {
            get => Get<IParticlePositionModifier>();
            set => Set(value);
        }

        public ObservableCollection<IParticleModifier> Modifiers { get; }
        public ObservableCollection<TextureSectionCoords> TextureSections { get; }

        public EditorItem? SelectedItem
        {
            get => Get<EditorItem?>();
            set => Set(value);
        }

        public Workbench(SettingsCommandHandler commandHandler)
        {
            _commandHandler = commandHandler;
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
                
                ImGui.NewLine();
                
                ImGui.Text("Moving Emitter Preview:");
                
                ImGui.SameLine();
                if (ImGui.Button("Stop"))
                {
                    EmitterVelocity = Vector2.Zero;
                }
                
                var magnitude = (float) Math.Sqrt(Math.Pow(EmitterVelocity.X, 2) + Math.Pow(EmitterVelocity.Y, 2));
                var angleRadians = Math.Atan2(EmitterVelocity.Y, EmitterVelocity.X);
                var angleDegrees = (int) (angleRadians * (180 / Math.PI));
                angleDegrees = ClampAngle(angleDegrees);

                ImGui.SameLine();
                ImGui.SetNextItemWidth(100);
                var magnitudeChanged = ImGui.InputFloat("Speed", ref magnitude);
                
                ImGui.SameLine();
                ImGui.SetNextItemWidth(100);
                var angleChanged = ImGui.SliderInt("Angle", ref angleDegrees, 0, 360);

                if (magnitudeChanged || angleChanged)
                {
                    if (magnitude < 0)
                    {
                        magnitude *= -1;
                        angleDegrees += 180;
                    }
                    
                    angleDegrees = ClampAngle(angleDegrees);
                    
                    var radians = angleDegrees * (Math.PI / 180);
                    var x = magnitude * Math.Cos(radians);
                    var y = magnitude * Math.Sin(radians);
                    
                    EmitterVelocity = new Vector2((float) x, (float) y);
                }

                if (_commandHandler.CanUndo)
                {
                    ImGui.SameLine(Size.X - 100);
                    if (ImGui.Button("Undo"))
                    {
                        _commandHandler.Undo();
                    }
                }

                if (_commandHandler.CanRedo)
                {
                    ImGui.SameLine(Size.X - 50);
                    if (ImGui.Button("Redo"))
                    {
                        _commandHandler.Redo();
                    }
                }
            }
            
            ImGui.End();
        }

        private static string EditorObjectNameAndValue(IEditorObject initializer)
        {
            return initializer == null 
                ? "<None>" 
                : $"{initializer.EditorShortName} {initializer.EditorShortValue}";
        }

        private static int ClampAngle(int angleInDegrees)
        {
            while (angleInDegrees < 0)
            {
                angleInDegrees += 360;
            }

            while (angleInDegrees >= 360)
            {
                angleInDegrees -= 360;
            }

            return angleInDegrees;
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
            
            ImGui.BeginChild("Initializers", new Vector2((Size.X / 2), Size.Y - ChildWindowHeightSubtractionFactor), true);
            
            ImGui.Columns(2, "initializercolumns", false);
            ImGui.SetColumnWidth(0, firstColumnWidth);
            
            RightAlignText("Texture File:"); 
            
            ImGui.NextColumn();
            var fileName = !string.IsNullOrWhiteSpace(TextureFilename) ? TextureFilename : "<None>"; 
            Selectable(fileName, new EditorItem(EditorItemType.TextureFileName, null));
            
            ImGui.NextColumn();
            RightAlignText("Texture Sections:");
            
            ImGui.NextColumn();
            Selectable(TextureSections.Count.ToString(), new EditorItem(EditorItemType.TextureSection, null));

            ImGui.NextColumn();
            RightAlignText("Initial Texture:");
            
            ImGui.NextColumn();
            Selectable(EditorObjectNameAndValue(TextureSectionInitializer),
                new EditorItem(EditorItemType.Initializer, InitializerType.TextureSectionIndex));
            
            ImGui.NextColumn();
            RightAlignText("Max Particle Lifetime:"); 
            
            ImGui.NextColumn();
            Selectable($"{ParticleLifeTime} Seconds", new EditorItem(EditorItemType.Lifetime, null));

            ImGui.NextColumn();
            RightAlignText("Particles Emitted:");
            
            ImGui.NextColumn();
            Selectable(EditorObjectNameAndValue(ParticleCountInitializer), 
                new EditorItem(EditorItemType.Initializer, InitializerType.ParticleCount));
            
            ImGui.NextColumn();
            RightAlignText("Spawn Trigger:");
            
            ImGui.NextColumn();
            Selectable(EditorObjectNameAndValue(Trigger), 
                new EditorItem(EditorItemType.Trigger, null));

            ImGui.NextColumn();
            RightAlignText("Color Multiplier:");
            
            ImGui.NextColumn();
            Selectable(EditorObjectNameAndValue(ColorMultiplierInitializer),
                new EditorItem(EditorItemType.Initializer, InitializerType.ColorMultiplier));
            
            ImGui.NextColumn();
            RightAlignText("Position:");
            
            ImGui.NextColumn();
            Selectable(EditorObjectNameAndValue(PositionInitializer), 
                new EditorItem(EditorItemType.Initializer, InitializerType.Position));
            
            ImGui.NextColumn();
            RightAlignText("Size:");
            
            ImGui.NextColumn();
            Selectable(EditorObjectNameAndValue(SizeInitializer),
                new EditorItem(EditorItemType.Initializer, InitializerType.Size));
            
            ImGui.NextColumn();
            RightAlignText("Movement Velocity:");
            
            ImGui.NextColumn();
            Selectable(EditorObjectNameAndValue(VelocityInitializer),
                new EditorItem(EditorItemType.Initializer, InitializerType.Velocity));
            
            ImGui.NextColumn();
            RightAlignText("Rotational Velocity:");
            
            ImGui.NextColumn();
            Selectable(EditorObjectNameAndValue(RotationalVelocityInitializer),
                new EditorItem(EditorItemType.Initializer, InitializerType.RotationalVelocity));
            
            ImGui.NextColumn();
            RightAlignText("Initial Rotation");
            
            ImGui.NextColumn();
            Selectable(EditorObjectNameAndValue(RotationalOrientationInitializer),
                new EditorItem(EditorItemType.Initializer, InitializerType.RotationalOrientation));

            ImGui.Columns(1);
            ImGui.EndChild();
        }

        private void RenderModifiersSection()
        {
            ImGui.BeginChild("Modifiers", new Vector2((Size.X / 2) - 30, Size.Y - ChildWindowHeightSubtractionFactor), true);

            Selectable("<Add Modifier>", new EditorItem(EditorItemType.NewModifier, null));
            Selectable($"Position Modifier: {EditorObjectNameAndValue(PositionModifier)}", new EditorItem(PositionModifier));
            
            foreach (var modifier in Modifiers.Where(x => x != null))
            {
                ImGui.PushID($"remove-{modifier.GetType()}");
                if (ImGui.Button("-"))
                {
                    ModifierRemovalRequested?.Invoke(this, modifier);
                }
                ImGui.PopID();

                ImGui.SameLine();
                Selectable($"{EditorObjectNameAndValue(modifier)}",
                    new EditorItem(modifier));
            }
            
            ImGui.EndChild();
        }

        private void TextureSectionsOnCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
        }

        private void ModifiersOnCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
        }
        
        private void Selectable(string text, EditorItem item)
        {
            var isSelected = item.Equals(SelectedItem);
            
            // While most emitter properties will have distinct selectable text, new emitters will all display 
            // "<none>" because they have not had different initializers set up yet.  This causes an issue because
            // all selectables will have the same identifier of "<none>" and therefore imgui can't properly figure out
            // what's selectable.  To address this we have to give each selectable an explicit identifier, and for our
            // purposes the editor item's hash code works for that since it's a struct with consistent hash codes.
            var label = $"{text}##{item.GetHashCode()}";
            ImGui.Selectable(label, ref isSelected);

            if (isSelected && !item.Equals(SelectedItem))
            {
                SelectedItem = item;
            }
        }
    }
}