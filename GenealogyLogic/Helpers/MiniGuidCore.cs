namespace GenealogyLogic.Helpers
{
    using GenealogyLogic.Exceptions;
    using System;
    using System.Collections.Generic;

    public class MiniGuidCore
    {
        public MiniGuidCore(int min, int max)
        {
            this.min = min;
            this.max = max;
        }

        private int min = 0;
        private int max = 999;
        private Random myRandom = new Random();

        private List<int> usedMiniGuids = new List<int>();

        public int NewGuid()
        {
            var suggestedGuid = myRandom.Next(min, max);

            if (this.usedMiniGuids.Contains(suggestedGuid))
            {
                var lowestAvailable = GetLowestAvailableGuid();

                if (lowestAvailable == -1)
                {
                    throw new OutOfMiniGuidsException(string.Format("All MiniGuids {0} till {1} used", min, max));
                }

                suggestedGuid = lowestAvailable;
            }

            this.usedMiniGuids.Add(suggestedGuid);

            return suggestedGuid;
        }

        private int GetLowestAvailableGuid()
        {
            for (int i = this.min; i < this.max; i++)
            {
                if (!this.usedMiniGuids.Contains(i))
                {
                    return i;
                }
            }

            return -1;
        }
    }
}
