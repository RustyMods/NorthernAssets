using System.Collections.Generic;
using Managers;
using NorthernAssets.Behaviors;
using NorthernAssets.Managers;
using PieceManager;
using UnityEngine;
using CraftingTable = PieceManager.CraftingTable;

namespace NorthernAssets.prefabs;

public static class BuildPieces
{
    private static readonly List<string> AnimatedChests = new();
    private static readonly List<string> DoubleDoors = new();
    
    public static void Load()
    {
        LoadDungeonPieces();
        LoadShinglePieces();
        LoadMiscPieces();
        LoadBarrels();
        LoadTables();
        LoadCupboards();
        LoadBenches();
        LoadChairs();
        LoadBooks();
        LoadBottles();
        LoadFlasks();
        LoadPotions();
        LoadGlassJars();
        LoadJugs();
        LoadPapers();
        LoadPodiums();
        LoadOther();
        LoadStations();
        LoadStainedGlass();
        LoadBanners();
        LoadCoatOfArms();
        LoadLights();
        LoadDoors();
        LoadChests();
        LoadBahometBossStone();
    }
    
    public static void AddAnimatedChestComponent(Piece __instance)
    {
        if (AnimatedChests.Contains(Helpers.GetNormalizedName(__instance.name)) && !__instance.GetComponent<AnimatedChest>())
        {
            __instance.gameObject.AddComponent<AnimatedChest>();
        }
    }

    public static void AddDoubleDoorComponent(Piece __instance)
    {
        if (DoubleDoors.Contains(Helpers.GetNormalizedName(__instance.name)) && !__instance.GetComponent<DoubleDoor>())
        {
            __instance.gameObject.AddComponent<DoubleDoor>();
        }
    }

    public static bool IsDungeonPiece(string name) => DungeonPieces.ContainsKey(Helpers.GetNormalizedName(name));

    private static readonly Dictionary<string, string> DungeonPieces = new()
    {
        ["RS_Arch_A"] = "Archway Pillar",
        ["RS_Arch_A1"] = "Archway Pillar Large",
        ["RS_Arch_B"] = "Archway B",
        ["RS_Arch_B1"] = "Archway B 1",
        ["RS_Arch_C"] = "Archway B",
        ["RS_Floor_A"] = "Floor A 2x2",
        ["RS_Floor_B"] = "Floor B 2x2",
        ["RS_Handrail"] = "Handrail",
        ["RS_Handrail_Cap"] = "Handrail Cap",
        ["RS_Pillar_A"] = "Pillar A",
        ["RS_Pillar_B"] = "Pillar B",
        ["RS_Wall_A"] = "Wall A 2x2",
        ["RS_Wall_B"] = "Wall B 2x2",
        ["RS_Wall_A_Short"] = "Wall A 1x2",
        ["RS_Wall_B_Short"] = "Wall B 1x2",
        ["RS_Stairs"] = "Stairs 2x2",
        ["RS_Arch_D"] = "Arch A Thick",
        ["RS_Floor_C"] = "Floor A 1x1",
        ["RS_Floor_D"] = "Floor B 1x1",
        ["RS_Wall_C"] = "Wall A Thin",
        ["RS_Arch_F"] = "Archway",
        ["RS_Stair_1x1"] = "Stairs 1x1",
        ["RS_StoneArch"] = "Arch",
        ["RS_StonePillar"] = "Stone Pillar",
        ["RS_WoodPillar"] = "Wood Pillar"
    };

    private static void LoadDungeonPieces()
    {
        foreach (var kvp in DungeonPieces)
        {
            BuildPiece piece = new BuildPiece(NorthernAssetsPlugin._Assets, kvp.Key);
            piece.Name.English(kvp.Value);
            piece.Description.English("");
            piece.Category.Set("Rusty Pieces");
            piece.RequiredItems.Add("Stone", 10, true);
            piece.PlaceEffects.Add("vfx_Place_stone_wall_2x1");
            piece.PlaceEffects.Add("sfx_build_hammer_stone");
            piece.DestroyedEffects.Add("vfx_RockDestroyed");
            piece.DestroyedEffects.Add("sfx_rock_destroyed");
            piece.HitEffects.Add("vfx_RockHit");
            piece.HitEffects.Add("sfx_rock_hit");
            piece.Crafting.Set("RS_AlchemyTable");
        }

        BuildPiece wallDoor = new(NorthernAssetsPlugin._Assets, "RS_WallDoor_1");
        wallDoor.Name.English("Wall Door");
        wallDoor.Description.English("");
        wallDoor.Category.Set("Rusty Pieces");
        wallDoor.RequiredItems.Add("DeadWood_RS", 10, true);
        wallDoor.RequiredItems.Add("IronNails", 4, true);
        wallDoor.PlaceEffects.Add("vfx_Place_stone_wall_2x1");
        wallDoor.PlaceEffects.Add("sfx_build_hammer_stone");
        wallDoor.DestroyedEffects.Add("vfx_RockDestroyed");
        wallDoor.DestroyedEffects.Add("sfx_rock_destroyed");
        wallDoor.HitEffects.Add("vfx_RockHit");
        wallDoor.HitEffects.Add("sfx_rock_hit");

        BuildPiece wallWindow = new(NorthernAssetsPlugin._Assets, "RS_WindowWall_1");
        wallWindow.Name.English("Window 1");
        wallWindow.Description.English("");
        wallWindow.Category.Set("Rusty Pieces");
        wallWindow.RequiredItems.Add("Stone", 10, true);
        wallWindow.RequiredItems.Add("Iron", 4, true);
        wallWindow.PlaceEffects.Add("vfx_Place_stone_wall_2x1");
        wallWindow.PlaceEffects.Add("sfx_build_hammer_stone");
        wallWindow.DestroyedEffects.Add("vfx_RockDestroyed");
        wallWindow.DestroyedEffects.Add("sfx_rock_destroyed");
        wallWindow.HitEffects.Add("vfx_RockHit");
        wallWindow.HitEffects.Add("sfx_rock_hit");
        DungeonPieces[wallWindow.Prefab.name] = "Window 1";

        BuildPiece wallWindow2 = new(NorthernAssetsPlugin._Assets, "RS_WindowWall_2");
        wallWindow2.Name.English("Window 2");
        wallWindow2.Description.English("");
        wallWindow2.Category.Set("Rusty Pieces");
        wallWindow2.RequiredItems.Add("Stone", 10, true);
        wallWindow2.RequiredItems.Add("Iron", 1, true);
        wallWindow2.PlaceEffects.Add("vfx_Place_stone_wall_2x1");
        wallWindow2.PlaceEffects.Add("sfx_build_hammer_stone");
        wallWindow2.DestroyedEffects.Add("vfx_RockDestroyed");
        wallWindow2.DestroyedEffects.Add("sfx_rock_destroyed");
        wallWindow2.HitEffects.Add("vfx_RockHit");
        wallWindow2.HitEffects.Add("sfx_rock_hit");
        DungeonPieces[wallWindow2.Prefab.name] = "Window 2";
        
        LoadGroundPieces();
        LoadStairs();
    }

    private static void LoadBahometBossStone()
    {
        BuildPiece bossStone = new BuildPiece(NorthernAssetsPlugin._Assets, "BossStone_Bahomet_RS");
        bossStone.Name.English("Boss Stone Bahomet");
        bossStone.Description.English("");
        bossStone.Category.Set(BuildPieceCategory.Misc);
        bossStone.RequiredItems.Add("Platinum_RS", 10, true);
        bossStone.PlaceEffects.Add("vfx_Place_stone_wall_2x1");
        bossStone.PlaceEffects.Add("sfx_build_hammer_stone");
        bossStone.Crafting.Set("RS_AlchemyTable");
        bossStone.CloneBossStoneEffectsFrom = "BossStone_Bonemass";
        SE_Stats BahometGP = ScriptableObject.CreateInstance<SE_Stats>();
        BahometGP.name = "GP_Bahomet";
        BahometGP.m_name = "$enemy_bahomet";
        BahometGP.m_repeatInterval = 60f;
        BahometGP.m_ttl = 300f;
        BahometGP.m_cooldown = 1200f;
        BahometGP.m_activationAnimation = "gpower";
        BahometGP.m_startMessage = "$se_bahomet_start";
        BahometGP.m_startMessageType = MessageHud.MessageType.Center;
        BahometGP.m_tooltip = "$se_bahomet_tooltip";
        BahometGP.m_mods = new List<HitData.DamageModPair>()
        {
            new HitData.DamageModPair()
            {
                m_type = HitData.DamageType.Fire,
                m_modifier = HitData.DamageModifier.Immune
            },
            new HitData.DamageModPair()
            {
                m_type = HitData.DamageType.Spirit,
                m_modifier = HitData.DamageModifier.Immune
            }
        };
        BahometGP.m_eitrRegenMultiplier = 1.1f;
        BahometGP.m_healthRegenMultiplier = 1.1f;
        BahometGP.m_staminaRegenMultiplier = 1.1f;
        bossStone.GuardianPower = BahometGP;
        bossStone.BossTrophy = "TrophyBahomet_RS";
    }

    private static void LoadGroundPieces()
    {
        for (int index = 1; index < 26; ++index)
        {
            var prefabName = $"RS_Ground_{index}";
            var name = $"Ground {index}";
            BuildPiece piece = new BuildPiece(NorthernAssetsPlugin._Assets, prefabName);
            piece.Name.English(name);
            piece.Description.English("");
            piece.Category.Set("Floor Tiles");
            piece.RequiredItems.Add("Stone", 10, true);
            piece.PlaceEffects.Add("vfx_Place_stone_wall_2x1");
            piece.PlaceEffects.Add("sfx_build_hammer_stone");
            piece.DestroyedEffects.Add("vfx_RockDestroyed");
            piece.DestroyedEffects.Add("sfx_rock_destroyed");
            piece.HitEffects.Add("vfx_RockHit");
            piece.HitEffects.Add("sfx_rock_hit");
            piece.Crafting.Set("RS_AlchemyTable");
            DungeonPieces[prefabName] = name;
        }
    }

