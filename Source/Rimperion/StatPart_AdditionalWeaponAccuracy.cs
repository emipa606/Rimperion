using System;
using System.Collections.Generic;
using RimBuff;
using RimWorld;
using UnityEngine;
using Verse;

namespace NegativeRecoil;

public class StatPart_AdditionalWeaponAccuracy : StatPart
{
    public override void TransformValue(StatRequest req, ref float val)
    {
        try
        {
            if (!req.HasThing)
            {
                return;
            }

            if (req.Thing.def.weaponTags == null)
            {
                return;
            }

            if (!req.Thing.def.weaponTags.Contains("NegativeRecoil"))
            {
                return;
            }

            if (req.Thing is not ThingWithComps weaponThing)
            {
                return;
            }

            var buffComp = weaponThing.GetComp<CompBuffManager>();

            if (buffComp?.FindWithTags(new List<string> { "NegativeRecoil", "Weapon" }) is not NegativeRecoilBuff buff
               )
            {
                return;
            }

            var additionalVal = Mathf.Pow(buff.AdditionalAccuracy, buff.CurrentOverlapLevel);
            val += additionalVal - 1;
        }
        catch (Exception ee)
        {
            if (req.HasThing)
            {
                Log.Error($"StatPart_AdditionalWeaponAccuracy.TransformValue() Fail from :{req.Thing}: {ee}");
            }
        }
    }

    public override string ExplanationPart(StatRequest req)
    {
        if (!req.HasThing)
        {
            return string.Empty;
        }

        try
        {
            if (req.Thing.def.weaponTags != null)
            {
                if (req.Thing.def.weaponTags.Contains("NegativeRecoil"))
                {
                    if (req.Thing is ThingWithComps weaponThing)
                    {
                        var buffComp = weaponThing.GetComp<CompBuffManager>();
                        if (buffComp?.FindWithTags(new List<string> { "NegativeRecoil", "Weapon" }) is
                            NegativeRecoilBuff buff)
                        {
                            string text = "StatReport_AdditionalWeaponAccuracy".Translate() + ": +" +
                                          (Mathf.Pow(buff.AdditionalAccuracy, buff.CurrentOverlapLevel) - 1)
                                          .ToStringPercent();
                            return text;
                        }
                    }
                }
            }
        }
        catch (Exception ee)
        {
            Log.Error($"{parentStat.defName} - {req.Thing} ExplanationPart :{ee}");
        }

        return string.Empty;
    }
}