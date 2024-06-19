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
            float healthPercent = Owner.health / Owner.healthMax;
            Owner.SetStrength(Owner.strengthStatMod + 2);
            Owner.SetEndurance(Owner.enduranceStatMod + 2);
            Owner.SetAccuracy(Owner.accuracyStatMod - 2);
            Owner.SetSpeed(Owner.speedStatMod + 2);
            Owner.critChance += 50;
            Owner.health = Owner.healthMax * healthPercent;
        }
        public override void OnRemoved()
        {
            float healthPercent = Owner.health / Owner.healthMax;
            Owner.SetStrength(Owner.strengthStatMod - 2);
            Owner.SetEndurance(Owner.enduranceStatMod - 2);
            Owner.SetAccuracy(Owner.accuracyStatMod + 2);
            Owner.SetSpeed(Owner.speedStatMod - 2);
            Owner.critChance -= 50;
            Owner.health = Owner.healthMax * healthPercent;
            
        }
        public override void OnUpdated(EffectUpdatedArgs e)
        {
            e.UpdateDelay = 1f;
            Owner.AgentUpdate();
            CurrentTime--;
        }
    }
}
