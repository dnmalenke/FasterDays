using BepInEx;
using HarmonyLib;

namespace MuckMod.FasterDayCycle
{
    [BepInPlugin("dnmal.fasterDayCycle", "FasterDayCycle", "1.1.0")]
    public class Main : BaseUnityPlugin
    {
        public const string Id = "dnmal.fasterDayCycle";
        public static Main instance;
        public Harmony harmony;

        private void Awake()
        {
            if (instance == null)
            {
                instance = this;
            }
            else
            {
                Destroy(this);
            }

            Harmony.CreateAndPatchAll(typeof(FasterDayCyclePatch));
        }

        private void OnDestroy() => this.harmony.UnpatchSelf();
    }
}
