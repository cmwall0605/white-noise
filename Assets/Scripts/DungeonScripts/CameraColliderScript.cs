using System.Collections;
using System.Collections.Generic;
using UnityEngine;

///<summary>
/// Script which handles the collider for the player's camera. Handles creating
/// the cross section functionality of the walls in the dungeon.
///</summary>
public class CameraColliderScript : MonoBehaviour {

    public float wallDownPosition = 0.5f,

                 wallDownScale = 1.5f,

                 wallUpPosition = 1.5f,

                 wallUpScale = 3.0f;

    private const string WALL_TAG = "Wall";
    
    ///<summary>
    /// When the collider hits a wall, create a cross section of the wall
    ///</summary>
    ///<param name="collision"> what is being hit by the collider </param>
    void OnTriggerEnter(Collider collision) {

        // If the collided object is a wall, lower it down.
        if(collision.gameObject.tag == WALL_TAG) {
            Transform wall = collision.transform;
            wall.position = new Vector3(wall.position.x, wallDownPosition, 
                wall.position.z);
            wall.localScale = new Vector3(wall.localScale.x, wallDownScale, 
                wall.localScale.z);
        }
    }

    ///<summary>
    /// When the collider exits a wall, revert the wall back to normal
    ///</summary>
    ///<param name="collision"> what is being hit by the collider </param>
    void OnTriggerExit(Collider collision) {

        // If the un-collided object is a wall, raise it.
        if(collision.gameObject.tag == WALL_TAG) {
            Transform wall = collision.transform;
            wall.position = new Vector3(wall.position.x, wallUpPosition, 
                wall.position.z);
            wall.localScale = new Vector3(wall.localScale.x, wallUpScale, 
                wall.localScale.z);
        }
    }
}
