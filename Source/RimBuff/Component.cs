using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Verse;
using RimWorld;
using HarmonyLib;
using System.Reflection;
namespace RimBuff
{
    public class CompBuffManager : ThingComp
    {
        #region Fields
        private List<Buff> buffList;
        #endregion

        #region Constructors
        public CompBuffManager()
        {
            if (buffList == null)
            {
                buffList = new List<Buff>();
            }
            BuffController.CompList.Add(this);
        }
        ~CompBuffManager()
        {
            BuffController.CompList.Remove(this);
        }
        #endregion

        #region Properties
        public List<Buff> BuffList
        {
            get
            {
                if (buffList == null)
                {
                    buffList = new List<Buff>();
                }
                return buffList;
            }
        }
        #endregion

        #region Public Methods
        public void Tick()
        {
            try
            {
                if (buffList.Count > 0)
                {
                    for (var index = 0; index < buffList.Count; index++)
                    {
                        buffList[index].TickTest(1).MoveNext();
                    }
                }
            }
            catch (Exception ee)
            {
                Log.Error("Error : " + ee.ToString());
            }

        }

        //버프 추가
        public void AddBuff(Buff buff)
        {
            try
            {
                buffList.Add(buff);
                buff.Owner = this;
            }
            catch (Exception ee)
            {
                Log.Error("Error : " + ee.ToString());
            }
        }
        public Buff AddBuff(BuffDef def,ThingWithComps caster)
        {
            try
            {
                var buff = Activator.CreateInstance(def.buffClass,def,caster) as Buff;
                buff.Caster = caster;
                buffList.Add(buff);
                buff.Owner = this;
                return buff;
            }
            catch (Exception ee)
            {
                Log.Error("Error : " + ee.ToString());
            }
            return default;
        }
        

        //버프 삭제
        public void RemoveBuff(Buff buff)
        {
            buff.OnDestroy();
            buffList.Remove(buff);
            buff.Owner = null;
        }
        public void RemoveWithDefName(string defName)
        {
            try
            {
                if (defName != null)
                {
                    RemoveBuff(buffList.Find(buff => buff.Def.defName == defName));
                }
                Log.Error(parent.ToString() + " RemoveBuff - Can't Find Buff: Cause uniqueName is Empty ");
            }
            catch (Exception ee)
            {
                Log.Error("Error : " + ee.ToString());
            }
        }
        public void RemoveWithLabel(string label)
        {
            try
            {
                if (label != null)
                {
                    RemoveBuff(buffList.Find(buff => buff.Def.label == label));
                }
                Log.Error(parent.ToString() + " RemoveBuff - Can't Find Buff: Cause label is Empty ");
            }
            catch (Exception ee)
            {
                Log.Error("Error : " + ee.ToString());
            }
        }
        public void RemoveWithUniqueID(string id)
        {
            try
            {
                if (id != null)
                {
                    RemoveBuff(buffList.Find(buff => buff.UniqueID == id));
                }
                Log.Error(parent.ToString() + " RemoveBuff - Can't Find Buff: Cause id is Empty ");
            }
            catch (Exception ee)
            {
                Log.Error("Error : " + ee.ToString());
            }
        }
        public void RemoveWithDef(BuffDef def)
        {
            try
            {
                if (def != null)
                {
                    RemoveBuff(buffList.Find(buff => buff.Def == def));
                }
                Log.Error(parent.ToString() + " RemoveBuff - Can't Find Buff: Cause def is Empty ");
            }
            catch (Exception ee)
            {
                Log.Error("Error : " + ee.ToString());
            }
        }
        public void RemoveWithTag(string tag)
        {
            try
            {
                if (tag != null)
                {
                    RemoveBuff(buffList.Find(buff => buff.Def.tagList.Contains(tag)));
                }
                Log.Error(parent.ToString() + " RemoveBuff - Can't Find Buff: Cause tag is Empty ");
            }
            catch (Exception ee)
            {
                Log.Error("Error : " + ee.ToString());
            }
        }
        public void RemoveWithTags(List<string> tagList)
        {
            try
            {
                if (tagList != null)
                {
                    RemoveBuff(FindWithTags(tagList));
                }
                Log.Error(parent.ToString() + " RemoveBuff - Can't Find Buff: Cause tagList is Empty ");
            }
            catch (Exception ee)
            {
                Log.Error("Error : " + ee.ToString());
            }
        }
        public void RemoveWithCaster(ThingWithComps caster)
        {
            try
            {
                if (caster != null)
                {
                    RemoveBuff(buffList.Find(buff => buff.Caster == caster));
                }
                Log.Error(parent.ToString() + " RemoveBuff - Can't Find Buff: Cause target is Empty ");
            }
            catch (Exception ee)
            {
                Log.Error("Error : " + ee.ToString());
            }
        }

