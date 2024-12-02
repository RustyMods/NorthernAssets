using Managers;
using NorthernAssets.Behaviors;
using NorthernAssets.Managers;
using UnityEngine;

namespace NorthernAssets.prefabs;

public static class AssetManager
{
    public static void Load()
    {
        LoadCliffs();
        LoadHotSprings();
        LoadTrees();
        LoadFloraAssets();
        LoadEnvironments();
        LoadLocations();
        LoadSpawners();
        LoadSnowmen();
        LoadRuneStones();
        LoadOres();
        Items.Load();
        BuildPieces.Load();
    }
    
    private static void LoadSnowmen()
    {
        for (int index = 1; index < 4; ++index)
        {
            string prefabName = $"snowman_0{index}";
            VegetationManager.Vegetation snowman = new(NorthernAssetsPlugin._Assets, prefabName);
            snowman.Biome = Heightmap.Biome.None;
            snowman.ScaleMin = 1f;
            snowman.ScaleMax = 1.1f;
            snowman.ChanceToTilt = 0f;
            snowman.MaxSpawn = 1f;
            snowman.MinAltitude = 0f;
            snowman.MaxAltitude = 1000f;
            snowman.GroundOffset = -0.1f;
            snowman.GroupRadius = 1000f;
            snowman.MinGroupSize = 1;
            snowman.MaxGroupSize = 1;
            snowman.SnapToStaticSolid = true;
        }
    }

    private static void LoadRuneStones()
    {
        VegetationManager.Vegetation runestone_1 = new(NorthernAssetsPlugin._Assets, "Runestone_DeepNorth");
        runestone_1.Biome = Heightmap.Biome.DeepNorth;
        runestone_1.ScaleMin = 1f;
        runestone_1.ScaleMax = 1.1f;
        runestone_1.ChanceToTilt = 0f;
        runestone_1.MaxSpawn = 1f;
        runestone_1.MinAltitude = 0f;
        runestone_1.MaxAltitude = 1000f;
        runestone_1.GroundOffset = -0.5f;
        runestone_1.GroupRadius = 1000f;
        runestone_1.MinGroupSize = 1;
        runestone_1.MaxGroupSize = 1;
        runestone_1.SnapToStaticSolid = true;
    }

    private static void LoadCliffs()
    {
        for (int i = 1; i < 12; ++i)
        {
            string prefabName = $"Cliff_{i}_DeepNorth";
            VegetationManager.Vegetation cliff = new(NorthernAssetsPlugin._Assets, prefabName);
            cliff.Biome = Heightmap.Biome.DeepNorth;
            cliff.ScaleMin = 2f;
            cliff.ScaleMax = 3f;
            cliff.ChanceToTilt = 0.5f;
            cliff.MaxTilt = 90f;
            cliff.MaxSpawn = 1f;
            cliff.MinAltitude = 0f;
            cliff.MaxAltitude = 1000f;
            cliff.GroundOffset = -10f;
            cliff.DestroyedEffects.Add("sfx_rock_destroyed");
            cliff.DestroyedEffects.Add("vfx_RockDestroyed_large");
            cliff.HitEffects.Add("sfx_rock_hit");
            cliff.HitEffects.Add("vfx_RockHit");
            cliff.MinToolTier = 0;
            cliff.m_drops.Add(new VegetationManager.RockDrop("Stone", 1, 10));
        }
    }

    private static void LoadHotSprings()
    {
        VegetationManager.Vegetation HotSprings = new(NorthernAssetsPlugin._Assets, "HotSpring_DeepNorth");
        HotSprings.Biome = Heightmap.Biome.DeepNorth;
        HotSprings.ScaleMin = 1f;
        HotSprings.ScaleMax = 1f;
        HotSprings.ChanceToTilt = 0f;
        HotSprings.MaxSpawn = 1f;
        HotSprings.MinAltitude = 0f;
        HotSprings.MaxAltitude = 1000f;
        HotSprings.MaxGroupSize = 1;
        HotSprings.GroupRadius = 1000f;
        HotSprings.GroundOffset = -5f;
        MaterialReplacer.RegisterGameObjectForMatSwap(HotSprings.Prefab);
    }

