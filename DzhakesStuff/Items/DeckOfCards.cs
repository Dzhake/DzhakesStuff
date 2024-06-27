using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RogueLibsCore;
using Random = UnityEngine.Random;

namespace DzhakesStuff.Items
{
    [ItemCategories(RogueCategories.Weird, RogueCategories.NonStandardWeapons, RogueCategories.Defense)]
    public class DeckOfCards : CustomItem, IItemTargetable, IDoUpdate
    {
        [RLSetup]
        public static void Setup()
        {
            RogueLibs.CreateCustomItem<DeckOfCards>()
                .WithName(new CustomNameInfo("[DS] Deck Of Cards"))
                .WithDescription(new CustomNameInfo("[The Lost Room] Induces startling visions relating to the Sunshine Motel in people who see the faces of the playing cards, which can be used to temporarily incapacitate victims. "))
                .WithSprite(Properties.Resources.DeckOfCards)
                .WithUnlock(new ItemUnlock
                {
                    LoadoutCost = 5,
                    CharacterCreationCost = 5,
                    IsAvailable = true,
                    IsAvailableInCC = true
                });
        }

        private float Cooldown;

        public void Update() => Cooldown -= Time.deltaTime;

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
            if (!(target is Agent agent2)) return false;

            return Vector2.Distance(target.curPosition, ownerPos) < 1.5f * 0.64f && agent2.movement.HasLOSPosition360(ownerPos);
        }

        public bool TargetObject(PlayfieldObject target)
        {
            if (Cooldown > 0f) return false;
            Vector2 ownerPos = Owner!.curPosition;
            if (!(target is Agent agent) || !(Vector2.Distance(target.curPosition, ownerPos) < 1.5f * 0.64f && agent.movement.HasLOSPosition360(ownerPos))) return false;
            agent.statusEffects.AddStatusEffect(VanillaEffects.SuperDizzy, 10);
            if(agent != Owner) agent.relationships.SetRelHate(Owner, 50);
            Cooldown = 15f;
            if (agent.isPlayer > 0) agent.StartCoroutine(ShockPlayer());
            return true;
        }

        private static IEnumerator ShockPlayer()
        {
            D3Shader.Strength = 1f;
            BlurShader.BlurSize = 0.5f;
            float t = 0f;
            while (t < 10f)
            {
                t += Time.deltaTime;
                TintShader.Color = Random.ColorHSV(0f, 1f, 1f, 1f, 1f, 1f, 1f, 1f);
                yield return new WaitForEndOfFrame();
            }

            TintShader.Color = Color.grey;
            D3Shader.Strength = 0f;
            BlurShader.BlurSize = 0f;
        }

        public CustomTooltip TargetCursorText(PlayfieldObject? target)
        {
            if (Cooldown > 0f) return new($"Show card ({Math.Round(Cooldown, 1)}s)");
            return new("Show card");
        }
    }
}