        public void RemoveAllWithDefName(string defName)
        {
            try
            {
                if (defName != null)
                {
                    List<Buff> removeList = FindAllWithDefName(defName);
                    if (removeList != null)
                    {
                        for (var index = 0; index < removeList.Count; index++)
                        {
                            RemoveBuff(removeList[index]);
                        }
                    }
                }
                Log.Error(parent.ToString() + " RemoveBuffAll - Can't Find Buff: Cause buffName is Empty ");
            }
            catch (Exception ee)
            {
                Log.Error("Error : " + ee.ToString());
            }
        }
        public void RemoveAllWithLabel(string label)
        {
            try
            {
                if (label != null)
                {
                    List<Buff> removeList = FindAllWithLabel(label);
                    if (removeList != null)
                    {
                        for (var index = 0; index < removeList.Count; index++)
                        {
                            RemoveBuff(removeList[index]);
                        }
                    }
                }
                Log.Error(parent.ToString() + " RemoveBuffAll - Can't Find Buff: Cause label is Empty ");
            }
            catch (Exception ee)
            {
                Log.Error("Error : " + ee.ToString());
            }
        }
        public void RemoveAllWithTag(string tag)
        {
            try
            {
                if (tag != null)
                {
                    List<Buff> removeList = FindAllWithTag(tag);
                    if (removeList != null)
                    {
                        for (var index = 0; index < removeList.Count; index++)
                        {
                            RemoveBuff(removeList[index]);
                        }
                    }
                }
                Log.Error(parent.ToString() + " RemoveBuffAll - Can't Find Buff: Cause tag is Empty ");
            }
            catch (Exception ee)
            {
                Log.Error("Error : " + ee.ToString());
            }
        }
        public void RemoveAllWithTags(List<string> tagList)
        {
            try
            {
                if (tagList != null)
                {
                    List<Buff> removeList = FindAllWithTags(tagList);
                    if (removeList != null)
                    {
                        for (var index = 0; index < removeList.Count; index++)
                        {
                            RemoveBuff(removeList[index]);
                        }
                    }
                }
                Log.Error(parent.ToString() + " RemoveBuffAll - Can't Find Buff: Cause tag is Empty ");
            }
            catch (Exception ee)
            {
                Log.Error("Error : " + ee.ToString());
            }
        }
        public void RemoveBuffAll(BuffDef def)
        {
            try
            {
                if (def != null)
                {
                    List<Buff> removeList = FindAllWithDef(def);
                    if (removeList != null)
                    {
                        for (var index = 0; index < removeList.Count; index++)
                        {
                            RemoveBuff(removeList[index]);
                        }
                    }
                }
                Log.Error(parent.ToString() + " RemoveBuffAll - Can't Find Buff: Cause def is Empty ");
            }
            catch (Exception ee)
            {
                Log.Error("Error : " + ee.ToString());
            }
        }
        public void RemoveBuffAll(ThingWithComps caster)
        {
            try
            {
                if (caster != null)
                {
                    List<Buff> removeList = FindAllWithCaster(caster);
                    if (removeList != null)
                    {
                        for (var index = 0; index < removeList.Count; index++)
                        {
                            RemoveBuff(removeList[index]);
                        }
                    }
                }
                Log.Error(parent.ToString() + " RemoveBuffAll - Can't Find Buff: Cause caster is Empty ");
            }
            catch (Exception ee)
            {
                Log.Error("Error : " + ee.ToString());
            }

        }
        public void RemoveBuffAll()
        {
            try
            {

                for (var index = 0; index < buffList.Count; index++)
                {
                    RemoveBuff(buffList[index]);
                }
            }
            catch (Exception ee)
            {
                Log.Error("Error : " + ee.ToString());
            }
        }

