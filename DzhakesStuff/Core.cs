using System;
using System.Collections.Generic;
using BepInEx;
using BepInEx.Logging;
using RogueLibsCore;
using UnityEngine;

namespace DzhakesStuff
{
    [BepInPlugin(PluginGuid, PluginName, PluginVersion)]
    [BepInDependency(RogueLibs.GUID, RogueLibs.CompiledVersion)]
    public class Core : BaseUnityPlugin, IDoUpdate ,IDoLateUpdate
    {
        public static Core Instance = new();

        public const string PluginGuid = "dzhake.streetsofrogue.DzhakesStuff";
        public const string PluginName = "Dzhakes Utilities";
        public const string PluginVersion = "1.0.0";

        public new static ManualLogSource Logger = null!;

        public static List<Action> Updatables = new();
        public static List<Action> LateUpdatables = new();

        public void Awake()
        {
            Instance = this;
            Logger = base.Logger;
            RogueLibs.LoadFromAssembly();
        }

        public void Update()
        {
            foreach (Action action in Updatables)
                action();
        }

        public void LateUpdate()
        {
            foreach (Action action in LateUpdatables)
                action();
        }
    }
}
