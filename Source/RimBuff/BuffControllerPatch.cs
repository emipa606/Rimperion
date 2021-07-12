using HarmonyLib;
using Verse;

namespace RimBuff
{
    [HarmonyPatch(typeof(TickManager))]
    [HarmonyPatch("DoSingleTick")]
    internal class BuffControllerPatch
    {
        private static bool Prefix()
        {
            BuffController.Tick();
            return true;
        }
    }
}