using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Steamworks;

public class SteamAchievements : MonoBehaviour
{
    protected Callback<GameOverlayActivated_t> GameOverlayActivated;
    private static bool unlockTest = false;

    private static void TestSteamAchievement(string id)
    {
        SteamUserStats.GetAchievement(id, out unlockTest);
    }

    public static void UnlockAchievement(string id)
    {
        TestSteamAchievement(id);
        if (!unlockTest)
        {
            SteamUserStats.SetAchievement(id);
            SteamUserStats.StoreStats();
        }
    }

    void OnEnable()
    {
        if (SteamManager.Initialized)
        {
            GameOverlayActivated = Callback<GameOverlayActivated_t>.Create(OpenSteamOverlay);
        }
    }

    void OpenSteamOverlay(GameOverlayActivated_t overlay)
    {
        if (overlay.m_bActive != 0)
        {
            Time.timeScale = 0f;
        }
        else
        {
            Time.timeScale = 1f;
        }
    }
}
