using RogueLibsCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace DzhakesStuff
{
    public class DisablePlagueEverywhere() : MutatorUnlock(nameof(DisablePlagueEverywhere))
    {
        public static DisablePlagueEverywhere? Instance;

        [RLSetup]
        public static void Setup()
        {
            DisablePlagueEverywhere mutator = new DisablePlagueEverywhere();
            RogueLibs.CreateCustomUnlock(mutator)
                .WithName(new CustomNameInfo("[DS] Disable \"Plague Everywhere\""))
                .WithDescription(new CustomNameInfo("Disaster: Some people are infected!"));
            Instance = mutator;
        }
    }
}
