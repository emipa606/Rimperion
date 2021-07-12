using System;
using RimBuff;
using Verse;

namespace NegativeRecoil
{
    public class Verb_NegativeRecoil_Shoot : Verb_Shoot
    {
        private CompBuffManager compBuffM;
        private NegativeRecoilProperties properties;

        protected override bool TryCastShot()
        {
            var result = base.TryCastShot();
            try
            {
                if (compBuffM == null)
                {
                    compBuffM = EquipmentSource.GetComp<CompBuffManager>();
                    properties = (NegativeRecoilProperties) verbProps;
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
                Log.Error("Error : " + ee);
            }

            return result;
        }
    }
}