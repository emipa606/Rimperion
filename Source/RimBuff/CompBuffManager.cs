using System;
using System.Collections.Generic;
using Verse;

namespace RimBuff;

public class CompBuffManager : ThingComp
{
    private List<Buff> buffList;

    public CompBuffManager()
    {
        buffList = [];

        BuffController.CompList.Add(this);
    }

    public List<Buff> BuffList
    {
        get
        {
            buffList ??= [];

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

        return null;
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

        return null;
    }


    public override void PostExposeData()
    {
        try
        {
            base.PostExposeData();
            Scribe_Collections.Look(ref buffList, "buffList", LookMode.Deep);
            if (Scribe.mode != LoadSaveMode.LoadingVars)
            {
                return;
            }

            if (buffList == null)
            {
                buffList = [];
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