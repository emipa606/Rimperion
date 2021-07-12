using System.Collections.Generic;

namespace RimBuff
{
    public static class BuffController
    {
        private static List<CompBuffManager> compList = new List<CompBuffManager>();

        public static List<CompBuffManager> CompList
        {
            get
            {
                if (compList == null)
                {
                    compList = new List<CompBuffManager>();
                }

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
}