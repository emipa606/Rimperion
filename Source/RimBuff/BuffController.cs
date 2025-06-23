using System.Collections.Generic;

namespace RimBuff;

public static class BuffController
{
    private static List<CompBuffManager> compList = [];

    public static List<CompBuffManager> CompList
    {
        get
        {
            compList ??= [];

            return compList;
        }
    }

    public static void Tick()
    {
        foreach (var compBuffManager in compList)
        {
            compBuffManager.Tick();
        }
    }
}