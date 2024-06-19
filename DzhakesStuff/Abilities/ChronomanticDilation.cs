using System.Collections;
using System.Collections.Generic;
using BepInEx.Logging;
using RogueLibsCore;
using UnityEngine;

namespace DzhakesStuff
{
	public class ChronomanticDilation : CustomAbility, IAbilityRechargeable
	{
		public static float baseTimeScale = 1f;

        private Agent agent => Item.agent;

        public static bool Casting;
        public static float Density = 0f;

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
		public override void OnAdded() {}
		public override void OnPressed()
		{
			if (Count <= 0)
				agent.StartCoroutine(Cast(2f));
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
			Item.stackable = true;
            Item.thiefCantSteal = false;
            Item.itemType = "";
        }

		public void OnRecharging(AbilityRechargingArgs e)
		{
			e.UpdateDelay = 1f;
            Count--;
        }

		public IEnumerator Cast(float speedupfactor)
        {
            if (Casting) yield break;

            Casting = true;
            gc.audioHandler.Play(agent, "MakeOffering");

			gc.selectedTimeScale = baseTimeScale / speedupfactor;
			gc.mainTimeScale = baseTimeScale / speedupfactor;
			agent.speedMax = agent.FindSpeed() * (int)speedupfactor;

            Count = 15;

            agent.StartCoroutine(LerpDensity(1f, 1f));

            yield return new WaitForSeconds(5f);

			Decast();
        }

		public void Decast()
        {
            if (!Casting) return;
			agent.speedMax = agent.FindSpeed();

			gc.selectedTimeScale = baseTimeScale;
			gc.mainTimeScale = baseTimeScale;

            gc.audioHandler.Play(agent, "MakeOffering");

            agent.StartCoroutine(LerpDensity(0f,2f, true));
        }

        public IEnumerator LerpDensity(float to, float time, bool disable = false)
        {
            float t = 0f;
            float from = Density;
            while (t <= time)
            {
                t += Time.deltaTime;
                Density = Mathf.Lerp(from, to, t / time);
                yield return new WaitForEndOfFrame();
            }

            if (disable)
                Casting = false;
        }
	}
}
