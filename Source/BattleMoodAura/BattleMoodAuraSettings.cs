using UnityEngine;
using Verse;

namespace BattleMoodAura
{
    public class BattleMoodAuraSettings : ModSettings
    {
        public float Radius = 8f;
        public int CheckIntervalTicks = 60;

        public int FriendlyOpinionThreshold = 40;
        public int HostileOpinionThreshold = -20;

        public float FriendlyWeight = 1f;
        public float HostileWeight = 1f;

        public float BuffLowThreshold = 2f;
        public float BuffMidThreshold = 4f;
        public float BuffHighThreshold = 6f;

        public float DebuffLowThreshold = -2f;
        public float DebuffMidThreshold = -4f;
        public float DebuffHighThreshold = -6f;

        public override void ExposeData()
        {
            Scribe_Values.Look(ref Radius, "Radius", 8f);
            Scribe_Values.Look(ref CheckIntervalTicks, "CheckIntervalTicks", 60);
            Scribe_Values.Look(ref FriendlyOpinionThreshold, "FriendlyOpinionThreshold", 40);
            Scribe_Values.Look(ref HostileOpinionThreshold, "HostileOpinionThreshold", -20);
            Scribe_Values.Look(ref FriendlyWeight, "FriendlyWeight", 1f);
            Scribe_Values.Look(ref HostileWeight, "HostileWeight", 1f);
            Scribe_Values.Look(ref BuffLowThreshold, "BuffLowThreshold", 2f);
            Scribe_Values.Look(ref BuffMidThreshold, "BuffMidThreshold", 4f);
            Scribe_Values.Look(ref BuffHighThreshold, "BuffHighThreshold", 6f);
            Scribe_Values.Look(ref DebuffLowThreshold, "DebuffLowThreshold", -2f);
            Scribe_Values.Look(ref DebuffMidThreshold, "DebuffMidThreshold", -4f);
            Scribe_Values.Look(ref DebuffHighThreshold, "DebuffHighThreshold", -6f);
        }

        public void DoWindowContents(Rect inRect)
        {
            var list = new Listing_Standard();
            list.Begin(inRect);

            list.Label($"判定半径: {Radius:F1}");
            Radius = list.Slider(Radius, 3f, 20f);

            list.Label($"更新間隔(ticks): {CheckIntervalTicks}");
            CheckIntervalTicks = (int)list.Slider(CheckIntervalTicks, 30, 300);

            list.GapLine();
            list.Label("好感しきい値 / 不和しきい値");
            list.Label($"Friendly Opinion >= {FriendlyOpinionThreshold}");
            FriendlyOpinionThreshold = (int)list.Slider(FriendlyOpinionThreshold, 0, 100);
            list.Label($"Hostile Opinion <= {HostileOpinionThreshold}");
            HostileOpinionThreshold = (int)list.Slider(HostileOpinionThreshold, -100, 0);

            list.GapLine();
            list.Label($"友好ウェイト: {FriendlyWeight:F1}");
            FriendlyWeight = list.Slider(FriendlyWeight, 0.5f, 3f);
            list.Label($"不仲ウェイト: {HostileWeight:F1}");
            HostileWeight = list.Slider(HostileWeight, 0.5f, 3f);

            list.GapLine();
            list.Label("バフ段階しきい値");
            list.Label($"弱 >= {BuffLowThreshold:F1} / 中 >= {BuffMidThreshold:F1} / 強 >= {BuffHighThreshold:F1}");
            BuffLowThreshold = list.Slider(BuffLowThreshold, 0.5f, 6f);
            BuffMidThreshold = list.Slider(BuffMidThreshold, BuffLowThreshold + 0.5f, 10f);
            BuffHighThreshold = list.Slider(BuffHighThreshold, BuffMidThreshold + 0.5f, 14f);

            list.GapLine();
            list.Label("デバフ段階しきい値");
            list.Label($"弱 <= {DebuffLowThreshold:F1} / 中 <= {DebuffMidThreshold:F1} / 強 <= {DebuffHighThreshold:F1}");
            DebuffLowThreshold = list.Slider(DebuffLowThreshold, -6f, -0.5f);
            DebuffMidThreshold = list.Slider(DebuffMidThreshold, -10f, DebuffLowThreshold - 0.5f);
            DebuffHighThreshold = list.Slider(DebuffHighThreshold, -14f, DebuffMidThreshold - 0.5f);

            list.End();
        }
    }
}
