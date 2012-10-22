namespace DEMW.SpaceWar2
{
#if WINDOWS || XBOX
    static class Program
    {
        static void Main()
        {
            using (var game = new SpaceWar2Game())
            {
                game.Run();
            }
        }
    }
#endif
}

