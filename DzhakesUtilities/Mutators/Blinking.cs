﻿using RogueLibsCore;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DzhakesUtilities
{
    public class Blinking : MutatorUnlock
    {
        private static readonly Dictionary<Agent, int> coroutineIds = new();
        private static readonly Dictionary<Agent, Coroutine> coroutines = new();
        private static readonly Dictionary<Agent, float> blinkTimes = new();

        public Blinking() : base(nameof(Blinking))
        {
            UnlockCost = 3;
        }

        [RLSetup]
        public static void Setup()
        {
            Blinking mutator = new Blinking();
            RogueLibs.CreateCustomUnlock(mutator)
                .WithName(new CustomNameInfo("[DU] Blinking"))
                .WithDescription(new CustomNameInfo("Makes everyone blink"));

            RoguePatcher patcher = new RoguePatcher(Core.Instance) { TypeWithPatches = typeof(Blinking) };
            patcher.Postfix(typeof(Agent), "Start");
        }

        public static void Agent_Start(Agent __instance)
        {
            if (coroutines.TryGetValue(__instance, out Coroutine? coroutine))
            {
                coroutineIds[__instance]++;
                __instance.StopCoroutine(coroutine);
            }
            else coroutineIds[__instance] = 0;
            blinkTimes[__instance] = 0f;
            coroutines[__instance] = __instance.StartCoroutine(BlinkingCoroutine(coroutineIds[__instance], __instance));
        }
        private static IEnumerator BlinkingCoroutine(int id, Agent agent)
        {
            const float blinkDuration = 0.1f;
            while (coroutineIds[agent] == id)
            {
                float nextBlink = blinkTimes[agent];
                if (Time.time >= nextBlink)
                {
                    float unBlinkAt = Time.time + blinkDuration;
                    while (Time.time < unBlinkAt)
                    {
                        agent.agentHitboxScript?.eyes?.SetSprite("Clear");
                        agent.agentHitboxScript?.eyesH?.SetSprite("Clear");
                        yield return null;
                    }
                    agent.agentHitboxScript?.MustRefresh();
                    blinkTimes[agent] = Time.time + Random.Range(2f, 5f);
                }
                yield return null;
            }
        }
    }
}
