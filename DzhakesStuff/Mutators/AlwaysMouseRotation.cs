using System.Collections.Generic;
using BepInEx.Logging;
using RogueLibsCore;
using UnityEngine;

namespace DzhakesStuff
{
    public class AlwaysMouseRotation : MutatorUnlock
    {
        public new static AlwaysMouseRotation? Instance;
        public static bool WasEnabled;

        public AlwaysMouseRotation() : base(nameof(AlwaysMouseRotation))
        {
            UnlockCost = 3;
        }

        [RLSetup]
        public static void Setup()
        {
            AlwaysMouseRotation mutator = new AlwaysMouseRotation();
            RogueLibs.CreateCustomUnlock(mutator)
                .WithName(new CustomNameInfo("[DS] Always Mouse Rotation"))
                .WithDescription(new CustomNameInfo("When you shoot, you gun points to the mouse instead of direction where you move. With this mutator, it always does that"));
            Instance = mutator;

            Core.LateUpdatables.Add(() => Instance.LateUpdate());
        }

        public void LateUpdate()
        {
            if (!this.IsEnabled())
                return;

            if (gc?.sessionDataBig != null)
                gc.sessionDataBig.trackpadMode = false;
        }

    }
}
