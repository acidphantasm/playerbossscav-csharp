using System.Reflection;
using SPTarkov.DI.Annotations;
using SPTarkov.Server.Core.DI;
using SPTarkov.Server.Core.Helpers;
using SPTarkov.Server.Core.Models.Spt.Mod;

namespace _playerBossScav;

public record ModMetadata : AbstractModMetadata
{
    public override string ModGuid { get; init; } = "com.acidphantasm.playerbossscav";
    public override string Name { get; init; } = "Player Boss Scavs";
    public override string Author { get; init; } = "acidphantasm";
    public override List<string>? Contributors { get; init; }
    public override SemanticVersioning.Version Version { get; init; } = new("1.0.0");
    public override SemanticVersioning.Range SptVersion { get; init; } = new("~4.0.12");
    public override List<string>? Incompatibilities { get; init; } = ["com.acidphantasm.progressivebotsystem"];
    public override Dictionary<string, SemanticVersioning.Range>? ModDependencies { get; init; }
    public override string? Url { get; init; }
    public override bool? IsBundleMod { get; init; }
    public override string? License { get; init; } = "MIT";
}

[Injectable(TypePriority = OnLoadOrder.PreSptModLoader + 1)]
public class PlayerBossScav(
    ModHelper modHelper)
    : IOnLoad
{
    public static ModConfig? ModConfig;
    public static List<string> AllowedBosses =
    [
        "bossBoar",
        "bossBully",
        "bossGluhar",
        "bossKilla",
        "bossKillaAgro",
        "bossKnight",
        "bossKojaniy",
        "bossKolontay",
        "bossPartisan",
        "bossSanitar",
        "bossTagilla",
        "bossTagillaAgro",
        "bossZryachiy",
        "followerBigPipe",
        "followerBirdEye",
        "sectantPriest"
    ];

    public Task OnLoad()
    {
        var pathToMod = modHelper.GetAbsolutePathToModFolder(Assembly.GetExecutingAssembly());
        ModConfig = modHelper.GetJsonDataFromFile<ModConfig>(pathToMod, "config.json");
        
        AllowedBosses = AllowedBosses
            .Where(x => !ModConfig.DisallowedBosses.Contains(x))
            .ToList();

        new GeneratePlayerScavPatch().Enable();
        
        return Task.CompletedTask;
    }
}