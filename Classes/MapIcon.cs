using ExileCore2.PoEMemory.Components;
using ExileCore2.PoEMemory.MemoryObjects;
using ExileCore2.Shared;
using ExileCore2.Shared.Enums;
using System.Numerics;

namespace MapIcons;
public class MapIcon {

    public int Version;
    public Entity Entity { get; }
    public RectangleF? DrawRect { get; set; }
    public Func<Vector2> GridPosition { get; set; }
    public MonsterRarity Rarity { get; protected set; }
    public IconPriority Priority { get; set; }
    public Func<bool> Render { get; set; }
    public Func<bool> Hidden { get; set; } = () => false;
    public Life Life => Entity?.GetComponent<Life>();
    public HudTexture InGameTexture { get; set; }
    public string Name { get; set; }
    public MapIconRenderers Renderer { get; set; } = MapIconRenderers.Default;
    public MapIconTypes Type { get; set; }
    public MapIconSettings Settings { get; set; }

    public MapIcon(Entity entity) {
        if (entity == null) throw new ArgumentNullException(nameof(entity));

        Entity = entity;
        Rarity = Entity.Rarity;
        Priority = Rarity switch {
            MonsterRarity.White => IconPriority.Low,
            MonsterRarity.Magic => IconPriority.Medium,
            MonsterRarity.Rare => IconPriority.High,
            MonsterRarity.Unique => IconPriority.VeryHigh,
            _ => IconPriority.Medium
        };
        Render = () => Entity.IsValid;
        Hidden = () => Entity.IsHidden;
        GridPosition = () => Entity.GridPos;
    }

}