using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LeaderboardStatistics
{
    private int _kills = 0;

    public int Kills => _kills;

    public LeaderboardStatistics()
    {
    }

    public void AddKill()
    {
        ++_kills;
    }
}