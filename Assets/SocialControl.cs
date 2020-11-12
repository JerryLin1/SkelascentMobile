using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SocialControl : MonoBehaviour
{
    public string leaderboardID;
    void Start()
    {
        if (!Social.localUser.authenticated)
            Social.localUser.Authenticate (success => {});
    }
    public void ReportScore(long score) {
        if (Social.localUser.authenticated) {
            Social.ReportScore (score, leaderboardID, success => {});
        }
    }
    public void ShowLeaderboard ()
    {
        if (Social.localUser.authenticated)
            Social.ShowLeaderboardUI ();
    }
}
