using UnityEngine;
using RogueLibsCore;

namespace DzhakesStuff.Items
{
    [ItemCategories(RogueCategories.Weird, RogueCategories.NonStandardWeapons, RogueCategories.Defense)]
    public class BusTicket : CustomItem, IItemTargetable
    {
        [RLSetup]
        public static void Setup()
        {
            RogueLibs.CreateCustomItem<BusTicket>()
                .WithName(new CustomNameInfo("[DS] Bus Ticket"))
                .WithDescription(new CustomNameInfo("[The Lost Room] When the Bus Ticket is touched to a person's skin, that person is transported to U.S. Route 66 outside Gallup, New Mexico the town where the Sunshine Motel is found. "))
                .WithSprite(Properties.Resources.BusTicket)
                .WithUnlock(new ItemUnlock
                {
                    LoadoutCost = 5,
                    CharacterCreationCost = 5,
                    UnlockCost = 5,
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

        public bool TargetFilter(PlayfieldObject target)
        {
            Vector2 ownerPos = Owner!.curPosition;
            if (!(target is Agent agent2) || agent2 == Owner) return false;

            return Vector2.Distance(target.curPosition, ownerPos) < 1.5f * 0.64f && agent2.movement.HasLOSPosition360(ownerPos);
        }

        public bool TargetObject(PlayfieldObject target)
        {
            Agent? agent = target as Agent;
            if (!TargetFilter(target) || agent is null) return false;
            agent!.SpawnParticleEffect("Spawn", agent.curPosition);
            agent.statusEffects.SetupDeath(Owner, true);
            agent.Teleport(new (-999, -999), false, true);
            return true;
        }

        public CustomTooltip TargetCursorText(PlayfieldObject? target)
        {
            return new("Send to U.S. Route 66 outside Gallup, New Mexico");
        }
    }
}
