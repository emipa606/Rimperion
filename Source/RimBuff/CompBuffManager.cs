using System;
using System.Collections.Generic;
using Verse;

namespace RimBuff;

public class CompBuffManager : ThingComp
{
    private List<Buff> buffList;

    public CompBuffManager()
    {
        buffList = new List<Buff>();

        BuffController.CompList.Add(this);
    }

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

    ~CompBuffManager()
    {
        BuffController.CompList.Remove(this);
    }

    public void Tick()
    {
        try
        {
            if (buffList.Count <= 0)
            {
                return;
            }

            foreach (var buff in buffList)
            {
                buff.TickTest(1).MoveNext();
            }
        }
        catch (Exception ee)
        {
            Log.Error($"Error : {ee}");
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
            Log.Error($"Error : {ee}");
        }
    }

    public void AddBuff(BuffDef def, ThingWithComps caster)
    {
        try
        {
            if (Activator.CreateInstance(def.buffClass, def, caster) is not Buff buff)
            {
                return;
            }

            buff.Caster = caster;
            buffList.Add(buff);
            buff.Owner = this;
        }
        catch (Exception ee)
        {
            Log.Error($"Error : {ee}");
        }
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

            Log.Error($"{parent} RemoveBuff - Can't Find Buff: Cause uniqueName is Empty ");
        }
        catch (Exception ee)
        {
            Log.Error($"Error : {ee}");
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

            Log.Error($"{parent} RemoveBuff - Can't Find Buff: Cause label is Empty ");
        }
        catch (Exception ee)
        {
            Log.Error($"Error : {ee}");
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

            Log.Error($"{parent} RemoveBuff - Can't Find Buff: Cause id is Empty ");
        }
        catch (Exception ee)
        {
            Log.Error($"Error : {ee}");
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

            Log.Error($"{parent} RemoveBuff - Can't Find Buff: Cause def is Empty ");
        }
        catch (Exception ee)
        {
            Log.Error($"Error : {ee}");
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

            Log.Error($"{parent} RemoveBuff - Can't Find Buff: Cause tag is Empty ");
        }
        catch (Exception ee)
        {
            Log.Error($"Error : {ee}");
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

            Log.Error($"{parent} RemoveBuff - Can't Find Buff: Cause tagList is Empty ");
        }
        catch (Exception ee)
        {
            Log.Error($"Error : {ee}");
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

            Log.Error($"{parent} RemoveBuff - Can't Find Buff: Cause target is Empty ");
        }
        catch (Exception ee)
        {
            Log.Error($"Error : {ee}");
        }
    }

    public void RemoveAllWithDefName(string defName)
    {
        try
        {
            if (defName != null)
            {
                var removeList = FindAllWithDefName(defName);
                if (removeList != null)
                {
                    foreach (var buff in removeList)
                    {
                        RemoveBuff(buff);
                    }
                }
            }

            Log.Error($"{parent} RemoveBuffAll - Can't Find Buff: Cause buffName is Empty ");
        }
        catch (Exception ee)
        {
            Log.Error($"Error : {ee}");
        }
    }

    public void RemoveAllWithLabel(string label)
    {
        try
        {
            if (label != null)
            {
                var removeList = FindAllWithLabel(label);
                if (removeList != null)
                {
                    foreach (var buff in removeList)
                    {
                        RemoveBuff(buff);
                    }
                }
            }

            Log.Error($"{parent} RemoveBuffAll - Can't Find Buff: Cause label is Empty ");
        }
        catch (Exception ee)
        {
            Log.Error($"Error : {ee}");
        }
    }

    public void RemoveAllWithTag(string tag)
    {
        try
        {
            if (tag != null)
            {
                var removeList = FindAllWithTag(tag);
                if (removeList != null)
                {
                    foreach (var buff in removeList)
                    {
                        RemoveBuff(buff);
                    }
                }
            }

            Log.Error($"{parent} RemoveBuffAll - Can't Find Buff: Cause tag is Empty ");
        }
        catch (Exception ee)
        {
            Log.Error($"Error : {ee}");
        }
    }

    public void RemoveAllWithTags(List<string> tagList)
    {
        try
        {
            if (tagList != null)
            {
                var removeList = FindAllWithTags(tagList);
                if (removeList != null)
                {
                    foreach (var buff in removeList)
                    {
                        RemoveBuff(buff);
                    }
                }
            }

            Log.Error($"{parent} RemoveBuffAll - Can't Find Buff: Cause tag is Empty ");
        }
        catch (Exception ee)
        {
            Log.Error($"Error : {ee}");
        }
    }

    public void RemoveBuffAll(BuffDef def)
    {
        try
        {
            if (def != null)
            {
                var removeList = FindAllWithDef(def);
                if (removeList != null)
                {
                    foreach (var buff in removeList)
                    {
                        RemoveBuff(buff);
                    }
                }
            }

            Log.Error($"{parent} RemoveBuffAll - Can't Find Buff: Cause def is Empty ");
        }
        catch (Exception ee)
        {
            Log.Error($"Error : {ee}");
        }
    }

    public void RemoveBuffAll(ThingWithComps caster)
    {
        try
        {
            if (caster != null)
            {
                var removeList = FindAllWithCaster(caster);
                if (removeList != null)
                {
                    foreach (var buff in removeList)
                    {
                        RemoveBuff(buff);
                    }
                }
            }

            Log.Error($"{parent} RemoveBuffAll - Can't Find Buff: Cause caster is Empty ");
        }
        catch (Exception ee)
        {
            Log.Error($"Error : {ee}");
        }
    }

    public void RemoveBuffAll()
    {
        try
        {
            foreach (var buff in buffList)
            {
                RemoveBuff(buff);
            }
        }
        catch (Exception ee)
        {
            Log.Error($"Error : {ee}");
        }
    }

    //버프 확인
    public bool ContainBuff(Buff buff)
    {
        return buffList.Contains(buff);
    }

    public bool ContainDef(BuffDef def)
    {
        return FindWithDef(def) != null;
    }

    public bool ContainTag(string tag)
    {
        return FindWithTag(tag) != null;
    }

    public bool ContainTags(List<string> tagList)
    {
        return FindWithTags(tagList) != null;
    }

    public bool ContainDefName(string defName)
    {
        return FindWithName(defName) != null;
    }

    public bool ContainLabel(string label)
    {
        return FindWithLabel(label) != null;
    }

    public bool ContainUniqueID(string uniqueID)
    {
        return FindWithUniqueID(uniqueID) != null;
    }

    public bool ContainCaster(ThingWithComps caster)
    {
        return FindWithCaster(caster) != null;
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

            Log.Error($"{parent} Can't Find Buff: Cause buff is Empty ");
        }
        catch (Exception ee)
        {
            Log.Error($"Error : {ee}");
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

            Log.Error($"{parent} Can't Find Buff: Cause buffName is Empty ");
        }
        catch (Exception ee)
        {
            Log.Error($"Error : {ee}");
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

            Log.Error($"{parent} Can't Find Buff: Cause tag is Empty ");
        }
        catch (Exception ee)
        {
            Log.Error($"Error : {ee}");
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
                foreach (var buff in buffList)
                {
                    foreach (var item in tagList)
                    {
                        if (buff.Def.tagList.Contains(item))
                        {
                            result = buff;
                        }
                        else
                        {
                            result = null;
                            break;
                        }
                    }

                    if (result != null)
                    {
                        return result;
                    }
                }

                return null;
            }

            Log.Error($"{parent} Can't Find Buff: Cause tagList is Empty ");
        }
        catch (Exception ee)
        {
            Log.Error($"Error : {ee}");
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

            Log.Error($"{parent} Can't Find Buff: Cause buffName is Empty ");
        }
        catch (Exception ee)
        {
            Log.Error($"Error : {ee}");
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

            Log.Error($"{parent} Can't Find Buff: Cause label is Empty ");
        }
        catch (Exception ee)
        {
            Log.Error($"Error : {ee}");
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

            Log.Error($"{parent} Can't Find Buff: Cause def is Empty ");
        }
        catch (Exception ee)
        {
            Log.Error($"Error : {ee}");
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

            Log.Error($"{parent} Can't Find Buff: Cause caster is Empty ");
        }
        catch (Exception ee)
        {
            Log.Error($"Error : {ee}");
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

            Log.Error($"{parent} Can't Find Buff: Cause def is Empty ");
        }
        catch (Exception ee)
        {
            Log.Error($"Error : {ee}");
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

            Log.Error($"{parent} Can't Find Buff: Cause buffName is Empty ");
        }
        catch (Exception ee)
        {
            Log.Error($"Error : {ee}");
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

            Log.Error($"{parent} Can't Find Buff: Cause label is Empty ");
        }
        catch (Exception ee)
        {
            Log.Error($"Error : {ee}");
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

            Log.Error($"{parent} Can't Find Buff: Cause tag is Empty ");
        }
        catch (Exception ee)
        {
            Log.Error($"Error : {ee}");
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
                foreach (var buff in buffList)
                {
                    foreach (var item in tagList)
                    {
                        if (buff.Def.tagList.Contains(item))
                        {
                            result = buff;
                        }
                        else
                        {
                            result = null;
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

            Log.Error($"{parent} Can't Find Buff: Cause tagList is Empty ");
        }
        catch (Exception ee)
        {
            Log.Error($"Error : {ee}");
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

            Log.Error($"{parent} Can't Find Buff: Cause target is Empty ");
        }
        catch (Exception ee)
        {
            Log.Error($"Error : {ee}");
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
            Scribe_Collections.Look(ref buffList, true, "buffList", LookMode.Deep);
            if (Scribe.mode != LoadSaveMode.LoadingVars)
            {
                return;
            }

            if (buffList == null)
            {
                buffList = new List<Buff>();
                Log.Message("BuffList is null. Auto Create New BuffList");
            }

            foreach (var buff in buffList)
            {
                if (buff != null)
                {
                    buff.Owner = this;
                }
            }
        }
        catch (Exception ee)
        {
            Log.Error($"Error : {ee}");
        }
    }
}