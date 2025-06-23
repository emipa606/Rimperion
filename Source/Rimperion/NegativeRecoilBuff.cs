using System;
using System.Collections;
using RimBuff;
using Verse;

namespace NegativeRecoil;

public class NegativeRecoilBuff : Buff
{
    private float additionalAccuracy = 1f;

    public NegativeRecoilBuff()
    {
    }

    public NegativeRecoilBuff(NegativeRecoilBuffDef buffDef, ThingWithComps caster) : base(buffDef, caster)
    {
        additionalAccuracy = buffDef.additionalAccuracy;
    }

    public float AdditionalAccuracy => additionalAccuracy;

    public override void AddOverlapLevel(int level)
    {
        base.AddOverlapLevel(level);
    }

    public override IEnumerator TickTest(int interval)
    {
        while (true)
        {
            currentDuration += interval;
            if (currentDuration >= duration)
            {
                OnDurationExpire();
                yield break;
            }

            yield return null;
        }
    }

    protected override void OnRefresh()
    {
        currentDuration = 0;
    }

    protected override void OnDurationExpire()
    {
        CurrentOverlapLevel -= 1;
        OnRefresh();
        if (CurrentOverlapLevel <= 0)
        {
            owner.RemoveBuff(this);
        }
    }

    public override void ExposeData()
    {
        base.ExposeData();
        try
        {
            Scribe_Values.Look(ref additionalAccuracy, "additionalAccuracy");
        }
        catch (Exception ee)
        {
            Log.Error($"Error : {ee}");
        }
    }
}