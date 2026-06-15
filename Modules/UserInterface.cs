using DieselExileTools.Common;
using DieselExileTools.ExileCore2;
using ExileCore2.Shared.Helpers;
using ImGuiNET;
using SDColor = System.Drawing.Color;
using SVector2 = System.Numerics.Vector2;

namespace MapIcons;

public class Tree
{
    public List<TreeCategory> Categories { get; set; } = new List<TreeCategory>();
}
public class TreeCategory
{
    public string Name { get; set; }
    public List<TreeIcon> TreeIcons { get; set; } = new List<TreeIcon>();

}
public class TreeIcon
{
    public string Name { get; set; }
    public TreeIconConfigs Config { get; set; }
    public MapIconTypes MapIconType { get; set; }
    public Action<MapIconSettings> CustomDrawAction { get; set; }
    public MapIconSettings DefaultSettings { get; set; } = new MapIconSettings();
}

public sealed class UserInterface : PluginModule {
    public UserInterface(Plugin plugin) : base(plugin) { }

    private Tree UITree = new Tree {
        Categories = new List<TreeCategory> {
            // Ingame Icons
            new TreeCategory {
                Name = "Ingame Icons",
                TreeIcons = new List<TreeIcon> {
                    new TreeIcon {
                        Name = "Shrine",
                        MapIconType = MapIconTypes.Shrine,
                        Config = TreeIconConfigs.IngameIcon,
                        DefaultSettings = new MapIconSettings { DrawState = IngameIconDrawStates.Always, DrawName = true }
                    },
                    new TreeIcon {
                        Name = "Breach",
                        MapIconType = MapIconTypes.Breach,
                        Config = TreeIconConfigs.IngameIcon,
                        DefaultSettings = new MapIconSettings { DrawState = IngameIconDrawStates.Ranged }
                    },
                    new TreeIcon {
                        Name = "Area Transition",
                        MapIconType = MapIconTypes.AreaTransition,
                        Config = TreeIconConfigs.IngameIcon,
                        DefaultSettings = new MapIconSettings { DrawState = IngameIconDrawStates.Ranged }
                    },
                    new TreeIcon {
                        Name = "Quest Object",
                        MapIconType = MapIconTypes.QuestObject,
                        Config = TreeIconConfigs.IngameIcon,
                        DefaultSettings = new MapIconSettings { DrawState = IngameIconDrawStates.Ranged, DrawName = true }
                    },
                    new TreeIcon {
                        Name = "Ritual",
                        MapIconType = MapIconTypes.Ritual,
                        Config = TreeIconConfigs.IngameIcon,
                        DefaultSettings = new MapIconSettings { DrawState = IngameIconDrawStates.Always }
                    },
                    new TreeIcon {
                        Name = "Waypoint",
                        MapIconType = MapIconTypes.Waypoint,
                        Config = TreeIconConfigs.IngameIcon,
                        DefaultSettings = new MapIconSettings { DrawState = IngameIconDrawStates.Ranged }
                    },
                    new TreeIcon {
                        Name = "Checkpoint",
                        MapIconType = MapIconTypes.Checkpoint,
                        Config = TreeIconConfigs.IngameIcon,
                        DefaultSettings = new MapIconSettings { DrawState = IngameIconDrawStates.Ranged }
                    },
                    new TreeIcon {
                        Name = "Ingame NPC",
                        MapIconType = MapIconTypes.IngameNPC,
                        Config = TreeIconConfigs.IngameIcon,
                        DefaultSettings = new MapIconSettings { DrawState = IngameIconDrawStates.Ranged }
                    },
                    new TreeIcon {
                        Name = "Ingame Uncategorized",
                        MapIconType = MapIconTypes.IngameUncategorized,
                        Config = TreeIconConfigs.IngameIcon,
                        DefaultSettings = new MapIconSettings { DrawState = IngameIconDrawStates.Ranged }
                    },
                }
            },
            // Trap Icons
            new TreeCategory {
                Name = "Trap Icons",
                TreeIcons = new List<TreeIcon> {
                    new TreeIcon {
                        Name = "Ground Spike",
                        Config = TreeIconConfigs.Trap,
                        MapIconType = MapIconTypes.GroundSpike,
                        DefaultSettings = new MapIconSettings
                        {
                            Tint = DXTC.Color.FromRGBA(213,0,0,255),
                            ArmingTint = DXTC.Color.FromRGBA(213,0,0,100),
                            HiddenTint = DXTC.Color.FromRGBA(213,0,0,0),
                        }
                    },
                }
            },  
            // Dangerous Icons
            new TreeCategory {
                Name = "Dangerous Icons",
                TreeIcons = new List<TreeIcon> {
                    new TreeIcon {
                        Name = "Volatile Core",
                        MapIconType = MapIconTypes.VolatileCore,
                        Config = TreeIconConfigs.Default,
                        DefaultSettings = new MapIconSettings {
                            Index = 25,
                            Tint = DXTC.Color.FromRGBA(213,0,249,255),
                            HiddenTint = DXTC.Color.FromRGBA(244,179,255,255),
                        }
                    },
                    new TreeIcon {
                        Name = "Drowning Orb",
                        MapIconType = MapIconTypes.DrowningOrb,
                        Config = TreeIconConfigs.Default,
                        DefaultSettings = new MapIconSettings {
                            Index = 25,
                            Tint = DXTC.Color.FromRGBA(213,0,249,255),
                            HiddenTint = DXTC.Color.FromRGBA(244,179,255,255),
                        }
                    },
                    new TreeIcon {
                        Name = "Lightning Clone",
                        MapIconType = MapIconTypes.LightningClone,
                        Config = TreeIconConfigs.Default,
                        DefaultSettings = new MapIconSettings {
                            Index = 25,
                            Tint = DXTC.Color.FromRGBA(213,0,249,255),
                            HiddenTint = DXTC.Color.FromRGBA(244,179,255,255),
                        }
                    },
                    new TreeIcon {
                        Name = "Consuming Phantasm",
                        MapIconType = MapIconTypes.ConsumingPhantasm,
                        Config = TreeIconConfigs.Default,
                        DefaultSettings = new MapIconSettings {
                            Index = 25,
                            Tint = DXTC.Color.FromRGBA(213,0,249,255),
                            HiddenTint = DXTC.Color.FromRGBA(244,179,255,255),
                        }
                    },
                },
            },
            // Monster Icons
            new TreeCategory {
                Name = "Monster Icons",
                TreeIcons = new List<TreeIcon> {
                    new TreeIcon {
                        Name = "Normal Monster",
                        Config = TreeIconConfigs.Monster,
                        MapIconType = MapIconTypes.WhiteMonster,
                        DefaultSettings = new MapIconSettings
                        {
                            Tint = DXTC.Color.FromRGBA(213,0,0,255),
                            HiddenTint = DXTC.Color.FromRGBA(255,185,179,255), // Light Red
                        }
                    },
                    new TreeIcon {
                        Name = "Magic Monster",
                        Config = TreeIconConfigs.Monster,
                        MapIconType = MapIconTypes.MagicMonster,
                        DefaultSettings = new MapIconSettings {
                            Index = 1,
                            Tint = DXTC.Color.FromRGBA(0,145,234,255),
                            HiddenTint = DXTC.Color.FromRGBA(179,232,255,255),
                        }
                    },
                    new TreeIcon {
                        Name = "Rare Monster",
                        Config = TreeIconConfigs.Monster,
                        MapIconType = MapIconTypes.RareMonster,
                        DefaultSettings = new MapIconSettings {
                            Index = 2,
                            Tint = DXTC.Color.FromRGBA(255,214,0,255),
                            HiddenTint = DXTC.Color.FromRGBA(255,255,179,255),
                        }
                    },
                    new TreeIcon {
                        Name = "Unique Monster",
                        Config = TreeIconConfigs.Monster,
                        MapIconType = MapIconTypes.UniqueMonster,
                        DefaultSettings = new MapIconSettings {
                            AnimateLife = true,
                            Index = 40,
                            Tint = DXTC.Color.FromRGBA(255,109,0,255),
                            HiddenTint = DXTC.Color.FromRGBA(255,218,153,255),
                        }
                    },
                    new TreeIcon {
                        Name = "Rogue Exile",
                        Config = TreeIconConfigs.Monster,
                        MapIconType = MapIconTypes.RogueExile,
                        DefaultSettings = new MapIconSettings {
                            Index = 2,
                            Tint = DXTC.Color.FromRGBA(255,109,0,255),
                            HiddenTint = DXTC.Color.FromRGBA(255, 218, 153, 255),
                        }
                    },
                    new TreeIcon {
                        Name = "Giant Rogue Exile",
                        Config = TreeIconConfigs.Monster,
                        MapIconType = MapIconTypes.GiantRogueExile,
                        DefaultSettings = new MapIconSettings {
                            Index = 98,
                            Tint = DXTC.Color.FromRGBA(255,109,0,255),
                            HiddenTint = DXTC.Color.FromRGBA(255,218,153,255),
                        }
                    },
                    new TreeIcon {
                        Name = "Spirit",
                        Config = TreeIconConfigs.Monster,
                        MapIconType = MapIconTypes.Spirit,
                        DefaultSettings = new MapIconSettings {
                            Index = 2,
                            Tint = DXTC.Color.FromRGBA(192,202,51,255),
                            HiddenTint = DXTC.Color.FromRGBA(233,240,168,255),
                        }
                    },
                    new TreeIcon {
                        Name = "Amanamu Clouded",
                        Config = TreeIconConfigs.IconOnly,
                        MapIconType = MapIconTypes.AmanamuClouded,
                        DefaultSettings = new MapIconSettings {
                            Index = 53,
                        }
                    },
                },
            },
            // Delirium Icons
            new TreeCategory {
                Name = "Delirium Icons",
                TreeIcons = new List<TreeIcon> {
                    new TreeIcon {
                        Name = "Fractuirng Mirror",
                        MapIconType = MapIconTypes.FracturingMirror,
                        Config = TreeIconConfigs.Default,
                        DefaultSettings = new MapIconSettings {
                            Index = 74,
                            Tint = DXTC.Color.FromRGBA(213,0,249,255),
                            HiddenTint = DXTC.Color.FromRGBA(244,179,255,255),
                        }
                    },
                    new TreeIcon {
                        Name = "Egg Fodder",
                        MapIconType = MapIconTypes.EggFodder,
                        Config = TreeIconConfigs.Default,
                        DefaultSettings = new MapIconSettings {
                            Index = 74,
                            Tint = DXTC.Color.FromRGBA(213,0,249,255),
                            HiddenTint = DXTC.Color.FromRGBA(244,179,255,255),
                        }
                    },
                    new TreeIcon {
                        Name = "Glob Spawn",
                        MapIconType = MapIconTypes.GlobSpawn,
                        Config = TreeIconConfigs.Default,
                        DefaultSettings = new MapIconSettings {
                            Index = 74,
                            Tint = DXTC.Color.FromRGBA(213,0,249,255),
                            HiddenTint = DXTC.Color.FromRGBA(244,179,255,255),
                        }
                    },
                },
            },
            // Friendly Icons 
            new TreeCategory {
                Name = "Friendly Icons",
                TreeIcons = new List<TreeIcon> {
                    new TreeIcon {
                        Name = "Local Player",
                        Config = TreeIconConfigs.Friendly,
                        MapIconType = MapIconTypes.LocalPlayer,
                        DefaultSettings = new MapIconSettings
                        {
                            Draw = false,
                            Tint = DXTC.Color.FromRGBA(100,221,23,255),
                            HiddenTint = DXTC.Color.FromRGBA(210,247,186,255),
                        }
                    },
                    new TreeIcon {
                        Name = "Other Player",
                        Config = TreeIconConfigs.Friendly,
                        MapIconType = MapIconTypes.OtherPlayer,
                        DefaultSettings = new MapIconSettings
                        {
                            Draw = false,
                            Tint = DXTC.Color.FromRGBA(100,221,23,255),
                            HiddenTint = DXTC.Color.FromRGBA(210,247,186,255),
                        }
                    },
                    new TreeIcon {
                        Name = "NPC",
                        Config = TreeIconConfigs.Friendly,
                        MapIconType = MapIconTypes.NPC,
                        DefaultSettings = new MapIconSettings
                        {
                            Draw = false,
                            Index = 1,
                            Tint = DXTC.Color.FromRGBA(100,221,23,255),
                            HiddenTint = DXTC.Color.FromRGBA(210,247,186,255),
                        }
                    },
                    new TreeIcon {
                        Name = "Minion",
                        Config = TreeIconConfigs.Friendly,
                        MapIconType = MapIconTypes.Minion,
                        DefaultSettings = new MapIconSettings
                        {
                            Draw = false,
                            Tint = DXTC.Color.FromRGBA(100,221,23,255),
                            HiddenTint = DXTC.Color.FromRGBA(210,247,186,255),
                        }
                    },
                    new TreeIcon {
                        Name = "Decoy Totem",
                        Config = TreeIconConfigs.Friendly,
                        MapIconType = MapIconTypes.DecoyTotem,
                        DefaultSettings = new MapIconSettings
                        {
                            AnimateLife = true,
                            Index = 16,
                            Tint = DXTC.Color.FromRGBA(100,221,23,255),
                            HiddenTint = DXTC.Color.FromRGBA(210,247,186,255),
                        }
                    },
                },

            },
            // Chest Icons 
            new TreeCategory {
                Name = "Chest Icons",
                TreeIcons = new List<TreeIcon> {
                    new TreeIcon {
                        Name = "Breakable Object",
                        MapIconType = MapIconTypes.BreakableObject,
                        Config = TreeIconConfigs.Chest,
                        DefaultSettings = new MapIconSettings {
                            Draw = false,
                            Index = 360,
                            Tint = DXTC.Color.FromRGBA(137, 137, 137, 255),
                        }
                    },
                    new TreeIcon {
                        Name = "Chest White",
                        MapIconType = MapIconTypes.ChestWhite,
                        Config = TreeIconConfigs.Chest,
                        DefaultSettings = new MapIconSettings {
                            Draw = false,
                            Index = 360,
                            Tint = DXTC.Color.FromRGBA(255, 255, 255, 255),
                        }
                    },
                    new TreeIcon {
                        Name = "Chest Magic",
                        MapIconType = MapIconTypes.ChestMagic,
                        Config = TreeIconConfigs.Chest,
                        DefaultSettings = new MapIconSettings {
                            Index = 360,
                            Tint = DXTC.Color.FromRGBA(0,145,234,255),
                        }
                    },
                    new TreeIcon {
                        Name = "Chest Rare",
                        MapIconType = MapIconTypes.ChestRare,
                        Config = TreeIconConfigs.Chest,
                        DefaultSettings = new MapIconSettings {
                            Index = 360,
                            Tint = DXTC.Color.FromRGBA(255,214,0,255),
                        }
                    },
                    new TreeIcon {
                        Name = "Chest Unique",
                        MapIconType = MapIconTypes.ChestUnique,
                        Config = TreeIconConfigs.Chest,
                        DefaultSettings = new MapIconSettings {
                            Index = 360,
                            Tint = DXTC.Color.FromRGBA(255,109,0,255),
                        }
                    },                    
                    new TreeIcon {
                        Name = "Breach Chest Large",
                        MapIconType = MapIconTypes.BreachChestLarge,
                        Config = TreeIconConfigs.Chest,
                        DefaultSettings = new MapIconSettings {
                            Index = 361,
                            Tint = DXTC.Color.FromRGBA(233, 0, 255, 255),
                        }
                    },
                    new TreeIcon {
                        Name = "Expedition Chest White",
                        MapIconType = MapIconTypes.ExpeditionChestWhite,
                        Config = TreeIconConfigs.Chest,
                        DefaultSettings = new MapIconSettings {
                            Index = 360,
                            Tint = DXTC.Color.FromRGBA(255, 255, 255, 255),
                        }
                    },
                    new TreeIcon {
                        Name = "Expedition Chest Magic",
                        MapIconType = MapIconTypes.ExpeditionChestMagic,
                        Config = TreeIconConfigs.Chest,
                        DefaultSettings = new MapIconSettings {
                            Index = 360,
                            Tint = DXTC.Color.FromRGBA(0,145,234,255),
                        }
                    },
                    new TreeIcon {
                        Name = "Expedition Chest Rare",
                        MapIconType = MapIconTypes.ExpeditionChestRare,
                        Config = TreeIconConfigs.Chest,
                        DefaultSettings = new MapIconSettings {
                            Index = 360,
                            Tint = DXTC.Color.FromRGBA(255,214,0,255),
                        }
                    },
                    new TreeIcon {
                        Name = "Sanctum Chest",
                        MapIconType = MapIconTypes.SanctumChest,
                        Config = TreeIconConfigs.Chest,
                        DefaultSettings = new MapIconSettings {
                            Index = 360,
                            Tint =DXTC.Color.FromRGBA(219, 0, 255, 255),
                        }
                    },
                    new TreeIcon {
                        Name = "Abyss Chest",
                        MapIconType = MapIconTypes.AbyssChest,
                        Config = TreeIconConfigs.Chest,
                        DefaultSettings = new MapIconSettings {
                            Index = 360,
                            Tint = DXTC.Color.FromRGBA(0, 160, 0, 255),
                        }
                    },
                },
            },
            // Wildwood
            new TreeCategory {
                Name = "Wildwood Icons",
                TreeIcons = new List<TreeIcon> {
                    new TreeIcon {
                        Name = "Omen Altar",
                        MapIconType = MapIconTypes.OmenAltar,
                        Config = TreeIconConfigs.Chest,
                        DefaultSettings = new MapIconSettings {
                            Index = 360,
                            Tint = DXTC.Color.FromRGBA(233, 0, 255, 255),
                        }
                    },
                    new TreeIcon {
                        Name = "Light Bomb",
                        MapIconType = MapIconTypes.LightBmb,
                        Config = TreeIconConfigs.Chest,
                        DefaultSettings = new MapIconSettings {
                            Index = 194,
                            Tint = DXTC.Color.FromRGBA(128,222,234,255),
                        }
                    },
                },
            },            
            // Currency items
            new TreeCategory {
                Name = "Currency Icons",
                TreeIcons = new List<TreeIcon> {
                    new TreeIcon {
                        Name = "Sanctum Mote",
                        MapIconType = MapIconTypes.SanctumMote,
                        Config = TreeIconConfigs.Chest,
                        DefaultSettings = new MapIconSettings {
                            Index = 120,
                            Tint = DXTC.Color.FromRGBA(24,255,255,255),
                        }
                    },
                },
            },
        },
    };

