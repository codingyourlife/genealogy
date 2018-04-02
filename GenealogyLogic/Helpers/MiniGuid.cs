namespace GenealogyLogic.Helpers
{
    /// <summary>
    /// Gets a random number between min and max. If used take the lowest available.
    /// </summary>
    public static class MiniGuid
    {
        private static MiniGuidCore miniGuidCore;

        static MiniGuid()
        {
            Reset();
        }

        public static int NewGuid()
        {
            return miniGuidCore.NewGuid();
        }

        public static void Reset()
        {
            miniGuidCore = new MiniGuidCore(0, 999);
        }
    }
}
