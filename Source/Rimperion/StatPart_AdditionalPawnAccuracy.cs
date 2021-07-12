using System.Collections.Generic;
using RimBuff;
using RimWorld;
using UnityEngine;
using Verse;

namespace NegativeRecoil
{
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

                if (!(req.Thing is Pawn pawn) || pawn.equipment.Primary?.def.weaponTags == null)
                {
                    return;
                }

                if (!pawn.equipment.Primary.def.weaponTags.Contains("NegativeRecoil"))
                {
                    return;
                }

                var compBuffManager = pawn.equipment.Primary.GetComp<CompBuffManager>();

                if (compBuffManager?.FindWithTags(new List<string> {"NegativeRecoil", "Pawn"}) is not NegativeRecoilBuff
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
                    Log.Error("StatPart_AdditionalPawnAccuracy.TransformValue() Fail from :" + req.Thing);
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
                            if (compBuffManager?.FindWithTags(new List<string> {"NegativeRecoil", "Pawn"}) is
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
                if (req.HasThing)
                {
                    Log.Error(parentStat.defName + " - " + req.Thing + " ExplanationPart");
                }
                else
                {
                    Log.Error(parentStat.defName + " - " + " ExplanationPart");
                }
            }

            return string.Empty;
        }
    }
}