using System.Collections.Generic;

public static class SaveKeys
{
    /// =========
    /// Core Keys
    /// =========

    public struct Progression
    {
        public const string CurrentLevelIndex = "current_level_index";
    }

    public struct Currency
    {
        public const string Coins = "currency_coins";
        public const string Gems = "currency_gems";
        public static readonly List<string> AllCurrencyKeys = new() { Coins, Gems };
    }

    /// ===========
    /// Custom Keys
    /// ===========

    //..
}
