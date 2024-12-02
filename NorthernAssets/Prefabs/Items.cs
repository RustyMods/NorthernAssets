using System;
using System.Collections.Generic;
using BepInEx.Configuration;
using ItemManager;
using NorthernAssets.Behaviors;
using NorthernAssets.Managers;
using NorthernAssets.StatusEffects;
using UnityEngine;

namespace NorthernAssets.prefabs;

public static class Items
{
    private static ConfigEntry<float> m_simpleHealthPotionHealAmount = null!;
    private static ConfigEntry<float> m_fancyHealthPotionHealAmount = null!;
    private static ConfigEntry<float> m_potentHealthPotionHealAmount = null!;
    private static ConfigEntry<float> m_simpleSpeedPotionMultiplier = null!;
    private static ConfigEntry<float> m_sweetWineStaminaAmount = null!;
    private static ConfigEntry<float> m_strengthPotionAmount = null!;
    private static ConfigEntry<float> m_etherDuration = null!;
    private static ConfigEntry<float> m_revitalizeDuration = null!;
    private static ConfigEntry<Toggle> m_expByLevel = null!;
    private static ConfigEntry<float> m_expPerTome = null!;
    
    public static void Load()
    {
        m_expByLevel = NorthernAssetsPlugin._Plugin.config("2 - Experience Tomes", "Increase till next level", Toggle.Off, "If on, tomes will give enough experience to level to next level");
        m_expPerTome = NorthernAssetsPlugin._Plugin.config("2 - Experience Tomes", "Experience Amount", 10f, "Set the amount of experience each tomes gives, if not whole level");
        LoadFoods();
        LoadMaterials();
        LoadKeys();
        LoadCoatOfArms();
        LoadWeapons();
        LoadArmors();
        LoadAlchemyMaterials();
        LoadPotions();
        LoadExperienceTomes();
        LoadLantern();
    }

    private static void LoadArmors()
    {
        Item HornedHelm = new Item(NorthernAssetsPlugin._Assets, "HelmetHorned_RS");
        HornedHelm.Name.English("Horned Cask");
        HornedHelm.Description.English("");

        Item WereWolfChest = new Item(NorthernAssetsPlugin._Assets, "ArmorWereWolfChest_RS");
        WereWolfChest.Name.English("Were-Wolf Chestpiece");
        WereWolfChest.Description.English("Rugged, sinewy design forged from dark leather and reinforced steel, shaped to enhance mobility and primal ferocity.");
        WereWolfChest.CloneArmorMaterialFrom = "ArmorFlametalChest";

        Item WereWolfLegs = new Item(NorthernAssetsPlugin._Assets, "ArmorWereWolfLegs_RS");
        WereWolfLegs.Name.English("Were-Wolf Leg Braces");
        WereWolfLegs.Description.English("Crafted from reinforced leather and metal, designed to protect the legs while maintaining agility for swift, predatory movements");
        WereWolfLegs.CloneArmorMaterialFrom = "ArmorFlametalLegs";

        Item HelmetWarlord = new Item(NorthernAssetsPlugin._Assets, "HelmetSkeletonWarlord_RS");
        HelmetWarlord.Description.English("Crafted from jagged bone and dark metal, with a skull-like visage and horned accents, exuding a chilling aura of dominance and death");
        HelmetWarlord.Name.English("Warlord Helmet");
    }

