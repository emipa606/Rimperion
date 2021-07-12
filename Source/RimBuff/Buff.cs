using System;
using System.Collections;
using Verse;

namespace RimBuff
{
    public class Buff : IExposable
    {
        protected bool canDespell = true;

        protected ThingWithComps caster;
        protected int currentDuration;

        protected int currentOverlapLevel;
        protected int currentRepeatCycle;
        protected BuffDef def;
        protected int duration;
        protected int maxOverlapLevel;
        protected CompBuffManager owner;
        protected int repeatCycle;

        protected int spellLevel;
        protected string uniqueID = string.Empty;

        public Buff()
        {
            uniqueID = "NeedDefName" + "_" + GetHashCode();
        }

        public Buff(BuffDef buffDef)
        {
            def = buffDef;
            uniqueID = def.defName + "_" + GetHashCode();

            canDespell = buffDef.canDespell;

            caster = null;

            spellLevel = buffDef.spellLevel;
            maxOverlapLevel = buffDef.maxOverlapLevel;
            duration = buffDef.duration.SecondsToTicks();
            repeatCycle = buffDef.duration.SecondsToTicks();
        }

        public Buff(BuffDef buffDef, ThingWithComps caster)
        {
            def = buffDef;
            uniqueID = def.defName + "_" + GetHashCode();

            canDespell = buffDef.canDespell;

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

        public bool CanDespell => canDespell;

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
            set => currentOverlapLevel = value < maxOverlapLevel ? value : maxOverlapLevel;
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
                Log.Error("Error : " + ee);
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

        public virtual void OnRefresh()
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
        } //test

        /*
        public virtual void Tick(int interval)
        {
            if (currentDuration >= duration)
            {
                OnDurationExpire();
            }
            else
            {
                currentDuration += interval;
            }
            if (currentRepeatCycle >= repeatCycle)
            {
                OnIterate();
                currentRepeatCycle = 0;
            }
            else
            {
                currentRepeatCycle += interval;
            }
        }*/
        public virtual void OnCreate()
        {
        }

        public virtual void OnDestroy()
        {
        }

        public virtual void OnIterate()
        {
        }

        /// <summary>
        ///     Basically when duration expires, the buff is destroyed.
        /// </summary>
        public virtual void OnDurationExpire()
        {
            Owner.RemoveBuff(this);
        }
    }
}