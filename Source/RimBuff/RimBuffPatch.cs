using System.Reflection;
using HarmonyLib;
using Verse;

namespace RimBuff;

[StaticConstructorOnStartup]
public static class RimBuffPatch
{
    static RimBuffPatch()
    {
        new Harmony("com.RimBuffPatch.rimworld.mod").PatchAll(Assembly.GetExecutingAssembly());
    }
}