using RogueLibsCore;

namespace DzhakesStuff
{
    [EffectParameters(EffectLimitations.RemoveOnDeath | EffectLimitations.RemoveOnKnockOut)]
    public class Adrenaline : CustomEffect
    {
        [RLSetup]
        public static void Setup()
        {
            RogueLibs.CreateCustomEffect<Adrenaline>()
                .WithName(new CustomNameInfo("Adrenaline"))
                .WithDescription(new CustomNameInfo("Gives you a ton of boosts for a short period of time."));
        }

        public override int GetEffectTime() => 5;
        public override int GetEffectHate() => 0;
        public override void OnAdded()
        {
            Owner.SetStrength(Owner.strengthStatMod + 2);
            Owner.SetAccuracy(Owner.accuracyStatMod - 2);
            Owner.SetSpeed(Owner.speedStatMod + 2);
            Owner.critChance += 50;
        }
        public override void OnRemoved()
        {
            Owner.SetStrength(Owner.strengthStatMod - 2);
            Owner.SetAccuracy(Owner.accuracyStatMod + 2);
            Owner.SetSpeed(Owner.speedStatMod - 2);
            Owner.critChance -= 50;
        }
        public override void OnUpdated(EffectUpdatedArgs e)
        {
            e.UpdateDelay = 1f;
            CurrentTime--;
        }
    }
}
