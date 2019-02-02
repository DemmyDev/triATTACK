using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Room", menuName = "Room")]
public class RoomChunk : ScriptableObject
{
    public enum Type { Normal = 0, Entry = 1, Item = 2, Boss = 3, Shop = 4};
    public Type type;
    public bool up, down, left, right;
    public Transform roomPrefab;
}