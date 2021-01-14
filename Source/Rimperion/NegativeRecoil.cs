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
        public NegativeRecoilBuff() : base() { }
        public NegativeRecoilBuff(NegativeRecoilBuffDef buffDef, ThingWithComps caster):base (buffDef,caster)
        {
            additionalAccuracy = buffDef.additionalAccuracy;
        }
        #endregion

        #region Properties
        public float AdditionalAccuracy => additionalAccuracy;
        #endregion

        #region Public Methods
        public override void OnCreate()
        {
            base.OnCreate();
        }
        public override void AddOverlapLevel(int level)
        {
            base.AddOverlapLevel(level);
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
  
    public class Verb_NegativeRecoil_Shoot:Verb_Shoot
    {
        CompBuffManager compBuffM;
        NegativeRecoilProperties properties;

        protected override bool TryCastShot()
        {
            var result = base.TryCastShot();
            try
            {
                if (compBuffM == null)
                {
                    compBuffM = EquipmentSource.GetComp<CompBuffManager>();
                    properties = (NegativeRecoilProperties)verbProps;
                }
                //네거티브 리코일 버프가 없다면
                if (!(compBuffM.FindWithDef(properties.weaponBuffDef) is NegativeRecoilBuff nRWBuff))
                {
                    compBuffM.AddBuff(properties.weaponBuffDef, EquipmentSource);
                }
                else
                {
                    nRWBuff.AddOverlapLevel(1);
                }

                if (!(compBuffM.FindWithDef(properties.pawnBuffDef) is NegativeRecoilBuff nRPBuff))
                {
                    compBuffM.AddBuff(properties.pawnBuffDef, EquipmentSource);
                }
                else
                {
                    nRPBuff.AddOverlapLevel(1);
                }
            }
            catch (Exception ee)
            {
                Log.Error("Error : " + ee.ToString());
            }

            return result;
        }
    }
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
                if (!req.HasThing)
                {
                    return;
                }
                if (!(req.Thing is Pawn pawn) || pawn.equipment.Primary == null || pawn.equipment.Primary.def.weaponTags == null)
                {
                    return;
                }
                if (!pawn.equipment.Primary.def.weaponTags.Contains("NegativeRecoil"))
                {
                    return;
                }
                CompBuffManager compBuffManager = pawn.equipment.Primary.GetComp<CompBuffManager>();
                if (compBuffManager == null)
                {
                    return;
                }
                if (compBuffManager.FindWithTags(new List<string> { "NegativeRecoil", "Pawn" }) is NegativeRecoilBuff buff)
                {
                    var additionalVal = Mathf.Pow(buff.AdditionalAccuracy, buff.CurrentOverlapLevel);
                    val += additionalVal - 1;
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
                if (req.HasThing)
                {
                    if (req.Thing is Pawn pawn && pawn.equipment.Primary != null && pawn.equipment.Primary.def.weaponTags != null)
                    {
                        if (pawn.equipment.Primary.def.weaponTags.Contains("NegativeRecoil"))
                        {
                            CompBuffManager compBuffManager = pawn.equipment.Primary.GetComp<CompBuffManager>();
                            if (compBuffManager != null)
                            {
                                if (compBuffManager.FindWithTags(new List<string> { "NegativeRecoil", "Pawn" }) is NegativeRecoilBuff buff)
                                {
                                    string text = "StatReport_AdditionalPawnAccuracy".Translate() + ": +" + (Mathf.Pow(buff.AdditionalAccuracy, buff.CurrentOverlapLevel) - 1).ToStringPercent();

                                    return text;
                                }
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
                if (!(req.Thing is ThingWithComps weaponThing))
                {
                    return;
                }
                CompBuffManager buffComp = weaponThing.GetComp<CompBuffManager>();
                if (buffComp == null)
                {
                    return;
                }
                if (buffComp.FindWithTags(new List<string> { "NegativeRecoil", "Weapon" }) is NegativeRecoilBuff buff)
                {
                    var additionalVal = Mathf.Pow(buff.AdditionalAccuracy, buff.CurrentOverlapLevel);
                    val += additionalVal - 1;
                }
            }
            catch (Exception ee)
            {
                if (req.HasThing)
                {
                    Log.Error("StatPart_AdditionalWeaponAccuracy.TransformValue() Fail from :" + req.Thing.ToString() + ": "+ ee);
                }
            }
        }
        public override string ExplanationPart(StatRequest req)
        {
            if (req.HasThing)
            {
                try
                {
                    if (req.Thing.def.weaponTags != null)
                    {
                        if (req.Thing.def.weaponTags.Contains("NegativeRecoil"))
                        {
                            if (req.Thing is ThingWithComps weaponThing)
                            {
                                CompBuffManager buffComp = weaponThing.GetComp<CompBuffManager>();
                                if (buffComp != null)
                                {
                                    if (buffComp.FindWithTags(new List<string> { "NegativeRecoil", "Weapon" }) is NegativeRecoilBuff buff)
                                    {
                                        string text = "StatReport_AdditionalWeaponAccuracy".Translate() + ": +" + (Mathf.Pow(buff.AdditionalAccuracy, buff.CurrentOverlapLevel) - 1).ToStringPercent();
                                        return text;
                                    }
                                }
                            }

                        }
                    }
                }
                catch (Exception ee)
                {
                    Log.Error(parentStat.defName + " - " + req.Thing.ToString() + " ExplanationPart :" + ee.ToString());

                }
            }
            return string.Empty;
        }
        #endregion

    }


}