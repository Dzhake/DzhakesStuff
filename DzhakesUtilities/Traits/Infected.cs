using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RogueLibsCore;
using UnityEngine;

namespace DzhakesStuff
{
    public class Infected : CustomTrait, ITraitUpdateable
    {
        public override void OnAdded()
        {
            Owner.AddEffect<Plague>();
        }

        public override void OnRemoved() {}

        [RLSetup]
        public static void Setup()
        {
            RogueLibs.CreateCustomTrait<Infected>()
                .WithName(new CustomNameInfo("[DS] Infected"))
                .WithDescription(new CustomNameInfo("You're infected, but hey, you can spend a bit more points on useful stuff!"))
                .WithUnlock(new TraitUnlock
                {
                    CharacterCreationCost = -15,
                    IsAvailable = false
                });
        }

        public void OnUpdated(TraitUpdatedArgs e)
        {
            if (!Owner.HasEffect<Plague>())
            {
                Owner.AddEffect<Plague>();
            }
        }
    }
}