    private static void LoadStairs()
    {
        for (int index = 1; index < 13; ++index)
        {
            var prefabName = $"RS_Stair_{index}";
            var name = $"Stair {index}";
            BuildPiece piece = new BuildPiece(NorthernAssetsPlugin._Assets, prefabName);
            piece.Name.English(name);
            piece.Description.English("");
            piece.Category.Set("Rusty Stairs");
            piece.RequiredItems.Add("Stone", 10, true);
            piece.PlaceEffects.Add("vfx_Place_stone_wall_2x1");
            piece.PlaceEffects.Add("sfx_build_hammer_stone");
            piece.DestroyedEffects.Add("vfx_RockDestroyed");
            piece.DestroyedEffects.Add("sfx_rock_destroyed");
            piece.HitEffects.Add("vfx_RockHit");
            piece.HitEffects.Add("sfx_rock_hit");
            piece.Crafting.Set("RS_AlchemyTable");
            DungeonPieces[prefabName] = name;
        }
    }

    private static void LoadShinglePieces()
    {
        List<string> ShingleRoofPieces = new()
        {
            "shingled_roof_26", "shingled_roof_45", "shingled_roof_45_peak",
            "shingled_roof_icorner_26", "shingled_roof_icorner_45", "shingled_roof_ocorner_26",
            "shingled_roof_ocorner_45", "shingled_roof_peak_26", "shingled_roof_t_26",
            "shingled_roof_t_45", "shingled_roof_top_26", "shingled_roof_top_45"
        };
        
        foreach (var prefabName in ShingleRoofPieces)
        {
            BuildPiece piece = new BuildPiece(NorthernAssetsPlugin._Assets, prefabName);
            piece.Name.English(ConvertPrefabNameToFriendlyName(prefabName));
            piece.Description.English("");
            piece.Category.Set("Rusty Pieces");
            piece.Crafting.Set("RS_AlchemyTable");
            piece.RequiredItems.Add("Stone", 10, true);
            piece.RequiredItems.Add("DeadWood_RS", 5, true);
            piece.PlaceEffects.Add("vfx_Place_stone_wall_2x1");
            piece.PlaceEffects.Add("sfx_build_hammer_stone");
            piece.DestroyedEffects.Add("vfx_RockDestroyed");
            piece.DestroyedEffects.Add("sfx_rock_destroyed");
            piece.HitEffects.Add("vfx_RockHit");
            piece.HitEffects.Add("sfx_rock_hit");
        }
    }

    private static void LoadJugs()
    {
        for (int index = 1; index < 4; ++index)
        {
            var prefabName = $"RS_Jug_{index}";
            var name = $"Vase {index}";
            BuildPiece Barrel1 = new(NorthernAssetsPlugin._Assets, prefabName);
            Barrel1.Name.English(name);
            Barrel1.Description.English("");
            Barrel1.Category.Set("Rusty Props");
            Barrel1.Crafting.Set("RS_AlchemyTable");
            Barrel1.RequiredItems.Add("Stone", 5, true);
            Barrel1.PlaceEffects.Add("vfx_Place_wood_floor");
            Barrel1.PlaceEffects.Add("sfx_build_hammer_wood");
            Barrel1.DestroyedEffects.Add("sfx_wood_destroyed");
            Barrel1.DestroyedEffects.Add("vfx_SawDust");
            Barrel1.HitEffects.Add("vfx_SawDust");
        }
    }

    private static void LoadMiscPieces()
    {
        BuildPiece BrokenBridge = new BuildPiece(NorthernAssetsPlugin._Assets, "RS_Broken_Bridge");
        BrokenBridge.Name.English("Broken Bridge");
        BrokenBridge.Description.English("");
        BrokenBridge.Category.Set("Rusty Pieces");
        BrokenBridge.Crafting.Set("RS_AlchemyTable");
        BrokenBridge.RequiredItems.Add("Stone", 10, true);
        BrokenBridge.PlaceEffects.Add("vfx_Place_stone_wall_2x1");
        BrokenBridge.PlaceEffects.Add("sfx_build_hammer_stone");
        BrokenBridge.DestroyedEffects.Add("vfx_RockDestroyed");
        BrokenBridge.DestroyedEffects.Add("sfx_rock_destroyed");
        BrokenBridge.HitEffects.Add("vfx_RockHit");
        BrokenBridge.HitEffects.Add("sfx_rock_hit");
        DungeonPieces["RS_Broken_Bridge"] = "Broken Bridge";
        
        BuildPiece BrokenBridgeB = new BuildPiece(NorthernAssetsPlugin._Assets, "RS_Broken_Bridge_B");
        BrokenBridgeB.Name.English("Bridge");
        BrokenBridgeB.Description.English("");
        BrokenBridgeB.Category.Set("Rusty Pieces");
        BrokenBridgeB.Crafting.Set("RS_AlchemyTable");
        BrokenBridgeB.RequiredItems.Add("Stone", 10, true);
        BrokenBridgeB.PlaceEffects.Add("vfx_Place_stone_wall_2x1");
        BrokenBridgeB.PlaceEffects.Add("sfx_build_hammer_stone");
        BrokenBridgeB.DestroyedEffects.Add("vfx_RockDestroyed");
        BrokenBridgeB.DestroyedEffects.Add("sfx_rock_destroyed");
        BrokenBridgeB.HitEffects.Add("vfx_RockHit");
        BrokenBridgeB.HitEffects.Add("sfx_rock_hit");
        DungeonPieces["RS_Broken_Bridge_B"] = "Bridge";
        
        BuildPiece SwordPiece = new BuildPiece(NorthernAssetsPlugin._Assets, "RS_Sword_Decor");
        SwordPiece.Name.English("Ancient Sword");
        SwordPiece.Description.English("");
        SwordPiece.Category.Set("Rusty Props");
        SwordPiece.Crafting.Set("RS_AlchemyTable");
        SwordPiece.RequiredItems.Add("Stone", 10, true);
        SwordPiece.PlaceEffects.Add("vfx_Place_stone_wall_2x1");
        SwordPiece.PlaceEffects.Add("sfx_build_hammer_stone");
        SwordPiece.DestroyedEffects.Add("vfx_RockDestroyed");
        SwordPiece.DestroyedEffects.Add("sfx_rock_destroyed");
        SwordPiece.HitEffects.Add("vfx_RockHit");
        SwordPiece.HitEffects.Add("sfx_rock_hit");
        SwordPiece.Prefab.AddComponent<ToggleLight>();
        DungeonPieces["RS_Sword_Decor"] = "Ancient Sword";
    }

    private static void LoadBarrels()
    {
        for (int index = 1; index < 7; ++index)
        {
            var prefabName = $"RS_Barrel_{index}";
            var name = $"Barrel {index}";
            BuildPiece Barrel1 = new(NorthernAssetsPlugin._Assets, prefabName);
            Barrel1.Name.English(name);
            Barrel1.Description.English("");
            Barrel1.Category.Set("Rusty Props");
            Barrel1.Crafting.Set("RS_AlchemyTable");
            Barrel1.RequiredItems.Add("DeadWood_RS", 5, true);
            Barrel1.PlaceEffects.Add("vfx_Place_wood_floor");
            Barrel1.PlaceEffects.Add("sfx_build_hammer_wood");
            Barrel1.DestroyedEffects.Add("sfx_wood_destroyed");
            Barrel1.DestroyedEffects.Add("vfx_SawDust");
            Barrel1.HitEffects.Add("vfx_SawDust");
        }
    }

    private static void LoadPapers()
    {
        for (int index = 1; index < 4; ++index)
        {
            var prefabName = $"RS_Paper_{index}";
            var name = $"Paper {index}";
            BuildPiece Barrel1 = new(NorthernAssetsPlugin._Assets, prefabName);
            Barrel1.Name.English(name);
            Barrel1.Description.English("");
            Barrel1.Category.Set("Rusty Props");
            Barrel1.Crafting.Set("RS_AlchemyTable");
            Barrel1.RequiredItems.Add("DeadWood_RS", 1, true);
            Barrel1.PlaceEffects.Add("vfx_Place_wood_floor");
            Barrel1.PlaceEffects.Add("sfx_build_hammer_wood");
            Barrel1.DestroyedEffects.Add("sfx_wood_destroyed");
            Barrel1.DestroyedEffects.Add("vfx_SawDust");
            Barrel1.HitEffects.Add("vfx_SawDust");
        }
    }

    private static void LoadTables()
    {
        for (int index = 1; index < 3; ++index)
        {
            var prefabName = $"RS_Table_{index}";
            var name = $"Table {index}";
            BuildPiece piece = new(NorthernAssetsPlugin._Assets, prefabName);
            piece.Name.English(name);
            piece.Description.English("");
            piece.Category.Set("Rusty Pieces");
            piece.Crafting.Set("RS_AlchemyTable");
            piece.RequiredItems.Add("FineWood", 10, true);
            piece.RequiredItems.Add("BronzeNails", 8, true);
            piece.PlaceEffects.Add("vfx_Place_wood_floor");
            piece.PlaceEffects.Add("sfx_build_hammer_wood");
            piece.DestroyedEffects.Add("sfx_wood_destroyed");
            piece.DestroyedEffects.Add("vfx_SawDust");
            piece.HitEffects.Add("vfx_SawDust");
        }
    }

    private static void LoadCupboards()
    {
        for (int index = 1; index < 12; ++index)
        {
            var prefabName = $"RS_Cupboard_{index}";
            var name = $"Cupboard {index}";
            BuildPiece piece = new(NorthernAssetsPlugin._Assets, prefabName);
            piece.Name.English(name);
            piece.Description.English("");
            piece.Category.Set("Rusty Pieces");
            piece.Crafting.Set("RS_AlchemyTable");
            piece.RequiredItems.Add("DeadWood_RS", 10, true);
            piece.RequiredItems.Add("BronzeNails", 8, true);
            piece.PlaceEffects.Add("vfx_Place_wood_floor");
            piece.PlaceEffects.Add("sfx_build_hammer_wood");
            piece.DestroyedEffects.Add("sfx_wood_destroyed");
            piece.DestroyedEffects.Add("vfx_SawDust");
            piece.HitEffects.Add("vfx_SawDust");
        }
    }

