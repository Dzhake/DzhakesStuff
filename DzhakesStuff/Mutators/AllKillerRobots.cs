using UnityEngine;
using RogueLibsCore;
using System.Diagnostics.Tracing;
using System.Runtime.CompilerServices;

namespace DzhakesStuff.Mutators
{
    public class AllKillerRobots : MutatorUnlock
    {
        public new static AllKillerRobots? Instance;

        public AllKillerRobots() : base(nameof(AllKillerRobots))
        {
            UnlockCost = 3;
        }

        [RLSetup]
        public static void Setup()
        {
            AllKillerRobots mutator = new AllKillerRobots();
            RogueLibs.CreateCustomUnlock(mutator)
                .WithName(new CustomNameInfo("[DS] All Killer Robots"))
                .WithDescription(new CustomNameInfo("For some reason now everybody is a killer robot..."));
            Instance = mutator;

            RoguePatcher patcher = Core.GetPatcher<AllKillerRobots>();
            patcher.Prefix(typeof(SpawnerMain), nameof(SpawnerMain.SpawnAgent), [typeof(Vector3), typeof(PlayfieldObject), typeof(string), typeof(int), typeof(ObjectReal),
                typeof(bool), typeof(string), typeof(Agent), typeof(int), typeof(Fire), typeof(bool), typeof(WorldDataAgent), typeof(int), typeof(int)]);
        }

        public static void SpawnerMain_SpawnAgent(ref string agentType)
        {
            if (!(Instance?.IsEnabled ?? false)) return;
            agentType = "Robot";
        }
    }
}
