using System.Collections;
using RogueLibsCore;
using UnityEngine;

namespace DzhakesStuff
{
    public class PlagueEverywhere : CustomDisaster
    {
        public static int Timer; // Imagine someone with plague spawns near you. Not cool.
        public static PlagueEverywhere? Instance;

        [RLSetup]
        public static void Setup()
        {
            RogueLibs.CreateCustomDisaster<PlagueEverywhere>()
                .WithName(new CustomNameInfo("Plague Everywhere"))
                .WithDescription(new CustomNameInfo("Some people are infected!"))
                .WithMessage(new CustomNameInfo("Plague Everywhere!"))
                .WithMessage(new CustomNameInfo("Some people are infected!!"));
        }

        public override bool Test()
        {
            return EnablePlagueEverywhere.Instance?.IsEnabled ?? false;
        }

        public override void Start()
        {
            Timer = 10;
            Instance = this;
            foreach (Agent agent in gc.agentList)
            {
                if (Random.Range(0f, 1f) <= 0.4f)
                {
                    agent.AddEffect<Plague>();
                }
            }
        }

        public override void Finish()
        {
            Timer = 0;
            Agent player = gc.playerAgent;
            if (!player.HasTrait<Infected>())
            {
                Plague? plague = player.GetEffect<Plague>();
                if (plague != null)
                    plague.CurrentTime = 0;
            }
        }

        public override IEnumerator? Updating()
        {
            while (Instance.IsActive)
            {
                Timer--;
                yield return 1f;
            }
        }
    }
}
