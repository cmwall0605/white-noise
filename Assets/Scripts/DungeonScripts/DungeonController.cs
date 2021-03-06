using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// Used to control and manipulate the camera and the player's ability to 
///     interact with the dungeon.
/// </summary>

[RequireComponent(typeof(CharacterController))]
public class DungeonController : MonoBehaviour {

    private const string MASTER_DUNGEON_LOOP = "masterDungeonLoop",
                         ACTUAL_CAMERA = "actualCamera",
                         UI_CANVAS = "UI Canvas",
                         PLAYER = "Player",
                         WAYPOINT = "waypoint",
                         SCROLL_WHEEL = "Mouse ScrollWheel",
                         VERTICAL = "Vertical",
                         HORIZONTAL = "Horizontal";

                // The cam's movement velocity
    public float cameraVelocity = 10.0f,

                // How far away from any from the edge of the screen the mouse needs to be 
                //   to trigger movement
                 edgeDeltaTrigger = 3.0f,

                // The max allowed distance to scroll out
                 maxScrollDistance = 30.0f,

                // The min allowed distance to scroll in
                 minScrollDistance = 5.0f,

                // The velocity for camera scrolling
                 scrollVelocity = 10.0f,

                // The current zoom level calculated by the scroll wheel * scroll velocity
                 zoomLevel = 5.0f,

                // The velocity of the party.
                 partyVelocity = 5.0f,
                // The leeway for Charlie's movement to the way point.
                 distanceDeleta = 0.1f;

    private bool isMoving = false;

    private bool newRoom = false;

    public LayerMask roomSelectLayers;

    private GameObject actualCamera, player;

    private CharacterController playerCameraController;

    private MasterDungeonScript masterDungeonScript;

    private Transform selectedPuppet;

    private Room currentRoom;

    /// <summary>
    /// Called when the script is called and grabs needed game objects in the
    /// scene.
    /// </summary>
    void Start() {
        
        masterDungeonScript = GameObject.Find(MASTER_DUNGEON_LOOP).
            GetComponent<MasterDungeonScript>();

        actualCamera = transform.Find(ACTUAL_CAMERA).gameObject;
        
        if(actualCamera == null) {

            Debug.LogError("The camera could not be found in "
                           + gameObject.name);
        }

        playerCameraController = GetComponent<CharacterController>();

        player = GameObject.Find(PLAYER);

        Cursor.lockState = CursorLockMode.Confined;
    }

    /// <summary>
    /// Updates each frame to determine what the player/camera must calculate 
    /// and execute in that exact frame. Currently calculates player selection
    /// </summary>
    public void Update() {

        // If there is a current room selected...
        if (currentRoom != null) {

            // If the player should be moving, continue the movement
            if(isMoving){

                newRoom = true;

                // Get the position to move the party towards to be the current
                // room's waypoint position.
                Vector3 positionToMoveTo = 
                    currentRoom.transform.Find(WAYPOINT).position;

                // Set the y value of position to move to be the same y
                //   position of the player
                positionToMoveTo.y = player.transform.position.y;

                // Move the player's position
                player.transform.position = 
                    Vector3.MoveTowards(player.transform.position, 
                                        positionToMoveTo, 
                                        partyVelocity * Time.deltaTime);

                // If the hplayer has reached the position by an offset of
                //   0.1, finish movement.
                if (Vector3.Distance(positionToMoveTo, 
                                     player.transform.position) < 
                                        distanceDeleta) {
                
                    isMoving = false;
                }

            // If the player is not moving, then handle what to do in the
            //   current room.
            } else {
                if(newRoom) {
                    newRoom = false;
                    handleRoom();
                }
            }
        } else {

            currentRoom = masterDungeonScript.spawnRoom;

            currentRoom.setVisit(true);
        }

        // Player Selection, only execute if the cursor isn't blocked by any 
        //   UI elements.
        if (Input.GetMouseButtonDown(0)) {

            /* The ray where the the mouse is positioned */
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            /* Holds the data for what got hit by the ray IF something got 
               hit */
            RaycastHit hit;

            // If something was hit
            if (Physics.Raycast(ray, out hit, Mathf.Infinity, 
                                roomSelectLayers)) {
                
                Room potentialRoom = hit.transform.parent.GetComponent<Room>();

                if(currentRoom.isConnected(potentialRoom) && !isMoving) {

                currentRoom = potentialRoom;

                isMoving = true;
                }
            }
        }
    }

    public void FixedUpdate() {

        Vector3 movement = Vector3.zero;

        // Right Movement
        if (Input.GetAxis(HORIZONTAL) > 0 
        || (Input.mousePosition.x >= Screen.width - edgeDeltaTrigger && 
            Input.mousePosition.x <= Screen.width)) {

            movement += transform.right * cameraVelocity * Time.fixedDeltaTime;

        // Left Movement
        } else if (Input.GetAxis(HORIZONTAL) < 0 
        || (Input.mousePosition.x <= edgeDeltaTrigger && 
            Input.mousePosition.x >= 0)) {

            movement += -transform.right * cameraVelocity * Time.fixedDeltaTime;
        }

        // Up Movement
        if (Input.GetAxis(VERTICAL) > 0 
        || (Input.mousePosition.y >= Screen.height - edgeDeltaTrigger && 
            Input.mousePosition.y <= Screen.height)) {

            movement += transform.forward * cameraVelocity * 
                Time.fixedDeltaTime;

        // Down Movement
        } else if (Input.GetAxis(VERTICAL) < 0
        || (Input.mousePosition.y <= edgeDeltaTrigger && 
            Input.mousePosition.y >= 0)) {

            movement += -transform.forward * cameraVelocity * 
                Time.fixedDeltaTime;
        }

        // The command which moves the camera after all axis are read.
        playerCameraController.Move(movement);

        // Get the scrollwheel value and sum it to the current zoom level
        zoomLevel += Input.GetAxis(SCROLL_WHEEL) * -scrollVelocity;

        // Clamp the zoom level between the min and max scroll distance
        zoomLevel = Mathf.Clamp(zoomLevel, minScrollDistance, 
                                maxScrollDistance);

        // The command which moves the camera after the scroll value is 
        //   calculated.
        actualCamera.transform.localPosition = new Vector3(
            actualCamera.transform.localPosition.x,
            zoomLevel,
            -zoomLevel);

    }

    public void handleRoom() {
        switch(currentRoom.roomData.roomType) {
            case RoomType.Boss:
                masterDungeonScript.runBossEvent(currentRoom);
                break;
            case RoomType.Encounter:
                masterDungeonScript.runEncounterEvent(currentRoom);
                break;
            case RoomType.Event:
                masterDungeonScript.runEventEvent(currentRoom);
                break;
            case RoomType.Shop:
                masterDungeonScript.runShopEvent(currentRoom);
                break;
            default:
                break;
        }
    }
}

