using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using RimBuff;
using Verse;
using RimWorld;
using System.Collections;

namespace NegativeRecoil
{
    public class NegativeRecoilBuffDef:BuffDef
    {
        public float additionalAccuracy;
    }
    public class NegativeRecoilBuff : Buff
    {
        #region Fields
        float additionalAccuracy = 1f;

        #endregion

        #region Constructors        
        public NegativeRecoilBuff(NegativeRecoilBuffDef buffDef, ThingWithComps caster):base (buffDef,caster)
        {
            additionalAccuracy = buffDef.additionalAccuracy;
        }
        #endregion

        #region Properties
        public float AdditionalAccuracy
        {
            get
            {
                return additionalAccuracy;
            }
        }
        #endregion

        #region Public Methods
        public override void AddLevel(int level)
        {
            base.AddLevel(level);
        }
        public override IEnumerator TickTest(int interval)
        {
            while (true)
            {
                currentDuration += interval;
                if (currentDuration>=duration)
                {
                    OnDurationExpire();
                }                
                yield return null;
            }
        }
        /*
        public override void Tick(int interval)
        {
            if (duration <= currentDuration)
            {
                currentDuration = 0;
                OnDurationExpire();
            }
            currentDuration += interval;
        }
        */
        public override void OnRefresh()
        {
            currentDuration = 0;
        }
        public override void OnDurationExpire()
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
                Scribe_Values.Look<float>(ref additionalAccuracy, "additionalAccuracy");
            }
            catch (Exception ee)
            {
                Log.Error("Error : " + ee.ToString());
            }
        }
        #endregion
    }
    /*
    public class NegativeRecoilWeaponBuff : Buff
    {
        #region Fields
        public float additionalGunAccuracy = 1.02f;

        #endregion

        #region Constructors
        public NegativeRecoilWeaponBuff(BuffDef def,ThingWithComps caster):base (def,caster)
        {
            
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
            catch (Exception ee)
            {
                Log.Error("Error : " + ee.ToString());
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
            catch (Exception ee)
            {
                Log.Error("Error : " + ee.ToString());
            }
        }
        #endregion

        #region protected Methods

        #endregion

    }*/
    public class Verb_NegativeRecoil_Shoot:Verb_Shoot
    {
        CompBuffManager compBuffM;
        NegativeRecoilProperties properties;

        protected override bool TryCastShot()
        {
            bool result = base.TryCastShot();
            try
            {
                if (compBuffM == null)
                {
                    compBuffM = EquipmentSource.GetComp<CompBuffManager>();
                    properties = (NegativeRecoilProperties)verbProps;
                }
                NegativeRecoilBuff nRWBuff = compBuffM.FindWithDef(properties.weaponBuffDef) as NegativeRecoilBuff;
                NegativeRecoilBuff nRPBuff = compBuffM.FindWithDef(properties.pawnBuffDef) as NegativeRecoilBuff;
                //네거티브 리코일 버프가 없다면
                if (nRWBuff == null)
                {
                    compBuffM.AddBuff(properties.weaponBuffDef,EquipmentSource);
                }
                else
                {
                    nRWBuff.AddLevel(1);
                }

                if (nRPBuff == null)
                {
                    compBuffM.AddBuff(properties.pawnBuffDef, EquipmentSource);
                }
                else
                {
                    nRPBuff.AddLevel(1);
                }
            }
            catch (Exception ee)
            {
                Log.Error("Error : " + ee.ToString());
            }

            return result;
        }
    }
    /*
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
    */
    public class NegativeRecoilProperties : VerbProperties
    {
        public NegativeRecoilBuffDef weaponBuffDef;
        public NegativeRecoilBuffDef pawnBuffDef;
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
                            NegativeRecoilBuff buff = compBuffManager.FindWithTags(new List<string>{"NegativeRecoil","Pawn"}) as NegativeRecoilBuff;
                            if (buff != null)
                            {
                                float additionalVal = Mathf.Pow(buff.AdditionalAccuracy, buff.CurrentOverlapLevel);
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
                            NegativeRecoilBuff buff = compBuffManager.FindWithTags(new List<string> { "NegativeRecoil", "Pawn" }) as NegativeRecoilBuff;
                            if (buff != null)
                            {
                                string text = "StatReport_AdditionalPawnAccuracy".Translate() + ": +" + (Mathf.Pow(buff.AdditionalAccuracy, buff.CurrentOverlapLevel) - 1).ToStringPercent();

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
                        NegativeRecoilBuff buff = buffComp.FindWithTags(new List<string> { "NegativeRecoil", "Weapon" }) as NegativeRecoilBuff;
                        if(buff!=null)
                        {
                            float additionalVal = Mathf.Pow(buff.AdditionalAccuracy, buff.CurrentOverlapLevel);
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
                        NegativeRecoilBuff buff = buffComp.FindWithTags(new List<string> { "NegativeRecoil", "Weapon" }) as NegativeRecoilBuff;
                        if (buff != null)
                        {
                            string text = "StatReport_AdditionalWeaponAccuracy".Translate() + ": +" + (Mathf.Pow(buff.AdditionalAccuracy, buff.CurrentOverlapLevel) - 1).ToStringPercent();
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