        //버프 확인
        public bool ContainBuff(Buff buff)
        {
            return buffList.Contains(buff);
        }
        public bool ContainDef(BuffDef def)
        {
            if (FindWithDef(def) != null)
            {
                return true;
            }
            return false;
        }
        public bool ContainTag(string tag)
        {
            if (FindWithTag(tag) != null)
            {
                return true;
            }
            return false;
        }
        public bool ContainTags(List<string> tagList)
        {
            if (FindWithTags(tagList) != null)
            {
                return true;
            }
            return false;
        }
        public bool ContainDefName(string defName)
        {
            if (FindWithName(defName) != null)
            {
                return true;
            }
            return false;
        }
        public bool ContainLabel(string label)
        {
            if (FindWithLabel(label) != null)
            {
                return true;
            }
            return false;
        }
        public bool ContainUniqueID(string uniqueID)
        {
            if (FindWithUniqueID(uniqueID) != null)
            {
                return true;
            }
            return false;
        }
        public bool ContainCaster(ThingWithComps caster)
        {
            if (FindWithCaster(caster) != null)
            {
                return true;
            }
            return false;
        }

        //버프 검색
        public Buff FindBuff(Buff targetBuff)
        {
            try
            {
                if (targetBuff != null)
                {
                    return buffList.Find(buff => buff == targetBuff);
                }
                Log.Error(parent.ToString() + " Can't Find Buff: Cause buff is Empty ");
            }
            catch (Exception ee)
            {
                Log.Error("Error : " + ee.ToString());
            }
            return default;
        }
        public Buff FindWithUniqueID(string id)
        {
            try
            {
                if (id != null)
                {
                    return buffList.Find(buff => buff.UniqueID == id);
                }
                Log.Error(parent.ToString() + " Can't Find Buff: Cause buffName is Empty ");
            }
            catch (Exception ee)
            {
                Log.Error("Error : " + ee.ToString());
            }
            return default;
        }
        public Buff FindWithTag(string tag)
        {
            try
            {
                if (tag != null)
                {
                    return buffList.Find(buff => buff.Def.tagList.Contains(tag));
                }
                Log.Error(parent.ToString() + " Can't Find Buff: Cause tag is Empty ");
            }
            catch (Exception ee)
            {
                Log.Error("Error : " + ee.ToString());
            }
            return default;
        }
        public Buff FindWithTags(List<string> tagList)
        {
            try
            {
                if (tagList != null)
                {
                    
                    Buff result = null;
                    for(var index=0;index<buffList.Count;index++)
                    {
                        for (var innerIndex = 0; innerIndex < tagList.Count; innerIndex++)
                        {
                            if(buffList[index].Def.tagList.Contains(tagList[innerIndex]))
                            {
                                result = buffList[index];
                            }
                            else
                            {
                                result = null;
                                break;
                            }
                        }
                        if(result!=null)
                        {
                            return result;
                        }
                    }
                    return result;
                }
                Log.Error(parent.ToString() + " Can't Find Buff: Cause tagList is Empty ");
            }
            catch (Exception ee)
            {
                Log.Error("Error : " + ee.ToString());
            }
            return default;
        }
        public Buff FindWithName(string defName)
        {
            try
            {
                if (defName != null)
                {
                    return buffList.Find(buff => buff.Def.defName == defName);
                }
                Log.Error(parent.ToString() + " Can't Find Buff: Cause buffName is Empty ");
            }
            catch (Exception ee)
            {
                Log.Error("Error : " + ee.ToString());
            }
            return default;
        }
        public Buff FindWithLabel(string label)
        {
            try
            {
                if (label != null)
                {
                    return buffList.Find(buff => buff.Def.label == label);
                }
                Log.Error(parent.ToString() + " Can't Find Buff: Cause label is Empty ");
            }
            catch (Exception ee)
            {
                Log.Error("Error : " + ee.ToString());
            }
            return default;
        }
        public Buff FindWithDef(BuffDef def)
        {
            try
            {
                if (def != null)
                {
                    return buffList.Find(buff => buff.Def == def);
                }
                Log.Error(parent.ToString() + " Can't Find Buff: Cause def is Empty ");
            }
            catch (Exception ee)
            {
                Log.Error("Error : " + ee.ToString());
            }
            return default;
        }
        public Buff FindWithCaster(ThingWithComps caster)
        {
            try
            {
                if (caster != null)
                {
                    return buffList.Find(buff => buff.Caster == caster);
                }
                Log.Error(parent.ToString() + " Can't Find Buff: Cause caster is Empty ");
            }
            catch (Exception ee)
            {
                Log.Error("Error : " + ee.ToString());
            }
            return default;
        }
        
