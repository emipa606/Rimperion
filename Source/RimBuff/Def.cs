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
    public class BuffDef : Def
    {
        internal Type buffClass = typeof(Buff);

        public bool isVisualze = false;
        public bool canDespell=true;
        public List<string> tagList=new List<string>();

        public int spellLevel = 0;
        public int maxOverlapLevel = 0;
        public float duration = 0;
        public float repeatCycle = 0;
    }
}
