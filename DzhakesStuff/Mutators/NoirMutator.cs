using RogueLibsCore;

namespace DzhakesStuff
{
    public class NoirMutator : MutatorUnlock
    {
        public new static NoirMutator? Instance;
        public static bool WasEnabled;

        public NoirMutator() : base(nameof(NoirMutator))
        {
            UnlockCost = 3;
        }

        [RLSetup]
        public static void Setup()
        {
            NoirMutator mutator = new NoirMutator();
            RogueLibs.CreateCustomUnlock(mutator)
                .WithName(new CustomNameInfo("[DS] Noir"))
                .WithDescription(new CustomNameInfo("I like shaders ^^"));
            Instance = mutator;
        }
    }
}