    private static void LoadBenches()
    {
        for (int index = 1; index < 3; ++index)
        {
            var prefabName = $"RS_Bench_{index}";
            var name = $"Bench {index}";
            BuildPiece piece = new(NorthernAssetsPlugin._Assets, prefabName);
            piece.Name.English(name);
            piece.Description.English("");
            piece.Category.Set("Rusty Pieces");
            piece.Crafting.Set("RS_AlchemyTable");
            piece.RequiredItems.Add("DeadWood_RS", 10, true);
            piece.RequiredItems.Add("BronzeNails", 4, true);
            piece.PlaceEffects.Add("vfx_Place_wood_floor");
            piece.PlaceEffects.Add("sfx_build_hammer_wood");
            piece.DestroyedEffects.Add("sfx_wood_destroyed");
            piece.DestroyedEffects.Add("vfx_SawDust");
            piece.HitEffects.Add("vfx_SawDust");
        }
    }

    private static void LoadChairs()
    {
        for (int index = 1; index < 3; ++index)
        {
            var prefabName = $"RS_Chair_{index}";
            var name = $"Chair {index}";
            BuildPiece piece = new(NorthernAssetsPlugin._Assets, prefabName);
            piece.Name.English(name);
            piece.Description.English("");
            piece.Category.Set("Rusty Pieces");
            piece.Crafting.Set("RS_AlchemyTable");
            piece.RequiredItems.Add("DeadWood_RS", 10, true);
            piece.RequiredItems.Add("BronzeNails", 4, true);
            piece.PlaceEffects.Add("vfx_Place_wood_floor");
            piece.PlaceEffects.Add("sfx_build_hammer_wood");
            piece.DestroyedEffects.Add("sfx_wood_destroyed");
            piece.DestroyedEffects.Add("vfx_SawDust");
            piece.HitEffects.Add("vfx_SawDust");
        }
    }

    private static void LoadCandles()
    {
        for (int index = 1; index < 10; ++index)
        {
            var prefabName = $"RS_Candle_{index}";
            var name = $"Candle {index}";
            BuildPiece piece = new BuildPiece(NorthernAssetsPlugin._Assets, prefabName);
            piece.Name.English(name);
            piece.Description.English("");
            piece.Category.Set("Rusty Lighting");
            piece.Crafting.Set("RS_AlchemyTable");
            piece.RequiredItems.Add("Honey", 10, true);
            piece.PlaceEffects.Add("vfx_Place_stone_wall_2x1");
            piece.PlaceEffects.Add("sfx_build_hammer_metal");
            piece.DestroyedEffects.Add("vfx_RockDestroyed");
            piece.DestroyedEffects.Add("vfx_SawDust");
            piece.HitEffects.Add("vfx_RockHit");
            piece.HitEffects.Add("vfx_HitSparks");
            piece.Prefab.AddComponent<ToggleLight>();
            piece.Prefab.AddComponent<BrightnessControl>();
        }
    }

    private static void LoadCandlesticks()
    {
        for (int index = 1; index < 3; ++index)
        {
            var prefabName = $"RS_Candlestick_Floor_{index}";
            var name = $"Candlestick Floor {index}";
            BuildPiece piece = new BuildPiece(NorthernAssetsPlugin._Assets, prefabName);
            piece.Name.English(name);
            piece.Description.English("");
            piece.Category.Set("Rusty Lighting");
            piece.Crafting.Set("RS_AlchemyTable");
            piece.RequiredItems.Add("Honey", 10, true);
            piece.RequiredItems.Add("Bronze", 2, true);
            piece.PlaceEffects.Add("vfx_Place_stone_wall_2x1");
            piece.PlaceEffects.Add("sfx_build_hammer_metal");
            piece.DestroyedEffects.Add("vfx_RockDestroyed");
            piece.DestroyedEffects.Add("vfx_SawDust");
            piece.HitEffects.Add("vfx_RockHit");
            piece.HitEffects.Add("vfx_HitSparks");
            piece.Prefab.AddComponent<ToggleLight>();
        }
    }

    private static void LoadBooks()
    {
        for (int index = 1; index < 13; ++index)
        {
            var prefabName = $"RS_Book_{index}";
            var name = $"Book {index}";
            BuildPiece piece = new(NorthernAssetsPlugin._Assets, prefabName);
            piece.Name.English(name);
            piece.Description.English("");
            piece.Category.Set("Rusty Props");
            piece.Crafting.Set("RS_AlchemyTable");
            piece.RequiredItems.Add("DeadWood_RS", 1, true);
            piece.PlaceEffects.Add("vfx_Place_wood_floor");
            piece.PlaceEffects.Add("sfx_build_hammer_wood");
            piece.DestroyedEffects.Add("sfx_wood_destroyed");
            piece.DestroyedEffects.Add("vfx_SawDust");
            piece.HitEffects.Add("vfx_SawDust");
            piece.Prefab.AddComponent<Book>();
        }
    }

    private static void LoadBottles()
    {
        for (int index = 1; index < 4; ++index)
        {
            var prefabName = $"RS_Bottle_{index}";
            var name = $"Bottle {index}";
            BuildPiece piece = new(NorthernAssetsPlugin._Assets, prefabName);
            piece.Name.English(name);
            piece.Description.English("");
            piece.Category.Set("Rusty Props");
            piece.Crafting.Set("RS_AlchemyTable");
            piece.RequiredItems.Add("Crystal", 1, true);
            piece.PlaceEffects.Add("vfx_Place_wood_pole");
            piece.PlaceEffects.Add("sfx_build_hammer_crystal");
            piece.DestroyedEffects.Add("fx_crystal_destruction");
        }
    }

    private static void LoadFlasks()
    {
        for (int index = 1; index < 6; ++index)
        {
            var prefabName = $"RS_Flask_{index}";
            var name = $"Flask {index}";
            BuildPiece piece = new(NorthernAssetsPlugin._Assets, prefabName);
            piece.Name.English(name);
            piece.Description.English("");
            piece.Category.Set("Rusty Props");
            piece.Crafting.Set("RS_AlchemyTable");
            piece.RequiredItems.Add("Crystal", 1, true);
            piece.RequiredItems.Add("Bronze", 1, true);
            piece.PlaceEffects.Add("vfx_Place_wood_pole");
            piece.PlaceEffects.Add("sfx_build_hammer_crystal");
            piece.DestroyedEffects.Add("fx_crystal_destruction");
        }
    }

    private static void LoadPotions()
    {
        for (int index = 1; index < 7; ++index)
        {
            var prefabName = $"RS_Potion_{index}";
            var name = $"Potion {index}";
            BuildPiece piece = new(NorthernAssetsPlugin._Assets, prefabName);
            piece.Name.English(name);
            piece.Description.English("");
            piece.Category.Set("Rusty Props");
            piece.Crafting.Set("RS_AlchemyTable");
            piece.RequiredItems.Add("Crystal", 1, true);
            piece.RequiredItems.Add("Bronze", 1, true);
            piece.PlaceEffects.Add("vfx_Place_wood_pole");
            piece.PlaceEffects.Add("sfx_build_hammer_crystal");
            piece.DestroyedEffects.Add("fx_crystal_destruction");
        }
    }

    private static void LoadGlassJars()
    {
        for (int index = 1; index < 3; ++index)
        {
            var prefabName = $"RS_GlassJar_{index}";
            var name = $"Glass Jar {index}";
            BuildPiece piece = new(NorthernAssetsPlugin._Assets, prefabName);
            piece.Name.English(name);
            piece.Description.English("");
            piece.Category.Set("Rusty Props");
            piece.Crafting.Set("RS_AlchemyTable");
            piece.RequiredItems.Add("Crystal", 1, true);
            piece.RequiredItems.Add("Bronze", 1, true);
            piece.PlaceEffects.Add("vfx_Place_wood_pole");
            piece.PlaceEffects.Add("sfx_build_hammer_crystal");
            piece.DestroyedEffects.Add("fx_crystal_destruction");
        }
    }

    private static void LoadPodiums()
    {
        BuildPiece podium1 = new(NorthernAssetsPlugin._Assets, "RS_Podium_1");
        podium1.Name.English("Empty Podium");
        podium1.Description.English("");
        podium1.Category.Set("Rusty Props");
        podium1.Crafting.Set("RS_AlchemyTable");
        podium1.RequiredItems.Add("DeadWood_RS", 10, true);
        podium1.RequiredItems.Add("BronzeNails", 5, true);
        podium1.PlaceEffects.Add("vfx_Place_wood_floor");
        podium1.PlaceEffects.Add("sfx_build_hammer_wood");
        podium1.DestroyedEffects.Add("sfx_wood_destroyed");
        podium1.DestroyedEffects.Add("vfx_SawDust");
        podium1.HitEffects.Add("vfx_SawDust");
        
        BuildPiece podium2 = new(NorthernAssetsPlugin._Assets, "RS_Podium_2");
        podium2.Name.English("Podium");
        podium2.Description.English("");
        podium2.Category.Set("Rusty Props");
        podium2.Crafting.Set("RS_AlchemyTable");
        podium2.RequiredItems.Add("DeadWood_RS", 10, true);
        podium2.RequiredItems.Add("Honey", 10, true);
        podium2.RequiredItems.Add("BronzeNails", 5, true);
        podium2.PlaceEffects.Add("vfx_Place_wood_floor");
        podium2.PlaceEffects.Add("sfx_build_hammer_wood");
        podium2.DestroyedEffects.Add("sfx_wood_destroyed");
        podium2.DestroyedEffects.Add("vfx_SawDust");
        podium2.HitEffects.Add("vfx_SawDust");
        podium2.Prefab.AddComponent<ToggleLight>();
    }
    
