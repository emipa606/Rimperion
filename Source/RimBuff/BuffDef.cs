using System;
using System.Collections.Generic;
using Verse;

namespace RimBuff;

public class BuffDef : Def
{
    internal readonly Type buffClass = typeof(Buff);
    public readonly bool canDespell = true;
    public readonly float duration = 0;
    public readonly int maxOverlapLevel = 0;

    public readonly int spellLevel = 0;
    public readonly List<string> tagList = [];

    public bool isVisualze = false;
    public float repeatCycle = 0;
}