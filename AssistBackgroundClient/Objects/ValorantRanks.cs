﻿using System.Collections.Generic;

namespace AssistBackgroundClient.Objects;

public class ValorantRanks
{
    public static IDictionary<int, string> Ranks = new Dictionary<int, string>()
    {
        {0, "Unranked"},
        {3, "Iron 1"},
        {4, "Iron 2"},
        {5, "Iron 3"},

        {6, "Bronze 1"},
        {7, "Bronze 2"},
        {8, "Bronze 3"},
        
        {9, "Silver 1"},
        {10, "Silver 2"},
        {11, "Silver 3"},
        
        {12, "Gold 1"},
        {13, "Gold 2"},
        {14, "Gold 3"},
        
        {15, "Platinum 1"},
        {16, "Platinum 2"},
        {17, "Platinum 3"},
        
        {18, "Diamond 1"},
        {19, "Diamond 2"},
        {20, "Diamond 3"},
        
        {21, "Immortal 1"},
        {22, "Immortal 2"},
        {23, "Immortal 3"},
        
        {24, "Radiant"},

    };
}