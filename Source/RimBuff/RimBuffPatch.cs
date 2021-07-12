using System.Reflection;
using HarmonyLib;
using Verse;

namespace RimBuff
{
    [StaticConstructorOnStartup]
    public static class RimBuffPatch
    {
        static RimBuffPatch()
        {
            var harmonyInstance = new Harmony("com.RimBuffPatch.rimworld.mod");
            harmonyInstance.PatchAll(Assembly.GetExecutingAssembly());
        }
    }
}