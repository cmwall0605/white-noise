using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu (menuName = "RoomGenerator/RoomData")]
public class RoomData : ScriptableObject {
    [Header("Room Data")]

    // The prefab of that room
    public GameObject prefab;

    // Determines if the room can be spawned into
    public bool isSpawnLocal;

    // 
    public RoomType roomType;
}

public enum RoomType {
    Encounter,
    Event,
    Shop,
    Boss,
    None
}
