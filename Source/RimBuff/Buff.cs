using System;
using System.Collections;
using System.Linq;
using System.Text;
using Verse;
using RimWorld;
using HarmonyLib;
using System.Reflection;

namespace RimBuff
{
    public class Buff : IExposable
    {
        #region Fields
        protected BuffDef def;
        protected string uniqueID = string.Empty;

        protected ThingWithComps caster = null;
        protected CompBuffManager owner = null;

        protected bool canDespell = true;

        protected int spellLevel = 0;
        protected int maxOverlapLevel = 0;
        protected int duration = 0;
        protected int repeatCycle = 0;
        
        protected int currentOverlapLevel = 0;
        protected int currentDuration = 0;
        protected int currentRepeatCycle = 0;
        #endregion   

        #region Properties
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
            set
            {
                if (value < maxOverlapLevel)
                {
                    currentOverlapLevel = value;
                }
                else
                {
                    currentOverlapLevel = maxOverlapLevel;
                }

            }
        }
        public int CurrentDuration => currentDuration;
        public int CurrentRepeatCycle => currentRepeatCycle;
        #endregion

        #region Constructors
        public Buff ()
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
            duration = GenTicks.SecondsToTicks(buffDef.duration);
            repeatCycle = GenTicks.SecondsToTicks(buffDef.duration);
        }
        public Buff(BuffDef buffDef,ThingWithComps caster)
        {
            def = buffDef;
            uniqueID = def.defName+"_"+ GetHashCode();

            canDespell = buffDef.canDespell;

            this.caster = caster;

            spellLevel = buffDef.spellLevel;
            maxOverlapLevel = buffDef.maxOverlapLevel;
            duration = GenTicks.SecondsToTicks(buffDef.duration);
            repeatCycle = GenTicks.SecondsToTicks(buffDef.duration);
        }
        #endregion

        #region Public Methods
        /// <summary>
        /// basically If the Level changes, refresh.
        /// </summary>
        /// <param name="level"></param>
        public virtual void AddOverlapLevel(int level)
        {
            CurrentOverlapLevel += level;//나중에 음수값 패치 추가
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
            
        }//test
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
        /// Basically when duration expires, the buff is destroyed.
        /// </summary>
        public virtual void OnDurationExpire()
        {
            Owner.RemoveBuff(this);
        }

        public virtual void ExposeData()
        {
            try
            {
                Scribe_Defs.Look<BuffDef>(ref def, "def");
                Scribe_Values.Look<string>(ref uniqueID, "uniqueID");

                Scribe_References.Look<ThingWithComps>(ref caster, "caster");

                Scribe_Values.Look<int>(ref spellLevel, "spellLevel");
                Scribe_Values.Look<int>(ref maxOverlapLevel, "maxOverlapCount");
                Scribe_Values.Look<int>(ref duration, "duration");
                Scribe_Values.Look<int>(ref repeatCycle, "repeatCycle");
                
                Scribe_Values.Look<int>(ref currentOverlapLevel, "currentOverlapCount");
                Scribe_Values.Look<int>(ref currentDuration, "currentDuration");
                Scribe_Values.Look<int>(ref currentRepeatCycle, "currentRepeatCycle");
            }
            catch (Exception ee)
            {
                Log.Error("Error : " + ee.ToString());
            }
        }
        #endregion
    }
}
