namespace MapIcons;


public enum IngameIconDrawStates {
    Off = 0,
    Ranged = 1,
    Always = 2
}


public enum MapIconTypes {
    Unset,

    CustomPath,
    // Friendly Icons
    NPC,
    LocalPlayer,
    OtherPlayer,
    DecoyTotem,
    Minion,
    // Monsters
    WhiteMonster,
    MagicMonster,
    RareMonster,
    UniqueMonster,
    RogueExile,
    GiantRogueExile,
    Spirit,
    AmanamuClouded,

    // Einhar Beasts
    VividVulture,
    BlackMorrigan,
    CraicicChimeral,
    WildBristleMatron,
    WildHellionAlpha,
    FenumalPlaguedArachnid,
    FenumusFirstOfTheNight,
    // Dangerous icons 
    DrowningOrb,
    VolatileCore,
    ConsumingPhantasm,
    LightningClone,
    // minimap icons 
    Shrine,
    Breach,
    QuestObject,
    Ritual,
    Waypoint,
    Checkpoint,
    AreaTransition,
    IngameNPC,
    IngameUncategorized,
    // Delirium 
    FracturingMirror,
    BloodBag,
    EggFodder,
    GlobSpawn,
    // Chest
    BreakableObject,
    BreachChestNormal,
    BreachChestLarge,
    ExpeditionChestWhite,
    ExpeditionChestMagic,
    ExpeditionChestRare,
    SanctumChest,
    PirateChest,
    AbyssChest,
    ChestWhite,
    ChestMagic,
    ChestRare,
    ChestUnique,
    OmenAltar,

    SanctumMote,

    // Strongbox types
    UnknownStrongbox,
    ArcanistStrongbox,
    ArmourerStrongbox,
    BlacksmithStrongbox,
    ArtisanStrongbox,
    CartographerStrongbox,
    ChemistStrongbox,
    GemcutterStrongbox,
    JewellerStrongbox,
    LargeStrongbox,
    OrnateStrongbox,
    DivinerStrongbox,
    OperativeStrongbox,
    ArcaneStrongbox,
    ResearcherStrongbox,

    // Traps
    GroundSpike,

    //wildwood
    LightBmb,
    WildwoodRefuel, // ?
    
}
public enum MapIconRenderers {
    Default,
    Monster,
    Friendly,
    IngameIcon,
    Chest,
    Player,
    Trap,
}
public enum TreeIconConfigs {
    Default,
    Custom,
    IngameIcon,
    Monster,
    Chest,
    Friendly,
    Trap,
    IconOnly,
}


