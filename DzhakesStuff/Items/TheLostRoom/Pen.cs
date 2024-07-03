using UnityEngine;
using RogueLibsCore;

namespace DzhakesStuff.Items
{
    [ItemCategories(RogueCategories.Weird, RogueCategories.Weapons, RogueCategories.NPCsCantPickUp)]
    public class Pen : CustomItem
    {
        [RLSetup]
        public static void Setup()
        {
            RogueLibs.CreateCustomItem<Pen>()
                .WithName(new CustomNameInfo("[DS] Pen"))
                .WithDescription(new CustomNameInfo("[The Lost Room] The ballpoint tip instantly microwaves any subject upon contact. Additionally, the pen has been known to launch its victims away with the force of a cannon."))
                .WithSprite(Properties.Resources.Pen)
                .WithUnlock(new ItemUnlock
                {
                    LoadoutCost = 5,
                    UnlockCost = 5,
                    CharacterCreationCost = 5,
                    IsAvailable = true,
                    IsAvailableInCC = true
                });
        }

        public override void SetupDetails()
        {
            Item.itemValue = 180;
            Item.rewardCount = 100;
            Item.initCount = 100;
        }
    }
}
