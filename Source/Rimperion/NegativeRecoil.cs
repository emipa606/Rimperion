using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using RimBuff;
using Verse;
using RimWorld;

namespace NegativeRecoil
{

    public class NegativeRecoilWeaponBuff : Buff
    {

        #region Fields
        /*private float initialShortValue = 0;
        private float initialMediumValue = 0;
        private float initialLongValue = 0;*/
        public float additionalGunAccuracy = 1.02f;

        #endregion

        #region Constructors
        public NegativeRecoilWeaponBuff() { }
        public NegativeRecoilWeaponBuff(string uniqueName,int maxLevel,float duration,float additional,ThingWithComps caster,ThingWithComps target)
        {
            this.uniqueName = uniqueName;
            uniqueID = uniqueName + GetHashCode();
            this.maxLevel = maxLevel;
            this.duration = GenTicks.SecondsToTicks(duration);
            this.additionalGunAccuracy = additional;
            this.caster = caster;
            this.target = target;

            currentLevel = 1;
            currentDuration = 0;
            innerElapseTick = 0;
        }
        #endregion

        #region Properties

        #endregion

        #region Public Methods
        public override void AddLevel(int level)
        {
            base.AddLevel(level);
            OnRefresh();
        }
        public override void Tick(int interval)
        {
            if (duration <= currentDuration)
            {
                currentDuration = 0;
                OnDurationExpire();
            }
            currentDuration += interval;
        }

        public override void OnCreate()
        {
        }
        public override void OnRefresh()
        {
            currentDuration = 0;

        }
        public override void OnDestroy()
        {
        }

        public override void OnDurationExpire()
        {
            currentLevel -= 1;
            OnRefresh();
            if (currentLevel <= 0)
            {
                owner.RemoveBuff(this);
            }
        }

        public override void ExposeData()
        {
            base.ExposeData();
            try
            {
                Scribe_Values.Look<float>(ref additionalGunAccuracy, "additionalGunAccuracy");
            }
            catch
            {

            }
        }
        #endregion

        #region protected Methods

        #endregion

    }
    public class NegativeRecoilPawnBuff : Buff
    {
        #region Fields
        public float additionalPawnAccuracy = 1.02f;
        #endregion

        #region Constructors
        public NegativeRecoilPawnBuff()
        {
            uniqueID = "needBuffName" + GetHashCode();
        }
        public NegativeRecoilPawnBuff(string uniqueName, int maxLevel, float duration, float additional,ThingWithComps caster, ThingWithComps target)
        {
            this.uniqueName = uniqueName;
            uniqueID = uniqueName + GetHashCode();
            this.maxLevel = maxLevel;
            this.duration = GenTicks.SecondsToTicks(duration);
            this.additionalPawnAccuracy = additional;
            this.caster = caster;
            this.target = target;

            currentLevel = 1;
            currentDuration = 0;
            innerElapseTick = 0;
        }
        #endregion

        #region Properties

        #endregion

        #region Public Methods
        public override void AddLevel(int level)
        {
            base.AddLevel(level);
            OnRefresh();
        }
        public override void Tick(int interval)
        {
            if (duration <= currentDuration)
            {
                currentDuration = 0;
                OnDurationExpire();
            }
            currentDuration++;
        }

        public override void OnCreate()
        {
        }
        public override void OnRefresh()
        {
            currentDuration = 0;
        }
        public override void OnDestroy()
        {
        }

        public override void OnDurationExpire()
        {
            currentLevel -= 1;
            OnRefresh();
            if (currentLevel <= 0)
            {
                owner.RemoveBuff(this);
            }
        }

        public override void ExposeData()
        {
            base.ExposeData();
            try
            {
                Scribe_Values.Look<float>(ref additionalPawnAccuracy, "additionalPawnAccuracy");
            }
            catch
            {

            }
        }
        #endregion

        #region protected Methods

        #endregion

    }

    public class Verb_Shoot_NegativeRecoil : Verb_LaunchProjectile
    {
        #region Fields		
        protected override int ShotsPerBurst
		{
			get
			{
				return this.verbProps.burstShotCount;
			}
        }
        #endregion

        #region Public Methods
        public override void WarmupComplete()
        {
            base.WarmupComplete();
            Pawn pawn = currentTarget.Thing as Pawn;
            if (pawn != null && !pawn.Downed && base.CasterIsPawn && base.CasterPawn.skills != null)
            {
                float num = (!pawn.HostileTo(this.caster)) ? 20f : 170f;
                float num2 = this.verbProps.AdjustedFullCycleTime(this, base.CasterPawn);
                base.CasterPawn.skills.Learn(SkillDefOf.Shooting, num * num2, false);
            }
        }
        #endregion

        #region Protected Methods
        protected override bool TryCastShot()
        {
            bool flag = base.TryCastShot();
            if (flag && base.CasterIsPawn)
            {
                base.CasterPawn.records.Increment(RecordDefOf.ShotsFired);
            }
            //WeaponBuff
            CompBuffManager weaponBuffManager = EquipmentSource.GetComp<CompBuffManager>();
            if(weaponBuffManager !=null)
            {
                NegativeRecoilProperties prop = verbProps as NegativeRecoilProperties;

                NegativeRecoilWeaponBuff negativeRecoilWeaponBuff = weaponBuffManager.FindWithName(prop.gunBuffName) as NegativeRecoilWeaponBuff;
                if(negativeRecoilWeaponBuff==null)
                {
                    negativeRecoilWeaponBuff = new NegativeRecoilWeaponBuff(prop.gunBuffName, prop.gunMaxLevel, prop.gunDuration, prop.gunAddtional, EquipmentSource, EquipmentSource);
                    weaponBuffManager.AddBuff(negativeRecoilWeaponBuff);
                }
                else
                {
                    negativeRecoilWeaponBuff.AddLevel(1);
                }

                NegativeRecoilPawnBuff negativeRecoilPawnBuff = weaponBuffManager.FindWithName(prop.pawnBuffName) as NegativeRecoilPawnBuff;
                if (negativeRecoilPawnBuff == null)
                {
                    negativeRecoilPawnBuff = new NegativeRecoilPawnBuff(prop.pawnBuffName, prop.pawnMaxLevel, prop.pawnDuration, prop.pawnAddtional, EquipmentSource, CasterPawn);
                    weaponBuffManager.AddBuff(negativeRecoilPawnBuff);
                }
                else
                {
                    negativeRecoilPawnBuff.AddLevel(1);
                }
            }
            return flag;
        }
        #endregion
    }

