using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RogueLibsCore;

namespace DzhakesStuff
{
    public class AdrenalineRush : CustomTrait
    {
        public override void OnAdded() {}

        public override void OnRemoved() {}

        [RLSetup]
        public static void Setup()
        {
            RogueLibs.CreateCustomTrait<AdrenalineRush>()
                .WithName(new CustomNameInfo("[DS] Adrenaline Rush"))
                .WithDescription(new CustomNameInfo("Get huge boost for 5 seconds after killing someone"))
                .WithUnlock(new TraitUnlock { UnlockCost = 5, CharacterCreationCost = 3 });

            RoguePatcher patcher = new RoguePatcher(Core.Instance) { TypeWithPatches = typeof(AdrenalineRush) };
            patcher.Postfix(typeof(StatusEffects), nameof(StatusEffects.SetupDeath));
        }

        public static void StatusEffects_SetupDeath(StatusEffects __instance)
        {
            Agent? killer = __instance.agent?.justHitByAgent2;

            if (killer == null) return;

            if (killer.HasTrait<AdrenalineRush>())
            {
                killer.AddEffect<Adrenaline>();
            }
        }
    }
}
