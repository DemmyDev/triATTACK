using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomProperties : MonoBehaviour
{
    public RoomChunk chunk;
    public enum Type { Normal = 0, Entry = 1, Item = 2, Boss = 3, Shop = 4 };
    public Type type;
    public bool up, down, left, right;

    void Start()
    {
        up = chunk.up;
        down = chunk.down;
        left = chunk.left;
        right = chunk.right;
        type = (Type)chunk.type;
    }
}
