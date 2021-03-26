using System.Collections;
using System.Collections.Generic;
using UnityEngine;

///<summary>
/// Used to house the settings a user can change
///</summary>
public class UserSettings {

    public float BASE_CAMERA_VELOCITY = 2.0f;
    public float BASE_TRANSLATION_VELOCITY = 0.2f;
    public float BASE_SCROLL_SCALE = 4.0f;
    public float BASE_HORIZONTAL_ROTATION_SPEED = 2.0f;
    public float BASE_VERTICAL_ROTATION_SPEED = 2.0f;
    public float BASE_LINE_WIDTH = 0.05f;

    public Color hunterSelectCircleColour = Color.green;
    public Color hunterMoveCircleColour = Color.black;

    public UserSettings() {

    }
}
