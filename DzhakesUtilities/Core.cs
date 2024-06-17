using BepInEx;
using BepInEx.Logging;
using DzhakesUtilities.Mutators;
using RogueLibsCore;

namespace DzhakesUtilities
{
    [BepInPlugin(PluginGuid, PluginName, PluginVersion)]
    [BepInDependency(RogueLibs.GUID, RogueLibs.CompiledVersion)]
    public class Core : BaseUnityPlugin, IDoLateUpdate
    {
        public static Core? Instance;

        public const string PluginGuid = "dzhake.streetsofrogue.dzhakesutilities";
        public const string PluginName = "Dzhakes Utilities";
        public const string PluginVersion = "1.0.0";

        public new static ManualLogSource Logger = null!;

        public void Awake()
        {
            Instance = this;
            Logger = base.Logger;
            RogueLibs.LoadFromAssembly();
            RoguePatcher patcher = new RoguePatcher(this);
        }

        public void LateUpdate()
        {
            SuperHot.Instance?.LateUpdate();
        }
    }
}
