using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// Used to control and manipulate the camera and the player's ability to 
///     interact with the field/hunters.
/// </summary>

[RequireComponent(typeof(CharacterController))]
public class HuntController : MonoBehaviour {

  private const string MASTER_HUNT_LOOP = "masterHuntLoop", 
    ACTUAL_CAMERA = "actualCamera", UI_CANVAS = "UI Canvas", 
    HUNTER_PARTY = "Party", WAYPOINT = "waypoint", 
    SCROLL_WHEEL = "Mouse ScrollWheel", VERTICAL = "Vertical", HORIZONTAL = "Horizontal";

  // The cam's movement velocity
  public float cameraVelocity = 10.0f,

  // How far away from any from the edge of the screen the mouse needs to be 
  // to trigger movement
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
  // The leeway for the hunter party's movement to the way point.
  distanceDeleta = 0.1f;

  private int battleSceneIndex = 2;

  private bool isMoving = false;

  public LayerMask roomSelectLayers;

  private GameObject actualCamera, hunterParty;

  private CharacterController playerCameraController;

  private MasterHuntScript masterHuntScript;

  // The hunter being selected by the player.
  private Transform selectedPuppet;

  private Room currentRoom;

  /// <summary>
  /// Called when the script is called and grabs needed game objects in the
  /// scene.
  /// </summary>
  void Start() {
    masterHuntScript = GameObject.Find(MASTER_HUNT_LOOP).
      GetComponent<MasterHuntScript>();
    actualCamera = transform.Find(ACTUAL_CAMERA).gameObject;
    if(actualCamera == null) {
      Debug.LogError("The camera could not be found in " + gameObject.name);
    }
    playerCameraController = GetComponent<CharacterController>();
    hunterParty = GameObject.Find(HUNTER_PARTY);
    Cursor.lockState = CursorLockMode.Confined;
  }

  /// <summary>
  /// Updates each frame to determine what the player/camera must calculate 
  /// and execute in that exact frame. Currently calculate shunter selection
  /// </summary>
  public void Update() {
    // If there is a current room selected...
    if (currentRoom != null) {
      // If the hunter should be moving, continue the movement
      if(isMoving){
          // Get the position to move the party towards to be the current
          // room's waypoint position.
          Vector3 positionToMoveTo = 
              currentRoom.transform.Find(WAYPOINT).position;
          // Set the y value of position to move to be the same y
          //   position of the hunter party
          positionToMoveTo.y = hunterParty.transform.position.y;
          // Move the hunter party's position
          hunterParty.transform.position = 
              Vector3.MoveTowards(hunterParty.transform.position, 
                                  positionToMoveTo, 
                                  partyVelocity * Time.deltaTime);
          // If the hunter has reached the position by an offset of
          //   0.1, finish movement.
          if (Vector3.Distance(positionToMoveTo, 
                                hunterParty.transform.position) < 
                                  distanceDeleta) {
              isMoving = false;
          }
      // If the hunter is not moving, the current room is the boss room
      // and the scene is not loading, load the fight scene.
      } else if (currentRoom.roomData.isBossRoom) {
          FindObjectOfType<TransitionController>().
              sceneTransition(battleSceneIndex);
      }
    } else {
      currentRoom = masterHuntScript.spawnRoom;
    }
    // Hunter Selection, only execute if the cursor isn't blocked by any 
    //   UI elements.
    if (Input.GetMouseButtonDown(0)) {
      // The ray where the the mouse is positioned
      Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
      // Holds the data for what got hit by the ray IF something got hit
      RaycastHit hit;
      // If something was hit
      if (Physics.Raycast(ray, out hit, Mathf.Infinity, roomSelectLayers)) {
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
    if (Input.GetAxis(HORIZONTAL) > 0 || 
      (Input.mousePosition.x >= Screen.width - edgeDeltaTrigger &&
      Input.mousePosition.x <= Screen.width)) {
      movement += transform.right * cameraVelocity * Time.fixedDeltaTime;
    // Left Movement
    } else if (Input.GetAxis(HORIZONTAL) < 0 || 
      (Input.mousePosition.x <= edgeDeltaTrigger && 
      Input.mousePosition.x >= 0)) {
      movement += -transform.right * cameraVelocity * Time.fixedDeltaTime;
    }
    // Up Movement
    if (Input.GetAxis(VERTICAL) > 0 || 
      (Input.mousePosition.y >= Screen.height - edgeDeltaTrigger && 
      Input.mousePosition.y <= Screen.height)) {
      movement += transform.forward * cameraVelocity * Time.fixedDeltaTime;
    // Down Movement
    } else if (Input.GetAxis(VERTICAL) < 0 || 
      (Input.mousePosition.y <= edgeDeltaTrigger && 
      Input.mousePosition.y >= 0)) {
      movement += -transform.forward * cameraVelocity * Time.fixedDeltaTime;
    }
    // The command which moves the camera after all axis are read.
    playerCameraController.Move(movement);
    // Get the scrollwheel value and sum it to the current zoom level
    zoomLevel += Input.GetAxis(SCROLL_WHEEL) * -scrollVelocity;
    // Clamp the zoom level between the min and max scroll distance
    zoomLevel = Mathf.Clamp(zoomLevel, minScrollDistance, maxScrollDistance);
    // The command which moves the camera after the scroll value is 
    //   calculated.
    actualCamera.transform.localPosition = new Vector3(
      actualCamera.transform.localPosition.x,
      zoomLevel,
      -zoomLevel);
  }

  public void OnDrawGizmos() {
    if(hunterParty == null) return;
    // get the positions of the hunter and the monster
    Vector3 pos = hunterParty.transform.position;
    Vector3 destination = currentRoom.transform.Find(WAYPOINT).position;
    // set a colour and draw a line between hunter and monster
    Gizmos.color = Color.cyan;
    Gizmos.DrawLine(pos, destination);
#if UNITY_EDITOR
    // if we're in the unity editor, display the distance between the 
    //  hunter and monster
    UnityEditor.Handles.Label((pos + destination) / 2, "" + 
      Vector3.Distance(pos, destination));
#endif
  }
}

