using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]

///<summary>
/// Used to house the game data.
///</summary>
public class GameSettings {

    /*Base Player Stats*/
    public const string BASE_NAME = "Charlie";

    public const int BASE_MELEE_ATTACK_COUNT = 1,
                     BASE_ACTION_POINTS = 5,
                     BASE_ACTION_REFILL = 4,
                     BASE_TOUGHNESS = 5,
                     BASE_STRENGTH = 5,
                     BASE_MOVEMENT = 5,
                     BASE_PRICE = 1,
                     BASE_SANITY = 5,
                     BASE_LEVEL = 0;


    public GameSettings() {
    }
}
