using System.Collections;
using RogueLibsCore;

namespace DzhakesStuff
{
    public class DropMoreBombs : CustomDisaster
    {
        public static DropMoreBombs? Instance;
        public static bool Active => Instance?.IsActive ?? false;

        [RLSetup]
        public static void Setup()
        {
            RogueLibs.CreateCustomDisaster<DropMoreBombs>()
                .WithName(new CustomNameInfo("Bombs Dropping++"))
                .WithDescription(new CustomNameInfo("Bombs are falling from the sky!"))
                .WithMessage(new CustomNameInfo("Bombs are falling from the sky!"))
                .WithRemovalMutator(new("[DS] Remove \"Bombs Dropping++\""));
        }

        //public override bool TestForced() => true;

        public override void Start() => Instance = this;
        public override void StartAfterNotification()
        {
            gc.levelFeelingsScript.InvokeRepeating("DropBomb", 0f, 1f);
        }

        public override IEnumerator? Updating()
        {
            yield return null;
        }
    }
}