    private static void LoadOres()
    {
        VegetationManager.MineRock platinumRock = new VegetationManager.MineRock(NorthernAssetsPlugin._Assets, "MineRock_Ore_RS", "Ore MineRock");
        platinumRock.Name.English("Ore Deposit");
        platinumRock.HitEffects.Add("vfx_RockHit");
        platinumRock.HitEffects.Add("sfx_rock_hit");
        platinumRock.DestroyedEffects.Add("vfx_RockDestroyed");
        platinumRock.DestroyedEffects.Add("sfx_rock_destroyed");
        platinumRock.DropTableData.Add("Stone", 3, 5);
        platinumRock.DropTableData.Add("PlatinumOre_RS", 1, 3);
        platinumRock.DropTableData.Add("IronOre", 1, 3);
        platinumRock.DropTableData.Add("TinOre", 1, 3);
        platinumRock.DropTableData.Add("CopperOre", 1, 3);
        platinumRock.SetDropTable(1, 5, 1f, true);
        platinumRock.MinAltitude = 10f;
        platinumRock.MaxSpawn = 2;
        platinumRock.MinSpawn = 1;
        platinumRock.Biome = Heightmap.Biome.DeepNorth;
        platinumRock.SnapToStaticSolid = true;
        platinumRock.GroundOffset = -0.5f;
        
        VegetationManager.MineRock CoalRock = new VegetationManager.MineRock(NorthernAssetsPlugin._Assets, "MineRock_Coal_RS", "Coal MineRock");
        CoalRock.Name.English("Coal Deposit");
        CoalRock.HitEffects.Add("vfx_RockHit");
        CoalRock.HitEffects.Add("sfx_rock_hit");
        CoalRock.DestroyedEffects.Add("vfx_RockDestroyed");
        CoalRock.DestroyedEffects.Add("sfx_rock_destroyed");
        CoalRock.DropTableData.Add("Stone", 3, 5);
        CoalRock.DropTableData.Add("Coal", 1, 3);
        CoalRock.SetDropTable(1, 2, 1f, true);
        CoalRock.MinAltitude = 10f;
        CoalRock.MaxSpawn = 2;
        CoalRock.MinSpawn = 1;
        CoalRock.Biome = Heightmap.Biome.DeepNorth;
        CoalRock.SnapToStaticSolid = true;
        CoalRock.GroundOffset = -0.5f;
    }

    private static void LoadTrees()
    {
        FloraManager.TreeLogFlora treeLog = new FloraManager.TreeLogFlora(NorthernAssetsPlugin._Assets, "TreeLog_DeepNorth", "Dead Tree Log");
        treeLog.AddDestroyedEffect("vfx_firlogdestroyed");
        treeLog.AddDestroyedEffect("sfx_wood_break");
        treeLog.AddHitEffect("vfx_SawDust");
        treeLog.AddHitEffect("sfx_tree_hit");
        treeLog.AddDrop("Wood", 1, 10);
        treeLog.AddDrop("DeadWood_RS", 1, 5);
        treeLog.AddHitImpactEffect("sfx_tree_fall_hit");
        treeLog.AddHitImpactEffect("vfx_tree_fall_hit");
        treeLog.AddHitImpactEffect("sfx_tree_hit");
        treeLog.AddFloatingImpactEffect("fx_WaterImpactBig");
        
        for (int i = 1; i < 8; ++i)
        {
            FloraManager.RegisterAsset(NorthernAssetsPlugin._Assets, $"vfx_tree_deepnorth_{i}");
            FloraManager.Vegetation tree = new FloraManager.Vegetation(NorthernAssetsPlugin._Assets, $"Tree_DeepNorth_{i}", $"Dead Tree {i}");
            tree.Name.English("Dead Tree");
            tree.SetBiome(Heightmap.Biome.DeepNorth);
            tree.SetMinMaxSpawn(1, 3);
            tree.SetScale(1f, 1.2f);
            tree.SetTilt(0.5f, 2f, 0f, 30f);
            tree.SetSnapToStaticSolid(true);
            tree.m_treeBase.AddDestroyedEffect($"vfx_tree_deepnorth_{i}");
            tree.m_treeBase.AddDestroyedEffect("sfx_tree_fall");
            tree.m_treeBase.AddHitEffect("vfx_SawDust");
            tree.m_treeBase.AddHitEffect("sfx_tree_hit");
            tree.SetGroundOffset(-0.5f);
        }
    }

