using System.Collections;
using System.Collections.Generic;
using UnityEngine;

///<summary>
/// The master hunt script which currently handles spawning the rooms of the
/// dungeon.
///</summary>
public class MasterHuntScript : MonoBehaviour {

    public int manualRoomCount = 5,
               manualSeed = -1;

    public RoomData[] roomLayouts;

    public GameObject[] hallwayLayouts;

    private GameObject dungeonHolder;

    public Vector3 manualSpawnProbability;

    private List<Room> roomList;

    private bool isGenerating = false;

    private System.Random random;
[HideInInspector]
    public Room spawnRoom {
        get;
        private set;
    }

    ///<summary>
    /// So far, the start function for the master hunter script generates the 
    /// rooms
    ///</summary>
    void Start() {
        if(manualSeed != -1){
            generateDungeon(manualRoomCount, manualSeed, manualSpawnProbability);
        } else {
            generateDungeon(manualRoomCount, manualSpawnProbability);
        }
    }

    void Update() {
        if(Input.GetKeyDown(KeyCode.G)) {
            if(manualSeed != -1){
                generateDungeon(manualRoomCount, manualSeed, manualSpawnProbability);
            } else {
                generateDungeon(manualRoomCount, manualSpawnProbability);
            }
        }
    }

    public void generateDungeon(int rooms) {
        generateDungeon(rooms, Random.Range(0, int.MaxValue));
    }

    public void generateDungeon(int rooms, int seed) {
        generateDungeon(rooms, seed, new Vector3(25,50,75));
    }

    public void generateDungeon(int rooms, Vector3 roomProbability) {
        generateDungeon(rooms, Random.Range(0, int.MaxValue), roomProbability);
    }

    public void generateDungeon(int rooms, int seed, Vector3 roomProbability) {
        StartCoroutine(spawnRooms(rooms, seed, roomProbability));
    }

    ///<summary>
    /// Coroutine for randoming generating a dungeon for the hunt phase
    ///</summary>
    private IEnumerator spawnRooms(int maxRooms, int givenSeed, Vector3 roomProbability) {

        if(isGenerating) yield break;

        isGenerating = true;

        Debug.Log(givenSeed);

        if(dungeonHolder != null) {
            Destroy(dungeonHolder);

            yield return null;
        }

        random = new System.Random(givenSeed);

        dungeonHolder = new GameObject();

        // List of the taken positions on the dungeon grid.
        List<Vector2> takenPosition = new List<Vector2>();

        // Lis of the potential spawn room layouts.
        List<RoomData> spawnLayouts = new List<RoomData>();

        List<RoomData> bossRoomLayouts = new List<RoomData>();

        List<RoomData> nonBossRoomLayouts = new List<RoomData>();

        roomList = new List<Room>();

        // Generate the list of possible spawn rooms, boss rooms and non boss
        //  rooms.
        foreach (RoomData room in roomLayouts) {

            // If it can be a spawn local, add it to spawn layouts.
            if (room.isSpawnLocal) {
                spawnLayouts.Add(room);
            }

            // If it is a boss room, add it to the boss room layouts
            if (room.isBossRoom) {

                bossRoomLayouts.Add(room);

            // If it is NOT a boss room, add it to the non-boss room layouts.
            } else {

                nonBossRoomLayouts.Add(room);
            }
        }

        // Grab a random spawn layout to set as the spawn point.
        RoomData spawnPoint = spawnLayouts[random.Next(0, spawnLayouts.Count)];

        // Create a list of rooms to process; this list indicates rooms which
        //  were created but its connected rooms were not created/set.
        List<Room> roomsToProcess = new List<Room>();

        // Create the spawn room.
        spawnRoom = createRoom(spawnPoint, new Vector2(0,0));

        // Add the spawn room to be processed.
        roomsToProcess.Add(spawnRoom);

        // Add the spawn room as a taken positon.
        takenPosition.Add(spawnRoom.position);

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
            //  entrance vector.
            List<Vector2> possibleEntrances = new List<Vector2>();
            possibleEntrances.Add(Vector2.left);
            possibleEntrances.Add(Vector2.right);
            possibleEntrances.Add(Vector2.up);
            possibleEntrances.Add(Vector2.down);

            for(int i = possibleEntrances.Count-1; i >= 0; i--) {
                Vector2 possibleNewPosition = currentRoom.position + 
                    possibleEntrances[i];
                foreach(Vector2 taken in takenPosition) {
                    if(possibleNewPosition.Equals(taken)) {
                        if(random.Next(0,5) == 0) {
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
            
            if(possibleEntrances.Count == 0) {
                roomsToProcess.Add(currentRoom.connectedRooms[0]);
                continue;
            }

            adjacentRoomCount = generateConnectionCount(possibleEntrances.Count, roomProbability);

            for(int i = 0; i < adjacentRoomCount; i++) {

                if (roomCount >= maxRooms) break;

                Vector2 entrance;

                int entranceIndex = random.Next(0, possibleEntrances.Count);

                entrance = possibleEntrances[entranceIndex];

                possibleEntrances.RemoveAt(entranceIndex);

                Vector2 newPosition = currentRoom.position + entrance;

                Room connectedRoom;

                if(roomCount == maxRooms - 1) {
                    connectedRoom = createRoom(bossRoomLayouts
                        [random.Next(0, bossRoomLayouts.Count)],
                        newPosition);
                } else {
                    connectedRoom = createRoom(nonBossRoomLayouts
                        [random.Next(0, nonBossRoomLayouts.Count)], 
                        newPosition);
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

    public Room createRoom(RoomData room, Vector2 position) {

        GameObject newRoomObject = GameObject.Instantiate(room.prefab, 
            new Vector3(position.x,0,position.y)*16, Quaternion.identity, 
            dungeonHolder.transform);

        newRoomObject.AddComponent<Room>().initRoom(room, position);

        return newRoomObject.GetComponent<Room>();
    }

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
    public void createHallway(Room givenRoom, Vector2 entrance){
        Vector3 hallwayPosition = new Vector3(givenRoom.position.x, 0, 
                                              givenRoom.position.y)*16;

        Quaternion hallwayRotation = Quaternion.identity;

        hallwayPosition.x += entrance.x * 8;

        hallwayPosition.z += entrance.y * 8;

        if(entrance.y != 0) {
            hallwayRotation = Quaternion.AngleAxis(90, Vector3.up);
        }

        GameObject.Instantiate(hallwayLayouts[random.Next(0,
            hallwayLayouts.Length)], hallwayPosition, hallwayRotation, 
            dungeonHolder.transform);
    }

    public int generateConnectionCount(int entranceCount, Vector3 spawnProbability) {

        int randomValue = random.Next(0,100);

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

    public Room getRoomFromPosition(Vector2 position) {
        foreach(Room room in roomList) {
            if(room.position.Equals(position)) return room;
        }
        return null;
    }
}