    private static void LoadChests()
    {
        BuildPiece chest = new(NorthernAssetsPlugin._Assets, "RS_Chest_1");
        chest.Name.English("Private Chest");
        chest.Description.English("");
        chest.Category.Set("Rusty Props");
        chest.Crafting.Set("RS_AlchemyTable");
        chest.RequiredItems.Add("DeadWood_RS", 10, true);
        chest.RequiredItems.Add("Iron", 8, true);
        chest.RequiredItems.Add("IronNails", 10, true);
        chest.PlaceEffects.Add("vfx_Place_wood_wall");
        chest.PlaceEffects.Add("sfx_build_hammer_metal");
        chest.DestroyedEffects.Add("sfx_metal_blocked");
        chest.DestroyedEffects.Add("vfx_HitSparks");
        chest.HitEffects.Add("vfx_SawDust");
        
        BuildPiece AnimatedChest = new BuildPiece(NorthernAssetsPlugin._Assets, "RS_ChestAnimated");
        AnimatedChest.Name.English("Live Chest");
        AnimatedChest.Prefab.AddComponent<AnimatedChest>();
        AnimatedChest.Crafting.Set("RS_AlchemyTable");
        AnimatedChest.Category.Set("Rusty Props");
        AnimatedChest.PlaceEffects.Add("vfx_Place_chest");
        AnimatedChest.PlaceEffects.Add("sfx_build_hammer_wood");
        AnimatedChest.OpenEffects.Add("sfx_chest_open");
        AnimatedChest.CloseEffects.Add("sfx_chest_close");
        AnimatedChest.DestroyedEffects.Add("sfx_metal_blocked");
        AnimatedChest.DestroyedEffects.Add("vfx_HitSparks");
        AnimatedChests.Add(AnimatedChest.Prefab.name);
        
        FloraManager.RegisterAsset(NorthernAssetsPlugin._Assets, "vfx_chest_explosion");
        
        BuildPiece AnimatedChest1 = new BuildPiece(NorthernAssetsPlugin._Assets, "RS_ChestAnimated1");
        AnimatedChest1.Name.English("Treasure Chest");
        AnimatedChest1.Prefab.AddComponent<AnimatedChest>();
        AnimatedChest1.Crafting.Set("RS_AlchemyTable");
        AnimatedChest1.Category.Set("Rusty Props");
        AnimatedChest1.PlaceEffects.Add("vfx_Place_chest");
        AnimatedChest1.PlaceEffects.Add("sfx_build_hammer_wood");
        AnimatedChest1.OpenEffects.Add("sfx_chest_open");
        AnimatedChest1.CloseEffects.Add("sfx_chest_close");
        AnimatedChest1.DestroyedEffects.Add("sfx_metal_blocked");
        AnimatedChest1.DestroyedEffects.Add("vfx_HitSparks");
        AnimatedChests.Add(AnimatedChest1.Prefab.name);
    }

    private static void LoadLights()
    {
        LoadLanternPosts();
        LoadCandles();
        LoadCandlesticks();
        BuildPiece ceilingLantern = new(NorthernAssetsPlugin._Assets, "RS_CeilingLantern");
        ceilingLantern.Name.English("Ceiling Lantern");
        ceilingLantern.Description.English("");
        ceilingLantern.Category.Set("Rusty Lighting");
        ceilingLantern.Crafting.Set("RS_AlchemyTable");
        ceilingLantern.RequiredItems.Add("Platinum_RS", 2, true);
        ceilingLantern.RequiredItems.Add("Honey", 10, true);
        ceilingLantern.PlaceEffects.Add("vfx_Place_wood_wall");
        ceilingLantern.PlaceEffects.Add("sfx_build_hammer_metal");
        ceilingLantern.DestroyedEffects.Add("sfx_metal_blocked");
        ceilingLantern.DestroyedEffects.Add("vfx_HitSparks");
        ceilingLantern.HitEffects.Add("vfx_SawDust");
        ceilingLantern.Prefab.AddComponent<ToggleLight>();
        
        BuildPiece WallTorchA = new BuildPiece(NorthernAssetsPlugin._Assets, "RS_Wall_Torch_A");
        WallTorchA.Name.English("Wall Torch");
        WallTorchA.Description.English("");
        WallTorchA.Category.Set("Rusty Lighting");
        WallTorchA.PlaceEffects.Add("vfx_Place_stone_wall_2x1");
        WallTorchA.PlaceEffects.Add("sfx_build_hammer_stone");
        WallTorchA.DestroyedEffects.Add("vfx_RockDestroyed");
        WallTorchA.DestroyedEffects.Add("sfx_rock_destroyed");
        WallTorchA.HitEffects.Add("vfx_RockHit");
        WallTorchA.HitEffects.Add("sfx_rock_hit");
        WallTorchA.RequiredItems.Add("Platinum_RS", 2, true);
        WallTorchA.RequiredItems.Add("BronzeNails", 4, true);
        WallTorchA.RequiredItems.Add("DeadWood_RS", 2, true);
        WallTorchA.Crafting.Set("RS_AlchemyTable");
        WallTorchA.Prefab.AddComponent<ToggleLight>();
        
        BuildPiece wallTorchB = new(NorthernAssetsPlugin._Assets, "RS_Wall_Torch_B");
        wallTorchB.Name.English("Wall Torch");
        wallTorchB.Description.English("");
        wallTorchB.Category.Set("Rusty Lighting");
        wallTorchB.Crafting.Set("RS_AlchemyTable");
        wallTorchB.RequiredItems.Add("Platinum_RS", 2, true);
        wallTorchB.PlaceEffects.Add("vfx_Place_wood_wall");
        wallTorchB.PlaceEffects.Add("sfx_build_hammer_metal");
        wallTorchB.DestroyedEffects.Add("sfx_metal_blocked");
        wallTorchB.DestroyedEffects.Add("vfx_HitSparks");
        wallTorchB.HitEffects.Add("vfx_SawDust");
        wallTorchB.Prefab.AddComponent<ToggleLight>();
        
        BuildPiece wallTorchC = new(NorthernAssetsPlugin._Assets, "RS_Wall_Torch_C");
        wallTorchC.Name.English("Wall Torch Blue");
        wallTorchC.Description.English("");
        wallTorchC.Category.Set("Rusty Lighting");
        wallTorchC.Crafting.Set("RS_AlchemyTable");
        wallTorchC.RequiredItems.Add("Platinum_RS", 2, true);
        wallTorchC.PlaceEffects.Add("vfx_Place_wood_wall");
        wallTorchC.PlaceEffects.Add("sfx_build_hammer_metal");
        wallTorchC.DestroyedEffects.Add("sfx_metal_blocked");
        wallTorchC.DestroyedEffects.Add("vfx_HitSparks");
        wallTorchC.HitEffects.Add("vfx_SawDust");
        wallTorchC.Prefab.AddComponent<ToggleLight>();

        BuildPiece lanternA = new(NorthernAssetsPlugin._Assets, "RS_Lantern_A");
        lanternA.Name.English("Fancy Lantern");
        lanternA.Description.English("");
        lanternA.Category.Set("Rusty Lighting");
        lanternA.Crafting.Set("RS_AlchemyTable");
        lanternA.RequiredItems.Add("Platinum_RS", 2, true);
        lanternA.RequiredItems.Add("Resin", 10, true);
        lanternA.PlaceEffects.Add("vfx_Place_wood_wall");
        lanternA.PlaceEffects.Add("sfx_build_hammer_metal");
        lanternA.DestroyedEffects.Add("sfx_metal_blocked");
        lanternA.DestroyedEffects.Add("vfx_HitSparks");
        lanternA.HitEffects.Add("vfx_SawDust");
        lanternA.Prefab.AddComponent<ToggleLight>();
        
        BuildPiece candleStick = new(NorthernAssetsPlugin._Assets, "RS_Candlestick");
        candleStick.Name.English("Candlestick");
        candleStick.Description.English("");
        candleStick.Category.Set("Rusty Lighting");
        candleStick.Crafting.Set("RS_AlchemyTable");
        candleStick.RequiredItems.Add("Honey", 10, true);
        candleStick.RequiredItems.Add("Platinum_RS", 1, true);
        candleStick.PlaceEffects.Add("vfx_Place_stone_wall_2x1");
        candleStick.PlaceEffects.Add("sfx_build_hammer_metal");
        candleStick.DestroyedEffects.Add("vfx_RockDestroyed");
        candleStick.DestroyedEffects.Add("vfx_SawDust");
        candleStick.HitEffects.Add("vfx_RockHit");
        candleStick.HitEffects.Add("vfx_HitSparks");
        candleStick.Prefab.AddComponent<ToggleLight>();
        
        BuildPiece lamp = new(NorthernAssetsPlugin._Assets, "RS_Lamp");
        lamp.Name.English("Fancy Lamp");
        lamp.Description.English("");
        lamp.Category.Set("Rusty Lighting");
        lamp.Crafting.Set("RS_AlchemyTable");
        lamp.RequiredItems.Add("Honey", 10, true);
        lamp.RequiredItems.Add("Platinum_RS", 2, true);
        lamp.RequiredItems.Add("Crystal", 1, true);
        lamp.PlaceEffects.Add("vfx_Place_stone_wall_2x1");
        lamp.PlaceEffects.Add("sfx_build_hammer_metal");
        lamp.DestroyedEffects.Add("vfx_RockDestroyed");
        lamp.DestroyedEffects.Add("vfx_SawDust");
        lamp.HitEffects.Add("vfx_RockHit");
        lamp.HitEffects.Add("vfx_HitSparks");
        lamp.Prefab.AddComponent<ToggleLight>();
        
        BuildPiece CeilingBrazier01 = new BuildPiece(NorthernAssetsPlugin._Assets, "CeilingBrazier01_RS");
        CeilingBrazier01.Name.English("Rustic Chandelier");
        CeilingBrazier01.Category.Set("Rusty Lighting");
        CeilingBrazier01.Crafting.Set("RS_AlchemyTable");
        CeilingBrazier01.RequiredItems.Add("Platinum_RS", 1, true);
        CeilingBrazier01.PlaceEffects.Add("vfx_Place_wood_wall");
        CeilingBrazier01.PlaceEffects.Add("sfx_build_hammer_metal");
        CeilingBrazier01.DestroyedEffects.Add("sfx_metal_blocked");
        CeilingBrazier01.DestroyedEffects.Add("vfx_HitSparks");
        CeilingBrazier01.HitEffects.Add("vfx_SawDust");
        CeilingBrazier01.Prefab.AddComponent<ToggleLight>();
        
        BuildPiece CeilingBrazier02 = new BuildPiece(NorthernAssetsPlugin._Assets, "CeilingBrazier02_RS");
        CeilingBrazier02.Name.English("Rustic Chandelier");
        CeilingBrazier02.Category.Set("Rusty Lighting");
        CeilingBrazier02.Crafting.Set("RS_AlchemyTable");
        CeilingBrazier02.RequiredItems.Add("Platinum_RS", 1, true);
        CeilingBrazier02.PlaceEffects.Add("vfx_Place_wood_wall");
        CeilingBrazier02.PlaceEffects.Add("sfx_build_hammer_metal");
        CeilingBrazier02.DestroyedEffects.Add("sfx_metal_blocked");
        CeilingBrazier02.DestroyedEffects.Add("vfx_HitSparks");
        CeilingBrazier02.HitEffects.Add("vfx_SawDust");
        CeilingBrazier02.Prefab.AddComponent<ToggleLight>();
        
        BuildPiece GateTorch = new BuildPiece(NorthernAssetsPlugin._Assets, "RS_GatewayTorch");
        GateTorch.Name.English("Gate Torch");
        GateTorch.Description.English("");
        GateTorch.Category.Set("Rusty Lighting");
        GateTorch.Crafting.Set("RS_AlchemyTable");
        GateTorch.RequiredItems.Add("Platinum_RS", 4, true);
        GateTorch.PlaceEffects.Add("vfx_Place_wood_wall");
        GateTorch.PlaceEffects.Add("sfx_build_hammer_metal");
        GateTorch.DestroyedEffects.Add("sfx_metal_blocked");
        GateTorch.DestroyedEffects.Add("vfx_HitSparks");
        GateTorch.HitEffects.Add("vfx_RockHit");
        GateTorch.HitEffects.Add("sfx_rock_hit");
        GateTorch.Prefab.AddComponent<ToggleLight>();
        
        BuildPiece ForgedBrazier = new BuildPiece(NorthernAssetsPlugin._Assets, "RS_ForgedBrazier");
        ForgedBrazier.Name.English("Forged Brazier");
        ForgedBrazier.Description.English("");
        ForgedBrazier.Category.Set("Rusty Lighting");
        ForgedBrazier.Crafting.Set("RS_AlchemyTable");
        ForgedBrazier.RequiredItems.Add("Platinum_RS", 4, true);
        ForgedBrazier.PlaceEffects.Add("vfx_Place_wood_wall");
        ForgedBrazier.PlaceEffects.Add("sfx_build_hammer_metal");
        ForgedBrazier.DestroyedEffects.Add("sfx_metal_blocked");
        ForgedBrazier.DestroyedEffects.Add("vfx_HitSparks");
        ForgedBrazier.HitEffects.Add("vfx_RockHit");
        ForgedBrazier.HitEffects.Add("sfx_rock_hit");
        ForgedBrazier.Prefab.AddComponent<ToggleLight>();
        
        BuildPiece ForgedBrazier1 = new BuildPiece(NorthernAssetsPlugin._Assets, "RS_ForgedBrazier1");
        ForgedBrazier1.Name.English("Short Forged Brazier");
        ForgedBrazier1.Description.English("");
        ForgedBrazier1.Category.Set("Rusty Lighting");
        ForgedBrazier1.Crafting.Set("RS_AlchemyTable");
        ForgedBrazier1.RequiredItems.Add("Platinum_RS", 4, true);
        ForgedBrazier1.PlaceEffects.Add("vfx_Place_wood_wall");
        ForgedBrazier1.PlaceEffects.Add("sfx_build_hammer_metal");
        ForgedBrazier1.DestroyedEffects.Add("sfx_metal_blocked");
        ForgedBrazier1.DestroyedEffects.Add("vfx_HitSparks");
        ForgedBrazier1.HitEffects.Add("vfx_RockHit");
        ForgedBrazier1.HitEffects.Add("sfx_rock_hit");
        ForgedBrazier1.Prefab.AddComponent<ToggleLight>();
        
        BuildPiece ForgedTorch = new BuildPiece(NorthernAssetsPlugin._Assets, "RS_ForgedTorch");
        ForgedTorch.Name.English("Forged Torch");
        ForgedTorch.Description.English("");
        ForgedTorch.Category.Set("Rusty Lighting");
        ForgedTorch.Crafting.Set("RS_AlchemyTable");
        ForgedTorch.RequiredItems.Add("Platinum_RS", 4, true);
        ForgedTorch.PlaceEffects.Add("vfx_Place_wood_wall");
        ForgedTorch.PlaceEffects.Add("sfx_build_hammer_metal");
        ForgedTorch.DestroyedEffects.Add("sfx_metal_blocked");
        ForgedTorch.DestroyedEffects.Add("vfx_HitSparks");
        ForgedTorch.HitEffects.Add("vfx_RockHit");
        ForgedTorch.HitEffects.Add("sfx_rock_hit");
        ForgedTorch.Prefab.AddComponent<ToggleLight>();
        
        BuildPiece ForgedTorch1 = new BuildPiece(NorthernAssetsPlugin._Assets, "RS_ForgedTorch1");
        ForgedTorch1.Name.English("Forged Torch");
        ForgedTorch1.Description.English("");
        ForgedTorch1.Category.Set("Rusty Lighting");
        ForgedTorch1.Crafting.Set("RS_AlchemyTable");
        ForgedTorch1.RequiredItems.Add("Platinum_RS", 4, true);
        ForgedTorch1.PlaceEffects.Add("vfx_Place_wood_wall");
        ForgedTorch1.PlaceEffects.Add("sfx_build_hammer_metal");
        ForgedTorch1.DestroyedEffects.Add("sfx_metal_blocked");
        ForgedTorch1.DestroyedEffects.Add("vfx_HitSparks");
        ForgedTorch1.HitEffects.Add("vfx_RockHit");
        ForgedTorch1.HitEffects.Add("sfx_rock_hit");
        ForgedTorch1.Prefab.AddComponent<ToggleLight>();

    }

