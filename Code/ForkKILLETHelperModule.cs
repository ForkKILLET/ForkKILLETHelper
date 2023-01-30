using System;
using Microsoft.Xna.Framework;

namespace Celeste.Mod.ForkKILLETHelper
{
    public class ForkKILLETHelperModule : EverestModule
    {
        public static ForkKILLETHelperModule Instance { get; private set; }

        public override Type SettingsType => typeof(ForkKILLETHelperModuleSettings);
        public static ForkKILLETHelperModuleSettings Settings => (ForkKILLETHelperModuleSettings) Instance._Settings;

        public override Type SessionType => typeof(ForkKILLETHelperModuleSession);
        public static ForkKILLETHelperModuleSession Session => (ForkKILLETHelperModuleSession) Instance._Session;

        public ForkKILLETHelperModule()
        {
            Instance = this;
#if DEBUG
            // debug builds use verbose logging
            Logger.SetLogLevel(nameof(ForkKILLETHelperModule), LogLevel.Verbose);
#else
            // release builds use info logging to reduce spam in log files
            Logger.SetLogLevel(nameof(ForkKILLETHelperModule), LogLevel.Info);
#endif
        }

        public override void Load()
        {
            // TODO: apply any hooks that should always be active
        }

        public override void Unload()
        {
            // TODO: unapply any hooks applied in Load()
        }
    }
}