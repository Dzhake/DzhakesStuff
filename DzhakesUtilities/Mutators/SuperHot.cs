using System.Collections.Generic;
using BepInEx.Logging;
using RogueLibsCore;
using UnityEngine;

namespace DzhakesUtilities
{
    public class SuperHot : MutatorUnlock
    {
        public static SuperHot? Instance;
        public static bool WasEnabled;

        public SuperHot() : base(nameof(SuperHot))
        {
            UnlockCost = 3;
        }

        [RLSetup]
        public static void Setup()
        {
            SuperHot mutator = new SuperHot();
            RogueLibs.CreateCustomUnlock(mutator)
                .WithName(new CustomNameInfo("[DS] Super Hot"))
                .WithDescription(new CustomNameInfo("Time only moves when you move"));

            RoguePatcher patcher = new RoguePatcher(Core.Instance) { TypeWithPatches = typeof(SuperHot) };
            patcher.Postfix(typeof(GameController), nameof(GameController.SetTimeScale));
            Instance = mutator;

            Core.LateUpdatables.Add(() => Instance.LateUpdate());
        }

        public static void GameController_SetTimeScale(GameController __instance)
            => __instance.musicPlayer.pitch = Time.timeScale;

        public void LateUpdate()
        {
            if (!this.IsEnabled())
            {
                if (gc != null && WasEnabled)
                    gc.secondaryTimeScale = -1f;
                return;
            }

            Agent player = gc.playerAgent;
            int num = player.isPlayer - 1;
            PlayerControl pc = gc.playerControl;
            bool playerCanMove = pc.cantPressGameplayButtonsP[num] == 0 && pc.cantPressGameplayButtonsPB[num] == 0;
            bool playerMoving = pc.heldLeftK[num] || pc.heldRightK[num] || pc.heldDownK[num] || pc.heldUpK[num];
            bool playerBusy = player.melee.attackAnimPlaying || pc.cantPressButtons;

            gc.secondaryTimeScale = !playerCanMove || player.dead || playerMoving || playerBusy ? -1f : 1f / 1000f;
            gc.SetTimeScale();
            WasEnabled = true;
        }

    }
}
