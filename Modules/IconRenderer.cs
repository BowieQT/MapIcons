using DieselExileTools.ExileCore2;
using ExileCore2.PoEMemory.Components;
using ExileCore2.PoEMemory.MemoryObjects;
using ExileCore2.Shared;
using ExileCore2.Shared.Cache;
using ExileCore2.Shared.Enums;
using ExileCore2.Shared.Helpers;
using SColor = System.Drawing.Color;
using SVector2 = System.Numerics.Vector2;

namespace MapIcons;

public sealed class IconRenderer : PluginModule {
    public IconRenderer(Plugin plugin) : base(plugin) { }

    private CachedValue<List<MapIcon>> RenderedMapIconsCache;
    private const float CameraAngle = 38.7f * MathF.PI / 180;
    private static readonly float CameraAngleCos = MathF.Cos(CameraAngle);
    private static readonly float CameraAngleSin = MathF.Sin(CameraAngle);

    private const string BuffInCloud = "abyss_lightless_well_immune";
    private const string BuffInCloudAlt = "abyss_lightless_well";

    public void Initialise() {
        RenderedMapIconsCache = new TimeCache<List<MapIcon>>(() => {
            var entitySource = Settings.DrawCachedEntities
                ? GameController?.EntityListWrapper.Entities
                : GameController?.EntityListWrapper?.OnlyValidEntities;
            var baseIcons = entitySource?.Select(x => x.GetHudComponent<MapIcon>())
                .Where(icon => icon != null)
                .OrderBy(x => x.Priority)
                .ToList();
            return baseIcons ?? [];
        }, Settings.IconCacheUpdateMS);
    }
    public void Render() {
        var ingameUi = GameController.IngameState.IngameUi;
        if (ingameUi == null || !ingameUi.IsVisibleLocal) return;

        // get map state
        var mapCenter = new SVector2();
        var mapScale = 1.0f;
        bool? largeMapOpen = null;
        var smallMiniMap = ingameUi.Map.SmallMiniMap;
        var largeMapWindow = ingameUi.Map.LargeMap;
        if (smallMiniMap.IsValid && smallMiniMap.IsVisibleLocal) {
            var mapRect = smallMiniMap.GetClientRectCache;
            mapCenter = mapRect.Center;
            largeMapOpen = false;
            mapScale = smallMiniMap.MapScale;
        }
        else if (ingameUi.Map.LargeMap.IsVisibleLocal) {
            mapCenter = largeMapWindow.MapCenter;
            largeMapOpen = true;
            mapScale = largeMapWindow.MapScale;
        }

        // check game states 
        if (largeMapOpen == null || !GameController.InGame || !Settings.DrawOnMinimap && largeMapOpen != true) return;
        if (!Settings.DrawOverFullscreenPanels && ingameUi.FullscreenPanels.Any(x => x.IsVisible) || Settings.DrawOverLargePanels && ingameUi.LargePanels.Any(x => x.IsVisible)) return;
        if (largeMapWindow == null) return;

        // get player position
        var playerRender = GameController?.Player?.GetComponent<Render>();
        if (playerRender == null) return;
        var playerPos = playerRender.Pos.WorldToGrid();
        var playerHeight = -playerRender.UnclampedHeight;

        // Get the cached icons list
        if (RenderedMapIconsCache == null) return;
        var renderedMapIcons = RenderedMapIconsCache.Value;
        if (renderedMapIcons == null) return;

        // Render each icon on the map
        foreach (var mapIcon in renderedMapIcons) {

            if (mapIcon?.Entity == null) continue;
            if (mapIcon.Settings == null) {
                mapIcon.Settings = Settings.GetIconSettings(mapIcon.Type);
                if (mapIcon.Settings == null) continue;
            }
            if (!mapIcon.Render()) continue;


            if (mapIcon.Settings.Check_IsAlive && !mapIcon.Entity.IsAlive) continue;
            if (mapIcon.Settings.Check_IsOpened && mapIcon.Entity.IsOpened) continue;

            var iconFileName = Plugin.IconAtlas.Name;
            var iconSize = 0;
            var iconColor = SColor.White;
            var iconUV = new RectangleF(); // Default UV coordinates

            var iconGridPos = mapIcon.GridPosition();
            var iconDelta = iconGridPos - playerPos;
            var iconDeltaZ = (playerHeight + GameController.IngameState.Data.GetTerrainHeightAt(iconGridPos)) * PoeMapExtension.WorldToGridConversion;
            var iconPosition = mapCenter + (mapScale * SVector2.Multiply(new SVector2(iconDelta.X - iconDelta.Y, iconDeltaZ - (iconDelta.X + iconDelta.Y)), new SVector2(CameraAngleCos, CameraAngleSin)));

            // draw ingame icons
            if (mapIcon.Renderer == MapIconRenderers.IngameIcon) {

                iconFileName = mapIcon.InGameTexture.FileName;
                iconSize = (int)mapIcon.InGameTexture.Size;
                iconUV = mapIcon.InGameTexture.UV;
                mapIcon.DrawRect = GetIconPositionRect(iconSize, iconPosition, ingameUi);
                if (mapIcon.DrawRect == null) continue;
                Graphics.DrawImage(iconFileName, mapIcon.DrawRect.Value, iconUV, iconColor);
                if (mapIcon.Settings.DrawName) Graphics.DrawText(mapIcon.Name, iconPosition.Translate(0, 0), FontAlign.Center);

                continue;
            }

            // draw default/custom icons
            if (!mapIcon.Settings.Draw) continue;
            iconSize = mapIcon.Settings.Size;
            iconUV = Plugin.IconAtlas.GetIconUV(mapIcon.Settings.Index);
            mapIcon.DrawRect = GetIconPositionRect(iconSize, iconPosition, ingameUi);
            if (mapIcon.DrawRect == null) continue;
            var lifeComponent = mapIcon.Life;

            switch (mapIcon.Renderer) {
                case MapIconRenderers.Default:
                    iconColor = mapIcon.Hidden() ? mapIcon.Settings.HiddenTint : mapIcon.Settings.Tint;
                    // icon
                    Graphics.DrawImage(iconFileName, mapIcon.DrawRect.Value, iconUV, iconColor);
                    // text
                    if (mapIcon.Settings.DrawName) Graphics.DrawText(mapIcon.Name, iconPosition.Translate(0, 0), FontAlign.Center);
                break;
                case MapIconRenderers.Monster:
                    iconColor = mapIcon.Hidden() ? mapIcon.Settings.HiddenTint : mapIcon.Settings.Tint;
                    // AmanamuClouded
                    var inCloud = false;
                    if (mapIcon.Entity.TryGetComponent<Buffs>(out var buffComp))
                        inCloud = buffComp.HasBuff(BuffInCloud) || buffComp.HasBuff(BuffInCloudAlt);

                    var monsterIconUV = inCloud
                        ? Plugin.IconAtlas.GetIconUV(Settings.GetIconSettings(MapIconTypes.AmanamuClouded).Index)
                        : mapIcon.Settings.AnimateLife && lifeComponent != null
                            ? Plugin.IconAtlas.GetIconUV(GetLifeIconIndex(mapIcon.Settings.Index, lifeComponent, 8))
                            : iconUV;

                    Graphics.DrawImage(iconFileName, mapIcon.DrawRect.Value, monsterIconUV, iconColor);

                    if (mapIcon.Settings.DrawName)
                        Graphics.DrawText(mapIcon.Name, iconPosition.Translate(0, 0), FontAlign.Center);
                break;
                case MapIconRenderers.Trap:
                    var stateMachine = mapIcon.Entity.GetComponent<StateMachine>();
                    if (stateMachine == null) {
                        iconColor = mapIcon.Hidden() ? mapIcon.Settings.HiddenTint : mapIcon.Settings.Tint;
                    }
                    else {
                        bool isSpikeActive = false, isRumbleActive = false;
                        foreach (var s in stateMachine.States) {
                            if (s.Name == "spike" && s.Value == 1) isSpikeActive = true;
                            if (s.Name == "rumble" && s.Value == 1) isRumbleActive = true;
                        }
                        if (isSpikeActive) {
                            iconColor = mapIcon.Settings.Tint;
                        }
                        else if (isRumbleActive) {
                            iconColor = mapIcon.Settings.ArmingTint;
                        }
                        else {
                            iconColor = mapIcon.Settings.HiddenTint;
                        }
                    }
                    // icon
                    Graphics.DrawImage(iconFileName, mapIcon.DrawRect.Value, iconUV, iconColor);
                    // text 
                    if (mapIcon.Settings.DrawName) Graphics.DrawText(mapIcon.Name, iconPosition.Translate(0, 0), FontAlign.Center);
                break;
                case MapIconRenderers.Chest:
                    iconColor = mapIcon.Settings.Tint;
                    Graphics.DrawImage(iconFileName, mapIcon.DrawRect.Value, iconUV, iconColor);
                break;
                case MapIconRenderers.Friendly:
                    iconColor = mapIcon.Hidden() ? mapIcon.Settings.HiddenTint : mapIcon.Settings.Tint;

                    // icon
                    if (mapIcon.Settings.AnimateLife && lifeComponent != null && lifeComponent.HPPercentage < 0.875f) {
                        // use switch to get life percentages in 12.5% increments
                        var iconIndex = mapIcon.Settings.Index;
                        switch (lifeComponent.HPPercentage) {
                            case > 0.75f: iconIndex += 1; break;
                            case > 0.625f: iconIndex += 2; break;
                            case > 0.5f: iconIndex += 3; break;
                            case > 0.375f: iconIndex += 4; break;
                            case > 0.25f: iconIndex += 5; break;
                            case > 0.125f: iconIndex += 6; break;
                            default: iconIndex += 7; break; // Handles 12.5% and below
                        }
                        iconUV = Plugin.IconAtlas.GetIconUV(iconIndex);
                        Graphics.DrawImage(iconFileName, mapIcon.DrawRect.Value, iconUV, iconColor);
                    }
                    else
                        Graphics.DrawImage(iconFileName, mapIcon.DrawRect.Value, iconUV, iconColor);
                    // text 
                    string name = mapIcon.Settings.DrawName ? mapIcon.Name : null;
                    string lifeText = null;
                    if (mapIcon.Settings.DrawHealth) {
                        if (lifeComponent != null) {
                            lifeText = $"{lifeComponent.CurHP}";
                        }
                    }
                    if (!string.IsNullOrEmpty(name) && !string.IsNullOrEmpty(lifeText)) {
                        Graphics.DrawText($"{name} {lifeText}", iconPosition.Translate(0, 0), FontAlign.Center);
                    }
                    else if (!string.IsNullOrEmpty(name)) {
                        Graphics.DrawText(name, iconPosition.Translate(0, 0), FontAlign.Center);
                    }
                    else if (!string.IsNullOrEmpty(lifeText)) {
                        Graphics.DrawText(lifeText, iconPosition.Translate(0, 0), FontAlign.Center);
                    }
                break;
                default:
                break;
            }
        }
    }

    public int GetLifeIconIndex(int baseIconIndex, Life lifeComponent, int steps) {
        float hp = Math.Clamp(lifeComponent.HPPercentage, 0f, 1f);
        int offset = (int)((1f - hp) * steps);
        offset = Math.Clamp(offset, 0, steps - 1);
        return baseIconIndex + offset;
    }
    public RectangleF? GetIconPositionRect(int iconSize, SVector2 iconPosition, IngameUIElements inGameUI) {
        float halfSize = iconSize / 2;
        float iconX = iconPosition.X - halfSize;
        float iconY = iconPosition.Y - halfSize;
        if (Settings.PixelPerfectIcons) {
            iconX = MathF.Round(iconX);
            iconY = MathF.Round(iconY);
        }

        var rect = new RectangleF(iconX, iconY, iconSize, iconSize);

        if (inGameUI.Map.LargeMap.IsVisibleLocal == false && !inGameUI.Map.SmallMiniMap.GetClientRectCache.Contains(rect)) return null;

        return rect;
    }
}





