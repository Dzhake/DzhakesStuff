using RogueLibsCore;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;

namespace DzhakesUtilities
{
    public class BetterMenusSlowMotion : MutatorUnlock
    {
        public static BetterMenusSlowMotion Instance;

        public BetterMenusSlowMotion() : base(nameof(BetterMenusSlowMotion)) { UnlockCost = 3; }

        [RLSetup]
        public static void Setup()
        {
            BetterMenusSlowMotion mutator = new BetterMenusSlowMotion();
            RogueLibs.CreateCustomUnlock(mutator)
                .WithName(new CustomNameInfo("[DU] Better Menus Slow Motion"))
                .WithDescription(new CustomNameInfo("Time scale is set to very small number instead of 0.2 when in menu"));
            Instance = mutator;

            RoguePatcher patcher = new RoguePatcher(Core.Instance) { TypeWithPatches = typeof(BetterMenusSlowMotion) };
            patcher.Postfix(typeof(InvInterface),nameof(InvInterface.ShowDraggedItem), nameof(SetSlowMotion));
            patcher.Postfix(typeof(InvInterface),nameof(InvInterface.ShowTarget), nameof(SetSlowMotion));
            patcher.Postfix(typeof(ScrollingMenu),"HideRewardsDelay", nameof(SetSlowMotion));
            patcher.Postfix(typeof(WeaponSelectImage),nameof(WeaponSelectImage.SlowTime), nameof(SetSlowMotion));
            patcher.Postfix(typeof(MainGUI), nameof(MainGUI.ShowInterfaceAnim));
            //patcher.Postfix(typeof(MainGUI), nameof(MainGUI.ShowScrollingMenuPersonal));
        }

        private static void SetSlowMotion()
        {
            if (!Instance.IsEnabled) return;

            if (gc.mainGUI.CanSpeedUpAfterScrollingMenu())
            {
                gc.mainTimeScale = gc.selectedTimeScale;
            }
            else if (!gc.coopMode && !gc.fourPlayerMode && !gc.multiplayerMode) // && !gc.finishedLevel
            {
                gc.mainTimeScale = float.Epsilon;
            }
            else
            {
                gc.mainTimeScale = gc.selectedTimeScale;
            }
        }

        private static void MainGUI_ShowInterfaceAnim(string interfaceType, MainGUI __instance)
        {
            if (!Instance.IsEnabled) return;
            if (((interfaceType == "Inventory" && __instance.openedInventory) || (interfaceType == "CharacterSheet" && __instance.openedCharacterSheet) || (interfaceType == "BigImage" && __instance.openedBigImage) || (interfaceType == "UseOn" && __instance.agent.worldSpaceGUI.openedUseOn) || (interfaceType == "ObjectButtons" && __instance.agent.worldSpaceGUI.openedObjectButtons)) && !gc.pauseOnMenus)
            {
                if (!gc.coopMode && !gc.fourPlayerMode && !gc.multiplayerMode && (!__instance.agent.interactionHelper.interactingFar || __instance.agent.interactionHelper.interactingCounter))
                {
                    gc.mainTimeScale = float.Epsilon;
                }
            }
        }

        private static void MainGUI_ShowScrollingMenuPersonal(string type)
        {
            if (!Instance.IsEnabled) return;

            if (type == "ChangeTraitRandom")
            {
                if (!gc.coopMode && !gc.fourPlayerMode && !gc.multiplayerMode)
                {
                    gc.mainTimeScale = float.Epsilon;
                }
            }
        }
    }
}