    private void DrawTree() {
        foreach (var category in UITree.Categories) {

            bool isOpen = Settings.GetCategoryHeaderOpen(category.Name);
            if (DXT.CollapsingHeader(category.Name, ref isOpen)) {

                ImGui.Indent();
                foreach (var treeIcon in category.TreeIcons) {
                    var iconSettings = Settings.SetDefaultIconSettings(treeIcon.MapIconType, treeIcon.DefaultSettings);

                    switch (treeIcon.Config) {
                        case TreeIconConfigs.Default:
                            DXT.Checkbox.Draw("##Draw{treeIcon.Name}",$"Draw {treeIcon.Name}", ref iconSettings.Draw); ImGui.SameLine();
                            DXT.ColorSelect.Draw($"IC{treeIcon.Name}", $"Icon Color", ref iconSettings.Tint); ImGui.SameLine();
                            DXT.ColorSelect.Draw($"IHC{treeIcon.Name}", $"Icon Hidden Color", ref iconSettings.HiddenTint); ImGui.SameLine();
                            DXT.IconSelect.Draw($"Icon{treeIcon.Name}", $"Icon", ref iconSettings.Index, Plugin.IconAtlas, new DXT.IconSelect.Options { IconColor = iconSettings.Tint }); ImGui.SameLine();
                            IconSizeSliderInt($"Icons Slider {treeIcon.Name}", ref iconSettings.Size, 0, 32); ImGui.SameLine();
                            ImGui.Text($"{treeIcon.Name}");
                            break;
                        case TreeIconConfigs.IngameIcon:
                            IngameIconComboBox($"##{treeIcon.Name}", ref iconSettings.DrawState); ImGui.SameLine();
                            DXT.Checkbox.Draw($"##checkbox{treeIcon.Name}", $"Show {treeIcon.Name} Name", ref iconSettings.DrawName); ImGui.SameLine();
                            ImGui.Text($"{treeIcon.Name}");
                            break;
                        case TreeIconConfigs.Monster:
                            DXT.Checkbox.Draw($"##Draw_{treeIcon.Name}", $"Draw {treeIcon.Name}", ref iconSettings.Draw); ImGui.SameLine();
                            DXT.ColorSelect.Draw($"IC{treeIcon.Name}", $"Icon Color", ref iconSettings.Tint); ImGui.SameLine();
                            DXT.ColorSelect.Draw($"IHC{treeIcon.Name}", $"Icon Hidden Color", ref iconSettings.HiddenTint); ImGui.SameLine();
                            DXT.IconSelect.Draw($"Icon{treeIcon.Name}", $"Icon", ref iconSettings.Index, Plugin.IconAtlas, new DXT.IconSelect.Options { IconColor = iconSettings.Tint }); ImGui.SameLine();
                            DXT.Checkbox.Draw($"##Animate_{treeIcon.Name}", $"Animate Icon Health, uses 8 sequential icons to visualise health", ref iconSettings.AnimateLife); ImGui.SameLine();
                            IconSizeSliderInt($"Icons Slider {treeIcon.Name}", ref iconSettings.Size, 0, 32); ImGui.SameLine();
                            ImGui.Text(treeIcon.Name);
                            break;
                        case TreeIconConfigs.Friendly:
                            DXT.Checkbox.Draw($"##Draw_{treeIcon.Name}", $"Draw {treeIcon.Name}", ref iconSettings.Draw); ImGui.SameLine();
                            DXT.ColorSelect.Draw($"IC{treeIcon.Name}", $"Icon Color", ref iconSettings.Tint); ImGui.SameLine();
                            DXT.ColorSelect.Draw($"IHC{treeIcon.Name}", $"Icon Hidden Color", ref iconSettings.HiddenTint); ImGui.SameLine();
                            DXT.IconSelect.Draw($"Icon{treeIcon.Name}", $"Icon", ref iconSettings.Index, Plugin.IconAtlas, new DXT.IconSelect.Options { IconColor = iconSettings.Tint }); ImGui.SameLine();
                            DXT.Checkbox.Draw($"##Animate_{treeIcon.Name}", $"Animate Icon Health, uses 8 sequential icons to visualise health", ref iconSettings.AnimateLife); ImGui.SameLine();
                            IconSizeSliderInt($"Icons Slider {treeIcon.Name}", ref iconSettings.Size, 0, 32); ImGui.SameLine();
                            DXT.Checkbox.Draw($"##checkboxname{treeIcon.Name}", $"Show Name", ref iconSettings.DrawName); ImGui.SameLine();
                            DXT.Checkbox.Draw($"##checkboxhealth{treeIcon.Name}", $"Show Health", ref iconSettings.DrawHealth); ImGui.SameLine();
                            ImGui.Text(treeIcon.Name);
                            break;
                        case TreeIconConfigs.Chest:
                            DXT.Checkbox.Draw($"##Draw_{treeIcon.Name}", $"Draw {treeIcon.Name}", ref iconSettings.Draw); ImGui.SameLine();
                            DXT.ColorSelect.Draw($"IC{treeIcon.Name}", $"Icon Color", ref iconSettings.Tint); ImGui.SameLine();
                            DXT.IconSelect.Draw($"Icon{treeIcon.Name}", $"Icon", ref iconSettings.Index, Plugin.IconAtlas, new DXT.IconSelect.Options { IconColor = iconSettings.Tint }); ImGui.SameLine();
                            IconSizeSliderInt($"Icons Slider {treeIcon.Name}", ref iconSettings.Size, 0, 32); ImGui.SameLine();
                            ImGui.Text(treeIcon.Name);
                            break;
                        case TreeIconConfigs.Trap:
                            DXT.Checkbox.Draw($"##Draw_{treeIcon.Name}", $"Draw {treeIcon.Name}", ref iconSettings.Draw); ImGui.SameLine();
                            DXT.ColorSelect.Draw($"IC{treeIcon.Name}", $"Icon Color", ref iconSettings.Tint); ImGui.SameLine();
                            DXT.ColorSelect.Draw($"IAC{treeIcon.Name}", $"Icon Arming Color", ref iconSettings.ArmingTint); ImGui.SameLine();
                            DXT.ColorSelect.Draw($"IHC{treeIcon.Name}", $"Icon Hidden Color", ref iconSettings.HiddenTint); ImGui.SameLine();
                            DXT.IconSelect.Draw($"Icon{treeIcon.Name}", $"Icon", ref iconSettings.Index, Plugin.IconAtlas, new DXT.IconSelect.Options { IconColor = iconSettings.Tint }); ImGui.SameLine();
                            IconSizeSliderInt($"Icons Slider {treeIcon.Name}", ref iconSettings.Size, 0, 32); ImGui.SameLine();
                            ImGui.Text(treeIcon.Name);
                            break;
                        case TreeIconConfigs.IconOnly:
                            DXT.Checkbox.Draw($"##Draw_{treeIcon.Name}", $"Draw {treeIcon.Name}", ref iconSettings.Draw); ImGui.SameLine();
                            DXT.IconSelect.Draw($"Icon{treeIcon.Name}", $"Icon", ref iconSettings.Index, Plugin.IconAtlas, new DXT.IconSelect.Options { IconColor = iconSettings.Tint }); ImGui.SameLine();
                            IconSizeSliderInt($"Icons Slider {treeIcon.Name}", ref iconSettings.Size, 0, 32); ImGui.SameLine();
                            ImGui.Text(treeIcon.Name);
                            break;
                        case TreeIconConfigs.Custom:
                            treeIcon.CustomDrawAction?.Invoke(iconSettings);
                            break;
                    }
                }
                ImGui.Unindent();
            }
            Settings.SetCategoryHeader(category.Name, isOpen);
        }
    }