    private static void LoadFloraAssets()
    {
        FloraManager.Vegetation FrozenGrowth = new FloraManager.Vegetation(NorthernAssetsPlugin._Assets, "Pickable_FrozenGrowth", "Frozen Growth");
        FrozenGrowth.SetBiome(Heightmap.Biome.DeepNorth);
        FrozenGrowth.SetMinMaxSpawn(1, 3);
        FrozenGrowth.SetScale(1f, 1.2f);
        FrozenGrowth.SetTilt(0.2f, 1f, 1f, 4f);
        FrozenGrowth.SetGroupSize(1, 2, 10f);
        FrozenGrowth.SetForcePlacement(true);
        FrozenGrowth.AddPickEffect("sfx_pickable_pick");
        FrozenGrowth.AddPickEffect("vfx_pickable_pick");
        FrozenGrowth.SetSnapToStaticSolid(true);

        FloraManager.Vegetation FrozenBerries = new FloraManager.Vegetation(NorthernAssetsPlugin._Assets, "Pickable_FrozenBerries", "Frozen Berries");
        FrozenBerries.SetBiome(Heightmap.Biome.DeepNorth);
        FrozenBerries.SetMinMaxSpawn(1, 3);
        FrozenBerries.SetScale(1f, 1.2f);
        FrozenBerries.SetTilt(0.2f, 1f, 1f, 4f);
        FrozenBerries.SetGroupSize(1, 2, 10f);
        FrozenBerries.SetForcePlacement(true);
        FrozenBerries.AddPickEffect("sfx_pickable_pick");
        FrozenBerries.AddPickEffect("vfx_pickable_pick");
        FrozenBerries.SetSnapToStaticSolid(true);

        FloraManager.Vegetation FrozenFungi = new FloraManager.Vegetation(NorthernAssetsPlugin._Assets, "Pickable_Fungi", "Fungi");
        FrozenFungi.SetBiome(Heightmap.Biome.DeepNorth);
        FrozenFungi.SetMinMaxSpawn(1, 3);
        FrozenFungi.SetScale(1f, 1.2f);
        FrozenFungi.SetTilt(0.2f, 1f, 1f, 4f);
        FrozenFungi.SetGroupSize(1, 2, 10f);
        FrozenFungi.SetForcePlacement(true);
        FrozenFungi.AddPickEffect("sfx_pickable_pick");
        FrozenFungi.AddPickEffect("vfx_pickable_pick");
        FrozenFungi.SetSnapToStaticSolid(true);
        
        FloraManager.Vegetation TubeMushroom = new FloraManager.Vegetation(NorthernAssetsPlugin._Assets, "Pickable_Tube", "Tube Mushroom");
        TubeMushroom.SetBiome(Heightmap.Biome.DeepNorth);
        TubeMushroom.SetMinMaxSpawn(1, 3);
        TubeMushroom.SetScale(1f, 1.2f);
        TubeMushroom.SetTilt(0.2f, 1f, 1f, 4f);
        TubeMushroom.SetGroupSize(1, 2, 10f);
        TubeMushroom.SetForcePlacement(true);
        TubeMushroom.AddPickEffect("sfx_pickable_pick");
        TubeMushroom.AddPickEffect("vfx_pickable_pick");
        TubeMushroom.SetSnapToStaticSolid(true);
    }

