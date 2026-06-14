using DieselExileTools.ExileCore2;
using ExileCore2;
using ImGuiNET;
using System.Numerics;

namespace MapIcons;

public class Plugin : BaseSettingsPlugin<Settings> {

    //--| Properties |-------------------------------------------------------------------------------------------------
    private DXT.IconAtlas _iconAtlas;
    public DXT.IconAtlas IconAtlas => _iconAtlas ??= new(Graphics, "Diesel_MapIcons", Path.Combine(Path.GetDirectoryName(typeof(Plugin).Assembly.Location), "media", "MapIcons.png"), new Vector2(32, 32));


    private IconBuilder _iconBuilder;
    public IconBuilder IconBuilder => _iconBuilder ??= new IconBuilder(this);

    private IconRenderer _iconRenderer;
    private IconRenderer IconRenderer => _iconRenderer ??= new IconRenderer(this);

    private UserInterface _userInterface;
    private UserInterface UserInterface => _userInterface ??= new UserInterface(this);

    //--| Initialise |--------------------------------------------------------------------------------------------------
    public override bool Initialise() {
        CanUseMultiThreading = true;
        Initialise_DXT();

        IconBuilder.Initialise();
        IconRenderer.Initialise();
        UserInterface.Initialise();

        return base.Initialise();
    }
    private void Initialise_DXT()
    {

        DXT.Initialise(new DXT.Config
        {
            PluginName = Name,
            PluginDirectory = DirectoryFullName,
            GameController = GameController,
            Graphics = Graphics,
            Settings = Settings.DXT,
        });

        DBug.LogHeader = (width, height) => {
            DXT.Button.Draw($"{Name}Friendly", ref Settings.DebugFriendlyIcon, new DXT.Button.Options { Label = "Friendly", Width = 80, Height = 22 }); ImGui.SameLine();
            DXT.Button.Draw($"{Name}Monster", ref Settings.DebugMonsterIcon, new DXT.Button.Options { Label = "Monster", Width = 80, Height = 22 }); ImGui.SameLine();
            DXT.Button.Draw($"{Name}Chest", ref Settings.DebugChestIcon, new DXT.Button.Options { Label = "Chest", Width = 80, Height = 22 }); ImGui.SameLine();
            DXT.Button.Draw($"{Name}Ingame", ref Settings.DebugMinimapIcon, new DXT.Button.Options { Label = "Ingame", Width = 80, Height = 22 }); ImGui.SameLine();
            DXT.Button.Draw($"{Name}Misc", ref Settings.DebugMiscIcon, new DXT.Button.Options { Label = "Misc", Width = 80, Height = 22 }); ImGui.SameLine();
            DXT.Button.Draw($"{Name}User", ref Settings.DebugUser, new DXT.Button.Options { Label = "User", Width = 80, Height = 22 }); ImGui.SameLine();
            if (DXT.Button.Draw($"{Name}RebuildIcons", new DXT.Button.Options { Label = "Rebuild", Width = 80, Height = 22, Tooltip = DXT.Tooltip.BasicOptions("Rebuild Icons") }))
            {
                IconBuilder.RebuildIcons();
            }
            //DBug.Monitor("IMGUI", "GetFont", ImGui.GetFont().GetDebugName(), false);


        };
    }



    //--| Draw Settings |-----------------------------------------------------------------------------------------------
    public override void DrawSettings() {
        UserInterface.Draw();
    }

    //--| Tick |-------------------------------------------------------------------------------------------------------
    public override void Tick() {
        IconBuilder.Tick();
    }

    //--| Render |-----------------------------------------------------------------------------------------------------
    public override void Render() {
        IconRenderer.Render();
        DBug.Render();
    }



}
