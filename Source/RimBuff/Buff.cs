using System;
using System.Collections;
using Verse;

namespace RimBuff;

public class Buff : IExposable
{
    private ThingWithComps caster;
    protected int currentDuration;

    private int currentOverlapLevel;
    private int currentRepeatCycle;
    private BuffDef def;
    protected int duration;
    private int maxOverlapLevel;
    protected CompBuffManager owner;
    private int repeatCycle;

    private int spellLevel;
    private string uniqueID;

    protected Buff()
    {
        uniqueID = $"NeedDefName_{GetHashCode()}";
    }

    protected Buff(BuffDef buffDef, ThingWithComps caster)
    {
        def = buffDef;
        uniqueID = $"{def.defName}_{GetHashCode()}";

        CanDespell = buffDef.canDespell;

        this.caster = caster;

        spellLevel = buffDef.spellLevel;
        maxOverlapLevel = buffDef.maxOverlapLevel;
        duration = buffDef.duration.SecondsToTicks();
        repeatCycle = buffDef.duration.SecondsToTicks();
    }

    public BuffDef Def => def;
    public string DefName => def.defName;
    public string Label => def.label;
    public string UniqueID => uniqueID;

    public ThingWithComps Caster
    {
        get => caster;
        set => caster = value;
    }

    public CompBuffManager Owner
    {
        get => owner;
        set => owner = value;
    }

    public bool CanDespell { get; } = true;

    public int SpellLevel => spellLevel;

    public int MaxOverlapLevel
    {
        get => currentOverlapLevel;
        set
        {
            if (value < currentOverlapLevel)
            {
                currentOverlapLevel = value;
            }

            maxOverlapLevel = value;
        }
    }

    public int Duration => duration;
    public int RepeatCycle => repeatCycle;

    public int CurrentOverlapLevel
    {
        get => currentOverlapLevel;
        protected set => currentOverlapLevel = value < maxOverlapLevel ? value : maxOverlapLevel;
    }

    public int CurrentDuration => currentDuration;
    public int CurrentRepeatCycle => currentRepeatCycle;

    public virtual void ExposeData()
    {
        try
        {
            Scribe_Defs.Look(ref def, "def");
            Scribe_Values.Look(ref uniqueID, "uniqueID");

            Scribe_References.Look(ref caster, "caster");

            Scribe_Values.Look(ref spellLevel, "spellLevel");
            Scribe_Values.Look(ref maxOverlapLevel, "maxOverlapCount");
            Scribe_Values.Look(ref duration, "duration");
            Scribe_Values.Look(ref repeatCycle, "repeatCycle");

            Scribe_Values.Look(ref currentOverlapLevel, "currentOverlapCount");
            Scribe_Values.Look(ref currentDuration, "currentDuration");
            Scribe_Values.Look(ref currentRepeatCycle, "currentRepeatCycle");
        }
        catch (Exception ee)
        {
            Log.Error($"Error : {ee}");
        }
    }

    /// <summary>
    ///     basically If the Level changes, refresh.
    /// </summary>
    /// <param name="level"></param>
    public virtual void AddOverlapLevel(int level)
    {
        CurrentOverlapLevel += level; //나중에 음수값 패치 추가
        OnRefresh();
    }

    protected virtual void OnRefresh()
    {
    }

    public virtual IEnumerator TickTest(int interval)
    {
        OnCreate();
        while (currentDuration > duration)
        {
            currentDuration += interval;

            if (currentRepeatCycle >= repeatCycle)
            {
                OnIterate();
                currentRepeatCycle = 0;
            }
            else
            {
                currentRepeatCycle += interval;
            }

            yield return null;
        }

        OnDurationExpire();
    }

    protected virtual void OnCreate()
    {
    }

    public virtual void OnDestroy()
    {
    }

    protected virtual void OnIterate()
    {
    }

    /// <summary>
    ///     Basically when duration expires, the buff is destroyed.
    /// </summary>
    protected virtual void OnDurationExpire()
    {
        Owner.RemoveBuff(this);
    }
}