    private static void LoadCoatOfArms()
    {
        List<string> coatOfArmsNames = new List<string>
        {
            "",
            "Ironspire Crest",
            "Silverfang Emblem",
            "Nightshade Shield",
            "Stormbearer Sigil",
            "Crimson Howl Banner",
            "Shadowfen Insignia",
            "Frostclaw Standard",
            "Golden Wyvern Seal",
            "Blazing Wolf Aegis",
            "Thornhelm Badge",
            "Ebonwing Heraldry",
            "Azure Phoenix Mark"
        };
        for (int index = 1; index < 13; ++index)
        {
            var prefabName = $"CoatOfArms{index}_RS";
            BuildPiece piece = new BuildPiece(NorthernAssetsPlugin._Assets, prefabName);
            piece.Name.English(coatOfArmsNames[index]);
            piece.Description.English("");
            piece.Category.Set("Rusty Props");
            piece.Crafting.Set("RS_AlchemyTable");
            piece.RequiredItems.Add($"CoatOfArmsItem{index}_RS", 1, true);
            piece.PlaceEffects.Add("vfx_Place_wood_wall");
            piece.PlaceEffects.Add("sfx_build_hammer_metal");
            piece.DestroyedEffects.Add("sfx_metal_blocked");
            piece.DestroyedEffects.Add("sfx_metal_blocked");
            piece.DestroyedEffects.Add("vfx_HitSparks");
            piece.HitEffects.Add("vfx_SawDust");
        }
    }

    private static void LoadBanners()
    {
        BuildPiece Banner_1 = new(NorthernAssetsPlugin._Assets, "RS_Banner_1");
        Banner_1.Name.English("Banner 1");
        Banner_1.Description.English("");
        Banner_1.Category.Set("Rusty Pieces");
        Banner_1.Crafting.Set("RS_AlchemyTable");
        Banner_1.RequiredItems.Add("Platinum_RS", 1, true);
        Banner_1.RequiredItems.Add("Tin", 2, true);
        Banner_1.RequiredItems.Add("Coal", 10, true);
        Banner_1.RequiredItems.Add("Raspberry", 5, true);
        Banner_1.PlaceEffects.Add("vfx_Place_wood_wall");
        Banner_1.PlaceEffects.Add("sfx_build_hammer_metal");
        Banner_1.DestroyedEffects.Add("sfx_metal_blocked");
        Banner_1.DestroyedEffects.Add("vfx_HitSparks");
        Banner_1.HitEffects.Add("vfx_SawDust");
        MaterialReplacer.MaterialData flag1 = new(NorthernAssetsPlugin._Assets, "dfk_flag", MaterialReplacer.ShaderType.VegetationShader);
        flag1.AddFloatProperty("_Height", 15f);
        flag1.AddFloatProperty("_SwaySpeed", 15f);
        flag1.AddFloatProperty("_SwayDistance", 0f);
        flag1.AddFloatProperty("_PushDistance", 0.4f);
        flag1.AddFloatProperty("_AddRain", 1f);
        flag1.AddFloatProperty("_RippleDistance", 0.5f);
        flag1.AddFloatProperty("_PushClothMode", 1f);
        BuildPiece Banner_2 = new(NorthernAssetsPlugin._Assets, "RS_Banner_2");
        Banner_2.Name.English("Banner 2");
        Banner_2.Description.English("");
        Banner_2.Category.Set("Rusty Pieces");
        Banner_2.Crafting.Set("RS_AlchemyTable");
        Banner_2.RequiredItems.Add("Platinum_RS", 1, true);
        Banner_2.RequiredItems.Add("Tin", 2, true);
        Banner_2.RequiredItems.Add("Coal", 10, true);
        Banner_2.RequiredItems.Add("Blueberries", 5, true);
        Banner_2.PlaceEffects.Add("vfx_Place_wood_wall");
        Banner_2.PlaceEffects.Add("sfx_build_hammer_metal");
        Banner_2.DestroyedEffects.Add("sfx_metal_blocked");
        Banner_2.DestroyedEffects.Add("vfx_HitSparks");
        Banner_2.HitEffects.Add("vfx_SawDust");
        MaterialReplacer.MaterialData flag2 = new(NorthernAssetsPlugin._Assets, "dfk_flag1", MaterialReplacer.ShaderType.VegetationShader);
        flag2.AddFloatProperty("_Height", 15f);
        flag2.AddFloatProperty("_SwaySpeed", 15f);
        flag2.AddFloatProperty("_SwayDistance", 0f);
        flag2.AddFloatProperty("_PushDistance", 0.4f);
        flag2.AddFloatProperty("_AddRain", 1f);
        flag2.AddFloatProperty("_RippleDistance", 0.5f);
        flag2.AddFloatProperty("_PushClothMode", 1f);
    }

