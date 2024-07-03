using UnityEngine;
using RogueLibsCore;

namespace DzhakesStuff.Items
{
    [ItemCategories(RogueCategories.Weird, RogueCategories.Technology)]
    public class Pencil : CustomItem, IItemUsable
    {
        [RLSetup]
        public static void Setup()
        {
            RogueLibs.CreateCustomItem<Pencil>()
                .WithName(new CustomNameInfo("[DS] Pencil"))
                .WithDescription(new CustomNameInfo("[The Lost Room] Creates a 1961 US penny when the eraser tip is tapped against a solid surface. But you have small chance to become crazy.."))
                .WithSprite(Properties.Resources.Pencil)
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
            Item.cantBeCloned = true;
            Item.stackable = false;
            Item.itemType = ItemTypes.Tool;
            Item.rewardCount = 1;
        }

        public bool UseItem()
        {
            Owner!.inventory.money.invItemCount += 1;
            if (Random.Range(1, 500) == 1)
            {
                Owner.AddEffect(VanillaEffects.Rage, 9999);
                Owner.AddEffect(VanillaEffects.Fast, 9999);
                Owner.AddEffect(VanillaEffects.Paralyzed, 9999);
                Owner.AddEffect(VanillaEffects.Acid, 9999);
                Owner.AddEffect(VanillaEffects.OnFire, 9999);
            }
            return true;
        }
    }
}
