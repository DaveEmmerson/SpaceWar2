using System;

namespace SpaceWar2
{
#if WINDOWS || XBOX
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        static void Main(string[] args)
        {
            using (SpaceWar2Game game = new SpaceWar2Game())
            {
                game.Run();
            }
        }
    }
#endif
}

