using RogueLibsCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace DzhakesUtilities
{
    public class EnablePlagueEverywhere() : MutatorUnlock(nameof(EnablePlagueEverywhere))
    {
        public static EnablePlagueEverywhere? Instance;

        [RLSetup]
        public static void Setup()
        {
            EnablePlagueEverywhere mutator = new EnablePlagueEverywhere();
            RogueLibs.CreateCustomUnlock(mutator)
                .WithName(new CustomNameInfo("[DS] Enable \"Plague Everywhere\""))
                .WithDescription(new CustomNameInfo("Disaster: Some people are infected! Try to not get infection, or the rest of game may be hard!"));
            Instance = mutator;
        }
    }
}