    private static void IconSizeSliderInt(string id, ref int v, int v_min, int v_max) {
        ImGui.PushItemWidth(100);
        ImGui.SliderInt($"##{id}", ref v, v_min, v_max);
        if (ImGui.IsItemHovered()) {
            ImGui.BeginTooltip();
            ImGui.Text("Icon Size");
            ImGui.EndTooltip();
        }
        ImGui.PopItemWidth();
    }
    private string[] iconDrawStates = Enum.GetNames(typeof(IngameIconDrawStates));
    private bool IngameIconComboBox(string label, ref IngameIconDrawStates selectedState) {
        bool itemChanged = false;
        ImGui.PushItemWidth(100);
        int selectedIndex = (int)selectedState;
        if (ImGui.BeginCombo(label, iconDrawStates[selectedIndex])) {
            for (int i = 0; i < iconDrawStates.Length; i++) {
                bool isSelected = (selectedIndex == i);
                if (ImGui.Selectable(iconDrawStates[i], isSelected)) {
                    selectedState = (IngameIconDrawStates)i;
                    itemChanged = true;
                }
                if (isSelected) {
                    ImGui.SetItemDefaultFocus();
                }
            }
            ImGui.EndCombo();
        }
        ImGui.PopItemWidth();
        return itemChanged;
    }
    private void DrawCustomPathIcons() {

        for (int i = 0; i < Settings.CustomPathIcons.Count; i++) {
            var setting = Settings.CustomPathIcons[i];

            DXT.Checkbox.Draw($"##CustomPath{i}", "Draw Custom Path", ref setting.Draw); ImGui.SameLine();
            DXT.ColorSelect.Draw($"ICP{i}", "Icon Color", ref setting.Tint); ImGui.SameLine();
            DXT.ColorSelect.Draw($"IHCP{i}", "Icon Hidden Color", ref setting.HiddenTint); ImGui.SameLine();
            DXT.IconSelect.Draw($"CustomPathIcon{i}", "Custom Path Icon", ref setting.Index, Plugin.IconAtlas, new DXT.IconSelect.Options { IconColor = setting.Tint }); ImGui.SameLine();
            IconSizeSliderInt($"##CustomPath{i}", ref setting.Size, 0, 32); ImGui.SameLine();
            DXT.Checkbox.Draw($"##CustomPathText{i}", "Draw Text", ref setting.DrawName); ImGui.SameLine();
            DXT.Checkbox.Draw($"##CustomPathAlive{i}", "Check if Entity is Alive", ref setting.Check_IsAlive); ImGui.SameLine();
            DXT.Checkbox.Draw($"##CustomPathOpened{i}", "Check if Entity is Opened", ref setting.Check_IsOpened); ImGui.SameLine();
            float inputTextWidth = ImGui.GetContentRegionAvail().X - ImGui.CalcTextSize("Remove").X - ImGui.GetStyle().ItemSpacing.X;
            ImGui.SetNextItemWidth(inputTextWidth);
            ImGui.InputText($"##Path{i}", ref setting.Path, 100); ImGui.SameLine();
            ImGui.PopItemWidth();
            if (ImGui.Button($"Remove##Path{i}")) Settings.RemoveCustomPathIcon(i);
        }

        if (ImGui.Button("Add Icon")) Settings.NewCustomPathIcon(); ImGui.SameLine();
        if (ImGui.Button("Rebuild Icons")) Plugin.IconBuilder.RebuildIcons();
    }


