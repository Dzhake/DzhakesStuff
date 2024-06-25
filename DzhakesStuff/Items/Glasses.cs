using UnityEngine;
using RogueLibsCore;
using System.Runtime.Remoting;
using UnityEngine.EventSystems;

namespace DzhakesStuff.Items
{
    [ItemCategories(RogueCategories.Weird, RogueCategories.Defense, RogueCategories.NonUsableTool)]
    public class Glasses : CustomItem, IDoUpdate
    {
        [RLSetup]
        public static void Setup()
        {
            RogueLibs.CreateCustomItem<Glasses>()
                .WithName(new CustomNameInfo("[DS] Glasses"))
                .WithDescription(new CustomNameInfo("[The Lost Room] When worn, inhibit some types of combustion within a 5 blocks radius, ranging from fire open flames to firearm gun discharges."))
                .WithSprite(Properties.Resources.Glasses)
                .WithUnlock(new ItemUnlock
                {
                    LoadoutCost = 5,
                    CharacterCreationCost = 5,
                    IsAvailable = true,
                    IsAvailableInCC = true
                });

            var patcher = Core.GetPatcher<Glasses>();
            patcher.Prefix(typeof(Gun), nameof(Gun.Shoot));
        }

        public override void SetupDetails()
        {
            Item.itemValue = 180;
            Item.cantBeCloned = true;
            Item.stackable = false;
            Item.itemType = ItemTypes.Wearable;
            Item.rewardCount = 1;
            Item.isArmorHead = true;
            Item.behindHair = false;
            Item.cantShowHair = false;
            Item.goesInToolbar = true;
            Item.hierarchy = 9f;
        }

        public void Update()
        {
            if (Owner?.inventory?.equippedArmorHead != Item) return;

            foreach (Fire fire in gc.firesList)
            {
                Vector2 pos = fire.curPosition;
                if (!(Vector2.Distance(pos, Owner.curPosition) <= 6f * 0.64f &&
                      Owner.movement.HasLOSPosition360(pos))) continue;
                fire.puttingOutFire = true;
                fire.DestroyMe();
            }

            foreach (Agent agent in gc.agentList)
            {
                if (agent.HasEffect(VanillaEffects.OnFire) && Vector2.Distance(agent.curPosition, Owner!.curPosition) <= 6f * 0.64f && agent.movement.HasLOSPosition360(Owner.curPosition))
                {
                    agent.statusEffects.RemoveStatusEffect(VanillaEffects.OnFire, true, true);
                    Core.Log($"Glasses, applying to: {agent.agentName}");
                }
            }
        }

        private static bool Gun_Shoot(Gun __instance)
        {
            foreach (Agent agent in gc.agentList)
            {
                if (!(agent?.movement?.HasLOSPosition360(__instance.agent.curPosition) ?? false) ||
                    !(Vector2.Distance(__instance.agent.curPosition, agent.curPosition) <= 6f * 0.64f) ||
                    agent.inventory?.equippedArmorHead?.invItemName != "Glasses") continue;
                return false;
            }

            return true;
        }
    }
}
