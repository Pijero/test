using Verse;

namespace BattleMoodAura
{
    public class BattleMoodAuraMod : Mod
    {
        public static BattleMoodAuraSettings Settings;

        public BattleMoodAuraMod(ModContentPack content) : base(content)
        {
            Settings = GetSettings<BattleMoodAuraSettings>();
        }

        public override string SettingsCategory() => "Battle Mood Aura";

        public override void DoSettingsWindowContents(UnityEngine.Rect inRect)
        {
            Settings.DoWindowContents(inRect);
        }
    }
}
