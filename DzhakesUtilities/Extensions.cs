using RogueLibsCore;
using UnityEngine;

namespace DzhakesUtilities
{
    public static class Extensions
    {
        public static bool IsEnabled(this MutatorUnlock mutator)
        {
            return GameController.gameController?.challenges?.Contains(mutator.Name) ?? false;
        }
    }
}
