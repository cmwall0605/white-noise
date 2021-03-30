using System.Collections;
using System.Collections.Generic;
using UnityEngine;

///<summary>
/// The master dungeon script which currently handles spawning the rooms of the
/// dungeon.
///</summary>
public class MasterDungeonScript : MonoBehaviour {

    public int manualRoomCount = 5,
               manualSeed = -1;

    public RoomData[] roomLayouts;

    public GameObject[] hallwayLayouts;

    private GameObject dungeonHolder;

    public Vector3 manualSpawnProbability;

    public List<Item> spawnPool;

    private List<Room> roomList;

    private bool isGenerating = false;

[HideInInspector]
    public Room spawnRoom {
        get;
        private set;
    }

    ///<summary>
    /// So far, the start function for the master dungeoner script generates the 
    /// rooms
    ///</summary>
    void Start() {
        if(manualSeed != -1){
            generateDungeon(manualRoomCount, manualSeed, manualSpawnProbability);
        } else {
            generateDungeon(manualRoomCount, manualSpawnProbability);
        }
    }

    /// <summary>
    /// 
    /// </summary>
    void Update() {
        if(Input.GetKeyDown(KeyCode.G)) {
            if(manualSeed != -1){
                generateDungeon(manualRoomCount, manualSeed, manualSpawnProbability);
            } else {
                generateDungeon(manualRoomCount, manualSpawnProbability);
            }
        }
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="rooms"></param>
    public void generateDungeon(int rooms) {
        generateDungeon(rooms, Random.Range(0, int.MaxValue));
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="rooms"></param>
    /// <param name="seed"></param>
    public void generateDungeon(int rooms, int seed) {
        generateDungeon(rooms, seed, new Vector3(25,50,75));
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="rooms"></param>
    /// <param name="roomProbability"></param>
    public void generateDungeon(int rooms, Vector3 roomProbability) {
        generateDungeon(rooms, Random.Range(0, int.MaxValue), roomProbability);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="rooms"></param>
    /// <param name="seed"></param>
    /// <param name="roomProbability"></param>
    public void generateDungeon(int rooms, int seed, Vector3 roomProbability) {
        StartCoroutine(spawnRooms(rooms, seed, roomProbability));
    }

    ///<summary>
    /// Coroutine for randoming generating a dungeon for the dungeon phase
    ///</summary>
    private IEnumerator spawnRooms(int maxRooms, int givenSeed, Vector3 roomProbability) {

        if(isGenerating) yield break;

        isGenerating = true;

        Debug.Log(givenSeed);

        if(dungeonHolder != null) {
            Destroy(dungeonHolder);

            yield return null;
        }

        Random.InitState(givenSeed);

        dungeonHolder = new GameObject();

        // List of the taken positions on the dungeon grid.
        List<Vector2> takenPosition = new List<Vector2>();

        // List of the potential spawn room layouts.
        List<RoomData> spawnLayouts = new List<RoomData>();

        List<RoomData> bossRoomLayouts = new List<RoomData>();

        List<RoomData> shopRoomLayouts = new List<RoomData>();

        List<RoomData> encounterRoomLayouts = new List<RoomData>();

        List<RoomData> eventRoomLayouts = new List<RoomData>();

        int shopSpawn = Random.Range((maxRooms-1)/2, maxRooms-1);

        // Stack of the rooms to be searched through. Creates rooms in a DFS
        // style manner to create more enlongated dungeons.
        roomList = new List<Room>();

        // Generate the list of possible spawn rooms, boss rooms and non boss
        //  rooms.
        foreach (RoomData room in roomLayouts) {

            // If it can be a spawn local, add it to spawn layouts.
            if (room.isSpawnLocal) {
                spawnLayouts.Add(room);
            }

            switch (room.roomType) {
                case RoomType.Boss:
                    bossRoomLayouts.Add(room);
                    break;
                case RoomType.Shop:
                    shopRoomLayouts.Add(room);
                    break;
                case RoomType.Event:
                    eventRoomLayouts.Add(room);
                    break;
                case RoomType.Encounter:
                    encounterRoomLayouts.Add(room);
                    break;
            }
        }

        // Grab a random spawn layout to set as the spawn point.
        RoomData spawnPoint = spawnLayouts[Random.Range(0, spawnLayouts.Count)];

        // Create a list of rooms to process; this list indicates rooms which
        //  were created but its connected rooms were not created/set.
        List<Room> roomsToProcess = new List<Room>();

        // Create the spawn room.
        spawnRoom = createRoom(spawnPoint, new Vector2(0,0));

        // Add the spawn room to be processed.
        roomsToProcess.Add(spawnRoom);

        // Add the spawn room as a taken positon.
        takenPosition.Add(spawnRoom.position);

        // Add the spawn room to the room list.
        roomList.Add(spawnRoom);

        // Set the room count to be 1 to indicate the spawn room.
        int roomCount = 1;

        // While the room count is less than the max amount of rooms...
        while (roomCount < maxRooms) {

            // The probability to create a connecting room
            int adjacentRoomCount;

            // Pop the roomsToProcess list into the currentRoom.
            Room currentRoom = roomsToProcess[0];
            roomsToProcess.RemoveAt(0);

            // Create a list of possible entrances and add each possible
            //  entrance vector. If it is a shop, do not allow for other
            //  possible entrances.
            List<Vector2> possibleEntrances = new List<Vector2>();
            if(currentRoom.roomData.roomType != RoomType.Shop) {
                possibleEntrances.Add(Vector2.left);
                possibleEntrances.Add(Vector2.right);
                possibleEntrances.Add(Vector2.up);
                possibleEntrances.Add(Vector2.down);
            }


            // Process for creating connections between two existing rooms.
            //   Additionally, sets up possible entrances by removing all that
            //   have a room already at the possible destination.
            for(int i = possibleEntrances.Count-1; i >= 0; i--) {
                Vector2 possibleNewPosition = currentRoom.position + 
                    possibleEntrances[i];
                
                // For Each possible new connection, if the position is taken,
                // have a 1/5 chance of connecting them.
                foreach(Vector2 position in takenPosition) {
                    if(possibleNewPosition.Equals(position)) {
                        if(Random.Range(0,5) == 0 && 
                            getRoomFromPosition(possibleNewPosition).roomData.roomType 
                            != RoomType.Shop) {

                            Room adjacentRoom = getRoomFromPosition(possibleNewPosition);
                            currentRoom.addNewConnectedRoom(adjacentRoom);

                            adjacentRoom.addNewConnectedRoom(currentRoom);

                            createWall(currentRoom, possibleEntrances[i], true);

                            createHallway(currentRoom, possibleEntrances[i]);

                            createWall(adjacentRoom, possibleEntrances[i] * -1, true);

                        }
                        possibleEntrances.RemoveAt(i);
                    }
                }
            }
            
            // If the current room is surrounded by other rooms, add the room
            //   it is connected to and skip to the next room in the stack.
            if(possibleEntrances.Count == 0) {
                roomsToProcess.Add(currentRoom.connectedRooms[0]);
                continue;
            }

            // Get the amount of rooms that should be connected to the current
            // room.
            adjacentRoomCount = generateConnectionCount(possibleEntrances.Count, roomProbability);

            // For each adjacent room, create it in the scene and push it onto
            //   the stack IF room count != max rooms. Once it does, ends the
            //   dungeon generation process.
            for(int i = 0; i < adjacentRoomCount; i++) {

                if (roomCount >= maxRooms) break;

                Vector2 entrance;

                int entranceIndex = Random.Range(0, possibleEntrances.Count);

                entrance = possibleEntrances[entranceIndex];

                possibleEntrances.RemoveAt(entranceIndex);

                Vector2 newPosition = currentRoom.position + entrance;

                Room connectedRoom;

                if(roomCount == maxRooms - 1) {
                    connectedRoom = createRoom(bossRoomLayouts
                        [Random.Range(0, bossRoomLayouts.Count)], newPosition);
                } else if(roomCount == shopSpawn) {
                    connectedRoom = createRoom(shopRoomLayouts
                        [Random.Range(0, bossRoomLayouts.Count)], newPosition);
                } else {
                    if(Random.Range(0,4) != 0) {
                        connectedRoom = createRoom(encounterRoomLayouts
                            [Random.Range(0, encounterRoomLayouts.Count)], 
                            newPosition);
                    } else {
                        connectedRoom = createRoom(eventRoomLayouts
                            [Random.Range(0, eventRoomLayouts.Count)], 
                            newPosition);
                    }

                }

                currentRoom.addNewConnectedRoom(connectedRoom);

                connectedRoom.addNewConnectedRoom(currentRoom);

                roomsToProcess.Add(connectedRoom);

                takenPosition.Add(newPosition);

                createWall(currentRoom, entrance, true);

                createHallway(currentRoom, entrance);

                createWall(connectedRoom, entrance * -1, true);

                roomCount++;

                roomList.Add(connectedRoom);

                yield return null;
            }
        }

        isGenerating = false;
    }

    /// <summary>
    /// Create the room.
    /// </summary>
    /// <param name="room"></param>
    /// <param name="position"></param>
    /// <returns></returns>
    public Room createRoom(RoomData room, Vector2 position) {

        GameObject newRoomObject = GameObject.Instantiate(room.prefab, 
            new Vector3(position.x,0,position.y)*16, Quaternion.identity, 
            dungeonHolder.transform);

        newRoomObject.AddComponent<Room>().initRoom(room, position);

        return newRoomObject.GetComponent<Room>();
    }

    /// <summary>
    /// Create walls for the given room.
    /// </summary>
    /// <param name="givenRoom"></param>
    /// <param name="entrance"></param>
    /// <param name="open"></param>
    public void createWall(Room givenRoom, Vector2 entrance, bool open) {

        string position = "";

        if(entrance.y == 0) {
            if(entrance.x == 1) 
                position = "up";
             else 
                position = "down";

        } else {
            if(entrance.y == 1) 
                position = "left";
             else 
                position = "right";
        }

        if(open) {

            givenRoom.transform.Find(position+"Closed").gameObject.
                SetActive(false);

            givenRoom.transform.Find(position+"OpenA").gameObject.
                SetActive(true);
            givenRoom.transform.Find(position+"OpenB").gameObject.
                SetActive(true);
        }
    }

    /// <summary>
    /// Create a hallway between the given room and the entrance room.
    /// </summary>
    /// <param name="givenRoom">The room the hallway is going to.</param>
    /// <param name="entrance">The room the hallway is coming form.</param>
    public void createHallway(Room givenRoom, Vector2 entrance){
        Vector3 hallwayPosition = new Vector3(givenRoom.position.x, 0, 
                                              givenRoom.position.y)*16;

        Quaternion hallwayRotation = Quaternion.identity;

        hallwayPosition.x += entrance.x * 8;

        hallwayPosition.z += entrance.y * 8;

        if(entrance.y != 0) {
            hallwayRotation = Quaternion.AngleAxis(90, Vector3.up);
        }

        GameObject.Instantiate(hallwayLayouts[Random.Range(0,
            hallwayLayouts.Length)], hallwayPosition, hallwayRotation, 
            dungeonHolder.transform);
    }

    /// <summary>
    /// Get the amount of connections that a room should have via 
    /// randomization.
    /// </summary>
    /// <param name="entranceCount">The max amount of rooms that can be had</param>
    /// <param name="spawnProbability">The probability of each type of room (x = 1, y = 2, z = 3, remainder = 4)</param>
    /// <returns>Returns the amount of connected rooms a room should have</returns>
    public int generateConnectionCount(int entranceCount, Vector3 spawnProbability) {

        int randomValue = Random.Range(0,100);

        if(randomValue < spawnProbability.x || entranceCount < 2) {
            return 1;
        } else if(randomValue < spawnProbability.x  + spawnProbability.y || entranceCount < 3) {
            return 2;
        } else if(randomValue < spawnProbability.x  + spawnProbability.y + spawnProbability.z || entranceCount < 4) {
            return 3;
        } else {
            return 4;
        }
    }

    /// <summary>
    /// Get the room object from a coordinate
    /// </summary>
    /// <param name="position">The position that the room should be at.</param>
    /// <returns>The room at that positon, null if there is no room there.</returns>
    public Room getRoomFromPosition(Vector2 position) {
        foreach(Room room in roomList) {
            if(room.position.Equals(position)) return room;
        }
        return null;
    }

    /// <summary>
    /// Run when the player goes on a shop tile
    /// </summary>
    /// <param name="currentRoom"></param>
    public void runShopEvent(Room currentRoom) {
        MasterGameScript.instance.character.inv.Add(spawnPool[0]);
    }

    /// <summary>
    /// Run when the player goes on an event tile
    /// </summary>
    /// <param name="currentRoom"></param>
    public void runEventEvent(Room currentRoom) {
        if(!currentRoom.visited){
            print("Unvisited event");
            currentRoom.setVisit(true);
        } else {
            print("Visited event");
        }  
    }

    /// <summary>
    /// Run when the player goes on an encounter tile
    /// </summary>
    /// <param name="currentRoom"></param>
    public void runEncounterEvent(Room currentRoom) {
        if(!currentRoom.visited){
            print("Unvisited encounter");
            currentRoom.setVisit(true);
        } else {
            print("Visited encounter");
        }  
    }

    /// <summary>
    /// Run when the player goes on the boss tile
    /// </summary>
    /// <param name="currentRoom"></param>
    public void runBossEvent(Room currentRoom) {
        print("boss");
    }
}
