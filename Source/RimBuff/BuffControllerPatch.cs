using HarmonyLib;
using Verse;

namespace RimBuff;

[HarmonyPatch(typeof(TickManager), nameof(TickManager.DoSingleTick))]
internal class BuffControllerPatch
{
    private static bool Prefix()
    {
        BuffController.Tick();
        return true;
    }
}