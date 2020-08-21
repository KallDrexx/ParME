using System.Numerics;
using ImGuiHandler;
using ImGuiNET;
using Parme.Core.Initializers;

namespace Parme.Editor.Ui.Elements
{
    public class ActiveEditorWindow : ImGuiElement
    {
        public Vector2 Position { get; set; }
        public Vector2 Size { get; set; }

        public EditorItem? ItemBeingEdited
        {
            get => Get<EditorItem?>();
            set => Set(value);
        }
        
        protected override void CustomRender()
        {
            ImGui.SetNextWindowSize(Size);
            ImGui.SetNextWindowPos(Position);

            if (ImGui.Begin("Active Editor", ImGuiWindowFlags.NoDecoration | ImGuiWindowFlags.NoResize))
            {
                ImGui.Text("Editing: ");
                ImGui.SameLine();
                
                switch (ItemBeingEdited?.ItemType)
                {
                    case null:
                        ImGui.Text("<No Item Selected>");
                        break;
                    
                    case EditorItemType.Initializer:
                        switch (ItemBeingEdited.Value.InitializerType)
                        {
                            case InitializerType.Position:
                                ImGui.Text("Initial Position");
                                break;
                            
                            case InitializerType.Size:
                                ImGui.Text("Initial Size");
                                break;
                            
                            case InitializerType.Velocity:
                                ImGui.Text("Initial Velocity");
                                break;
                            
                            case InitializerType.ColorMultiplier:
                                ImGui.Text("Initial Color Multiplier");
                                break;
                            
                            case InitializerType.ParticleCount:
                                ImGui.Text("Particle Spawn Count");
                                break;
                            
                            case InitializerType.TextureSectionIndex:
                                ImGui.Text("Initial Texture Section");
                                break;
                            
                            default:
                                ImGui.Text("Initializer");
                                break;
                        }
                        
                        break;
                    
                    case EditorItemType.Lifetime:
                        ImGui.Text("Particle Lifetime");
                        break;
                    
                    case EditorItemType.Modifier:
                        ImGui.Text("Modifier");
                        ImGui.Text($"Type: {ItemBeingEdited.Value.ModifierInstance.EditorShortName}");
                        break;
                    
                    case EditorItemType.Trigger:
                        ImGui.Text("Trigger");
                        break;
                    
                    case EditorItemType.TextureSection:
                        ImGui.Text("Texture Sections");
                        break;
                    
                    case EditorItemType.TextureFileName:
                        ImGui.Text("Texture");
                        break;

                    default:
                        ImGui.Text(ItemBeingEdited.Value.ToString());
                        break;
                }
            }
            
            ImGui.End();
        }
    }
}