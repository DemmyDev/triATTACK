using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChunkSelector : MonoBehaviour
{
    [Header("General Variables")]
    public bool up; public bool down; public bool left; public bool right;

    // Normal room = 0, entry room = 1, item room = 3, shop room = 4, boss room = 5
    public enum RoomType { Normal = 0, Entry = 1, Item = 2, Boss = 3, Shop = 4 };
    public RoomType type;

    [Header("Wall Chunk Variables")]
    public Transform wallU; public Transform wallD; public Transform wallR; public Transform wallL;
    public Transform wallUD; public Transform wallRL; public Transform wallUR; public Transform wallUL; public Transform wallDR; public Transform wallDL;
    public Transform wallULD; public Transform wallRUL; public Transform wallDRU; public Transform wallLDR; public Transform wallUDRL;

    [HideInInspector]
    public Transform wall;

    [Header("Room Chunk Variables")]

    RoomChunk roomScriptable;

    Transform[] upNormRooms;
    Transform[] downNormRooms;
    Transform[] leftNormRooms;
    Transform[] rightNormRooms;

    [SerializeField] Transform entryRoom;
    [SerializeField] Transform itemRoom;
    [SerializeField] Transform bossRoom;
    [SerializeField] Transform shopRoom;

    [HideInInspector]
    public Transform room;

    void Start()
    {
        // Choose type of wall chunk to spawn, and spawns it
        wall = PickWall();
        var instWall = Instantiate(wall, gameObject.transform.position, gameObject.transform.rotation);
        instWall.transform.parent = gameObject.transform;
    }

    Transform PickWall()
    {
        if (up)
        {
            if (down)
            {
                if (right)
                {
                    if (left)
                    {
                        return wallUDRL;
                    }
                    else
                    {
                        return wallDRU;
                    }
                }
                else if (left)
                {
                    return wallULD;
                }
                else
                {
                    return wallUD;
                }
            }
            else
            {
                if (right)
                {
                    if (left)
                    {
                        return wallRUL;
                    }
                    else
                    {
                        return wallUR;
                    }
                }
                else if (left)
                {
                    return wallUL;
                }
                else
                {
                    return wallU;
                }
            }
        }
        if (down)
        {
            if (right)
            {
                if (left)
                {
                    return wallLDR;
                }
                else
                {
                    return wallDR;
                }
            }
            else if (left)
            {
                return wallDL;
            }
            else
            {
                return wallD;
            }
        }
        if (right)
        {
            if (left)
            {
                return wallRL;
            }
            else
            {
                return wallR;
            }
        }
        else
        {
            return wallL;
        }
    }

    public void SetScriptable(RoomChunk[] scriptableRooms)
    {

    }

    public void PickRoom()
    {
        switch (type)
        {
            case RoomType.Normal:

                break;
            case RoomType.Entry:
                room = entryRoom;
                var instRoom = Instantiate(room, gameObject.transform.position, gameObject.transform.rotation);
                instRoom.transform.parent = gameObject.transform;

                break;
            case RoomType.Item:

                break;
            case RoomType.Boss:

                break;
            case RoomType.Shop:

                break;
        }
    }
}