    private static void LoadLanternPosts()
    {
        for (int index = 1; index < 3; ++index)
        {
            var prefabName = $"RS_LanternPost_{index}";
            var name = $"Latern Post {index}";
            BuildPiece piece = new(NorthernAssetsPlugin._Assets, prefabName);
            piece.Name.English(name);
            piece.Description.English("");
            piece.Category.Set("Rusty Lighting");
            piece.Crafting.Set("RS_AlchemyTable");
            piece.RequiredItems.Add("Platinum_RS", 5, true);
            piece.RequiredItems.Add("Honey", 10, true);
            piece.PlaceEffects.Add("vfx_Place_wood_wall");
            piece.PlaceEffects.Add("sfx_build_hammer_metal");
            piece.DestroyedEffects.Add("sfx_metal_blocked");
            piece.DestroyedEffects.Add("vfx_HitSparks");
            piece.HitEffects.Add("vfx_SawDust");
            piece.Prefab.AddComponent<ToggleLight>();
        }
    }

    private static void LoadStainedGlass()
    {
        for (int index = 1; index < 10; ++index)
        {
            var prefabName = $"RS_StainedGlass_{index}";
            var name = $"Stained Glass {index}";
            BuildPiece stainglass1 = new(NorthernAssetsPlugin._Assets, prefabName);
            stainglass1.Name.English(name);
            stainglass1.Description.English("");
            stainglass1.Category.Set("Rusty Pieces");
            stainglass1.Crafting.Set("RS_AlchemyTable");
            stainglass1.RequiredItems.Add("Platinum_RS", 2, true);
            stainglass1.RequiredItems.Add("DeadWood_RS", 10, true);
            stainglass1.RequiredItems.Add("Crystal", 10, true);
            stainglass1.RequiredItems.Add("Stone", 20, true);
            stainglass1.PlaceEffects.Add("vfx_Place_stone_wall_2x1");
            stainglass1.PlaceEffects.Add("sfx_build_hammer_crystal");
            stainglass1.DestroyedEffects.Add("sfx_metal_blocked");
            stainglass1.DestroyedEffects.Add("vfx_HitSparks");
            stainglass1.HitEffects.Add("vfx_SawDust");

            DungeonPieces[prefabName] = name;
        }
    }
    
