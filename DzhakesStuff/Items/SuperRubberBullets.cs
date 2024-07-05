using UnityEngine;
using RogueLibsCore;

namespace DzhakesStuff
{
    [ItemCategories(RogueCategories.Technology, RogueCategories.GunAccessory, RogueCategories.Guns)]
    public class SuperRubberBulletsMod : CustomItem, IItemCombinable
    {
        [RLSetup]
        public static void Setup()
        {
            RogueLibs.CreateCustomItem<SuperRubberBulletsMod>()
                .WithName(new CustomNameInfo("[DS] Super Rubber Bullets Mod"))
                .WithDescription(new CustomNameInfo("Don't like killing people with guns? Then use this, and you will knock out them instead!"))
                .WithSprite(Properties.Resources.SuperRubberBulletsMod)
                .WithUnlock(new ItemUnlock
                {
                    UnlockCost = 10,
                    LoadoutCost = 5,
                    CharacterCreationCost = 3,
                });
        }

        public override void SetupDetails()
        {
            Item.itemType = ItemTypes.Combine;
            Item.itemValue = 30;
            Item.initCount = 1;
            Item.stackable = false;
        }
        public bool CombineFilter(InvItem other)
            => other.itemType == ItemTypes.WeaponProjectile && !other.contents.Contains(nameof(SuperRubberBulletsMod));
        public bool CombineItems(InvItem other)
        {
            if (!CombineFilter(other))
            {
                gc.audioHandler.Play(Owner, VanillaAudio.CantDo);
                return false;
            }
            if (other.invItemCount >= other.maxAmmo)
            {
                Owner!.SayDialogue("AmmoDispenserFull");
                gc.audioHandler.Play(Owner, VanillaAudio.CantDo);
                return false;
            }

            int amountToRefill = other.maxAmmo - other.invItemCount;
            float singleCost = (float)other.itemValue / other.maxAmmo;
            if (Owner!.oma.superSpecialAbility && Owner.agentName is VanillaAgents.Soldier or VanillaAgents.Doctor)
                singleCost = 0f;

            int affordableAmount = (int)Mathf.Ceil(Count / singleCost);
            int willBeBought = Mathf.Min(affordableAmount, amountToRefill);
            int willBeReduced = (int)Mathf.Min(Count, willBeBought * singleCost);

            Count -= willBeReduced;
            other.invItemCount += willBeBought;
            Owner.SayDialogue("AmmoDispenserFilled");
            gc.audioHandler.Play(Owner, VanillaAudio.BuyItem);
            return true;
        }

        public CustomTooltip CombineTooltip(InvItem other)
        {
            if (!CombineFilter(other)) return default;

            int amountToRefill = other.maxAmmo - other.invItemCount;
            if (amountToRefill == 0) return default;

            float singleCost = (float)other.itemValue / other.maxAmmo;
            if (Owner!.oma.superSpecialAbility && Owner.agentName is VanillaAgents.Soldier or VanillaAgents.Doctor)
                singleCost = 0f;
            int cost = (int)Mathf.Floor(amountToRefill * singleCost);
            int canAfford = (int)Mathf.Ceil(Count / singleCost);

            return "+" + Mathf.Min(amountToRefill, canAfford) + " (" + Mathf.Min(cost, Count) + ")";
        }

        public CustomTooltip CombineCursorText(InvItem other)
            => gc.nameDB.GetName("RefillGun", NameTypes.Interface);
        // it's one of the vanilla dialogues, so there's no need to define it in the mod
    }
}