        public List<Buff> FindAllWithDef(BuffDef def)
        {
            try
            {
                if (def != null)
                {
                    return buffList.FindAll(buff => buff.Def == def);
                }
                Log.Error(parent.ToString() + " Can't Find Buff: Cause def is Empty ");
            }
            catch (Exception ee)
            {
                Log.Error("Error : " + ee.ToString());
            }
            return default;
        }
        public List<Buff> FindAllWithDefName(string defName)
        {
            try
            {
                if (defName != null)
                {
                    return buffList.FindAll(buff => buff.Def.defName == defName);
                }
                Log.Error(parent.ToString() + " Can't Find Buff: Cause buffName is Empty ");
            }
            catch (Exception ee)
            {
                Log.Error("Error : " + ee.ToString());
            }
            return default;
        }
        public List<Buff> FindAllWithLabel(string label)
        {
            try
            {
                if (label != null)
                {
                    return buffList.FindAll(buff => buff.Def.label == label);
                }
                Log.Error(parent.ToString() + " Can't Find Buff: Cause label is Empty ");
            }
            catch (Exception ee)
            {
                Log.Error("Error : " + ee.ToString());
            }
            return default;
        }
        public List<Buff> FindAllWithTag(string tag)
        {
            try
            {
                if (tag != null)
                {
                    return buffList.FindAll(buff => buff.Def.tagList.Contains(tag));
                }
                Log.Error(parent.ToString() + " Can't Find Buff: Cause tag is Empty ");
            }
            catch (Exception ee)
            {
                Log.Error("Error : " + ee.ToString());
            }
            return default;
        }
        public List<Buff> FindAllWithTags(List<string> tagList)
        {
            var resultList = new List<Buff>();
            try
            {
                
                if (tagList != null)
                {
                    Buff result = null;
                    for (var index = 0; index < buffList.Count; index++)
                    {
                        for (var innerIndex = 0; innerIndex < tagList.Count; innerIndex++)
                        {
                            if (buffList[index].Def.tagList.Contains(tagList[innerIndex]))
                            {
                                result=buffList[index];
                            }
                            else
                            {
                                result=null;
                                break;
                            }
                        }

                        if (result != null)
                        {
                            resultList.Add(result);
                        }
                    }
                    return resultList;
                }
                Log.Error(parent.ToString() + " Can't Find Buff: Cause tagList is Empty ");
            }
            catch (Exception ee)
            {
                Log.Error("Error : " + ee.ToString());
            }
            return resultList;
        }
        public List<Buff> FindAllWithCaster(ThingWithComps caster)
        {
            try
            {
                if (caster != null)
                {
                    return buffList.FindAll(buff => buff.Caster == caster);
                }
                Log.Error(parent.ToString() + " Can't Find Buff: Cause target is Empty ");
            }
            catch (Exception ee)
            {
                Log.Error("Error : " + ee.ToString());
            }
            return default;
        }
        public List<Buff> FindAll()
        {
            return buffList;
        }


        public override void PostExposeData()
        {
            try
            {
                base.PostExposeData();
                Scribe_Collections.Look<Buff>(ref buffList, true, "buffList", LookMode.Deep, new object[0]);
                if (Scribe.mode == LoadSaveMode.LoadingVars)
                {
                    if (buffList == null)
                    {
                        buffList = new List<Buff>();
                        Log.Message("BuffList is null. Auto Create New BuffList");
                    }
                    for (var i = 0; i < this.buffList.Count; i++)
                    {
                        if (buffList[i] != null)
                        {
                            buffList[i].Owner = this;
                        }
                    }
                }
            }
            catch (Exception ee)
            {
                Log.Error("Error : " + ee.ToString());
            }
        }
        #endregion        
    }
}
