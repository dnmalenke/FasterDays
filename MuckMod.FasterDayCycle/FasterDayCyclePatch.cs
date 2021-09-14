using HarmonyLib;
using System.Reflection;
using UnityEngine;

namespace MuckMod.FasterDayCycle
{
    public class FasterDayCyclePatch
    {
        private static float _dayMultiplier = 0;
        private static PropertyInfo _totalTimeProp = null;

        [HarmonyPatch(typeof(GameLoop), "NewDay")]
        [HarmonyPostfix]
        private static void NewDayPostfix(ref GameLoop __instance)
        {
             _dayMultiplier += (int)GameManager.gameSettings.difficulty / 10f;

            if(__instance.currentDay == 0)
            {
                _dayMultiplier = 0;
            }

            Debug.Log($"Day speed multiplier set to {_dayMultiplier}");
        }

        [HarmonyPatch(typeof(DayCycle), "Update")]
        [HarmonyPrefix]
        private static void DayCycleUpdatePrefix(ref DayCycle __instance)
        {
            if (GameManager.state != GameManager.GameState.Playing)
            {
                return;
            }

            if (DayCycle.time <= 0.5f)
            {
                float num = 1f * __instance.timeSpeed / DayCycle.dayDuration;
                float num2 = num * Time.deltaTime;

                num2 *= _dayMultiplier;
                DayCycle.time += num2;
                DayCycle.time %= 1f;

                if(_totalTimeProp == null)
                {
                    _totalTimeProp = typeof(DayCycle).GetProperty("totalTime");
                }

                _totalTimeProp.SetValue(__instance, (float)_totalTimeProp.GetValue(__instance) + num2);
            }
        }
    }
}