    private static void LoadEnvironments()
    {
        EnvironmentManager.RegisterParticleSystems(NorthernAssetsPlugin._Assets, "AuroraPS");

        MusicManager.MusicEnvironment RatKing = new MusicManager.MusicEnvironment("RatKing_music", "RatKing", NorthernAssetsPlugin._Assets);
        RatKing.SetEnvironmentCopy("Bonemass");
        RatKing.m_volume = 2f;

        MusicManager.MusicEnvironment Bahomet = new MusicManager.MusicEnvironment("Bahomet_music", "Bahomet", NorthernAssetsPlugin._Assets);
        Bahomet.SetEnvironmentCopy("Crypt");
        Bahomet.m_volume = 2f;

        MusicManager.MusicEnvironment Ambient = new MusicManager.MusicEnvironment("Ambient_10", "Aurora", NorthernAssetsPlugin._Assets);
        Ambient.SetEnvironmentCopy("Darklands_dark");
        Ambient.m_volume = 2f;
        
        MusicManager.MusicEnvironment Christmas = new MusicManager.MusicEnvironment("Ambient_9", "Twilight_Aurora", NorthernAssetsPlugin._Assets);
        Christmas.SetEnvironmentCopy("Twilight_Clear");
        Christmas.m_volume = 2f;
        
        MusicManager.MusicEnvironment ChristmasDark = new MusicManager.MusicEnvironment("Ambient_9", "Darkland_Krampus", NorthernAssetsPlugin._Assets);
        ChristmasDark.SetEnvironmentCopy("Darklands_dark");
        ChristmasDark.m_volume = 2f;

        MusicManager.MusicEnvironment YetiCaves = new MusicManager.MusicEnvironment("Night_Ambient_2", "FrozenCaves", NorthernAssetsPlugin._Assets);
        YetiCaves.SetEnvironmentCopy("Darklands_dark");
        YetiCaves.m_volume = 2f;

        MusicManager.Music DeepNorthEvening = new MusicManager.Music(NorthernAssetsPlugin._Assets, "DeepNorth_Evening");
        DeepNorthEvening.m_volume = 2f;
        
        EnvironmentManager.EnvSetup DeepNorthWeather = new("DeepNorth_env", Heightmap.Biome.DeepNorth);
        EnvironmentManager.Weather TwilightKrampus = new EnvironmentManager.Weather("Twilight_Aurora", 1f);
        TwilightKrampus.m_deepNorthOverride = true;
        TwilightKrampus.CustomParticles = "AuroraPS";
        DeepNorthWeather.weathers.Add(TwilightKrampus);
        EnvironmentManager.Weather TwilightSnow = new EnvironmentManager.Weather("Twilight_Snow", 0.5f);
        DeepNorthWeather.weathers.Add(TwilightSnow);
        EnvironmentManager.Weather TwilightSnowStorm = new EnvironmentManager.Weather("Twilight_SnowStorm", 0.5f);
        DeepNorthWeather.weathers.Add(TwilightSnowStorm);
        EnvironmentManager.Weather Aurora = new EnvironmentManager.Weather("Aurora", 2f);
        Aurora.m_deepNorthOverride = true;
        Aurora.CustomParticles = "AuroraPS";
        DeepNorthWeather.weathers.Add(Aurora);
        DeepNorthWeather.musicMorning = "Twilight_Aurora";
        DeepNorthWeather.musicDay = "Aurora";
        DeepNorthWeather.musicEvening = "DeepNorth_Evening";
        DeepNorthWeather.musicNight = "FrozenCaves";
        DeepNorthWeather.OverrideOriginal = true;
    }

    private static void LoadSpawners()
    {
        var YetiSpawner = new LocationManager.CreatureSpawnerInfo(NorthernAssetsPlugin._Assets, "Spawner_Yeti_RS", "Yeti_RS");
        var LizardFishSpawner = new LocationManager.CreatureSpawnerInfo(NorthernAssetsPlugin._Assets, "Spawner_LizardFish_RS", "LizardFish_RS");
        var skeletonSpawner = new LocationManager.CreatureSpawnerInfo(NorthernAssetsPlugin._Assets, "Spawner_Skeleton_RS", "RS_Skeleton");
        skeletonSpawner.Add("RS_Skeleton_Mage");
        skeletonSpawner.Add("RS_Skeleton_Archer");
        skeletonSpawner.Add("RS_Skeleton_Rogue");
        skeletonSpawner.Add("RS_Skeleton_Warlord");
        skeletonSpawner.Add("RS_Skeleton_Warrior");
        var skeletonKingSpawner = new LocationManager.CreatureSpawnerInfo(NorthernAssetsPlugin._Assets, "Spawner_SkeletonKing_RS", "RS_Skeleton_King");
        var trollSpawner = new LocationManager.CreatureSpawnerInfo(NorthernAssetsPlugin._Assets, "Spawner_NorthTroll_RS", "NorthTroll_RS");
        var ratSpawner = new LocationManager.CreatureSpawnerInfo(NorthernAssetsPlugin._Assets, "Spawner_Rat_RS", "Rat_RS");
        var fishSpawner = new LocationManager.CreatureSpawnerInfo(NorthernAssetsPlugin._Assets, "Spawner_Fish_RS", "Fish4_cave");
    }

