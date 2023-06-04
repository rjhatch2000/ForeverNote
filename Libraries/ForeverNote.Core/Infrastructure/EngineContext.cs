using System.Runtime.CompilerServices;

namespace ForeverNote.Core.Infrastructure
{
    /// <summary>
    /// Provides access to the singleton instance of the ForeverNote engine.
    /// </summary>
    public static class EngineContext
    {
        #region Methods

        /// <summary>
        /// Create a static instance of the ForeverNote engine.
        /// </summary>
        [MethodImpl(MethodImplOptions.Synchronized)] 
        public static IEngine Create()
        {
            //create ForeverNoteEngine as engine
            if (Singleton<IEngine>.Instance == null)
                Singleton<IEngine>.Instance = new ForeverNoteEngine();

            return Singleton<IEngine>.Instance;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets the singleton ForeverNote engine used to access Nop services.
        /// </summary>
        public static IEngine Current
        {
            get
            {
                if (Singleton<IEngine>.Instance == null)
                {
                    Create();
                }

                return Singleton<IEngine>.Instance;
            }
        }

        #endregion
    }
}
