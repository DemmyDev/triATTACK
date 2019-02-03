using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Room
{
	public Vector2 gridPos;
    public enum RoomType { Normal = 0, Entry = 1, Item = 2, Boss = 3, Shop = 4 };
    public RoomType type;
	public bool doorTop, doorBot, doorLeft, doorRight;

	public Room(Vector2 _gridPos, RoomType _type)
    {
		gridPos = _gridPos;
        type = _type;
	}
}