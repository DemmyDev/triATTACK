using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Room", menuName = "Room")]
public class RoomChunk : ScriptableObject
{
    public int type;
    public bool up, down, left, right;
}