    private static void LoadLocations()
    {
        LocationManager.LocationData Krampus = new LocationManager.LocationData("Krampus_DeepNorth", NorthernAssetsPlugin._Assets);
        Krampus.m_data.m_biome = Heightmap.Biome.DeepNorth;
        Krampus.m_data.m_unique = true;
        Krampus.m_data.m_minAltitude = 50f;
        Krampus.m_data.m_clearArea = true;
        Krampus.m_data.m_quantity = 20;
        Krampus.m_data.m_prioritized = true;
        Krampus.m_data.m_group = "DeepNorthDungeons";
        Krampus.m_data.m_minDistanceFromSimilar = 300f;
        Krampus.Altitude = 8000f;
        Krampus.EnvironmentScale = new Vector3(1f, 1f, 1f);
        Krampus.Environment = "";
        Krampus.SpawnInteriorEnv = false;
        ForceTimeOfDay.RegisterForcedTODEnvZone("KrampusInteriorEnv", 0.1f);

        LocationManager.LocationData Tower = new LocationManager.LocationData("Tower_DeepNorth", NorthernAssetsPlugin._Assets);
        Tower.m_data.m_biome = Heightmap.Biome.DeepNorth;
        Tower.m_data.m_unique = true;
        Tower.m_data.m_minAltitude = 10f;
        Tower.m_data.m_clearArea = true;
        Tower.m_data.m_quantity = 20;
        Tower.m_data.m_prioritized = true;
        Tower.m_data.m_group = "UniqueDeepNorth";
        Tower.m_data.m_minDistanceFromSimilar = 300f;
        Tower.m_data.m_clearArea = true;

        LocationManager.LocationData Snowman1 = new LocationManager.LocationData("Snowman_DeepNorth_1", NorthernAssetsPlugin._Assets);
        Snowman1.m_data.m_biome = Heightmap.Biome.DeepNorth;
        Snowman1.m_data.m_unique = true;
        Snowman1.m_data.m_minAltitude = 50f;
        Snowman1.m_data.m_clearArea = false;
        Snowman1.m_data.m_quantity = 10;
        Snowman1.m_data.m_prioritized = true;
        Snowman1.m_data.m_group = "Snowmen";
        Snowman1.m_data.m_groupMax = "Snowmen";
        Snowman1.m_data.m_minDistanceFromSimilar = 700f;

        LocationManager.LocationData Snowman2 = new LocationManager.LocationData("Snowman_DeepNorth_2", NorthernAssetsPlugin._Assets);
        Snowman2.m_data.m_biome = Heightmap.Biome.DeepNorth;
        Snowman2.m_data.m_unique = true;
        Snowman2.m_data.m_minAltitude = 50f;
        Snowman2.m_data.m_clearArea = false;
        Snowman2.m_data.m_quantity = 10;
        Snowman2.m_data.m_prioritized = true;
        Snowman2.m_data.m_group = "Snowmen";
        Snowman2.m_data.m_groupMax = "Snowmen";
        Snowman2.m_data.m_minDistanceFromSimilar = 700f;
        
        LocationManager.LocationData Snowman3 = new LocationManager.LocationData("Snowman_DeepNorth_3", NorthernAssetsPlugin._Assets);
        Snowman3.m_data.m_biome = Heightmap.Biome.DeepNorth;
        Snowman3.m_data.m_unique = true;
        Snowman3.m_data.m_minAltitude = 50f;
        Snowman3.m_data.m_clearArea = false;
        Snowman3.m_data.m_quantity = 10;
        Snowman3.m_data.m_prioritized = true;
        Snowman2.m_data.m_group = "Snowmen";
        Snowman2.m_data.m_groupMax = "Snowmen";
        Snowman2.m_data.m_minDistanceFromSimilar = 700f;

        LocationManager.LocationData YetiCave1 = new LocationManager.LocationData("YetiCave_DeepNorth1", NorthernAssetsPlugin._Assets);
        YetiCave1.m_data.m_biome = Heightmap.Biome.DeepNorth;
        YetiCave1.m_data.m_prioritized = true;
        YetiCave1.m_data.m_clearArea = true;
        YetiCave1.m_data.m_quantity = 20;
        YetiCave1.m_data.m_group = "DeepNorthDungeons";
        YetiCave1.m_data.m_groupMax = "DeepNorthDungeons";
        YetiCave1.Altitude = 5000f;
        YetiCave1.EnvironmentScale = new Vector3(500f, 1000f, 500f);
        YetiCave1.Environment = "Aurora";
        YetiCave1.m_data.m_minDistanceFromSimilar = 500f;
        YetiCave1.SpawnInteriorEnv = false;

        LocationManager.DungeonData YetiCaveDungeon = new LocationManager.DungeonData(NorthernAssetsPlugin._Assets, "_RoomList_YetiCave", "DG_YetiCave");
        YetiCaveDungeon.AddRoom("cave_corridor1");
        YetiCaveDungeon.AddRoom("cave_corridor2");
        YetiCaveDungeon.AddRoom("cave_corridor3");
        YetiCaveDungeon.AddRoom("cave_endcap1");
        YetiCaveDungeon.AddRoom("cave_endcap2");
        YetiCaveDungeon.AddRoom("cave_endcap3");
        YetiCaveDungeon.AddRoom("cave_entrance");
        YetiCaveDungeon.AddRoom("cave_spiral1");
        YetiCaveDungeon.AddRoom("cave_water");

        LocationManager.LocationData Crypt = new LocationManager.LocationData("Crypt_DeepNorth", NorthernAssetsPlugin._Assets);
        Crypt.m_data.m_biome = Heightmap.Biome.DeepNorth;
        Crypt.m_data.m_prioritized = true;
        Crypt.m_data.m_clearArea = true;
        Crypt.m_data.m_quantity = 20;
        Crypt.m_data.m_group = "DeepNorthDungeons";
        Crypt.m_data.m_groupMax = "DeepNorthDungeons";
        Crypt.m_data.m_minDistanceFromSimilar = 50f;
        Crypt.Environment = "Crypt";
        Crypt.SpawnInteriorEnv = false;

        LocationManager.LocationData Crypt1 = new LocationManager.LocationData("Crypt_DeepNorth1", NorthernAssetsPlugin._Assets);
        Crypt1.m_data.m_biome = Heightmap.Biome.DeepNorth;
        Crypt1.m_data.m_prioritized = true;
        Crypt1.m_data.m_clearArea = true;
        Crypt1.m_data.m_quantity = 10;
        Crypt1.m_data.m_group = "DeepNorthDungeons";
        Crypt1.m_data.m_groupMax = "DeepNorthDungeons";
        Crypt1.m_data.m_minDistanceFromSimilar = 200f;
        Crypt1.Environment = "Crypt";
        Crypt1.SpawnInteriorEnv = false;
        
        LocationManager.LocationData FinalDungeon = new LocationManager.LocationData("Dungeon_DeepNorth", NorthernAssetsPlugin._Assets);
        FinalDungeon.m_data.m_biome = Heightmap.Biome.DeepNorth;
        FinalDungeon.m_data.m_prioritized = true;
        FinalDungeon.m_data.m_unique = true;
        FinalDungeon.m_data.m_clearArea = true;
        FinalDungeon.m_data.m_quantity = 20;
        FinalDungeon.m_data.m_group = "DeepNorthDungeons";
        FinalDungeon.m_data.m_groupMax = "DeepNorthDungeons";
        FinalDungeon.m_data.m_minDistanceFromSimilar = 200f;
        FinalDungeon.Environment = "";
        FinalDungeon.EnvironmentScale = new Vector3(1f, 1f, 1f);
        FinalDungeon.Altitude = 7000f;
        FinalDungeon.m_data.m_surroundCheckDistance = 60f;
        FinalDungeon.m_data.m_surroundCheckVegetation = true;
        FinalDungeon.SpawnInteriorEnv = false;

        TriggerOfferingBowl.TriggerOffering BahometOfferingAltar = new TriggerOfferingBowl.TriggerOffering(NorthernAssetsPlugin._Assets, "BahometOffering_RS");
        BahometOfferingAltar.OfferItem = "TrophyRatKing_RS";
        BahometOfferingAltar.BossPrefab = "Bahomet_RS";
        BahometOfferingAltar.StartEffects.Add("vfx_prespawn");
        BahometOfferingAltar.StartEffects.Add("sfx_prespawn");
        BahometOfferingAltar.DoneEffects.Add("vfx_spawn");
        BahometOfferingAltar.DoneEffects.Add("sfx_spawn");
        BahometOfferingAltar.HoverText = "Sacrifice ";
        BahometOfferingAltar.Name = "Bahomet's Altar";
        BahometOfferingAltar.Message = "The deed is done, Bahomet awakes";
        BahometOfferingAltar.DestroyItemEffects.Add("vfx_offering_fire");
        BahometOfferingAltar.DestroyItemEffects.Add("sfx_dverger_fireball_rain_start");

        OfferingBowlPatches.OfferingData RatKingOfferingAltar = new OfferingBowlPatches.OfferingData("RS_Podium_2");
        RatKingOfferingAltar.BossPrefab = "RatKing_RS";
        RatKingOfferingAltar.StartEffects.Add("vfx_acid_prespawn");
        RatKingOfferingAltar.StartEffects.Add("sfx_prespawn");
        RatKingOfferingAltar.DoneEffects.Add("vfx_spawn");
        RatKingOfferingAltar.DoneEffects.Add("sfx_spawn");
        RatKingOfferingAltar.Name = "Rat King's Altar";
        RatKingOfferingAltar.Message = "The deed is done, Rat King awakes";
        
        OfferingBowlPatches.OfferingData KrampusOfferingAltar = new OfferingBowlPatches.OfferingData("ChristmasTreeAltar_RS");
        KrampusOfferingAltar.BossPrefab = "Krampus_RS";
        KrampusOfferingAltar.StartEffects.Add("vfx_prespawn");
        KrampusOfferingAltar.StartEffects.Add("sfx_prespawn");
        KrampusOfferingAltar.DoneEffects.Add("vfx_spawn");
        KrampusOfferingAltar.DoneEffects.Add("sfx_spawn");
        KrampusOfferingAltar.Name = "Krampus Altar";
        KrampusOfferingAltar.Message = "Krampus awakens";

        LocationManager.DungeonData CryptDungeon = new LocationManager.DungeonData(NorthernAssetsPlugin._Assets, "_RoomList_Dungeon1", "DG_Crypt_DeepNorth");
        CryptDungeon.AddRoom("AVA_Dungeon1_Corridor");
        CryptDungeon.AddRoom("AVA_Dungeon1_Ramp");
        CryptDungeon.AddRoom("AVA_Dungeon1_Room1");
        CryptDungeon.AddRoom("AVA_Dungeon1_Room2");
        CryptDungeon.AddRoom("AVA_Dungeon1_Room3");
        CryptDungeon.AddRoom("AVA_Dungeon1_Room4");
        CryptDungeon.AddRoom("AVA_Dungeon1_Room5");
        CryptDungeon.AddRoom("AVA_Dungeon1_Room6");
        CryptDungeon.AddRoom("AVA_Dungeon1_Start1");
        CryptDungeon.AddRoom("AVA_Dungeon1_endcaps1");
        CryptDungeon.AddRoom("AVA_Dungeon1_endcaps2");
        CryptDungeon.AddRoom("AVA_Dungeon1_endcaps3");
        CryptDungeon.AddRoom("AVA_Dungeon1_endcaps4");
        CryptDungeon.AddRoom("AVA_Dungeon1_endcaps5");

        LocationManager.DungeonData CryptDungeon1 = new LocationManager.DungeonData(NorthernAssetsPlugin._Assets, "_RoomList_Dungeon2", "DG_Crypt_DeepNorth1");
        CryptDungeon1.AddRoom("AVA_Dungeon1_Corridor");
        CryptDungeon1.AddRoom("AVA_Dungeon1_Ramp");
        CryptDungeon1.AddRoom("AVA_Dungeon1_Room1");
        CryptDungeon1.AddRoom("AVA_Dungeon1_Room2");
        CryptDungeon1.AddRoom("AVA_Dungeon1_Room3");
        CryptDungeon1.AddRoom("AVA_Dungeon1_Room4");
        CryptDungeon1.AddRoom("AVA_Dungeon1_Room5");
        CryptDungeon1.AddRoom("AVA_Dungeon1_Room6");
        CryptDungeon1.AddRoom("AVA_Dungeon1_Start2");
        CryptDungeon1.AddRoom("AVA_Dungeon1_endcaps1");
        CryptDungeon1.AddRoom("AVA_Dungeon1_endcaps2");
        CryptDungeon1.AddRoom("AVA_Dungeon1_endcaps3");
        CryptDungeon1.AddRoom("AVA_Dungeon1_endcaps4");
        CryptDungeon1.AddRoom("AVA_Dungeon1_endcaps5");
        
        LoadLocationAssets();
    }

