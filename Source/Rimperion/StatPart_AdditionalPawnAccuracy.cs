using RimBuff;
using RimWorld;
using UnityEngine;
using Verse;

namespace NegativeRecoil;

public class StatPart_AdditionalPawnAccuracy : StatPart
{
    public override void TransformValue(StatRequest req, ref float val)
    {
        try
        {
            if (!req.HasThing)
            {
                return;
            }

            if (req.Thing is not Pawn pawn || pawn.equipment.Primary?.def.weaponTags == null)
            {
                return;
            }

            if (!pawn.equipment.Primary.def.weaponTags.Contains("NegativeRecoil"))
            {
                return;
            }

            var compBuffManager = pawn.equipment.Primary.GetComp<CompBuffManager>();

            if (compBuffManager?.FindWithTags(["NegativeRecoil", "Pawn"]) is not NegativeRecoilBuff
                buff)
            {
                return;
            }

            var additionalVal = Mathf.Pow(buff.AdditionalAccuracy, buff.CurrentOverlapLevel);
            val += additionalVal - 1;
        }
        catch
        {
            if (req.HasThing)
            {
                Log.Error($"StatPart_AdditionalPawnAccuracy.TransformValue() Fail from :{req.Thing}");
            }
        }
    }

    public override string ExplanationPart(StatRequest req)
    {
        try
        {
            if (req.HasThing)
            {
                if (req.Thing is Pawn pawn && pawn.equipment.Primary?.def.weaponTags != null)
                {
                    if (pawn.equipment.Primary.def.weaponTags.Contains("NegativeRecoil"))
                    {
                        var compBuffManager = pawn.equipment.Primary.GetComp<CompBuffManager>();
                        if (compBuffManager?.FindWithTags(["NegativeRecoil", "Pawn"]) is
                            NegativeRecoilBuff buff)
                        {
                            string text = "StatReport_AdditionalPawnAccuracy".Translate() + ": +" +
                                          (Mathf.Pow(buff.AdditionalAccuracy, buff.CurrentOverlapLevel) - 1)
                                          .ToStringPercent();

                            return text;
                        }
                    }
                }
            }
        }
        catch
        {
            Log.Error(req.HasThing
                ? $"{parentStat.defName} - {req.Thing} ExplanationPart"
                : $"{parentStat.defName} -  ExplanationPart");
        }

        return string.Empty;
    }
}