using System;

namespace T1_SE_SpaceInvaders
{
#if WINDOWS || XBOX
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        static void Main(string[] args)
        {
            using (SpaceInvadersSimulator game = new SpaceInvadersSimulator())
            {
                game.Run();
            }
        }
    }
#endif
}

