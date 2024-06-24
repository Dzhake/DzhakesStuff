using RogueLibsCore;

namespace DzhakesStuff.BigQuests
{
    public class AntiSlaveryService : CustomBigQuest, IDoUpdate
    {
        private bool Failed;

        [RLSetup]
        public static void Setup()
        {
            RogueLibs.CreateCustomBigQuest<AntiSlaveryService>()
                .WithUnlock(new BigQuestUnlock(nameof(AntiSlaveryService)))
                .WithName(new("[DS] Anti-Slavery Service"))
                .WithDescription(new("Higher levels are going to enslave all of us, we need to free all slaves before they do that!"));
        }

        public void Update()
        {
            int cur = 0;

            foreach (Agent agent2 in gc.agentList)
            {if (agent2.hasFormerSlaveOwner)
                {
                    cur++;
                    agent2.noBigQuestMarker = true;
                }
            }

            Current = cur;
        }

        public override void SetupQuestMarkers()
        {
            foreach (Agent agent2 in gc.agentList)
            {
                if (agent2.slaveOwners.Count > 0)
                {
                    Total++;
                    agent2.isBigQuestObject = true;
                    agent2.noBigQuestMarker = false;
                    agent2.bigQuestType = nameof(AntiSlaveryService);
                    agent2.showBigQuestMarker = true;
                    agent2.bigQuestMarkerAlwaysSeen = true;
                    if (!gc.quests.bigQuestObjectList.Contains(agent2))
                    {
                        gc.quests.bigQuestObjectList.Add(agent);
                    }
                    agent2.SpawnNewMapMarker();
                    gc.quests.CreateQuestMarker(agent2, null, "QuestTarget");
                }
            }
        }


        public override string GetProgress()
        {
            const string desc = "Free all slaves!\n";
            string text = $"<color=white>Floor: {Current}/{Total}</color>";
            if (Current == Total)
            {
                text = "<color=lime>Floor Clear!</color>";
            }
            return desc + text;
        }
    }
}