    private static void LoadFoods()
    {
        Item Lemon = new Item(NorthernAssetsPlugin._Assets, "Lemon_RS");
        Lemon.Name.English("Lemon");
        Lemon.Description.English("");
        Lemon.Configurable = Configurability.Disabled;
        
        Item Potato = new Item(NorthernAssetsPlugin._Assets, "Potato_RS");
        Potato.Name.English("Potato");
        Potato.Description.English("");
        
        Item Radish = new Item(NorthernAssetsPlugin._Assets, "Radish_RS");
        Radish.Name.English("Radish");
        Radish.Description.English("");
        
        Item Growth = new Item(NorthernAssetsPlugin._Assets, "FrozenGrowth_RS");
        Growth.Name.English("Frozen Growth");
        Growth.Description.English("Frozen flesh contains restorative properties, perfect for potions that heal or protect against cold");
        
        Item Berries = new Item(NorthernAssetsPlugin._Assets, "FrozenBerries_RS");
        Berries.Name.English("Frozen Berries");
        Berries.Description.English("Encased in a thin layer of frost, preserving their tart, intense flavor and potent nutrients");

        Item Fungi = new Item(NorthernAssetsPlugin._Assets, "Fungi_RS");
        Fungi.Name.English("Fungi");
        Fungi.Description.English("Perfect for magical potions");
        
        Item FrozenTube = new Item(NorthernAssetsPlugin._Assets, "FrozenTube_RS");
        FrozenTube.Name.English("Tube Mushroom");
        FrozenTube.Description.English("Prized for their earthy flavor and often used in potions");
        
        Item SquidMeat = new Item(NorthernAssetsPlugin._Assets, "SquidMeat_RS");
        SquidMeat.Name.English("Squid Meat");
        SquidMeat.Description.English("Thick and rubbery, with a slightly salty flavor, prized for its toughness and rich nutrients");
        SquidMeat.Configurable = Configurability.Disabled;
        
        Item BeastLeg = new Item(NorthernAssetsPlugin._Assets, "BeastLeg_RS");
        BeastLeg.Name.English("Raw Beast Leg");
        BeastLeg.Description.English("Thick with sinew and bone");
        BeastLeg.Configurable = Configurability.Disabled;
        
        Item LizardMeatRaw = new Item(NorthernAssetsPlugin._Assets, "LizardLeg_RS");
        LizardMeatRaw.Name.English("Skaldrik Leg");
        LizardMeatRaw.Description.English("Tough and sinewy, with a faintly gamey taste");
        LizardMeatRaw.Configurable = Configurability.Disabled;
        
        Item BeastChunk = new Item(NorthernAssetsPlugin._Assets, "BeastChunk_RS");
        BeastChunk.Name.English("Beast Chunk");
        BeastChunk.Description.English("Rich in natural juices and flavor");
        BeastChunk.Configurable = Configurability.Disabled;
        
        Item BeastMeat = new Item(NorthernAssetsPlugin._Assets, "BeastMeat_RS");
        BeastMeat.Name.English("Raw Beast Meat");
        BeastMeat.Description.English("Dense and fibrous, often requiring careful preparation");
        BeastMeat.Configurable = Configurability.Disabled;
        
        Item MeatPlate01 = new Item(NorthernAssetsPlugin._Assets, "MeatPlate01_RS");
        MeatPlate01.Name.English("Skaldrik Plate");
        MeatPlate01.Description.English("Tender, flavorful protein with a slightly smoky taste");
        MeatPlate01.RequiredItems.Add("LizardLeg_RS", 1);
        MeatPlate01.RequiredItems.Add("Carrot", 2);
        MeatPlate01.RequiredItems.Add("Onion", 5);
        MeatPlate01.RequiredItems.Add("DeadWood_RS", 1);
        MeatPlate01.Crafting.Add(CraftingTable.FoodPreparationTable, 1);
        
        Item MeatPlate02 = new Item(NorthernAssetsPlugin._Assets, "MeatPlate02_RS");
        MeatPlate02.Name.English("Beast Plate");
        MeatPlate02.Description.English("Tender and flavorful, seared to perfection");
        MeatPlate02.RequiredItems.Add("BeastChunk_RS", 1);
        MeatPlate02.RequiredItems.Add("BeastMeat_RS", 1);
        MeatPlate02.RequiredItems.Add("Lemon_RS", 1);
        MeatPlate02.RequiredItems.Add("Radish_RS", 3);
        MeatPlate02.Crafting.Add(CraftingTable.FoodPreparationTable, 1);
        
        Item MeatPlate03 = new Item(NorthernAssetsPlugin._Assets, "MeatPlate03_RS");
        MeatPlate03.Name.English("Beast Belly");
        MeatPlate03.Description.English("Tender and flavorful, melts to the tongue");
        MeatPlate03.RequiredItems.Add("BeastChunk_RS", 1);
        MeatPlate03.RequiredItems.Add("Raspberry", 5);
        MeatPlate03.RequiredItems.Add("Potato_RS", 3);
        MeatPlate03.RequiredItems.Add("DeadWood_RS", 1);
        MeatPlate03.Crafting.Add(CraftingTable.FoodPreparationTable, 1);
        
        Item MeatPlate04 = new Item(NorthernAssetsPlugin._Assets, "MeatPlate04_RS");
        MeatPlate04.Name.English("Pork Roast");
        MeatPlate04.Description.English("Grilled to perfection, served on a plate of vegetables");
        
        Item MeatPlate06 = new Item(NorthernAssetsPlugin._Assets, "MeatPlate06_RS");
        MeatPlate06.Name.English("Sliced Beast Meat");
        MeatPlate06.Description.English("Seared and sliced");
        MeatPlate06.RequiredItems.Add("BeastMeat_RS", 1);
        MeatPlate06.RequiredItems.Add("BeastChunk_RS", 1);
        MeatPlate06.RequiredItems.Add("DeadWood_RS", 1);
        MeatPlate06.Crafting.Add(CraftingTable.FoodPreparationTable, 1);
        
        Item MeatPlate07 = new Item(NorthernAssetsPlugin._Assets, "MeatPlate07_RS");
        MeatPlate07.Name.English("Were-Boar Bits");
        MeatPlate07.Description.English("Baked and sliced");
        MeatPlate07.RequiredItems.Add("BeastMeat_RS", 3);
        MeatPlate07.Crafting.Add(CraftingTable.FoodPreparationTable, 1);

        Item CookedBeastMeat = new Item(NorthernAssetsPlugin._Assets, "CookedBeastMeat_RS");
        CookedBeastMeat.Name.English("Beast Meat Platter");
        CookedBeastMeat.Description.English("Hearty, flavorful dish with a smoky, rich taste");
        CookedBeastMeat.RequiredItems.Add("BeastMeat_RS", 1);
        CookedBeastMeat.RequiredItems.Add("Thistle", 3);
        CookedBeastMeat.RequiredItems.Add("DeadWood_RS", 1);
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
            var prefabName = $"CoatOfArmsItem{index}_RS";
            Item item = new Item(NorthernAssetsPlugin._Assets, prefabName);
            item.Name.English(coatOfArmsNames[index]);
            item.Configurable = Configurability.Disabled;
        }
    }

    private static void LoadKeys()
    {
        Item DungeonKey = new Item(NorthernAssetsPlugin._Assets, "DungeonKey_RS");
        DungeonKey.Name.English("Cursed Key");
        DungeonKey.Description.English("Unlocks hidden realms");
        DungeonKey.Configurable = Configurability.Disabled;
        
        Item CryptKey = new Item(NorthernAssetsPlugin._Assets, "CryptKey_RS");
        CryptKey.Name.English("Crypt Key");
        CryptKey.Description.English("Access to deep north crypts");
        CryptKey.Configurable = Configurability.Disabled;
        
        Item CryptKey1 = new Item(NorthernAssetsPlugin._Assets, "SecretKey_RS");
        CryptKey1.Name.English("Secret Key");
        CryptKey1.Description.English("Access to deep north secret rooms");
        CryptKey.Crafting.Add("RS_AlchemyTable", 2);
        CryptKey.RequiredItems.Add("Platinum_RS", 1);
        CryptKey.RequiredItems.Add("RS_AlchemicEssence", 2);
    }
    
    private static void LoadMaterials()
    {
        Item BahometHorns = new Item(NorthernAssetsPlugin._Assets, "BahometHorn_RS");
        BahometHorns.Name.English("Bahomet's Horns");
        BahometHorns.Description.English("");
        BahometHorns.Configurable = Configurability.Disabled;
        Item LizardTail = new Item(NorthernAssetsPlugin._Assets, "LizardTail_RS");
        LizardTail.Name.English("Skaldrik Tail");
        LizardTail.Description.English("Flexible, scaly appendage, often used in alchemy or as a delicacy");
        LizardTail.Configurable = Configurability.Disabled;
        Item BeastEye = new Item(NorthernAssetsPlugin._Assets, "BeastEye_RS");
        BeastEye.Name.English("Eyeball");
        BeastEye.Description.English("Unblinking orb, rich with mystical energy");
        BeastEye.Configurable = Configurability.Disabled;
        Item DeadWood = new Item(NorthernAssetsPlugin._Assets, "DeadWood_RS");
        DeadWood.Name.English("Dead Wood");
        DeadWood.Description.English("Frost-covered timber, hardened by cold");
        DeadWood.Configurable = Configurability.Disabled;
        Item platinumOre = new Item(NorthernAssetsPlugin._Assets, "PlatinumOre_RS");
        platinumOre.Name.English("Platinum Ore");
        platinumOre.Description.English("Dense, silvery mineral with a lustrous sheen");
        platinumOre.Configurable = Configurability.Disabled;
        Item platinum = new Item(NorthernAssetsPlugin._Assets, "Platinum_RS");
        platinum.Name.English("Platinum");
        platinum.Description.English("Rare, heavy, silvery metal known for its durability");
        platinum.Configurable = Configurability.Disabled;
        Conversion platinumConversion = new Conversion(platinum);
        platinumConversion.Piece = ConversionPiece.BlastFurnace;
        platinumConversion.Input = "PlatinumOre_RS";
        Item Present1 = new Item(NorthernAssetsPlugin._Assets, "ChristmasPresent_RS");
        Present1.Name.English("Christmas Present");
        Present1.Description.English("Ominous, intricately wrapped package, exuding dark magic");
        Present1.Configurable = Configurability.Disabled;
        Item HairBundle = new Item(NorthernAssetsPlugin._Assets, "HairBundle_RS");
        HairBundle.Name.English("Fur Scraps");
        HairBundle.Description.English("Rough, uneven patches of thick, coarse fur");
        HairBundle.Configurable = Configurability.Disabled;
    }

    private static void LoadWeapons()
    {
        Item SpearCatFish = new Item(NorthernAssetsPlugin._Assets, "SpearCatFish_RS");
        SpearCatFish.Name.English("Catfish Spear");
        SpearCatFish.Description.English("Lightweight, yet sturdy weapon, crafted from sharpened, durable bones");
        SpearCatFish.HitEffects.Add("vfx_arrowhit");
        SpearCatFish.HitEffects.Add("sfx_spear_hit");
        SpearCatFish.HitEffects.Add("fx_hit_camshake");
        SpearCatFish.BlockEffects.Add("sfx_wood_blocked");
        SpearCatFish.BlockEffects.Add("vfx_blocked");
        SpearCatFish.BlockEffects.Add("fx_block_camshake");
        SpearCatFish.StartEffects.Add("sfx_spear_throw");
        SpearCatFish.TriggerEffects.Add("fx_swing_camshake");
        SpearCatFish.TrailStartEffects.Add("sfx_spear_poke");
        ProjectileMan.ProjectileData SpearCatFishProjectile = new ProjectileMan.ProjectileData("projectile_spear_catfish", NorthernAssetsPlugin._Assets);
        SpearCatFishProjectile.m_hitEffects.Add("sfx_arrow_hit");
        
        Item spearAncient = new Item(NorthernAssetsPlugin._Assets, "SpearAncient_RS");
        spearAncient.Name.English("Ancient Spear");
        spearAncient.Description.English("Etched with symbols or markings, prized for its historical significance");
        spearAncient.HitEffects.Add("vfx_arrowhit");
        spearAncient.HitEffects.Add("sfx_spear_hit");
        spearAncient.HitEffects.Add("fx_hit_camshake");
        spearAncient.BlockEffects.Add("sfx_wood_blocked");
        spearAncient.BlockEffects.Add("vfx_blocked");
        spearAncient.BlockEffects.Add("fx_block_camshake");
        spearAncient.StartEffects.Add("sfx_spear_throw");
        spearAncient.TriggerEffects.Add("fx_swing_camshake");
        spearAncient.TrailStartEffects.Add("sfx_spear_poke");
        ProjectileMan.ProjectileData spearProjectile = new ProjectileMan.ProjectileData("projectile_spear_ancient", NorthernAssetsPlugin._Assets);
        spearProjectile.m_hitEffects.Add("sfx_arrow_hit");
        
        Item SkeletonKnife = new(NorthernAssetsPlugin._Assets, "KnifeSkeletonRogue_RS");
        SkeletonKnife.Name.English("Rogue's Knife");
        SkeletonKnife.Description.English("Jagged, crude blade with a rough, unfinished appearance");
        SkeletonKnife.HitEffects.Add("vfx_HitSparks");
        SkeletonKnife.HitEffects.Add("sfx_arrow_hit");
        SkeletonKnife.HitEffects.Add("fx_hit_camshake_knife");
        SkeletonKnife.BlockEffects.Add("sfx_metal_blocked");
        SkeletonKnife.BlockEffects.Add("vfx_blocked");
        SkeletonKnife.BlockEffects.Add("fx_block_camshake");
        SkeletonKnife.TriggerEffects.Add("fx_swing_camshake");
        SkeletonKnife.TrailStartEffects.Add("sfx_knife_swing");
        
        Item SkeletonBow = new Item(NorthernAssetsPlugin._Assets, "BowSkeleton_RS");
        SkeletonBow.Name.English("Skeleton's Bow");
        SkeletonBow.Description.English("Weathered, gnarled weapon, shaped from brittle, aged branches");
        SkeletonBow.BlockEffects.Add("sfx_wood_blocked");
        SkeletonBow.BlockEffects.Add("vfx_blocked");
        SkeletonBow.BlockEffects.Add("fx_block_camshake");
        SkeletonBow.HoldEffects.Add("sfx_bow_draw");
        SkeletonBow.TriggerEffects.Add("fx_swing_camshake");
        SkeletonBow.TriggerEffects.Add("vfx_ashlandsbow_blood_fire");
        SkeletonBow.TriggerEffects.Add("sfx_bow_fire");
        SkeletonBow.TriggerEffects.Add("sfx_weapons_blood_impact");
        
        Item SkeletonStaff = new(NorthernAssetsPlugin._Assets, "StaffSkeletonMage_RS");
        SkeletonStaff.Name.English("Skeleton Mage Staff");
        SkeletonStaff.Description.English("Eerie, gnarled instrument, crafted from twisted, hollow roots");
        SkeletonStaff.BlockEffects.Add("sfx_wood_blocked");
        SkeletonStaff.BlockEffects.Add("vfx_blocked");
        SkeletonStaff.StartEffects.Add("sfx_staff_grenade_cast");
        
        Item SkeletonSword = new(NorthernAssetsPlugin._Assets, "SwordSkeletonWarlord_RS");
        SkeletonSword.Name.English("Warlord's Blade");
        SkeletonSword.Description.English("Forged for power and precision");
        SkeletonSword.HitEffects.Add("vfx_HitSparks");
        SkeletonSword.HitEffects.Add("sfx_sword_hit");
        SkeletonSword.HitEffects.Add("fx_hit_camshake");
        SkeletonSword.HitEffects.Add("sfx_weapons_blood_impact");
        SkeletonSword.BlockEffects.Add("sfx_metal_blocked");
        SkeletonSword.BlockEffects.Add("vfx_blocked");
        SkeletonSword.BlockEffects.Add("fx_block_camshake");
        SkeletonSword.TriggerEffects.Add("fx_swing_camshake");
        SkeletonSword.TrailStartEffects.Add("sfx_sword_swing");
        SkeletonSword.AttackEffects.Add("fx_bloodweapon_hit");
        SkeletonSword.EquipEffects.Add("sfx_weapons_blood_enable");
        
        Item SkeletonWarriorSword = new(NorthernAssetsPlugin._Assets, "SwordSkeletonWarrior_RS");
        SkeletonWarriorSword.Name.English("Black Blade");
        SkeletonWarriorSword.Description.English("Ominous sword with a jet-black metal edge");
        SkeletonWarriorSword.HitEffects.Add("vfx_HitSparks");
        SkeletonWarriorSword.HitEffects.Add("sfx_sword_hit");
        SkeletonWarriorSword.HitEffects.Add("fx_hit_camshake");
        SkeletonWarriorSword.HitEffects.Add("sfx_weapons_blood_impact");
        SkeletonWarriorSword.BlockEffects.Add("sfx_metal_blocked");
        SkeletonWarriorSword.BlockEffects.Add("vfx_blocked");
        SkeletonWarriorSword.BlockEffects.Add("fx_block_camshake");
        SkeletonWarriorSword.TriggerEffects.Add("fx_swing_camshake");
        SkeletonWarriorSword.TrailStartEffects.Add("sfx_sword_swing");
        
        Item SkeletonShield = new Item(NorthernAssetsPlugin._Assets, "ShieldSkeletonWarrior_RS");
        SkeletonShield.Name.English("Skeleton Buckler");
        SkeletonShield.Description.English("Rugged defense crafted from various discarded materials");
        SkeletonShield.HitEffects.Add("vfx_clubhit");
        SkeletonShield.HitEffects.Add("sfx_club_hit");
        SkeletonShield.BlockEffects.Add("sfx_wood_blocked");
        SkeletonShield.BlockEffects.Add("vfx_blocked");
        SkeletonShield.BlockEffects.Add("fx_block_camshake");
        SkeletonShield.StartEffects.Add("sfx_club_swing");
        
        Item SkeletonBowInfinite = new Item(NorthernAssetsPlugin._Assets, "BowSkeletonInfinite_RS");
        SkeletonBowInfinite.Name.English("Skeleton's Bow");
        SkeletonBowInfinite.BlockEffects.Add("sfx_wood_blocked");
        SkeletonBowInfinite.BlockEffects.Add("vfx_blocked");
        SkeletonBowInfinite.BlockEffects.Add("fx_block_camshake");
        SkeletonBowInfinite.HoldEffects.Add("sfx_bow_draw");
        SkeletonBowInfinite.TriggerEffects.Add("fx_swing_camshake");
        SkeletonBowInfinite.TriggerEffects.Add("vfx_ashlandsbow_blood_fire");
        SkeletonBowInfinite.TriggerEffects.Add("sfx_bow_fire");
        SkeletonBowInfinite.TriggerEffects.Add("sfx_weapons_blood_impact");
        SkeletonBowInfinite.Configurable = Configurability.Disabled;
        
        Item ForgottenArrow = new Item(NorthernAssetsPlugin._Assets, "ArrowForgotten_RS");
        ForgottenArrow.Name.English("Forgotten Arrow");
        
        Item DualKnives = new Item(NorthernAssetsPlugin._Assets, "KnifeSkeletonDual_RS");
        DualKnives.Name.English("Skeleton Knives");
        DualKnives.Description.English("Sturdy, utilitarian blades, with uneven edges and a raw, primitive feel");
        DualKnives.HitEffects.Add("vfx_HitSparks");
        DualKnives.HitEffects.Add("sfx_arrow_hit");
        DualKnives.HitEffects.Add("fx_hit_camshake_knife");
        DualKnives.BlockEffects.Add("sfx_metal_blocked");
        DualKnives.BlockEffects.Add("vfx_blocked");
        DualKnives.BlockEffects.Add("fx_block_camshake");
        DualKnives.TriggerEffects.Add("fx_swing_camshake");
        DualKnives.TrailStartEffects.Add("sfx_knife_swing");
        DualKnives.RequiredItems.Add("KnifeSkeletonRogue_RS", 2);
        DualKnives.Crafting.Add("RS_AlchemyTable", 2);
        
        Item daneBattleAxe = new Item(NorthernAssetsPlugin._Assets, "DaneBattleaxe_RS");
        daneBattleAxe.Name.English("Dane Battleaxe");
        daneBattleAxe.Description.English("Designed for powerful strikes");
        daneBattleAxe.HitEffects.Add("vfx_clubhit");
        daneBattleAxe.HitEffects.Add("sfx_battleaxe_hit");
        daneBattleAxe.HitEffects.Add("fx_hit_camshake");
        daneBattleAxe.BlockEffects.Add("sfx_wood_blocked");
        daneBattleAxe.BlockEffects.Add("vfx_blocked");
        daneBattleAxe.BlockEffects.Add("fx_block_camshake");
        daneBattleAxe.TriggerEffects.Add("fx_swing_camshake");
        daneBattleAxe.TrailStartEffects.Add("sfx_battleaxe_swing_wosh");
        
        Item Flail = new Item(NorthernAssetsPlugin._Assets, "Flail_RS");
        Flail.Name.English("Flail");
        Flail.Description.English("Designed for crushing and puncturing");
        Flail.HitEffects.Add("vfx_clubhit");
        Flail.HitEffects.Add("sfx_club_hit");
        Flail.HitEffects.Add("fx_hit_camshake");
        Flail.BlockEffects.Add("sfx_metal_blocked");
        Flail.BlockEffects.Add("vfx_blocked");
        Flail.BlockEffects.Add("fx_block_camshake");
        Flail.TriggerEffects.Add("fx_swing_camshake");
        Flail.TrailStartEffects.Add("sfx_club_swing");

        Item MorningStar = new Item(NorthernAssetsPlugin._Assets, "MorningStar_RS");
        MorningStar.Name.English("Morning Star");
        MorningStar.Description.English("Designed for crushing and puncturing");
        MorningStar.HitEffects.Add("vfx_clubhit");
        MorningStar.HitEffects.Add("sfx_club_hit");
        MorningStar.HitEffects.Add("fx_hit_camshake");
        MorningStar.BlockEffects.Add("sfx_metal_blocked");
        MorningStar.BlockEffects.Add("vfx_blocked");
        MorningStar.BlockEffects.Add("fx_block_camshake");
        MorningStar.TriggerEffects.Add("fx_swing_camshake");
        MorningStar.TrailStartEffects.Add("sfx_club_swing");
        
        Item BowElf = new Item(NorthernAssetsPlugin._Assets, "BowElvenLong_RS");
        BowElf.Name.English("Elven Longbow");
        BowElf.Description.English("Flexible, reinforced wood, with a smooth, refined design");
        BowElf.BlockEffects.Add("sfx_wood_blocked");
        BowElf.BlockEffects.Add("vfx_blocked");
        BowElf.BlockEffects.Add("fx_block_camshake");
        BowElf.HoldEffects.Add("sfx_bow_draw");
        BowElf.TriggerEffects.Add("fx_swing_camshake");
        BowElf.TriggerEffects.Add("vfx_bow_fire");
        BowElf.TriggerEffects.Add("sfx_bow_fire");

        Item AxeAncient = new Item(NorthernAssetsPlugin._Assets, "AxeAncient_RS");
        AxeAncient.Name.English("Sacred Axe");
        AxeAncient.Description.English("Intricately engraved with symbols");
        AxeAncient.HitEffects.Add("vfx_clubhit");
        AxeAncient.HitEffects.Add("sfx_axe_hit");
        AxeAncient.HitEffects.Add("sfx_axe_hit");   
        AxeAncient.BlockEffects.Add("sfx_metal_blocked");
        AxeAncient.BlockEffects.Add("vfx_blocked");
        AxeAncient.BlockEffects.Add("fx_block_camshake");
        AxeAncient.TriggerEffects.Add("fx_swing_camshake");
        AxeAncient.TrailStartEffects.Add("sfx_axe_swing");
        
        Item FistWereWolf = new Item(NorthernAssetsPlugin._Assets, "FistWereWolfClaw_RS");
        FistWereWolf.Name.English("Were-Wolf Braces");
        FistWereWolf.Description.English("Rugged, yet flexible");
        FistWereWolf.HitEffects.Add("sfx_sword_hit");
        FistWereWolf.HitEffects.Add("vfx_HitSparks");
        FistWereWolf.HitEffects.Add("sfx_unarmed_hit");
        FistWereWolf.BlockEffects.Add("sfx_fist)metal_blocked");
        FistWereWolf.StartEffects.Add("sfx_unarmed_swing");
        FistWereWolf.TriggerEffects.Add("fx_swing_camshake");
        FistWereWolf.TrailStartEffects.Add("sfx_claw_swing");
        FistWereWolf.DropsFrom.Add("WereWolf_RS", 0.1f, 1, 1, false);
        
        ProjectileMan.ProjectileData SkeletonArrowProjectile = new ProjectileMan.ProjectileData("bow_arrow_forgotten_projectile", NorthernAssetsPlugin._Assets);
        SkeletonArrowProjectile.m_hitEffects.Add("sfx_arrow_hit");
        SkeletonArrowProjectile.m_hitEffects.Add("vfx_arrowhit");
        Item ArrowSkeleton = new Item(NorthernAssetsPlugin._Assets, "ArrowSkeleton_RS");
        ArrowSkeleton.Name.English("Archer's Arrow");
        ArrowSkeleton.Description.English("Imbued with ethereal energy");

        ProjectileMan.ProjectileData ArrowSkeletonProjectile = new ProjectileMan.ProjectileData("bow_arrow_skeleton_projectile", NorthernAssetsPlugin._Assets);
        ArrowSkeletonProjectile.m_hitEffects.Add("sfx_arrow_hit");
        ArrowSkeletonProjectile.m_hitEffects.Add("vfx_arrowhit");
        
        Item SledgeSkeleton = new Item(NorthernAssetsPlugin._Assets, "SledgeSkeleton_RS");
        SledgeSkeleton.Name.English("Skeleton Maze");
        SledgeSkeleton.Description.English("Designed for delivering powerful strikes");
        SledgeSkeleton.BlockEffects.Add("sfx_metal_blocked");
        SledgeSkeleton.BlockEffects.Add("vfx_blocked");
        SledgeSkeleton.BlockEffects.Add("fx_block_camshake");
        SledgeSkeleton.StartEffects.Add("sfx_sledge_swing");
        SledgeSkeleton.TriggerEffects.Add("fx_swing_camshake");
        SledgeSkeleton.TriggerEffects.Add("fx_sledge_demolisher_hit");
        SledgeSkeleton.TrailStartEffects.Add("sfx_sledge_swing");
        
        Item SwordSkeletonAntler = new Item(NorthernAssetsPlugin._Assets, "SwordSkeleton_RS");
        SwordSkeletonAntler.Name.English("Antler Sword");
        SwordSkeletonAntler.Description.English("Exuding a wild, primal energy");
        SwordSkeletonAntler.HitEffects.Add("vfx_HitSparks");
        SwordSkeletonAntler.HitEffects.Add("sfx_sword_hit");
        SwordSkeletonAntler.HitEffects.Add("fx_hit_camshake");
        SwordSkeletonAntler.BlockEffects.Add("sfx_metal_blocked");
        SwordSkeletonAntler.BlockEffects.Add("vfx_blocked");
        SwordSkeletonAntler.BlockEffects.Add("fx_block_camshake");
        SwordSkeletonAntler.TriggerEffects.Add("fx_swing_camshake");
        SwordSkeletonAntler.TriggerEffects.Add("sfx_sword_swing");
        SwordSkeletonAntler.TrailStartEffects.Add("sfx_sword_swing");
        SwordSkeletonAntler.StartEffects.Add("sfx_sword_swing");
        
        Item CrossBowSkeleton = new Item(NorthernAssetsPlugin._Assets, "CrossBowSkeleton_RS");
        CrossBowSkeleton.Name.English("Skeleton Crossbow");
        CrossBowSkeleton.Description.English("Its frame dark and foreboding");
        CrossBowSkeleton.BlockEffects.Add("sfx_wood_blocked");
        CrossBowSkeleton.BlockEffects.Add("vfx_blocked");
        CrossBowSkeleton.BlockEffects.Add("fx_block_Camshake");
        CrossBowSkeleton.HoldEffects.Add("sfx_bow_draw");
        CrossBowSkeleton.TriggerEffects.Add("fx_swing_camshake");
        CrossBowSkeleton.TriggerEffects.Add("vfx_arbalest_fire");
        CrossBowSkeleton.TriggerEffects.Add("sfx_arbalest_fire");
    }
    
    private static void LoadAlchemyMaterials()
    {
        Item alchemicEssence = new(NorthernAssetsPlugin._Assets, "RS_AlchemicEssence");
        alchemicEssence.Name.English("Alchemic Essence");
        alchemicEssence.Description.English("A distilled, transformative core of a substance, believed by ancient alchemists to hold the purest, most potent properties of matter, embodying the mystical balance between the physical and spiritual realms.");
        alchemicEssence.Crafting.Add("RS_AlchemyTable", 1);
        alchemicEssence.CraftAmount = 1;
        alchemicEssence.RequiredItems.Add("HairBundle_RS", 10);
        alchemicEssence.RequiredItems.Add("Crystal", 1);
        alchemicEssence.RequiredItems.Add("Bloodbag", 1);
        alchemicEssence.RequiredItems.Add("GreydwarfEye", 2);
        
        Item forsakenEssence = new(NorthernAssetsPlugin._Assets, "RS_ForsakenEssence");
        forsakenEssence.Name.English("Forsaken Essence");
        forsakenEssence.Description.English("Corrupted energy drawn from ancient beasts, twisted by darkness, their primal power tainted and their spirits lost to time.");
        forsakenEssence.Crafting.Add("RS_AlchemyTable", 1);
        forsakenEssence.CraftAmount = 1;
        forsakenEssence.RequiredItems.Add("TrophyEikthyr", 1);
        forsakenEssence.RequiredItems.Add("TrophyTheElder", 1);
        forsakenEssence.RequiredItems.Add("TrophyBonemass", 1);
        forsakenEssence.RequiredItems.Add("TrophyDragonQueen", 1);
        forsakenEssence.RequiredItems.Add("TrophyGoblinKing", 1);
        forsakenEssence.RequiredItems.Add("TrophySeekerQueen", 1);
        forsakenEssence.RequiredItems.Add("TrophyFader", 1);
        forsakenEssence.RequireOnlyOneIngredient = true;
    }

    private static void LoadPotions()
    {
        LoadHealthPotions();
        LoadSpeedPotions();
        LoadStrengthPotions();
        LoadWindPotions();
        LoadBottleItems();
        
        Item repairPotion = new(NorthernAssetsPlugin._Assets, "RS_RepairPotion");
        repairPotion.Name.English("Repair Potion");
        repairPotion.Description.English("Repairs inventory equipment");
        repairPotion.Crafting.Add("RS_AlchemyTable", 1);
        repairPotion.CraftAmount = 1;
        repairPotion.RequiredItems.Add("Platinum_RS", 5);
        repairPotion.RequiredItems.Add("Crystal", 2);
        repairPotion.RequiredItems.Add("BeastEye_RS", 5);
        repairPotion.RequiredItems.Add("RS_AlchemicEssence", 1);
        SE_Potions RepairSE = ScriptableObject.CreateInstance<SE_Potions>();
        RepairSE.name = "SE_Repair";
        RepairSE.m_duration = NorthernAssetsPlugin._Plugin.config("Repair Potion", "Cooldown", 30f, "Set cooldown");
        RepairSE.m_name = $"${repairPotion.Name.Key}";
        RepairSE.m_cloneEffectsFrom = "Potion_stamina_minor";
        RepairSE.m_icon = repairPotion.Prefab.GetComponent<ItemDrop>().m_itemData.GetIcon();
        repairPotion.AddConsumeStatusEffect(RepairSE);
        Potion.RegisterAction("$item_repairpotion", (player, inventory, item) =>
        {
            if (!player.CanConsumeItem(item, true)) return;
            List<ItemDrop.ItemData> tempWornItems = new();
            player.GetInventory().GetWornItems(tempWornItems);
            if (tempWornItems.Count <= 0)
            {
                player.Message(MessageHud.MessageType.Center, "No items to repair");
                return;
            }
            if (item.m_shared.m_consumeStatusEffect)
            {
                player.GetSEMan().AddStatusEffect(item.m_shared.m_consumeStatusEffect, true);
            }
            foreach (var prefab in tempWornItems)
            {
                if (!prefab.m_shared.m_canBeReparied) continue;
                prefab.m_durability = prefab.GetMaxDurability();
            }
            inventory.RemoveOneItem(item);
            player.StartEmote("drink");
        });
    }
    
    private static void LoadHealthPotions()
    {
        m_simpleHealthPotionHealAmount = NorthernAssetsPlugin._Plugin.config("Simple Health Potion", "Heal Amount", 50f, "Set heal amount");
        Item healthPotion1 = new(NorthernAssetsPlugin._Assets, "RS_HealthPotion_1");
        healthPotion1.Name.English("Simple Health Potion");
        healthPotion1.Description.English($"Heals <color=orange>{m_simpleHealthPotionHealAmount.Value}</color> health points");
        healthPotion1.Crafting.Add("RS_AlchemyTable", 1);
        healthPotion1.CraftAmount = 1;
        healthPotion1.RequiredItems.Add("Dandelion", 10);
        healthPotion1.RequiredItems.Add("Crystal", 1);
        healthPotion1.RequiredItems.Add("Honey", 5);
        healthPotion1.RequiredItems.Add("RS_AlchemicEssence", 1);
        m_itemConfigData["Simple Health Potion"] = new("RS_HealthPotion_1", "$item_rs_healthpotion_1", "$heals ", " $health_points");
        m_simpleHealthPotionHealAmount.SettingChanged += OnItemConfigChange;
        SE_Potions SESimpleHealthPotion = ScriptableObject.CreateInstance<SE_Potions>();
        SESimpleHealthPotion.name = "SE_SimpleHealthPotion";
        SESimpleHealthPotion.m_name = $"${healthPotion1.Name.Key}";
        SESimpleHealthPotion.m_healAmount = m_simpleHealthPotionHealAmount;
        SESimpleHealthPotion.m_duration = NorthernAssetsPlugin._Plugin.config("Simple Health Potion", "Cooldown", 50f, "Set cooldown");
        SESimpleHealthPotion.m_icon = healthPotion1.Prefab.GetComponent<ItemDrop>().m_itemData.GetIcon();
        SESimpleHealthPotion.m_cloneEffectsFrom = "Potion_health_medium";
        SESimpleHealthPotion.m_category = "RS_HealthPotion";
        healthPotion1.AddConsumeStatusEffect(SESimpleHealthPotion);
        
        Potion.RegisterAction("$item_rs_healthpotion_1", (player, inventory, item) =>
        {
            if (!player.CanConsumeItem(item, true)) return;
            if (item.m_shared.m_consumeStatusEffect)
            {
                player.GetSEMan().AddStatusEffect(item.m_shared.m_consumeStatusEffect, true);
            }
            if (item.m_shared.m_food > 0.0) player.EatFood(item);
            inventory.RemoveOneItem(item);
            player.StartEmote("drink");
        });

        m_fancyHealthPotionHealAmount = NorthernAssetsPlugin._Plugin.config("Fancy Health Potion", "Heal Amount", 100f, "Set heal amount");
        Item healthPotion2 = new(NorthernAssetsPlugin._Assets, "RS_HealthPotion_2");
        healthPotion2.Name.English("Fancy Health Potion");
        healthPotion2.Description.English($"Heals <color=orange>{m_fancyHealthPotionHealAmount.Value}</color> health points");
        healthPotion2.Crafting.Add("RS_AlchemyTable", 2);
        healthPotion2.CraftAmount = 1;
        healthPotion2.RequiredItems.Add("Bloodbag", 10);
        healthPotion2.RequiredItems.Add("Crystal", 1);
        healthPotion2.RequiredItems.Add("Honey", 5);
        healthPotion2.RequiredItems.Add("RS_AlchemicEssence", 1);
        m_itemConfigData["Fancy Health Potion"] = new("RS_HealthPotion_1", "$item_rs_healthpotion_1", "$heals ", " $health_points");
        m_simpleHealthPotionHealAmount.SettingChanged += OnItemConfigChange;

        SE_Potions SEFancyHealthPotion = ScriptableObject.CreateInstance<SE_Potions>();
        SEFancyHealthPotion.name = "SE_FancyHealthPotion";
        SEFancyHealthPotion.m_name = $"${healthPotion2.Name.Key}";
        SEFancyHealthPotion.m_duration = NorthernAssetsPlugin._Plugin.config("Fancy Health Potion", "Cooldown", 50f, "Set cooldown");
        SEFancyHealthPotion.m_healAmount = m_fancyHealthPotionHealAmount;
        SEFancyHealthPotion.m_icon = healthPotion2.Prefab.GetComponent<ItemDrop>().m_itemData.GetIcon();
        SEFancyHealthPotion.m_cloneEffectsFrom = "Potion_health_medium";
        SEFancyHealthPotion.m_category = "RS_HealthPotion";
        healthPotion2.AddConsumeStatusEffect(SEFancyHealthPotion);
       Potion.RegisterAction("$item_rs_healthpotion_2", (player, inventory, item) =>
        {
            if (!player.CanConsumeItem(item, true)) return;
            if (item.m_shared.m_consumeStatusEffect)
            {
                player.GetSEMan().AddStatusEffect(item.m_shared.m_consumeStatusEffect, true);
            }
            if (item.m_shared.m_food > 0.0) player.EatFood(item);
            inventory.RemoveOneItem(item);
            player.StartEmote("drink");
        });

        m_potentHealthPotionHealAmount = NorthernAssetsPlugin._Plugin.config("Potent Health Potion", "Heal Amount", 150f, "Set heal amount");
        Item healthPotion3 = new(NorthernAssetsPlugin._Assets, "RS_HealthPotion_3");
        healthPotion3.Name.English("Potent Health Potion");
        healthPotion3.Description.English($"Heals <color=orange>{m_potentHealthPotionHealAmount.Value}</color> health points");
        healthPotion3.Crafting.Add("RS_AlchemyTable", 3);
        healthPotion3.CraftAmount = 1;
        healthPotion3.RequiredItems.Add("SerpentScale", 10);
        healthPotion3.RequiredItems.Add("Crystal", 1);
        healthPotion3.RequiredItems.Add("Honey", 5);
        healthPotion3.RequiredItems.Add("RS_AlchemicEssence", 1);
        m_itemConfigData["Potent Health Potion"] = new("RS_HealthPotion_1", "$item_rs_healthpotion_1", "$heals ", " $health_points");
        m_simpleHealthPotionHealAmount.SettingChanged += OnItemConfigChange;

        SE_Potions SEPotentHealthPotion = ScriptableObject.CreateInstance<SE_Potions>();
        SEPotentHealthPotion.name = "SE_PotentHealthPotion";
        SEPotentHealthPotion.m_name = $"${healthPotion3.Name.Key}";
        SEPotentHealthPotion.m_duration = NorthernAssetsPlugin._Plugin.config("Potent Health Potion", "Cooldown", 50f, "Set cooldown");
        SEPotentHealthPotion.m_healAmount = m_potentHealthPotionHealAmount;
        SEPotentHealthPotion.m_icon = healthPotion3.Prefab.GetComponent<ItemDrop>().m_itemData.GetIcon();
        SEPotentHealthPotion.m_cloneEffectsFrom = "Potion_health_medium";
        SEPotentHealthPotion.m_category = "RS_HealthPotion";
        healthPotion3.AddConsumeStatusEffect(SEPotentHealthPotion);
        Potion.RegisterAction("$item_rs_healthpotion_3", (player, inventory, item) =>
        {
            if (!player.CanConsumeItem(item, true)) return;
            if (item.m_shared.m_consumeStatusEffect)
            {
                player.GetSEMan().AddStatusEffect(item.m_shared.m_consumeStatusEffect, true);
            }
            if (item.m_shared.m_food > 0.0) player.EatFood(item);
            inventory.RemoveOneItem(item);
            player.StartEmote("drink");
        });
    }

    private static void LoadSpeedPotions()
    {
        m_simpleSpeedPotionMultiplier = NorthernAssetsPlugin._Plugin.config("Simple Speed Potion", "Speed Multiplier", 1.05f, "Set speed multiplier");
        Item speedPotion1 = new(NorthernAssetsPlugin._Assets, "RS_SpeedPotion_1");
        speedPotion1.Name.English("Simple Speed Potion");
        speedPotion1.Description.English($"Increases speed by <color=orange>{Mathf.CeilToInt(m_simpleSpeedPotionMultiplier.Value * 100 - 100)}</color>%");
        speedPotion1.Crafting.Add("RS_AlchemyTable", 1);
        speedPotion1.CraftAmount = 1;
        speedPotion1.RequiredItems.Add("RS_AlchemicEssence", 1);
        speedPotion1.RequiredItems.Add("Crystal", 1);
        speedPotion1.RequiredItems.Add("Honey", 5);
        speedPotion1.RequiredItems.Add("DeerMeat", 2);
        m_itemConfigData["Simple Speed Potion"] = new("RS_SpeedPotion_1", "$item_rs_speedpotion_1", "$increase_speed_by ", "%", true);
        m_simpleSpeedPotionMultiplier.SettingChanged += OnItemConfigChange;
        SE_Potions speedPower = ScriptableObject.CreateInstance<SE_Potions>();
        speedPower.name = "SE_SimpleSpeedPotion";
        speedPower.m_name = $"${speedPotion1.Name.Key}";
        speedPower.m_speedConfig = m_simpleSpeedPotionMultiplier;
        speedPower.m_duration = NorthernAssetsPlugin._Plugin.config("Simple Speed Potion", "Duration", 10f, "Set length of potion effect");;
        speedPower.m_icon = speedPotion1.Prefab.GetComponent<ItemDrop>().m_itemData.GetIcon();
        speedPower.m_cloneEffectsFrom = "Potion_stamina_minor";
        speedPotion1.AddConsumeStatusEffect(speedPower);
    }

    private static void LoadWindPotions()
    {
        Item windPotion = new(NorthernAssetsPlugin._Assets, "RS_WindPotion");
        windPotion.Name.English("Sailor Delight");
        windPotion.Description.English("Sets the wind to your back");
        windPotion.Crafting.Add("RS_AlchemyTable", 3);
        windPotion.CraftAmount = 1;
        windPotion.RequiredItems.Add("RS_AlchemicEssence", 1);
        windPotion.RequiredItems.Add("RS_ForsakenEssence", 1);
        windPotion.RequiredItems.Add("FreezeGland", 5);
        windPotion.RequiredItems.Add("Dandelion", 10);
        SE_Potions windPower = ScriptableObject.CreateInstance<SE_Potions>();
        windPower.name = "SE_WindPotion";
        windPower.m_duration = NorthernAssetsPlugin._Plugin.config("Sailor Delight", "Duration", 60f, "Set the length of the potion effect");
        windPower.m_attributes = StatusEffect.StatusAttribute.SailingPower;
        windPower.m_icon = windPotion.Prefab.GetComponent<ItemDrop>().m_itemData.GetIcon();
        windPower.m_cloneEffectsFrom = "Potion_stamina_lingering";
        windPower.m_name = $"${windPotion.Name.Key}";
        windPotion.AddConsumeStatusEffect(windPower);
        Potion.RegisterAction("$item_rs_windpotion", (player, inventory, item) =>
        {
            if (!player.CanConsumeItem(item, true)) return;
            if (item.m_shared.m_consumeStatusEffect)
            {
                player.GetSEMan().AddStatusEffect(item.m_shared.m_consumeStatusEffect, true);
            }
            if (item.m_shared.m_food > 0.0) player.EatFood(item);
            inventory.RemoveOneItem(item);
            player.StartEmote("drink");
        });
    }

    private static void LoadStrengthPotions()
    {
        m_strengthPotionAmount = NorthernAssetsPlugin._Plugin.config("Strength Potion", "Carry Weight", 50f, "Set the amount of added carry weight");
        Item strengthPotion = new(NorthernAssetsPlugin._Assets, "RS_StrengthPotion");
        strengthPotion.Name.English("Strength Potion");
        strengthPotion.Description.English($"Increase carry weight by <color=orange>{m_strengthPotionAmount.Value}</color>");
        strengthPotion.Crafting.Add("RS_AlchemyTable", 2);
        strengthPotion.RequiredItems.Add("RS_AlchemicEssence", 1);
        strengthPotion.RequiredItems.Add("RS_ForsakenEssence", 1);
        strengthPotion.RequiredItems.Add("GreydwarfEye", 10);
        strengthPotion.RequiredItems.Add("Dandelion", 5);
        SE_Potions strengthPower = ScriptableObject.CreateInstance<SE_Potions>();
        strengthPower.name = "SE_StrengthPotion";
        strengthPower.m_duration = NorthernAssetsPlugin._Plugin.config("Strength Potion", "Duration", 60f, "Set the length of the potion effect");
        strengthPower.m_carryWeightConfig = m_strengthPotionAmount;
        strengthPower.m_icon = strengthPotion.Prefab.GetComponent<ItemDrop>().m_itemData.GetIcon();
        strengthPower.m_cloneEffectsFrom = "Potion_tasty";
        strengthPower.m_name = $"${strengthPotion.Name.Key}";
        strengthPotion.AddConsumeStatusEffect(strengthPower);
        m_itemConfigData["Strength Potion"] = new("RS_StrengthPotion", "$item_rs_strengthpotion", "$increase_carry_weight ", "", false);
        m_strengthPotionAmount.SettingChanged += OnItemConfigChange;
        Potion.RegisterAction("$item_rs_strengthpotion", (player, inventory, item) =>
        {
            if (!player.CanConsumeItem(item, true)) return;
            if (item.m_shared.m_consumeStatusEffect)
            {
                player.GetSEMan().AddStatusEffect(item.m_shared.m_consumeStatusEffect, true);
            }
            if (item.m_shared.m_food > 0.0) player.EatFood(item);
            inventory.RemoveOneItem(item);
            player.StartEmote("drink");
        });
    }

    private static void LoadExperienceTomes()
    {
        Item knifeExperienceTom = new(NorthernAssetsPlugin._Assets, "RS_KnifeExperienceTome");
        knifeExperienceTom.Name.English("Assassin's Tome");
        knifeExperienceTom.Description.English("Increases knife skill");
        knifeExperienceTom.Crafting.Add("RS_AlchemyTable", 2);
        knifeExperienceTom.CraftAmount = 1;
        knifeExperienceTom.RequiredItems.Add("ElderBark", 10);
        knifeExperienceTom.RequiredItems.Add("Flint", 10);
        knifeExperienceTom.RequiredItems.Add("Dandelion", 10);
        knifeExperienceTom.RequiredItems.Add("RS_AlchemicEssence", 1);
        Potion.RegisterAction("$item_rs_knifeexperiencetome", (player, inventory, item) =>
        {
            if (!LevelSkill(player, Skills.SkillType.Knives))
            {
                player.Message(MessageHud.MessageType.Center, "$msg_unknownskill");
                return;
            };
            inventory.RemoveOneItem(item);
            player.StartEmote("flex");
        });
        
        Item swordExperienceTome = new(NorthernAssetsPlugin._Assets, "RS_SwordExperienceTome");
        swordExperienceTome.Name.English("Warrior's Tome");
        swordExperienceTome.Description.English("Increases sword skill");
        swordExperienceTome.Crafting.Add("RS_AlchemyTable", 2);
        swordExperienceTome.CraftAmount = 1;
        swordExperienceTome.RequiredItems.Add("ElderBark", 10);
        swordExperienceTome.RequiredItems.Add("Root", 3);
        swordExperienceTome.RequiredItems.Add("Dandelion", 10);
        swordExperienceTome.RequiredItems.Add("RS_AlchemicEssence", 1);
        Potion.RegisterAction("$item_rs_swordexperiencetome", (player, inventory, item) =>
        {
            if (!LevelSkill(player, Skills.SkillType.Swords))
            {
                player.Message(MessageHud.MessageType.Center, "$msg_unknownskill");
                return;
            };
            inventory.RemoveOneItem(item);
            player.StartEmote("flex");
        });
        
        Item axeExperienceTome = new(NorthernAssetsPlugin._Assets, "RS_AxeExperienceTome");
        axeExperienceTome.Name.English("Savage's Tome");
        axeExperienceTome.Description.English("Increases axe skill");
        axeExperienceTome.Crafting.Add("RS_AlchemyTable", 2);
        axeExperienceTome.CraftAmount = 1;
        axeExperienceTome.RequiredItems.Add("ElderBark", 10);
        axeExperienceTome.RequiredItems.Add("Acorn", 2);
        axeExperienceTome.RequiredItems.Add("Dandelion", 10);
        axeExperienceTome.RequiredItems.Add("RS_AlchemicEssence", 1);
        Potion.RegisterAction("$item_rs_axeexperiencetome", (player, inventory, item) =>
        {
            if (!LevelSkill(player, Skills.SkillType.Axes))
            {
                player.Message(MessageHud.MessageType.Center, "$msg_unknownskill");
                return;
            };
            inventory.RemoveOneItem(item);
            player.StartEmote("flex");
        });
        
        Item bowExperienceTome = new(NorthernAssetsPlugin._Assets, "RS_BowExperienceTome");
        bowExperienceTome.Name.English("Archer's Tome");
        bowExperienceTome.Description.English("Increases bow skill");
        bowExperienceTome.Crafting.Add("RS_AlchemyTable", 2);
        bowExperienceTome.CraftAmount = 1;
        bowExperienceTome.RequiredItems.Add("ElderBark", 10);
        bowExperienceTome.RequiredItems.Add("BoneFragments", 10);
        bowExperienceTome.RequiredItems.Add("Dandelion", 10);
        bowExperienceTome.RequiredItems.Add("RS_AlchemicEssence", 1);
        Potion.RegisterAction("$item_rs_bowexperiencetome", (player, inventory, item) =>
        {
            if (!LevelSkill(player, Skills.SkillType.Bows))
            {
                player.Message(MessageHud.MessageType.Center, "$msg_unknownskill");
                return;
            };
            inventory.RemoveOneItem(item);
            player.StartEmote("flex");
        });
        
        Item crossbowExperienceTome = new(NorthernAssetsPlugin._Assets, "RS_CrossbowExperienceTome");
        crossbowExperienceTome.Name.English("Ranger's Tome");
        crossbowExperienceTome.Description.English("Increases crossbow skill");
        crossbowExperienceTome.Crafting.Add("RS_AlchemyTable", 2);
        crossbowExperienceTome.CraftAmount = 1;
        crossbowExperienceTome.RequiredItems.Add("ElderBark", 10);
        crossbowExperienceTome.RequiredItems.Add("Softtissue", 5);
        crossbowExperienceTome.RequiredItems.Add("Dandelion", 10);
        crossbowExperienceTome.RequiredItems.Add("RS_AlchemicEssence", 1);
        Potion.RegisterAction("$item_rs_crossbowexperiencetome", (player, inventory, item) =>
        {
            if (!LevelSkill(player, Skills.SkillType.Crossbows))
            {
                player.Message(MessageHud.MessageType.Center, "$msg_unknownskill");
                return;
            };
            inventory.RemoveOneItem(item);
            player.StartEmote("flex");
        });
        
        Item spearExperienceTome = new(NorthernAssetsPlugin._Assets, "RS_SpearExperienceTome");
        spearExperienceTome.Name.English("Hunter's Tome");
        spearExperienceTome.Description.English("Increases spear skill");
        spearExperienceTome.Crafting.Add("RS_AlchemyTable", 2);
        spearExperienceTome.CraftAmount = 1;
        spearExperienceTome.RequiredItems.Add("ElderBark", 10);
        spearExperienceTome.RequiredItems.Add("NeckTail", 10);
        spearExperienceTome.RequiredItems.Add("Dandelion", 10);
        spearExperienceTome.RequiredItems.Add("RS_AlchemicEssence", 1);
        Potion.RegisterAction("$item_rs_spearexperiencetome", (player, inventory, item) =>
        {
            if (!LevelSkill(player, Skills.SkillType.Spears))
            {
                player.Message(MessageHud.MessageType.Center, "$msg_unknownskill");
                return;
            };
            inventory.RemoveOneItem(item);
            player.StartEmote("flex");
        });
        
        Item polearmExperienceTome = new(NorthernAssetsPlugin._Assets, "RS_PolearmExperienceTome");
        polearmExperienceTome.Name.English("Fighter's Tome");
        polearmExperienceTome.Description.English("Increases polearm skill");
        polearmExperienceTome.Crafting.Add("RS_AlchemyTable", 2);
        polearmExperienceTome.CraftAmount = 1;
        polearmExperienceTome.RequiredItems.Add("ElderBark", 10);
        polearmExperienceTome.RequiredItems.Add("Obsidian", 10);
        polearmExperienceTome.RequiredItems.Add("Dandelion", 10);
        polearmExperienceTome.RequiredItems.Add("RS_AlchemicEssence", 1);
        Potion.RegisterAction("$item_rs_polearmexperiencetome", (player, inventory, item) =>
        {
            if (!LevelSkill(player, Skills.SkillType.Polearms))
            {
                player.Message(MessageHud.MessageType.Center, "$msg_unknownskill");
                return;
            };
            inventory.RemoveOneItem(item);
            player.StartEmote("flex");
        });
        
        Item clubExperienceTome = new(NorthernAssetsPlugin._Assets, "RS_ClubExperienceTome");
        clubExperienceTome.Name.English("Brute's Tome");
        clubExperienceTome.Description.English("Increases club skill");
        clubExperienceTome.Crafting.Add("RS_AlchemyTable", 2);
        clubExperienceTome.CraftAmount = 1;
        clubExperienceTome.RequiredItems.Add("ElderBark", 10);
        clubExperienceTome.RequiredItems.Add("GreydwarfEye", 10);
        clubExperienceTome.RequiredItems.Add("Dandelion", 10);
        clubExperienceTome.RequiredItems.Add("RS_AlchemicEssence", 1);
        Potion.RegisterAction("$item_rs_clubexperiencetome", (player, inventory, item) =>
        {
            if (!LevelSkill(player, Skills.SkillType.Clubs))
            {
                player.Message(MessageHud.MessageType.Center, "$msg_unknownskill");
                return;
            };
            inventory.RemoveOneItem(item);
            player.StartEmote("flex");
        });
        
        Item unarmedExperienceTome = new(NorthernAssetsPlugin._Assets, "RS_UnarmedExperienceTome");
        unarmedExperienceTome.Name.English("Adventurer's Tome");
        unarmedExperienceTome.Description.English("Increases unarmed skill");
        unarmedExperienceTome.Crafting.Add("RS_AlchemyTable", 2);
        unarmedExperienceTome.CraftAmount = 1;
        unarmedExperienceTome.RequiredItems.Add("ElderBark", 10);
        unarmedExperienceTome.RequiredItems.Add("WolfClaw", 10);
        unarmedExperienceTome.RequiredItems.Add("Dandelion", 10);
        unarmedExperienceTome.RequiredItems.Add("RS_AlchemicEssence", 1);
        Potion.RegisterAction("$item_rs_unarmedexperiencetome", (player, inventory, item) =>
        {
            if (!LevelSkill(player, Skills.SkillType.Unarmed))
            {
                player.Message(MessageHud.MessageType.Center, "$msg_unknownskill");
                return;
            };
            inventory.RemoveOneItem(item);
            player.StartEmote("flex");
        });
        
        Item elementalExperienceTome = new(NorthernAssetsPlugin._Assets, "RS_ElementalExperienceTome");
        elementalExperienceTome.Name.English("Druid's Scroll");
        elementalExperienceTome.Description.English("Increases elemental magic skill");
        elementalExperienceTome.Crafting.Add("RS_AlchemyTable", 2);
        elementalExperienceTome.CraftAmount = 1;
        elementalExperienceTome.RequiredItems.Add("ElderBark", 10);
        elementalExperienceTome.RequiredItems.Add("Thistle", 10);
        elementalExperienceTome.RequiredItems.Add("Dandelion", 10);
        elementalExperienceTome.RequiredItems.Add("RS_AlchemicEssence", 1);
        Potion.RegisterAction("$item_rs_elementalexperiencetome", (player, inventory, item) =>
        {
            if (!LevelSkill(player, Skills.SkillType.ElementalMagic))
            {
                player.Message(MessageHud.MessageType.Center, "$msg_unknownskill");
                return;
            };
            inventory.RemoveOneItem(item);
            player.StartEmote("flex");
        });
        
        Item bloodExperienceTome = new(NorthernAssetsPlugin._Assets, "RS_BloodExperienceTome");
        bloodExperienceTome.Name.English("Necromancer's Scroll");
        bloodExperienceTome.Description.English("Increases blood magic skill");
        bloodExperienceTome.Crafting.Add("RS_AlchemyTable", 2);
        bloodExperienceTome.CraftAmount = 1;
        bloodExperienceTome.RequiredItems.Add("ElderBark", 10);
        bloodExperienceTome.RequiredItems.Add("Bloodbag", 10);
        bloodExperienceTome.RequiredItems.Add("Dandelion", 10);
        bloodExperienceTome.RequiredItems.Add("RS_AlchemicEssence", 1);
        Potion.RegisterAction("$item_rs_bloodexperiencetome", (player, inventory, item) =>
        {
            if (!LevelSkill(player, Skills.SkillType.BloodMagic))
            {
                player.Message(MessageHud.MessageType.Center, "$msg_unknownskill");
                return;
            };
            inventory.RemoveOneItem(item);
            player.StartEmote("flex");
        });
        
        Item fishingExperienceTome = new(NorthernAssetsPlugin._Assets, "RS_FishingExperienceTome");
        fishingExperienceTome.Name.English("Fisherman's Tome");
        fishingExperienceTome.Description.English("Increases fishing skill");
        fishingExperienceTome.Crafting.Add("RS_AlchemyTable", 2);
        fishingExperienceTome.CraftAmount = 1;
        fishingExperienceTome.RequiredItems.Add("ElderBark", 10);
        fishingExperienceTome.RequiredItems.Add("FishRaw", 2);
        fishingExperienceTome.RequiredItems.Add("Dandelion", 10);
        fishingExperienceTome.RequiredItems.Add("RS_AlchemicEssence", 1);
        Potion.RegisterAction("$item_rs_fishingexperiencetome", (player, inventory, item) =>
        {
            if (!LevelSkill(player, Skills.SkillType.Fishing))
            {
                player.Message(MessageHud.MessageType.Center, "$msg_unknownskill");
                return;
            }
            inventory.RemoveOneItem(item);
            player.StartEmote("flex");
        });
    }

    private static void LoadLantern()
    {
        Item lantern = new(NorthernAssetsPlugin._Assets, "RS_Lantern");
        lantern.Name.English("Candle Lantern");
        lantern.Description.English("A weathered iron lantern, cradles a flickering candle, casting warm, ancient light.");
        lantern.Crafting.Add("RS_AlchemyTable", 1);
        lantern.RequiredItems.Add("Iron", 5);
        lantern.RequiredItems.Add("Honey", 10);
        lantern.RequiredItems.Add("RS_AlchemicEssence", 1);
        lantern.RequiredItems.Add("RS_ForsakenEssence", 1);
    }

    private static void LoadBottleItems()
    {
        m_sweetWineStaminaAmount = NorthernAssetsPlugin._Plugin.config("Sweet Wine", "Stamina", 50f, "Set the amount of stamina recovered");
        Item berryWine = new(NorthernAssetsPlugin._Assets, "RS_BerryWine");
        berryWine.Name.English("Sweet Wine");
        berryWine.Description.English($"Recovers <color=orange>{m_sweetWineStaminaAmount.Value}</color> stamina");
        berryWine.Crafting.Add("RS_AlchemyTable", 3);
        berryWine.RequiredItems.Add("RS_AlchemicEssence", 1);
        berryWine.RequiredItems.Add("Blueberries", 10);
        berryWine.RequiredItems.Add("FrozenBerries_RS", 10);
        berryWine.RequiredItems.Add("Cloudberry", 10);
        m_itemConfigData["Sweet Wine"] = new("RS_BerryWine", "$item_rs_berrywine", "$recovers ", " $stamina", false);
        m_sweetWineStaminaAmount.SettingChanged += OnItemConfigChange;

        SE_Potions SEBerryWine = ScriptableObject.CreateInstance<SE_Potions>();
        SEBerryWine.name = "SE_BerryWine";
        SEBerryWine.m_name = $"${berryWine.Name.Key}";
        SEBerryWine.m_duration = NorthernAssetsPlugin._Plugin.config("Sweet Wine", "Cooldown", 50f, "Set cooldown");
        SEBerryWine.m_staminaAmount = m_sweetWineStaminaAmount;
        SEBerryWine.m_icon = berryWine.Prefab.GetComponent<ItemDrop>().m_itemData.GetIcon();
        SEBerryWine.m_cloneEffectsFrom = "Potion_stamina_medium";

        berryWine.AddConsumeStatusEffect(SEBerryWine);
        Potion.RegisterAction("$item_rs_berrywine", (player, inventory, item) =>
        {
            if (!player.CanConsumeItem(item, true)) return;
            if (item.m_shared.m_consumeStatusEffect)
            {
                player.GetSEMan().AddStatusEffect(item.m_shared.m_consumeStatusEffect, true);
            }
            if (item.m_shared.m_food > 0.0) player.EatFood(item);
            inventory.RemoveOneItem(item);
            player.StartEmote("drink");
        });

        m_etherDuration = NorthernAssetsPlugin._Plugin.config("Ether Potion", "Duration", 60f, "Set duration of effect");
        Item etherPotion = new(NorthernAssetsPlugin._Assets, "RS_EtherPotion");
        etherPotion.Name.English("Ether Potion");
        etherPotion.Description.English($"Grants the grace of a feathered descent for <color=orange>{m_etherDuration.Value}</color> seconds");
        etherPotion.Crafting.Add("RS_AlchemyTable", 3);
        etherPotion.RequiredItems.Add("RS_AlchemicEssence", 1);
        etherPotion.RequiredItems.Add("BeastMeat_RS", 1);
        etherPotion.RequiredItems.Add("Wisp", 1);
        etherPotion.RequiredItems.Add("Dandelion", 5);
        m_itemConfigData["Ether Potion"] = new("RS_EtherPotion", "$item_rs_etherpotion", "$feathered_descent ", " $seconds");
        m_etherDuration.SettingChanged += OnItemConfigChange;
        SE_Potions etherPower = ScriptableObject.CreateInstance<SE_Potions>();
        etherPower.name = "SE_EtherPotion";
        etherPower.m_name = $"${etherPotion.Name.Key}";
        etherPower.m_duration = m_etherDuration;
        etherPower.m_fallSpeedConfig = NorthernAssetsPlugin._Plugin.config("Ether Potion", "Fall Speed Reduction", 5f, "Set fall speed reduction");
        etherPower.m_icon = etherPotion.Prefab.GetComponent<ItemDrop>().m_itemData.GetIcon();
        etherPower.m_cloneEffectsFrom = "Potion_eitr_lingering";
        etherPotion.AddConsumeStatusEffect(etherPower);
        Potion.RegisterAction("$item_rs_etherpotion", (player, inventory, item) =>
        {
            if (!player.CanConsumeItem(item, true)) return;
            if (item.m_shared.m_consumeStatusEffect)
            {
                player.GetSEMan().AddStatusEffect(item.m_shared.m_consumeStatusEffect, true);
            }
            if (item.m_shared.m_food > 0.0) player.EatFood(item);
            inventory.RemoveOneItem(item);
            player.StartEmote("drink");
        });

        m_revitalizeDuration = NorthernAssetsPlugin._Plugin.config("Revitalize Potion", "Duration", 180f, "Set length of effect");
        Item revitalizePotion = new(NorthernAssetsPlugin._Assets, "RS_RevitalizePotion");
        revitalizePotion.Name.English("Revitalize Potion");
        revitalizePotion.Description.English("Restores rested bonus");
        revitalizePotion.Crafting.Add("RS_AlchemyTable", 2);
        revitalizePotion.RequiredItems.Add("RS_AlchemicEssence", 1);
        revitalizePotion.RequiredItems.Add("Dandelion", 10);
        revitalizePotion.RequiredItems.Add("Honey", 5);
        revitalizePotion.RequiredItems.Add("Flax", 10);
        Potion.RegisterAction("$item_rs_revitalizepotion", (player, inventory, item) =>
        {
            bool added = false;
            if (!player.GetSEMan().HaveStatusEffect("Rested".GetStableHashCode()))
            {
                player.GetSEMan().AddStatusEffect("Rested".GetStableHashCode());
                added = true;
            }

            StatusEffect effect = player.GetSEMan().GetStatusEffect("Rested".GetStableHashCode());
            if (added) effect.m_ttl = m_revitalizeDuration.Value;
            else effect.m_ttl += m_revitalizeDuration.Value;
            var status = ObjectDB.instance.GetStatusEffect("Potion_barleywine".GetStableHashCode());
            if (status)
            {
                var transform = player.transform;
                status.m_startEffects.Create(transform.position, transform.rotation, transform);
            }
            inventory.RemoveOneItem(item);
            player.StartEmote("drink");
        });

        Item TundraPotion = new Item(NorthernAssetsPlugin._Assets, "RS_TundraPotion");
        TundraPotion.Name.English("Frostbitten Potion");
        TundraPotion.Description.English("Counters the effects of frostbite");
        TundraPotion.Crafting.Add("RS_AlchemyTable", 2);
        TundraPotion.RequiredItems.Add("RS_AlchemicEssence", 1);
        TundraPotion.RequiredItems.Add("LizardTail_RS", 1);
        TundraPotion.RequiredItems.Add("Fungi_RS", 1);
        TundraPotion.RequiredItems.Add("FrozenBerries_RS", 1);
        SE_Potions tundraEffect = ScriptableObject.CreateInstance<SE_Potions>();
        tundraEffect.name = "SE_TundraPotion";
        tundraEffect.m_name = $"${TundraPotion.Name.Key}";
        tundraEffect.m_duration = NorthernAssetsPlugin._Plugin.config("Frostbitten Potion", "Duration", 100f, "Set duration of effect");
        tundraEffect.m_icon = TundraPotion.Prefab.GetComponent<ItemDrop>().m_itemData.GetIcon();
        tundraEffect.m_cloneEffectsFrom = "Potion_eitr_lingering";
        TundraPotion.AddConsumeStatusEffect(tundraEffect);
    }
    
    private static bool LevelSkill(Player player, Skills.SkillType type)
    {
        if (!player.m_skills.m_skillData.TryGetValue(type, out Skills.Skill data)) return false;
        if (m_expByLevel.Value is Toggle.Off)
        {
            player.RaiseSkill(type, m_expPerTome.Value);
        }
        else
        {
            float amount = data.GetNextLevelRequirement();
            player.m_skills.RaiseSkill(type, amount);
        }
        return true;
    }

    private static readonly Dictionary<string, ItemConfigData> m_itemConfigData = new();

    private static void OnItemConfigChange(object sender, EventArgs args)
    {
        if (sender is not ConfigEntry<float> config) return;
        var group = config.Definition.Section;
        var value = config.Value;
        if (!m_itemConfigData.TryGetValue(group, out ItemConfigData data)) return;
        if (data.m_percentage)
        {
            value = value * 100 - 100;
        }
        string description = $"{data.m_prefix}<color=orange>{Mathf.CeilToInt(value)}</color>{data.m_postfix}";
        UpdateItem(data.m_prefabName, data.m_sharedName, description);
    }

    private static void UpdateItem(string prefabName, string sharedName, string description)
    {
        var prefab = ObjectDB.instance.GetItemPrefab(prefabName);
        if (!prefab || !prefab.TryGetComponent(out ItemDrop component)) return;
        component.m_itemData.m_shared.m_description = description;
        if (!Player.m_localPlayer) return;
        foreach (var item in Player.m_localPlayer.GetInventory().GetAllItems())
        {
            if (item.m_shared.m_name != sharedName) continue;
            item.m_shared.m_description = description;
        }
    }
    
    private class ItemConfigData
    {
        public readonly string m_prefabName;
        public readonly string m_sharedName;
        public readonly string m_prefix;
        public readonly string m_postfix;
        public readonly bool m_percentage;

        public ItemConfigData(string PrefabName, string SharedName, string Prefix, string Postfix, bool percentage = false)
        {
            m_prefabName = PrefabName;
            m_sharedName = SharedName;
            m_prefix = Prefix;
            m_postfix = Postfix;
            m_percentage = percentage;
        }
    }
}