    private static void LoadDoors()
    {
        BuildPiece mainGate = new(NorthernAssetsPlugin._Assets, "MainGate_RS");
        mainGate.Name.English("Truss Gate");
        mainGate.Description.English("");
        mainGate.Category.Set("Rusty Doors");
        mainGate.Crafting.Set("RS_AlchemyTable");
        mainGate.RequiredItems.Add("Platinum_RS", 2, true);
        mainGate.RequiredItems.Add("Stone", 10, true);
        mainGate.PlaceEffects.Add("vfx_Place_wood_wall");
        mainGate.PlaceEffects.Add("sfx_build_hammer_metal");
        mainGate.DestroyedEffects.Add("sfx_metal_blocked");
        mainGate.DestroyedEffects.Add("vfx_HitSparks");
        mainGate.HitEffects.Add("vfx_SawDust");
        mainGate.CloneDoorEffectsFrom = "dvergrtown_secretdoor";

        BuildPiece doorGrate = new(NorthernAssetsPlugin._Assets, "RS_DoorGate");
        doorGrate.Name.English("Door Grate");
        doorGrate.Description.English("");
        doorGrate.Category.Set("Rusty Doors");
        doorGrate.Crafting.Set("RS_AlchemyTable");
        doorGrate.RequiredItems.Add("Platinum_RS", 2, true);
        doorGrate.PlaceEffects.Add("vfx_Place_wood_wall");
        doorGrate.PlaceEffects.Add("sfx_build_hammer_metal");
        doorGrate.DestroyedEffects.Add("sfx_metal_blocked");
        doorGrate.DestroyedEffects.Add("vfx_HitSparks");
        doorGrate.HitEffects.Add("vfx_SawDust");
        doorGrate.CloneDoorEffectsFrom = "darkwood_gate";
        
        BuildPiece doorSpecial = new(NorthernAssetsPlugin._Assets, "RS_DoorSpecial");
        doorSpecial.Name.English("Mystical Door");
        doorSpecial.Description.English("");
        doorSpecial.Category.Set("Rusty Doors");
        doorSpecial.Crafting.Set("RS_AlchemyTable");
        doorSpecial.RequiredItems.Add("DeadWood_RS", 10, true);
        doorSpecial.PlaceEffects.Add("vfx_Place_wood_wall");
        doorSpecial.PlaceEffects.Add("sfx_build_hammer_wood");
        doorSpecial.DestroyedEffects.Add("sfx_wood_blocked");
        doorSpecial.DestroyedEffects.Add("vfx_HitSparks");
        doorSpecial.HitEffects.Add("vfx_SawDust");
        doorSpecial.CloneDoorEffectsFrom = "darkwood_gate";
        
        BuildPiece doorGate1 = new(NorthernAssetsPlugin._Assets, "RS_DoorGate1");
        doorGate1.Name.English("Door Gate 1");
        doorGate1.Description.English("");
        doorGate1.Category.Set("Rusty Doors");
        doorGate1.Crafting.Set("RS_AlchemyTable");
        doorGate1.RequiredItems.Add("Platinum_RS", 2, true);
        doorGate1.RequiredItems.Add("DeadWood_RS", 10, true);
        doorGate1.PlaceEffects.Add("vfx_Place_wood_wall");
        doorGate1.PlaceEffects.Add("sfx_build_hammer_metal");
        doorGate1.DestroyedEffects.Add("sfx_metal_blocked");
        doorGate1.DestroyedEffects.Add("vfx_HitSparks");
        doorGate1.HitEffects.Add("vfx_SawDust");
        doorGate1.CloneDoorEffectsFrom = "darkwood_gate";
        
        BuildPiece doorGate2 = new(NorthernAssetsPlugin._Assets, "RS_DoorGate2");
        doorGate2.Name.English("Door Gate 2");
        doorGate2.Description.English("");
        doorGate2.Category.Set("Rusty Doors");
        doorGate2.Crafting.Set("RS_AlchemyTable");
        doorGate2.RequiredItems.Add("Platinum_RS", 2, true);
        doorGate2.RequiredItems.Add("DeadWood_RS", 10, true);
        doorGate2.PlaceEffects.Add("vfx_Place_wood_wall");
        doorGate2.PlaceEffects.Add("sfx_build_hammer_metal");
        doorGate2.DestroyedEffects.Add("sfx_metal_blocked");
        doorGate2.DestroyedEffects.Add("vfx_HitSparks");
        doorGate2.HitEffects.Add("vfx_SawDust");
        doorGate2.CloneDoorEffectsFrom = "darkwood_gate";
        
        BuildPiece bigDoor = new(NorthernAssetsPlugin._Assets, "WoodBigGate_RS");
        bigDoor.Name.English("Rustic Gate");
        bigDoor.Description.English("");
        bigDoor.Category.Set("Rusty Doors");
        bigDoor.Crafting.Set("RS_AlchemyTable");
        bigDoor.RequiredItems.Add("Platinum_RS", 2, true);
        bigDoor.RequiredItems.Add("DeadWood_RS", 10, true);
        bigDoor.PlaceEffects.Add("vfx_Place_wood_wall");
        bigDoor.PlaceEffects.Add("sfx_build_hammer_metal");
        bigDoor.DestroyedEffects.Add("sfx_metal_blocked");
        bigDoor.DestroyedEffects.Add("vfx_HitSparks");
        bigDoor.HitEffects.Add("vfx_SawDust");
        bigDoor.Prefab.AddComponent<DoubleDoor>();
        bigDoor.CloneDoorEffectsFrom = "darkwood_gate";
        DoubleDoors.Add(bigDoor.Prefab.name);
        
        BuildPiece bigDoor1 = new(NorthernAssetsPlugin._Assets, "WoodBigGate1_RS");
        bigDoor1.Name.English("Rustic Gate");
        bigDoor1.Description.English("");
        bigDoor1.Category.Set("Rusty Doors");
        bigDoor1.Crafting.Set("RS_AlchemyTable");
        bigDoor1.RequiredItems.Add("Platinum_RS", 2, true);
        bigDoor1.RequiredItems.Add("DeadWood_RS", 10, true);
        bigDoor1.PlaceEffects.Add("vfx_Place_wood_wall");
        bigDoor1.PlaceEffects.Add("sfx_build_hammer_metal");
        bigDoor1.DestroyedEffects.Add("sfx_metal_blocked");
        bigDoor1.DestroyedEffects.Add("vfx_HitSparks");
        bigDoor1.HitEffects.Add("vfx_SawDust");
        bigDoor1.Prefab.AddComponent<DoubleDoor>();
        bigDoor1.CloneDoorEffectsFrom = "darkwood_gate";
        DoubleDoors.Add(bigDoor1.Prefab.name);
        
        BuildPiece bigDoor2 = new(NorthernAssetsPlugin._Assets, "WoodBigGate2_RS");
        bigDoor2.Name.English("Rustic Gate");
        bigDoor2.Description.English("");
        bigDoor2.Category.Set("Rusty Doors");
        bigDoor2.Crafting.Set("RS_AlchemyTable");
        bigDoor2.RequiredItems.Add("Iron", 2, true);
        bigDoor2.RequiredItems.Add("DeadWood_RS", 10, true);
        bigDoor2.PlaceEffects.Add("vfx_Place_wood_wall");
        bigDoor2.PlaceEffects.Add("sfx_build_hammer_metal");
        bigDoor2.DestroyedEffects.Add("sfx_metal_blocked");
        bigDoor2.DestroyedEffects.Add("vfx_HitSparks");
        bigDoor2.HitEffects.Add("vfx_SawDust");
        bigDoor2.Prefab.AddComponent<DoubleDoor>();
        bigDoor2.CloneDoorEffectsFrom = "darkwood_gate";
        DoubleDoors.Add(bigDoor2.Prefab.name);
        
        BuildPiece bigDoor3 = new(NorthernAssetsPlugin._Assets, "WoodBigGate3_RS");
        bigDoor3.Name.English("Rustic Gate");
        bigDoor3.Description.English("");
        bigDoor3.Category.Set("Rusty Doors");
        bigDoor3.Crafting.Set("RS_AlchemyTable");
        bigDoor3.RequiredItems.Add("Iron", 2, true);
        bigDoor3.RequiredItems.Add("DeadWood_RS", 10, true);
        bigDoor3.PlaceEffects.Add("vfx_Place_wood_wall");
        bigDoor3.PlaceEffects.Add("sfx_build_hammer_metal");
        bigDoor3.DestroyedEffects.Add("sfx_metal_blocked");
        bigDoor3.DestroyedEffects.Add("vfx_HitSparks");
        bigDoor3.HitEffects.Add("vfx_SawDust");
        bigDoor3.Prefab.AddComponent<DoubleDoor>();
        bigDoor3.CloneDoorEffectsFrom = "darkwood_gate";
        DoubleDoors.Add(bigDoor3.Prefab.name);
        
        BuildPiece stoneBigDoor = new(NorthernAssetsPlugin._Assets, "StoneBigGate_RS");
        stoneBigDoor.Name.English("Stone Gate");
        stoneBigDoor.Description.English("");
        stoneBigDoor.Category.Set("Rusty Doors");
        stoneBigDoor.Crafting.Set("RS_AlchemyTable");
        stoneBigDoor.RequiredItems.Add("Iron", 2, true);
        stoneBigDoor.RequiredItems.Add("DeadWood_RS", 10, true);
        stoneBigDoor.PlaceEffects.Add("vfx_Place_wood_wall");
        stoneBigDoor.PlaceEffects.Add("sfx_build_hammer_metal");
        stoneBigDoor.DestroyedEffects.Add("sfx_metal_blocked");
        stoneBigDoor.DestroyedEffects.Add("vfx_HitSparks");
        stoneBigDoor.HitEffects.Add("vfx_SawDust");
        stoneBigDoor.Prefab.AddComponent<DoubleDoor>();
        stoneBigDoor.CloneDoorEffectsFrom = "darkwood_gate";
        DoubleDoors.Add(stoneBigDoor.Prefab.name);
        
        BuildPiece door = new(NorthernAssetsPlugin._Assets, "RS_Door");
        door.Name.English("Rustic Door");
        door.Description.English("");
        door.Category.Set("Rusty Doors");
        door.Crafting.Set("RS_AlchemyTable");
        door.RequiredItems.Add("Iron", 2, true);
        door.RequiredItems.Add("DeadWood_RS", 10, true);
        door.PlaceEffects.Add("vfx_Place_wood_wall");
        door.PlaceEffects.Add("sfx_build_hammer_metal");
        door.DestroyedEffects.Add("sfx_metal_blocked");
        door.DestroyedEffects.Add("vfx_HitSparks");
        door.HitEffects.Add("vfx_SawDust");
        door.CloneDoorEffectsFrom = "darkwood_gate";

        BuildPiece castleDoor1 = new(NorthernAssetsPlugin._Assets, "RS_CastleDoor_1");
        castleDoor1.Name.English("Castle Door");
        castleDoor1.Description.English("");
        castleDoor1.Category.Set("Rusty Doors");
        castleDoor1.Crafting.Set("RS_AlchemyTable");
        castleDoor1.RequiredItems.Add("Iron", 2, true);
        castleDoor1.RequiredItems.Add("DeadWood_RS", 10, true);
        castleDoor1.PlaceEffects.Add("vfx_Place_wood_wall");
        castleDoor1.PlaceEffects.Add("sfx_build_hammer_metal");
        castleDoor1.DestroyedEffects.Add("sfx_metal_blocked");
        castleDoor1.DestroyedEffects.Add("vfx_HitSparks");
        castleDoor1.HitEffects.Add("vfx_SawDust");
        castleDoor1.CloneDoorEffectsFrom = "darkwood_gate";

        BuildPiece doorLeft = new(NorthernAssetsPlugin._Assets, "RS_DoorLeft");
        doorLeft.Name.English("Rustic Door");
        doorLeft.Description.English("");
        doorLeft.Category.Set("Rusty Doors");
        doorLeft.Crafting.Set("RS_AlchemyTable");
        doorLeft.RequiredItems.Add("Iron", 2, true);
        doorLeft.RequiredItems.Add("Wood", 10, true);
        doorLeft.PlaceEffects.Add("vfx_Place_wood_wall");
        doorLeft.PlaceEffects.Add("sfx_build_hammer_metal");
        doorLeft.DestroyedEffects.Add("sfx_metal_blocked");
        doorLeft.DestroyedEffects.Add("vfx_HitSparks");
        doorLeft.HitEffects.Add("vfx_SawDust");
        doorLeft.CloneDoorEffectsFrom = "darkwood_gate";
        
        BuildPiece doorRight = new(NorthernAssetsPlugin._Assets, "RS_DoorRight");
        doorRight.Name.English("Rustic Door");
        doorRight.Description.English("");
        doorRight.Category.Set("Rusty Doors");
        doorRight.Crafting.Set("RS_AlchemyTable");
        doorRight.RequiredItems.Add("Iron", 2, true);
        doorRight.RequiredItems.Add("DeadWood_RS", 10, true);
        doorRight.PlaceEffects.Add("vfx_Place_wood_wall");
        doorRight.PlaceEffects.Add("sfx_build_hammer_metal");
        doorRight.DestroyedEffects.Add("sfx_metal_blocked");
        doorRight.DestroyedEffects.Add("vfx_HitSparks");
        doorRight.HitEffects.Add("vfx_SawDust");
        doorRight.CloneDoorEffectsFrom = "darkwood_gate";
    }

