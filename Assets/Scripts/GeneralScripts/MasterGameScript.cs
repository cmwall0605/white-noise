using System.Collections;
using System.Collections.Generic;
using UnityEngine;

///<summary>
/// The master game script which is called at the start of the game and persists
/// through each phase
///</summary>
public class MasterGameScript : MonoBehaviour {

    // define settings. Settings can only be set privately (within this class) 
    //  but can be gotten from anywhere.
    public UserSettings userSettings { private set; get; }
    public GameSettings gameSettings { private set; get; }

    public static MasterGameScript instance {
        private set; 
        get;
    }

    /// <summary>
    /// Called when the script is called and initializes the settings and loads
    ///     the first scene
    /// </summary>
    void Awake() {
        if (instance == null) {
            instance = this;
        } else {
            Destroy(this);
            return;
        }

        // Tells Unity to not destroy the gameObject on a new scene load.
        DontDestroyOnLoad(gameObject);

        /* A new series of user settings */
        userSettings = new UserSettings();

        /* A new series of game settings */
        gameSettings = new GameSettings();
    }
}
