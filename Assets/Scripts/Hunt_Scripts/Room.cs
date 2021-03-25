    using System.Collections;
using System.Collections.Generic;
using UnityEngine;

///<summary>
/// Room object which holds the information of the given room in the dungeon.
///</summary>
public class Room : MonoBehaviour{

    public RoomData roomData {
        get;
        private set;
    }

    public Vector2 position {
        get;
        private set;
    }

    public List<Room> connectedRooms {
        get;
        private set;
    }

    ///<summary>
    /// Intializes the room by setting the roomData and the position. Also
    /// creates a new empty list of connected rooms.
    ///</summary>
    public void initRoom(RoomData _roomData, Vector2 _position) {

        roomData = _roomData;

        position = _position;

        connectedRooms = new List<Room>();
    }

    ///<summary>
    /// Adds the given room to the connected rooms list.
    ///</summary>
    ///<param name="newRoom">The room being connected.</param>
    public void addNewConnectedRoom(Room newRoom) {

        connectedRooms.Add(newRoom);
    }

    ///<summary>
    /// Returns the vector entrance between this room and the given room.
    /// Gives an error if the given room is not connected.
    ///</summary>
    ///<param name="room">The room which the connection is being found 
    ///                   for</param>
    public Vector2 getConnection(Room room) {

        // Get the difference between the two rooms to get the entrance vector.
        Vector2 vectorDifference = room.position - position;

        // If the absolute value of the x or y value of the difference vector
        //   is greater than 1, then we know there is no valid connection
        //   between the two and spit out an error.
        if(Mathf.Abs(vectorDifference.x) > 1 || 
            Mathf.Abs(vectorDifference.y) > 1) {

            Debug.LogError("room given to getConnection is not connected to" + 
                " this room");
        }

        return vectorDifference;
    }

    ///<summary>
    /// Returns the connected room that the given entrance has. Returns null
    /// If the given entrance has no valid connection, return null.
    ///</summary>
    ///<param name="entrance">The entrance which the connection is being
    ///                       searched at </param>
    public Room getConnectedRoom(Vector2 entrance) {

        // Get the position of the potential room
        Vector2 roomPosition = position + entrance;

        // Check in the connected rooms list to see if there is a room which
        // matches the room position. If there is, return that room, otherwise
        // return null.
        foreach(Room room in connectedRooms) {
            if(room.position.Equals(roomPosition)) {
                return room;
            }
        }
        return null;
    }

    ///<summary>
    /// Returns if the two rooms are connected.
    ///</summary>
    ///<param name="givenRoom"> The room which the connection is being searched
    ///                         for at </param>

    public bool isConnected(Room givenRoom) {
        Vector2 vectorDifference = givenRoom.position - position;

        return getConnectedRoom(vectorDifference) != null;
    }

}
