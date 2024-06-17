using BepInEx;
using BepInEx.Logging;
using RogueLibsCore;

namespace DzhakesUtilities
{
    [BepInPlugin(PluginGuid, PluginName, PluginVersion)]
    [BepInDependency(RogueLibs.GUID, RogueLibs.CompiledVersion)]
    public class Core : BaseUnityPlugin, IDoLateUpdate
    {
        public static Core Instance = new();

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
            patcher.Prefix(typeof(AnalyticsFunctions),nameof(AnalyticsFunctions.SendData));
        }

        public void LateUpdate()
        {
            SuperHot.Instance?.LateUpdate();
        }

        //Don't send my data
        public static bool AnalyticsFunctions_SendData() => false;
    }
}
