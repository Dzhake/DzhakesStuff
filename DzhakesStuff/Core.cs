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
            RoguePatcher patcher = new RoguePatcher(this);
            patcher.Prefix(typeof(AnalyticsFunctions),nameof(AnalyticsFunctions.SendData));
            patcher.Prefix(typeof(LoadLevel), nameof(LoadLevel.HomeBaseAgentSpawns));
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

        //Don't send my data
        public static bool AnalyticsFunctions_SendData() => false;

        public static void LoadLevel_HomeBaseAgentSpawns()
        {
            for (float i = 88f; i < 95f; i++)
            {
                Demolish(76f, i); // Hacker
            }

            Demolish(64f, 54f); // Safe
            Demolish(84f, 66f); // Zombie
        }

        private static void Demolish(float x, float y)
        {
            GameController gc = GameController.gameController;
            Vector2 pos = new Vector2(x, y) * 0.64f;
            gc.tileInfo.DestroyWallTileAtPosition(pos.x, pos.y, true, gc.playerAgent);
            InvItem item = new InvItem { invItemName = "Wreckage" };
            item.SetupDetails(false);
            item.LoadItemSprite("WallBorderWreckage1");
            gc.spawnerMain.SpawnWreckage(pos, item, null, null, false).wallType = "WallBorder";
            item.LoadItemSprite("WallBorderWreckage2");
            gc.spawnerMain.SpawnWreckage(pos, item, null, null, false).wallType = "WallBorder";
            item.LoadItemSprite("WallBorderWreckage3");
            gc.spawnerMain.SpawnWreckage(pos, item, null, null, false).wallType = "WallBorder";
        }
    }
}
