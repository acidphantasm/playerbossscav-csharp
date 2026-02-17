namespace _playerBossScav;

public record ModConfig()
{
    public int Chance { get; set; }
    public List<string> DisallowedBosses { get; set; }
    public List<string> ValidBossListDontEditUseThisToCopyPasteValues { get; set; }
};