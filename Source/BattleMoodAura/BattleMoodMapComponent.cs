using RimWorld;
using Verse;

namespace BattleMoodAura
{
    public class BattleMoodMapComponent : MapComponent
    {
        private enum EffectLevel
        {
            None,
            BuffLow,
            BuffMid,
            BuffHigh,
            DebuffLow,
            DebuffMid,
            DebuffHigh
        }

        public BattleMoodMapComponent(Map map) : base(map)
        {
        }

        public override void MapComponentTick()
        {
            var settings = BattleMoodAuraMod.Settings;
            if (settings == null) return;

            int interval = settings.CheckIntervalTicks <= 0 ? 60 : settings.CheckIntervalTicks;
            if (Find.TickManager.TicksGame % interval != 0) return;

            var pawns = map.mapPawns.FreeColonistsSpawned;
            for (int i = 0; i < pawns.Count; i++)
            {
                Pawn pawn = pawns[i];
                if (pawn.Dead || pawn.Downed) continue;

                if (!IsInCombatState(pawn))
                {
                    RemoveAllEffects(pawn);
                    continue;
                }

                float score = CalcNearbyRelationScore(pawn);
                var level = ResolveLevel(score, settings);
                ApplyLevel(pawn, level);
            }
        }

        private static bool IsInCombatState(Pawn pawn)
        {
            return pawn.Drafted;
        }

        private float CalcNearbyRelationScore(Pawn pawn)
        {
            var settings = BattleMoodAuraMod.Settings;
            float score = 0f;
            int nearbyCount = 0;
            var colonists = map.mapPawns.FreeColonistsSpawned;

            for (int i = 0; i < colonists.Count; i++)
            {
                Pawn other = colonists[i];
                if (other == pawn || other.Dead || other.Downed) continue;
                if (other.Position.DistanceTo(pawn.Position) > settings.Radius) continue;

                nearbyCount++;
                int opinion = pawn.relations.OpinionOf(other);

                if (opinion >= settings.FriendlyOpinionThreshold)
                {
                    score += settings.FriendlyWeight;
                    if (pawn.relations.DirectRelationExists(PawnRelationDefOf.Spouse, other) ||
                        pawn.relations.DirectRelationExists(PawnRelationDefOf.Lover, other) ||
                        pawn.relations.DirectRelationExists(PawnRelationDefOf.Fiance, other))
                    {
                        score += settings.FriendlyWeight * 0.5f;
                    }
                }
                else if (opinion <= settings.HostileOpinionThreshold)
                {
                    score -= settings.HostileWeight;
                    if (pawn.relations.DirectRelationExists(PawnRelationDefOf.Rival, other) ||
                        pawn.relations.DirectRelationExists(PawnRelationDefOf.ExLover, other))
                    {
                        score -= settings.HostileWeight * 0.5f;
                    }
                }
            }

            if (nearbyCount > 0)
            {
                score *= (1f + nearbyCount * 0.15f);
            }

            return score;
        }

        private static EffectLevel ResolveLevel(float score, BattleMoodAuraSettings settings)
        {
            if (score >= settings.BuffHighThreshold) return EffectLevel.BuffHigh;
            if (score >= settings.BuffMidThreshold) return EffectLevel.BuffMid;
            if (score >= settings.BuffLowThreshold) return EffectLevel.BuffLow;

            if (score <= settings.DebuffHighThreshold) return EffectLevel.DebuffHigh;
            if (score <= settings.DebuffMidThreshold) return EffectLevel.DebuffMid;
            if (score <= settings.DebuffLowThreshold) return EffectLevel.DebuffLow;

            return EffectLevel.None;
        }

        private static void ApplyLevel(Pawn pawn, EffectLevel level)
        {
            RemoveAllEffects(pawn);
            switch (level)
            {
                case EffectLevel.BuffLow:
                    EnsureHediff(pawn, "BattleMood_Buff_Low");
                    break;
                case EffectLevel.BuffMid:
                    EnsureHediff(pawn, "BattleMood_Buff_Mid");
                    break;
                case EffectLevel.BuffHigh:
                    EnsureHediff(pawn, "BattleMood_Buff_High");
                    break;
                case EffectLevel.DebuffLow:
                    EnsureHediff(pawn, "BattleMood_Debuff_Low");
                    break;
                case EffectLevel.DebuffMid:
                    EnsureHediff(pawn, "BattleMood_Debuff_Mid");
                    break;
                case EffectLevel.DebuffHigh:
                    EnsureHediff(pawn, "BattleMood_Debuff_High");
                    break;
            }
        }

        private static void EnsureHediff(Pawn pawn, string defName)
        {
            var def = DefDatabase<HediffDef>.GetNamedSilentFail(defName);
            if (def == null) return;
            if (pawn.health.hediffSet.GetFirstHediffOfDef(def) == null)
            {
                pawn.health.AddHediff(def);
            }
        }

        private static void RemoveAllEffects(Pawn pawn)
        {
            RemoveIfPresent(pawn, "BattleMood_Buff_Low");
            RemoveIfPresent(pawn, "BattleMood_Buff_Mid");
            RemoveIfPresent(pawn, "BattleMood_Buff_High");
            RemoveIfPresent(pawn, "BattleMood_Debuff_Low");
            RemoveIfPresent(pawn, "BattleMood_Debuff_Mid");
            RemoveIfPresent(pawn, "BattleMood_Debuff_High");
        }

        private static void RemoveIfPresent(Pawn pawn, string defName)
        {
            var def = DefDatabase<HediffDef>.GetNamedSilentFail(defName);
            if (def == null) return;
            var h = pawn.health.hediffSet.GetFirstHediffOfDef(def);
            if (h != null) pawn.health.RemoveHediff(h);
        }
    }
}
