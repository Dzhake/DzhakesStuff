using System.Collections;
using RogueLibsCore;
using UnityEngine;

namespace DzhakesStuff
{
    public class RadiationBlasts : CustomDisaster
    {
        public static RadiationBlasts? Instance;
        public static bool Active => Instance?.IsActive ?? false;

        [RLSetup]
        public static void Setup()
        {
            RogueLibs.CreateCustomDisaster<RadiationBlasts>()
                .WithName(new CustomNameInfo("Radiation Blasts++"))
                .WithDescription(new CustomNameInfo("Get indoors!"))
                .WithMessage(new CustomNameInfo("Get indoors!"))
                .WithRemovalMutator(new("[DS] Remove \"Radiation Blasts++\""));
            var patcher = Core.GetPatcher<RadiationBlasts>();
            patcher.Postfix(typeof(Clock), nameof(Clock.StartClock));
            patcher.Postfix(typeof(Clock), "Update");
            patcher.Prefix(typeof(Clock), nameof(Clock.clockFlash));
        }

        public override void Start()
        {
            Instance = this;
        }

        public override void Finish() {}

        public override IEnumerator? Updating()
        {
            yield return null;
        }

        public static void Clock_StartClock(Clock __instance)
        {
            if (!Active) return;

            __instance.timeLeft = 10.999f;
            __instance.countingDown = true;
            __instance.clockText.text = "";
            __instance.gameObject.SetActive(value: true);
        }

        public static void Clock_Update(Clock __instance)
        {
            if (!Active) return;
            string text = Mathf.Floor(__instance.timeLeft / 60f).ToString("0");
            string text2 = Mathf.Floor(__instance.timeLeft % 60f).ToString("00");
            if (text == "0" && text2 == "00" && gc.serverPlayer)
            {
                gc.playerAgent.StartCoroutine(Flash());
                __instance.timeLeft = 10.999f;
                __instance.clockText.text = text + ":" + 10;
                gc.playerAgent.oma.clockTime = (int)__instance.timeLeft;
            
            }
        }

        public static IEnumerator Flash()
        {
            gc.levelFeelingsScript.HarmAtInterval();
            yield return new WaitForSeconds(0.1f);
            gc.levelFeelingsScript.HarmAtInterval();
            yield return new WaitForSeconds(0.1f);
            gc.levelFeelingsScript.HarmAtInterval();
            yield return new WaitForSeconds(0.1f);
        }

        public static void Clock_clockFlash(Clock __instance)
        {
            if (!Active) return;
            int num = int.Parse(__instance.prevSeconds);
            if (num < 5 && num > 1)
                gc.audioHandler.Play(gc.playerAgent, "Countdown");
        }
    }
}
