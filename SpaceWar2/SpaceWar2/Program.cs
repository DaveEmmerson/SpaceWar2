namespace SpaceWar2
{
#if WINDOWS || XBOX
    static class Program
    {
        static void Main(string[] args)
        {
            using (var game = new SpaceWar2Game())
            {
                game.Run();
            }
        }
    }
#endif
}