    private static void LoadLocationAssets()
    {
        LocationManager.DoorData secretDoor = new LocationManager.DoorData(NorthernAssetsPlugin._Assets, "deepnorth_secretdoor_RS");
        secretDoor.CloneEffectsFrom = "dvergrtown_secretdoor";
        secretDoor.KeyItem = "SecretKey_RS";
        LocationManager.DoorData KrampusDoor = new LocationManager.DoorData(NorthernAssetsPlugin._Assets, "RS_KrampusDoor");
        KrampusDoor.CloneEffectsFrom = "darkwood_gate";
        KrampusDoor.KeyItem = "CryptKey_RS";
        LocationManager.DoorData BahometDoor = new LocationManager.DoorData(NorthernAssetsPlugin._Assets, "RS_BahometDoor");
        BahometDoor.CloneEffectsFrom = "darkwood_gate";
        BahometDoor.KeyItem = "TrophyBahomet_RS";
        LocationManager.DoorData DungeonGate = new LocationManager.DoorData(NorthernAssetsPlugin._Assets, "DungeonGate_RS");
        DungeonGate.CloneEffectsFrom = "dvergrtown_secretdoor";
        DungeonGate.KeyItem = "CryptKey_RS";
        LocationManager.DoubleDoorData DungeonBigGate = new LocationManager.DoubleDoorData(NorthernAssetsPlugin._Assets, "DungeonBigGate_RS");
        DungeonBigGate.CloneEffectsFrom = "darkwood_gate";
        DungeonBigGate.KeyItem = "dvergrtown_secretdoor";
        LocationManager.DoorData RatKingGate = new LocationManager.DoorData(NorthernAssetsPlugin._Assets, "RatKingGate_RS");
        RatKingGate.CloneEffectsFrom = "dvergrtown_secretdoor";
        RatKingGate.KeyItem = "TrophyRatKing_RS";
        LocationManager.DoorData SkeletonKingGate = new LocationManager.DoorData(NorthernAssetsPlugin._Assets, "SkeletonKingGate_RS");
        SkeletonKingGate.CloneEffectsFrom = "dvergrtown_secretdoor";
        SkeletonKingGate.KeyItem = "SwordLichKing_RS";
        LocationManager.DoorData DoorHatch = new LocationManager.DoorData(NorthernAssetsPlugin._Assets, "RS_DoorHatch");
        DoorHatch.CloneEffectsFrom = "darkwood_gate";
        DoorHatch.KeyItem = "CryptKey_RS";
        LocationManager.DoorData TowerDoor = new LocationManager.DoorData(NorthernAssetsPlugin._Assets, "RS_TowerDoor");
        TowerDoor.CloneEffectsFrom = "darkwood_gate";
        TowerDoor.KeyItem = "CryptKey_RS";
        LocationManager.DoorData CryptDoor = new LocationManager.DoorData(NorthernAssetsPlugin._Assets, "CryptGate_RS");
        CryptDoor.CloneEffectsFrom = "darkwood_gate";
        CryptDoor.KeyItem = "CryptKey_RS";
        LocationManager.DoorData DungeonDoor = new LocationManager.DoorData(NorthernAssetsPlugin._Assets, "DungeonDoor_RS");
        DungeonDoor.CloneEffectsFrom = "darkwood_gate";
        DungeonDoor.KeyItem = "CryptKey_RS";
        LocationManager.DoubleDoorData KrampusMainGate = new LocationManager.DoubleDoorData(NorthernAssetsPlugin._Assets, "KrampusMainGate_RS");
        KrampusMainGate.CloneEffectsFrom = "darkwood_gate";
        KrampusMainGate.KeyItem = "CryptKey_RS";
        LocationManager.DoubleDoorData TowerMainGate = new LocationManager.DoubleDoorData(NorthernAssetsPlugin._Assets, "TowerMainGate_RS");
        TowerMainGate.CloneEffectsFrom = "darkwood_gate";
        TowerMainGate.KeyItem = "CryptKey_RS";
        
        LocationManager.RegisterToScene(NorthernAssetsPlugin._Assets, "vfx_offering_fire");
        LocationManager.RegisterToScene(NorthernAssetsPlugin._Assets, "vfx_acid_prespawn");
        
        for (int index = 1; index < 13; ++index)
        {
            ItemStandPatches.ItemStandData itemStandData = new ItemStandPatches.ItemStandData(NorthernAssetsPlugin._Assets, $"CoatOfArmsStand_RS{index}");
            itemStandData.Effects.Add("vfx_pickable_pick");
            itemStandData.Effects.Add("sfx_pickable_pick");
        }
        LocationManager.RegisterToScene(NorthernAssetsPlugin._Assets, "ChristmasTreeAltar_RS");
        ItemStandPatches.ItemStandData ChristmasBoxStand = new ItemStandPatches.ItemStandData(NorthernAssetsPlugin._Assets, "ChristmasBoxStand_RS");
        ChristmasBoxStand.Effects.Add("vfx_pickable_pick");
        ChristmasBoxStand.Effects.Add("sfx_pickable_pick");
    }
}