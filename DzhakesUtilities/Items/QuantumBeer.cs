using RogueLibsCore;
using UnityEngine;

namespace DzhakesStuff
{
    [ItemCategories(RogueCategories.Food, RogueCategories.Technology)]
    public class QuantumBeer : CustomItem, IItemUsable, IDoUpdate
    {
        [RLSetup]
        public static void Setup()
        {
            RogueLibs.CreateCustomItem<QuantumBeer>()
                .WithName(new CustomNameInfo("Quantum Beer"))
                .WithDescription(new CustomNameInfo("Created with quantum mechanics, it persists after being eaten."))
                .WithSprite(Properties.Resources.QuantumBeer)
                .WithUnlock(new ItemUnlock
                {
                    LoadoutCost = 15,
                    CharacterCreationCost = 10,
                    IsAvailable = true,
                    IsAvailableInCC = true
                });
        }

        public override void SetupDetails()
        {
            Item.itemType = ItemTypes.Food;
            Item.itemValue = 180;
            Item.healthChange = 5;
            Item.cantBeCloned = true;
            Item.goesInToolbar = true;
            Item.stackable = false;
        }

        public float Cooldown { get; set; }
        public void Update() => Cooldown = Mathf.Max(Cooldown - Time.deltaTime, 0f);

        public bool UseItem()
        {
            if (Cooldown != 0f) return false;

            int heal = new ItemFunctions().DetermineHealthChange(Item, Owner);
            Owner!.statusEffects.ChangeHealth(heal);

            if (Owner.HasTrait(VanillaTraits.ShareTheHealth)
                || Owner.HasTrait(VanillaTraits.ShareTheHealth2))
                new ItemFunctions().GiveFollowersHealth(Owner, heal);

            gc.audioHandler.Play(Owner, VanillaAudio.UseFood);
            Cooldown = 0.5f;
            return true;
        }
    }
}
