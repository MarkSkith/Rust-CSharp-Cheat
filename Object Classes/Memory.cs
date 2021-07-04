namespace Impure
{
    class Memory
    {
        public static MDriver.MEME.Requests MEMAPI;

        public static void LoadMemory(string ProcName)
        {
            MEMAPI = new MDriver.MEME.Requests(ProcName);
        }
    }
}
