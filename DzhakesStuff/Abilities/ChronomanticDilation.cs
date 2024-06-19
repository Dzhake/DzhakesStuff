using BepInEx.Logging;
using RogueLibsCore;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DzhakesStuff
{
	public class ChronomanticDilation : CustomAbility, IAbilityRechargeable
	{
		public static float baseTimeScale = 1f;

        private Agent agent => Item.agent;

		[RLSetup]
		public static void Setup()
		{
			RogueLibs.CreateCustomAbility<ChronomanticDilation>()
                .WithName(new CustomNameInfo("[DS] Chronomantic Dilation"))
                .WithDescription(new CustomNameInfo("Slow down time for everybody but you!"))
				.WithSprite(Properties.Resources.ChronomanticDilation)
				.WithUnlock(new AbilityUnlock
				{
					CharacterCreationCost = 10,
					IsAvailable = true,
					IsAvailableInCC = true,
					UnlockCost = 10,
                });

		}
		public override void OnAdded()
		{
		}
		public override void OnPressed()
		{
			if (IsWindingUp())
                gc.audioHandler.Play(Item.agent, "CantDo");
			else if (IsCast())
				StartDecast();
			else
				StartCast(2f);
		}
		public override void SetupDetails()
		{
			Item.cantDrop = true;
			Item.Categories.Add("Usable");
			Item.Categories.Add("NPCsCantPickup");
			Item.dontAutomaticallySelect = true;
			Item.dontSelectNPC = true;
			Item.otherDamage = 0; // Bitwise variable field, see Extension method class below
			Item.isWeapon = false;
			Item.initCount = 0;
			Item.stackable = false;
			Item.thiefCantSteal = false;
		}
		public void OnRecharging(AbilityRechargingArgs e)
		{
			e.UpdateDelay = 1f;
            e.ShowRechargedText = true;
            Count--;
        }
		public bool IsCast()
		{
			try
			{
				return (agent.inventory.equippedSpecialAbility.otherDamage & 0b_0001) != 0;
			}
			catch
			{
				return false;
			}
		}

		public bool IsWindingUp() =>
			(Item.agent.inventory.equippedSpecialAbility.otherDamage & 0b_0100) != 0;

		public void SetCast(bool value)
		{
			if (value) agent.inventory.equippedSpecialAbility.otherDamage |= 0b_0001;
			else agent.inventory.equippedSpecialAbility.otherDamage &= ~0b_0001;
		}

		public void SetWindingUp(bool value)
		{
			if (value) agent.inventory.equippedSpecialAbility.otherDamage |= 0b_0100;
			else agent.inventory.equippedSpecialAbility.otherDamage &= ~0b_0100;
		}

		public void DialogueCast()
		{
			agent.SpawnParticleEffect("ExplosionMindControl", agent.curPosition);
			gc.audioHandler.Play(agent, "MakeOffering");
		}

		public void StartCast(float speedupfactor)
		{
			SetCast(true);
			DialogueCast();

			gc.selectedTimeScale = baseTimeScale / speedupfactor;
			gc.mainTimeScale = baseTimeScale / speedupfactor;
			agent.speedMax = agent.FindSpeed() * (int)speedupfactor;

            Count = 15;
        }

		public async void StartDecast()
		{
			agent.speedMax = agent.FindSpeed();

			SetCast(false); // Needs to occur before delays or Overcast occurs erroneously

			gc.selectedTimeScale = baseTimeScale;
			gc.mainTimeScale = baseTimeScale;

            gc.audioHandler.Play(agent, "MakeOffering");

			await Task.Delay(1000);

			await StartWindingUp();
		}

		public void StartRecharge()
		{
			if (IsWindingUp())
				SetWindingUp(false);

		}

		public async Task StartWindingUp()
		{
			SetWindingUp(true);

			await Task.Delay(4000);

			StartRecharge();
		}
	}
}