    public class NegativeRecoilProperties : VerbProperties
    {
        public string gunBuffName;
        public int gunMaxLevel=10;
        public float gunDuration =3;
        public float gunAddtional=1.03f;

        public string pawnBuffName;
        public int pawnMaxLevel=10;
        public float pawnDuration =3;
        public float pawnAddtional=1.03f;
    }
    
    public class StatPart_AdditionalPawnAccuracy : StatPart
    {
        #region Public Method
        public override void TransformValue(StatRequest req, ref float val)
        {
            try
            {
                if (req.HasThing)
                {
                    Pawn pawn = req.Thing as Pawn;
                    if(pawn.equipment.Primary!=null)
                    {
                        if (pawn.equipment.Primary.def.weaponTags.Contains("NegativeRecoil"))
                        {
                            CompBuffManager compBuffManager = pawn.equipment.Primary.GetComp<CompBuffManager>();
                            NegativeRecoilPawnBuff buff = compBuffManager.FindWithName("NegativeRecoilPawn") as NegativeRecoilPawnBuff;
                            if (buff != null)
                            {
                                float additionalVal = Mathf.Pow(buff.additionalPawnAccuracy, buff.CurrentLevel);
                                val += (additionalVal - 1);
                            }
                        }
                    }
                }
            }
            catch
            {
                if (req.HasThing)
                {
                    Log.Error("StatPart_AdditionalPawnAccuracy.TransformValue() Fail from :" + req.Thing.ToString());
                }
            }
        }
        public override string ExplanationPart(StatRequest req)
        {
            try
            {
                if(req.HasThing)
                {
                    Pawn pawn = req.Thing as Pawn;
                    if (pawn.equipment.Primary != null)
                    {
                        if (pawn.equipment.Primary.def.weaponTags.Contains("NegativeRecoil"))
                        {
                            CompBuffManager compBuffManager = pawn.equipment.Primary.GetComp<CompBuffManager>();
                            NegativeRecoilPawnBuff buff = compBuffManager.FindWithName("NegativeRecoilPawn") as NegativeRecoilPawnBuff;
                            if (buff != null)
                            {
                                string text = "StatReport_AdditionalPawnAccuracy".Translate() + ": +" + (Mathf.Pow(buff.additionalPawnAccuracy, buff.CurrentLevel) - 1).ToStringPercent();

                                return text;
                            }
                        }                            
                    }
                }
            }
            catch
            {
                if(req.HasThing)
                {
                    Log.Error(parentStat.defName + " - "+req.Thing.ToString() + " ExplanationPart");
                }
                else
                {
                    Log.Error(parentStat.defName + " - " + " ExplanationPart");
                }
            }
            return string.Empty;
        }
        #endregion

    }
    public class StatPart_AdditionalWeaponAccuracy : StatPart
    {
        #region Public Method
        public override void TransformValue(StatRequest req, ref float val)
        {
            try
            {
                if (req.HasThing)
                {
                    if (req.Thing.def.weaponTags.Contains("NegativeRecoil"))
                    {
                        ThingWithComps weaponThing = req.Thing as ThingWithComps;
                        CompBuffManager buffComp = weaponThing.GetComp<CompBuffManager>();
                        NegativeRecoilWeaponBuff buff = buffComp.FindWithName("NegativeRecoilWeapon") as NegativeRecoilWeaponBuff;
                        if(buff!=null)
                        {
                            float additionalVal = Mathf.Pow(buff.additionalGunAccuracy, buff.CurrentLevel);
                            val += (additionalVal - 1);
                        }
                    }
                }
            }
            catch
            {
                if (req.HasThing)
                {
                    Log.Error("StatPart_AdditionalWeaponAccuracy.TransformValue() Fail from :" + req.Thing.ToString());
                }
            }
        }
        public override string ExplanationPart(StatRequest req)
        {
            if (req.HasThing)
            {
                try
                {
                    if (req.Thing.def.weaponTags.Contains("NegativeRecoil"))
                    {
                        ThingWithComps weaponThing = req.Thing as ThingWithComps;
                        CompBuffManager buffComp = weaponThing.GetComp<CompBuffManager>();
                        NegativeRecoilWeaponBuff buff = buffComp.FindWithName("NegativeRecoilWeapon") as NegativeRecoilWeaponBuff;
                        if (buff != null)
                        {
                            string text = "StatReport_AdditionalWeaponAccuracy".Translate() + ": +" + (Mathf.Pow(buff.additionalGunAccuracy, buff.CurrentLevel) - 1).ToStringPercent();
                            return text;
                        }
                    }
                }
                catch
                {
                    Log.Error(parentStat.defName + " - " + req.Thing.ToString() + " ExplanationPart");
                    
                }
            }
            return string.Empty;
        }
        #endregion

    }


}