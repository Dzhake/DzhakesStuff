using RogueLibsCore;
using System;
using BepInEx.Logging;
using UnityEngine;

namespace DzhakesStuff.Abilities
{
    
	public class TelemanticBlink : CustomAbility, IAbilityRechargeable, IAbilityChargeable
    {
        private float actualHeldTime;

		[RLSetup]
		public static void Setup()
		{
			RogueLibs.CreateCustomAbility<TelemanticBlink>()
                .WithName(new CustomNameInfo("[DS] TelemanticBlink"))
                .WithDescription(new CustomNameInfo("Teleport to mouse position!"))
				.WithSprite(Properties.Resources.TelemanticBlink)
				.WithUnlock(new AbilityUnlock
				{
					CharacterCreationCost = 17,
					IsAvailable = true,
					IsAvailableInCC = true,
					UnlockCost = 17,
				});
		}

        public override void SetupDetails()
        {
            base.SetupDetails();
            Item.cantDrop = true;
            Item.dontAutomaticallySelect = true;
            Item.dontSelectNPC = true;
            Item.isWeapon = false;
            Item.initCount = 0;
            Item.stackable = true;
            Item.thiefCantSteal = false;
        }

		public override void OnAdded() {}

		public void OnHeld(AbilityHeldArgs e)
        {
            if (Count > 0) return;

            actualHeldTime += Time.deltaTime;
            e.HeldTime = actualHeldTime;
        }
		public override void OnPressed() {}
        public void OnRecharging(AbilityRechargingArgs e)
		{
			e.UpdateDelay = 1f;

            Count--;
		}
		public void OnReleased(AbilityReleasedArgs e)
        {
            if (Count > 0) return;

			StartCast(Math.Max(5, actualHeldTime));
            actualHeldTime = 0f;
        }
		
        public void StartCast(float charge)
        {
            Vector2 targetPosition = Util.MouseIngamePosition();
            TileInfo tileInfo = gc.tileInfo;

            TileData tileData = tileInfo.GetTileData(targetPosition);

            if (tileData.solidObject || tileInfo.WallExist(tileData) || tileInfo.GetWallMaterial(targetPosition.x, targetPosition.y) == wallMaterialType.Border)
            {
                gc.audioHandler.Play(Owner, "CantDo");
                return;
            }

            Owner!.Teleport(targetPosition, false, true);
            Owner!.rb.velocity = Vector2.zero;

            Count = 10;

            DialogueCast(Owner);
        }	

		public static void DialogueCast(Agent agent)
		{
			agent.SpawnParticleEffect("Spawn", agent.curPosition);
			GameController.gameController.audioHandler.Play(agent, "Spawn");
		}
	}
}