    public void Draw() {

        ImGui.PushItemWidth(100); // Set slider width
        ImGui.SliderInt("Rebuild", ref Settings.IconRebuildUpdateTicks, 1, 20);
        if (ImGui.IsItemHovered()) {
            ImGui.BeginTooltip();
            ImGui.Text("Set the interval (in ticks) for rebuilding icons from entities");
            ImGui.EndTooltip();
        }
        ImGui.SameLine();
        ImGui.SliderInt("ReCache", ref Settings.IconCacheUpdateMS, 10, 1000);
        if (ImGui.IsItemHovered()) {
            ImGui.BeginTooltip();
            ImGui.Text("Set the interval (in milliseconds) for refreshing the drawn icons cache");
            ImGui.EndTooltip();
        }
        ImGui.SameLine();
        ImGui.PopItemWidth(); // Reset slider width
        DXT.Button.Draw("ShowDBugger", ref Settings.DXT.DBug.ShowToolbar, new DXT.Button.Options {
            Label = "DBugger",
            Width = 120,
            Height = 22,
        });

        if (DXT.CollapsingHeader("Draw Settings", ref Settings.DrawSettingsOpen)) {
            ImGui.Indent();
            DXT.Checkbox.Draw("Draw on Minimap", "Draw Monsters on the minimap", ref Settings.DrawOnMinimap);
            DXT.Checkbox.Draw("Draw Pixel Perfect Icons", "Enable pixel perfect icons", ref Settings.PixelPerfectIcons);
            DXT.Checkbox.Draw("Draw cached Entities", "Draw entities that are cached but no longer in proximity", ref Settings.DrawCachedEntities);
            DXT.Checkbox.Draw("Draw Over Large Panels", "Enable drawing over large panels", ref Settings.DrawOverLargePanels);
            DXT.Checkbox.Draw("Draw Over Fullscreen Panels", "Enable drawing over fullscreen panels", ref Settings.DrawOverFullscreenPanels);
            ImGui.Unindent();
        }

        if (DXT.CollapsingHeader("Ignored Entities", ref Settings.IgnoredEntitesOpen)) {
            ImGui.Indent();
            if (ImGui.Button("Update")) { Plugin.IconBuilder.UpdateUserSkippedEntities(); }
            ImGui.SameLine();
            ImGui.SliderInt("Height", ref Settings.IgnoredEntitiesHeight, 100, 1000);
            ImGui.InputTextMultiline("##ignoredEntitiesInput", ref Settings.ignoredPaths, 1000, new SVector2(ImGui.GetContentRegionAvail().X, Settings.IgnoredEntitiesHeight));
            ImGui.Unindent();
        }

        if (DXT.CollapsingHeader("Custom Path Icons", ref Settings.CustomIconsOpen)) {
            ImGui.Indent();
            DrawCustomPathIcons();
            ImGui.Unindent();
        }

        DrawTree();
    }

    public void Initialise() {
        // INITIALISE DEFAULT SETTINGS FOR ALL ICONS 
        foreach (var category in UITree.Categories)
            foreach (var icon in category.TreeIcons)
                Settings.SetDefaultIconSettings(icon.MapIconType, icon.DefaultSettings);
    }

}






