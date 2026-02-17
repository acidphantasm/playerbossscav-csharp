using System.Reflection;
using HarmonyLib;
using SPTarkov.Reflection.Patching;
using SPTarkov.Server.Core.DI;
using SPTarkov.Server.Core.Generators;
using SPTarkov.Server.Core.Helpers;
using SPTarkov.Server.Core.Models.Eft.Common;
using SPTarkov.Server.Core.Models.Eft.Common.Tables;
using SPTarkov.Server.Core.Services;
using SPTarkov.Server.Core.Utils;
using SPTarkov.Server.Core.Utils.Cloners;

namespace _playerBossScav;

public class GeneratePlayerScavPatch : AbstractPatch
{
    private static readonly RandomUtil RandomUtil = ServiceLocator.ServiceProvider.GetRequiredService<RandomUtil>();
    private static readonly ICloner Cloner = ServiceLocator.ServiceProvider.GetRequiredService<ICloner>();
    private static readonly BotHelper BotHelper = ServiceLocator.ServiceProvider.GetRequiredService<BotHelper>();
    
    protected override MethodBase GetTargetMethod()
    {
        return AccessTools.Method(typeof(BotGenerator),"GeneratePlayerScav");
    }

    [PatchPrefix]
    public static void Prefix(ref string role, ref BotType botTemplate)
    {
        // Maybe compatibility with Skills Extended?
        if (role == "sectantWarrior")
            return;

        if (PlayerBossScav.ModConfig is null)
            return;

        if (!RandomUtil.GetChance100(PlayerBossScav.ModConfig.Chance))
            return;

        if (PlayerBossScav.AllowedBosses.Count == 0)
            return;
        
        var selectedRole = RandomUtil.GetRandomElement(PlayerBossScav.AllowedBosses);
        var newTemplate = Cloner.Clone(BotHelper.GetBotTemplate(role));
        if (newTemplate is null)
            return;
        
        role = selectedRole;
        botTemplate.BotInventory = newTemplate.BotInventory;
        botTemplate.BotChances = newTemplate.BotChances;
        botTemplate.BotGeneration  = newTemplate.BotGeneration;
        botTemplate.BotAppearance = newTemplate.BotAppearance;
    }
}