    private static void LoadOther()
    {
        BuildPiece stoneGuard = new(NorthernAssetsPlugin._Assets, "StoneGuard_RS");
        stoneGuard.Name.English("Stone Guard");
        stoneGuard.Description.English("");
        stoneGuard.Category.Set("Rusty Props");
        stoneGuard.Crafting.Set("RS_AlchemyTable");
        stoneGuard.RequiredItems.Add("Stone", 20, true);
        stoneGuard.PlaceEffects.Add("vfx_Place_wood_wall");
        stoneGuard.PlaceEffects.Add("sfx_build_hammer_stone");
        stoneGuard.DestroyedEffects.Add("sfx_stone_blocked");
        stoneGuard.DestroyedEffects.Add("vfx_HitSparks");
        stoneGuard.HitEffects.Add("vfx_SawDust");
        
        BuildPiece LokiStatue = new(NorthernAssetsPlugin._Assets, "Krampus_Statue_RS");
        LokiStatue.Name.English("Krampus Statue");
        LokiStatue.Description.English("");
        LokiStatue.Category.Set("Rusty Props");
        LokiStatue.Crafting.Set("RS_AlchemyTable");
        LokiStatue.RequiredItems.Add("TrophyLoki_RS", 1, true);
        LokiStatue.RequiredItems.Add("Stone", 50, true);
        LokiStatue.PlaceEffects.Add("vfx_Place_wood_wall");
        LokiStatue.PlaceEffects.Add("sfx_build_hammer_stone");
        LokiStatue.DestroyedEffects.Add("sfx_stone_blocked");
        LokiStatue.DestroyedEffects.Add("vfx_HitSparks");
        LokiStatue.HitEffects.Add("vfx_SawDust");

        BuildPiece drawer = new(NorthernAssetsPlugin._Assets, "RS_Drawer");
        drawer.Name.English("Drawer");
        drawer.Description.English("");
        drawer.Category.Set("Rusty Pieces");
        drawer.Crafting.Set("RS_AlchemyTable");
        drawer.RequiredItems.Add("DeadWood_RS", 10, true);
        drawer.RequiredItems.Add("BronzeNails", 8, true);
        drawer.PlaceEffects.Add("vfx_Place_wood_floor");
        drawer.PlaceEffects.Add("sfx_build_hammer_wood");
        drawer.DestroyedEffects.Add("sfx_wood_destroyed");
        drawer.DestroyedEffects.Add("vfx_SawDust");
        drawer.HitEffects.Add("vfx_SawDust");

        BuildPiece bucket = new(NorthernAssetsPlugin._Assets, "RS_Bucket");
        bucket.Name.English("Bucket");
        bucket.Description.English("");
        bucket.Category.Set("Rusty Props");
        bucket.Crafting.Set("RS_AlchemyTable");
        bucket.RequiredItems.Add("DeadWood_RS", 3, true);
        bucket.PlaceEffects.Add("vfx_Place_wood_floor");
        bucket.PlaceEffects.Add("sfx_build_hammer_wood");
        bucket.DestroyedEffects.Add("sfx_wood_destroyed");
        bucket.DestroyedEffects.Add("vfx_SawDust");
        bucket.HitEffects.Add("vfx_SawDust");
        
        BuildPiece ladder = new(NorthernAssetsPlugin._Assets, "RS_Ladder");
        ladder.Name.English("Ladder");
        ladder.Description.English("");
        ladder.Category.Set("Rusty Pieces");
        ladder.Crafting.Set("RS_AlchemyTable");
        ladder.RequiredItems.Add("DeadWood_RS", 3, true);
        ladder.PlaceEffects.Add("vfx_Place_wood_floor");
        ladder.PlaceEffects.Add("sfx_build_hammer_wood");
        ladder.DestroyedEffects.Add("sfx_wood_destroyed");
        ladder.DestroyedEffects.Add("vfx_SawDust");
        ladder.HitEffects.Add("vfx_SawDust");

        BuildPiece gasBurner = new(NorthernAssetsPlugin._Assets, "RS_GasBurner");
        gasBurner.Name.English("Gas Burner");
        gasBurner.Description.English("");
        gasBurner.Category.Set("Rusty Props");
        gasBurner.Crafting.Set("RS_AlchemyTable");
        gasBurner.RequiredItems.Add("Platinum_RS", 2, true);
        gasBurner.PlaceEffects.Add("vfx_Place_wood_wall");
        gasBurner.PlaceEffects.Add("sfx_build_hammer_metal");
        gasBurner.DestroyedEffects.Add("sfx_metal_blocked");
        gasBurner.DestroyedEffects.Add("vfx_HitSparks");
        gasBurner.HitEffects.Add("vfx_SawDust");
        
        BuildPiece cup = new(NorthernAssetsPlugin._Assets, "RS_Cup");
        cup.Name.English("Chalice");
        cup.Description.English("");
        cup.Category.Set("Rusty Props");
        cup.Crafting.Set("RS_AlchemyTable");
        cup.RequiredItems.Add("Platinum_RS", 2, true);
        cup.PlaceEffects.Add("vfx_Place_wood_wall");
        cup.PlaceEffects.Add("sfx_build_hammer_metal");
        cup.DestroyedEffects.Add("sfx_metal_blocked");
        cup.DestroyedEffects.Add("vfx_HitSparks");
        cup.HitEffects.Add("vfx_SawDust");
        
        BuildPiece inkPot = new(NorthernAssetsPlugin._Assets, "RS_InkPot");
        inkPot.Name.English("Ink Pot");
        inkPot.Description.English("");
        inkPot.Category.Set("Rusty Props");
        inkPot.Crafting.Set("RS_AlchemyTable");
        inkPot.RequiredItems.Add("Platinum_RS", 2, true);
        inkPot.PlaceEffects.Add("vfx_Place_wood_wall");
        inkPot.PlaceEffects.Add("sfx_build_hammer_metal");
        inkPot.DestroyedEffects.Add("sfx_metal_blocked");
        inkPot.DestroyedEffects.Add("vfx_HitSparks");
        inkPot.HitEffects.Add("vfx_SawDust");

        BuildPiece sandGlass = new(NorthernAssetsPlugin._Assets, "RS_Sandglass");
        sandGlass.Name.English("Sandglass");
        sandGlass.Description.English("");
        sandGlass.Category.Set("Rusty Props");
        sandGlass.Crafting.Set("RS_AlchemyTable");
        sandGlass.RequiredItems.Add("Platinum_RS", 2, true);
        sandGlass.PlaceEffects.Add("vfx_Place_wood_wall");
        sandGlass.PlaceEffects.Add("sfx_build_hammer_metal");
        sandGlass.DestroyedEffects.Add("sfx_metal_blocked");
        sandGlass.DestroyedEffects.Add("vfx_HitSparks");
        sandGlass.HitEffects.Add("vfx_SawDust");
        
        BuildPiece scroll = new(NorthernAssetsPlugin._Assets, "RS_Scroll");
        scroll.Name.English("Ink Pot");
        scroll.Description.English("");
        scroll.Category.Set("Rusty Props");
        scroll.Crafting.Set("RS_AlchemyTable");
        scroll.RequiredItems.Add("DeadWood_RS", 1, true);
        scroll.PlaceEffects.Add("vfx_Place_wood_floor");
        scroll.PlaceEffects.Add("sfx_build_hammer_wood");
        scroll.DestroyedEffects.Add("sfx_wood_destroyed");
        scroll.DestroyedEffects.Add("vfx_SawDust");
        scroll.HitEffects.Add("vfx_SawDust");
        
        BuildPiece scale = new(NorthernAssetsPlugin._Assets, "RS_Scale");
        scale.Name.English("Scale");
        scale.Description.English("");
        scale.Category.Set("Rusty Props");
        scale.Crafting.Set("RS_AlchemyTable");
        scale.RequiredItems.Add("Platinum_RS", 2, true);
        scale.PlaceEffects.Add("vfx_Place_wood_wall");
        scale.PlaceEffects.Add("sfx_build_hammer_metal");
        scale.DestroyedEffects.Add("sfx_metal_blocked");
        scale.DestroyedEffects.Add("vfx_HitSparks");
        scale.HitEffects.Add("vfx_SawDust");
        
        BuildPiece shelf = new(NorthernAssetsPlugin._Assets, "RS_Shelf");
        shelf.Name.English("Fancy Shelf");
        shelf.Description.English("");
        shelf.Category.Set("Rusty Pieces");
        shelf.Crafting.Set("RS_AlchemyTable");
        shelf.RequiredItems.Add("DeadWood_RS", 10, true);
        shelf.RequiredItems.Add("IronNails", 5, true);
        shelf.PlaceEffects.Add("vfx_Place_wood_floor");
        shelf.PlaceEffects.Add("sfx_build_hammer_wood");
        shelf.DestroyedEffects.Add("sfx_wood_destroyed");
        shelf.DestroyedEffects.Add("vfx_SawDust");
        shelf.HitEffects.Add("vfx_SawDust");
    }

    private static void LoadStations()
    {
        BuildPiece alchemyTable = new(NorthernAssetsPlugin._Assets, "RS_AlchemyTable");
        alchemyTable.Name.English("Alchemy Table");
        alchemyTable.Description.English("Craft potions");
        alchemyTable.Category.Set(BuildPieceCategory.Crafting);
        alchemyTable.Crafting.Set(CraftingTable.Workbench);
        alchemyTable.RequiredItems.Add("DeadWood_RS", 20, true);
        alchemyTable.RequiredItems.Add("Crystal", 10, true);
        alchemyTable.RequiredItems.Add("Platinum_RS", 5, true);
        alchemyTable.RequiredItems.Add("Resin", 20, true);
        alchemyTable.PlaceEffects.Add("vfx_Place_wood_floor");
        alchemyTable.PlaceEffects.Add("sfx_build_hammer_wood");
        alchemyTable.DestroyedEffects.Add("sfx_wood_destroyed");
        alchemyTable.DestroyedEffects.Add("vfx_SawDust");
        alchemyTable.HitEffects.Add("vfx_SawDust");

        BuildPiece alchemyTable_ext_1 = new(NorthernAssetsPlugin._Assets, "RS_AlchemyTable_Ext_1");
        alchemyTable_ext_1.Name.English("Alchemist's Shelf");
        alchemyTable_ext_1.Description.English("Improves alchemy table");
        alchemyTable_ext_1.Category.Set(BuildPieceCategory.Crafting);
        alchemyTable_ext_1.Crafting.Set("RS_AlchemyTable");
        alchemyTable_ext_1.RequiredItems.Add("DeadWood_RS", 10, true);
        alchemyTable_ext_1.RequiredItems.Add("Crystal", 5, true);
        alchemyTable_ext_1.RequiredItems.Add("IronNails", 5, true);
        alchemyTable_ext_1.RequiredItems.Add("Resin", 20, true);
        alchemyTable_ext_1.PlaceEffects.Add("vfx_Place_wood_floor");
        alchemyTable_ext_1.PlaceEffects.Add("sfx_build_hammer_wood");
        alchemyTable_ext_1.DestroyedEffects.Add("sfx_wood_destroyed");
        alchemyTable_ext_1.DestroyedEffects.Add("vfx_SawDust");
        alchemyTable_ext_1.HitEffects.Add("vfx_SawDust");
        
        BuildPiece mortar = new(NorthernAssetsPlugin._Assets, "RS_AlchemyTable_Ext_2");
        mortar.Name.English("Mortar");
        mortar.Description.English("A bowl used in alchemy to crush and grind ingredients into powders for potions and mixtures.");
        mortar.Category.Set(BuildPieceCategory.Crafting);
        mortar.Crafting.Set("RS_AlchemyTable");
        mortar.RequiredItems.Add("DeadWood_RS", 10, true);
        mortar.RequiredItems.Add("Platinum_RS", 2, true);
        mortar.PlaceEffects.Add("vfx_Place_wood_wall");
        mortar.PlaceEffects.Add("sfx_build_hammer_metal");
        mortar.DestroyedEffects.Add("sfx_metal_blocked");
        mortar.DestroyedEffects.Add("vfx_HitSparks");
        mortar.HitEffects.Add("vfx_SawDust");
    }
    
    private static string ConvertPrefabNameToFriendlyName(string prefabName)
    {
        string[] parts = prefabName.Split('_');

        for (int i = 0; i < parts.Length; i++)
        {
            parts[i] = char.ToUpper(parts[i][0]) + parts[i].Substring(1);
        }

        return string.Join(" ", parts);
    }
}