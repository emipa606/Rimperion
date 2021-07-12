using System;
using System.Collections.Generic;
using Verse;

namespace RimBuff
{
    public class BuffDef : Def
    {
        internal Type buffClass = typeof(Buff);
        public bool canDespell = true;
        public float duration = 0;

        public bool isVisualze = false;
        public int maxOverlapLevel = 0;
        public float repeatCycle = 0;

        public int spellLevel = 0;
        public List<string> tagList = new List<string>();
    }
}