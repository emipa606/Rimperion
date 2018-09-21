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
    [DefOf]
    public static class NegativeRecoilDefOf
    {
        public static NegativeRecoilWeaponBuffDef NegativeRecoilWeaponBuff;
        public static NegativeRecoilPawnBuffDef NegativeRecoilPawnBuff;
    }
    public class NegativeRecoilWeaponBuffDef : BuffDef
    {
        public float additionalGunAccuracy;
    }
    public class NegativeRecoilPawnBuffDef : BuffDef
    {
        public float additionalPawnAccuracy;
    }

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
        public NegativeRecoilWeaponBuff(string buffName,int maxLevel,float duration,float additional,ThingWithComps target)
        {
            this.buffName = buffName;
            this.maxLevel = maxLevel;
            this.duration = GenTicks.SecondsToTicks(duration);
            this.additionalGunAccuracy = additional;
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
            OnRefresh();
        }
        public override void OnRefresh()
        {
            currentDuration = 0;
            /*target.def.SetStatBaseValue(StatDefOf.AccuracyLong, initialLongValue + (Mathf.Pow(additionalGunAccuracy, currentLevel) - 1));
            target.def.SetStatBaseValue(StatDefOf.AccuracyMedium, initialMediumValue + (Mathf.Pow(additionalGunAccuracy, currentLevel) - 1));
            target.def.SetStatBaseValue(StatDefOf.AccuracyShort, initialShortValue + (Mathf.Pow(additionalGunAccuracy, currentLevel) - 1));*/

        }
        public override void OnDestroy()
        {
            /*target.def.SetStatBaseValue(StatDefOf.AccuracyLong, initialLongValue);
            target.def.SetStatBaseValue(StatDefOf.AccuracyMedium, initialMediumValue);
            target.def.SetStatBaseValue(StatDefOf.AccuracyShort, initialShortValue);*/
            owner.RemoveBuff(buffName);
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
            /*
            if (Scribe.mode == LoadSaveMode.LoadingVars)
            {
                additionalGunAccuracy = NegativeRecoilDefOf.NegativeRecoilWeaponBuff.additionalGunAccuracy;
            }*/
        }
        #endregion

        #region protected Methods
        protected override void OnDurationExpire()
        {
            currentLevel -= 1;
            OnRefresh();
            if (currentLevel <= 0)
            {
                OnDestroy();
            }
        }
        #endregion

    }
    public class NegativeRecoilPawnBuff : Buff
    {
        #region Fields
        public float additionalPawnAccuracy = 1.02f;
        #endregion

        #region Constructors
        public NegativeRecoilPawnBuff() { }
        public NegativeRecoilPawnBuff(string buffName, int maxLevel, float duration, float additional, ThingWithComps target)
        {
            this.buffName = buffName;
            this.maxLevel = maxLevel;
            this.duration = GenTicks.SecondsToTicks(duration);
            this.additionalPawnAccuracy = additional;
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
            OnRefresh();
        }
        public override void OnRefresh()
        {
            currentDuration = 0;
            //target.def.SetStatBaseValue(StatDefOf.ShootingAccuracyPawn, initialValue + (Mathf.Pow(additionalPawnAccuracy, currentLevel) - 1));
        }
        public override void OnDestroy()
        {
            //target.def.SetStatBaseValue(StatDefOf.ShootingAccuracyPawn, initialValue);
            owner.RemoveBuff(buffName);
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

        protected override void OnDurationExpire()
        {
            currentLevel -= 1;
            OnRefresh();
            if (currentLevel <= 0)
            {
                OnDestroy();
            }
        }
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

            CompBuffManager compBuffM = CasterPawn.GetComp<CompBuffManager>();
            if(compBuffM.BuffList.Count>0)
            {
                Log.Message(compBuffM.BuffList.Count.ToString());
            }
            NegativeRecoilProperties prop = this.verbProps as NegativeRecoilProperties;
            if (compBuffM == null)
            {
                Log.Error(CasterPawn.ToString() + "doesn't have BuffManagerComponent");
            }
            else
            {
                try
                {
                    //Log.Message(prop.gunBuffName);
                    NegativeRecoilWeaponBuff tempWeaponBuff = compBuffM.FindBuff(prop.gunBuffName) as NegativeRecoilWeaponBuff;
                    if (tempWeaponBuff == null)
                    {
                        //Log.Message(prop.gunBuffName+"is none Create");
                        try
                        {
                            tempWeaponBuff = new NegativeRecoilWeaponBuff(prop.gunBuffName, prop.gunMaxLevel, prop.gunDuration, prop.gunAddtional, EquipmentSource);
                            tempWeaponBuff.Caster = CasterPawn;
                            compBuffM.AddBuff(tempWeaponBuff);
                            /*if(compBuffM.FindBuff(prop.gunBuffName)!=null)
                            {
                                Log.Message(compBuffM.FindBuff(prop.gunBuffName).BuffName + " alive");
                            }*/
                        }
                        catch
                        {
                            Log.Error("Here");
                        }

                    }
                    else
                    {
                        try
                        {
                            if(tempWeaponBuff.Target == EquipmentSource)
                            {
                                tempWeaponBuff.AddLevel(1);
                            }
                            else
                            {
                                compBuffM.RemoveBuffAll(prop.gunBuffName);
                                compBuffM.RemoveBuffAll(prop.pawnBuffName);

                                tempWeaponBuff = new NegativeRecoilWeaponBuff(prop.gunBuffName,prop.gunMaxLevel,prop.gunDuration,prop.gunAddtional, EquipmentSource);
                                tempWeaponBuff.Caster = CasterPawn;
                                compBuffM.AddBuff(tempWeaponBuff);
                                
                            }
                            
                        }
                        catch
                        {

                            Log.Error(tempWeaponBuff.ToString() + " And Here");
                        }
                    }
                }
                catch
                {
                    Log.Error(CasterPawn.ToString() + "Add NegativeRecoilWeapon Error");
                }
                //여기선 폰에게 반복
                try
                {
                    
                    NegativeRecoilPawnBuff tempPawnBuff = compBuffM.FindBuff(prop.pawnBuffName) as NegativeRecoilPawnBuff;
                    if (tempPawnBuff == null)
                    {
                        tempPawnBuff = new NegativeRecoilPawnBuff(prop.pawnBuffName, prop.pawnMaxLevel, prop.pawnDuration, prop.pawnAddtional, CasterPawn);
                        tempPawnBuff.Caster = CasterPawn;
                        compBuffM.AddBuff(tempPawnBuff);
                    }
                    else
                    {
                        tempPawnBuff.AddLevel(1);
                    }
                }
                catch
                {
                    Log.Error(CasterPawn.ToString() + "Add NegativeRecoilPawn Error");
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

            /*
             * 1. 폰인지 확인
             * 2. 폰이라면 들고잇는 무기가 네거티브리코일 무기인지 확인
             * 3. 네거티브 리코일 무기라면 현재 해당무기가 장착 되어있는지 재차 확인.
             * 4. 내가 적용대상이면 해당수치만큼 반환
             */

            try
            {
                if (req.HasThing)
                {
                    Pawn pawn = req.Thing as Pawn;
                    if(pawn.equipment.Primary.def.weaponTags.Contains("NegativeRecoil"))
                    {
                        CompBuffManager compBuffManager = pawn.GetComp<CompBuffManager>();
                        NegativeRecoilPawnBuff buff = compBuffManager.FindBuff("NegativeRecoilPawn") as NegativeRecoilPawnBuff;
                        if (buff != null)
                        {
                            float additionalVal = Mathf.Pow(buff.additionalPawnAccuracy, buff.CurrentLevel);
                            val += (additionalVal - 1);
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
            if (req.HasThing)
            {
                try
                {
                    if(req.HasThing)
                    {
                        Pawn pawn = req.Thing as Pawn;
                        CompBuffManager compBuffManager = pawn.GetComp<CompBuffManager>();
                        NegativeRecoilPawnBuff buff = compBuffManager.FindBuff("NegativeRecoilPawn") as NegativeRecoilPawnBuff;
                        if (buff != null)
                        {
                            string text = "StatReport_AdditionalPawnAccuracy".Translate() + ": +" + (Mathf.Pow(buff.additionalPawnAccuracy, buff.CurrentLevel) - 1).ToStringPercent();

                            return text;
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
                    if (req.Thing.holdingOwner != null)
                    {
                        Pawn_EquipmentTracker equipmentTracker = req.Thing.holdingOwner.Owner as Pawn_EquipmentTracker;
                        if (equipmentTracker != null)
                        {
                            CompBuffManager compBuffManager = equipmentTracker.pawn.GetComp<CompBuffManager>();
                            NegativeRecoilWeaponBuff buff = compBuffManager.FindBuff("NegativeRecoilWeapon") as NegativeRecoilWeaponBuff;
                            if(buff!=null)
                            {
                                float additionalVal = Mathf.Pow(buff.additionalGunAccuracy, buff.CurrentLevel);
                                //Log.Message(this.parentStat.ToString() + ": " + val + "(val) + " + additionalVal + "(add) = " + (val + additionalVal));


                                val += (additionalVal-1);
                            }
                            
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
                    if (req.HasThing)
                    {
                        if (req.Thing.holdingOwner != null)
                        {
                            Pawn_EquipmentTracker equipmentTracker = req.Thing.holdingOwner.Owner as Pawn_EquipmentTracker;
                            if (equipmentTracker != null)
                            {
                                CompBuffManager compBuffManager = equipmentTracker.pawn.GetComp<CompBuffManager>();
                                NegativeRecoilWeaponBuff buff = compBuffManager.FindBuff("NegativeRecoilWeapon") as NegativeRecoilWeaponBuff;
                                if (buff != null)
                                {
                                    string text = "StatReport_AdditionalWeaponAccuracy".Translate() + ": +" + (Mathf.Pow(buff.additionalGunAccuracy, buff.CurrentLevel) - 1).ToStringPercent();
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
                        Log.Error(parentStat.defName + " - " + req.Thing.ToString() + " ExplanationPart");
                    }
                    else
                    {
                        Log.Error(parentStat.defName + " - " + " ExplanationPart");
                    }
                }
            }
            return string.Empty;
        }
        #endregion

    }


}