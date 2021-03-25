using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu (menuName = "RoomGenerator/RoomData")]
public class RoomData : ScriptableObject {
    [Header("Room Data")]
    public GameObject prefab;

    public bool isSpawnLocal;

    public bool isBossRoom;
}
