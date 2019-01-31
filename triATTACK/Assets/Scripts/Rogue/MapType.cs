using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapType : MonoBehaviour {

    GameObject[] parentChunks;
    LevelGeneration levelGen;
    bool itemRoomIsSet = false, bossRoomIsSet = false, shopRoomIsSet = false;

    [Header("Normal Scriptables")]
    public RoomChunk[] normalRoomChunks;

    void Start()
    {
        levelGen = GameObject.Find("Level Generator").GetComponent<LevelGeneration>();
        parentChunks = GameObject.FindGameObjectsWithTag("ParentChunk");

        // Give the item, boss, and shop type to three rooms
        SetItemRoom();
        SetBossRoom();
        SetShopRoom();
        AssignScriptable();
    }

    void SetItemRoom()
    {
        int iterations = 0;
        do
        {
            iterations++;
            int rand = Random.Range(0, levelGen.numberOfRooms);
            ChunkSelector chunk = parentChunks[rand].GetComponent<ChunkSelector>();
            
            if (chunk.type == 0)
            {
                if (chunk.wall == chunk.wallD || chunk.wall == chunk.wallU || chunk.wall == chunk.wallR || chunk.wall == chunk.wallL)
                {
                    chunk.type = ChunkSelector.RoomType.Item;
                    itemRoomIsSet = true;
                }
            }
        } while (!itemRoomIsSet && iterations < 100);

        if (!itemRoomIsSet)
        {
            Debug.LogError("Could not find item room");
        }
    }

    void SetBossRoom()
    {
        int iterations = 0;

        do
        {
            iterations++;
            int rand = Random.Range(0, levelGen.numberOfRooms);
            ChunkSelector chunk = parentChunks[rand].GetComponent<ChunkSelector>();

            if (chunk.type == 0)
            {
                if (chunk.wall == chunk.wallD || chunk.wall == chunk.wallU || chunk.wall == chunk.wallR || chunk.wall == chunk.wallL)
                {
                    chunk.type = ChunkSelector.RoomType.Boss;
                    bossRoomIsSet = true;
                }
            }
        } while (!bossRoomIsSet && iterations < 100);

        if (!bossRoomIsSet)
        {
            Debug.LogError("Could not find boss room");
        }
    }

    void SetShopRoom()
    {
        int iterations = 0;

        do
        {
            iterations++;
            int rand = Random.Range(0, levelGen.numberOfRooms);
            ChunkSelector chunk = parentChunks[rand].GetComponent<ChunkSelector>();

            if (chunk.type == 0)
            {

                if (chunk.wall == chunk.wallD || chunk.wall == chunk.wallU || chunk.wall == chunk.wallR || chunk.wall == chunk.wallL)
                {
                    chunk.type = ChunkSelector.RoomType.Shop;
                    shopRoomIsSet = true;
                }
            }
        } while (!shopRoomIsSet && iterations < 100);

        if (!shopRoomIsSet)
        {
            Debug.LogError("Could not find shop room");
        }
    }

    void AssignScriptable()
    {
        foreach (GameObject parentChunk in parentChunks)
        {
            ChunkSelector chunk = parentChunk.GetComponent<ChunkSelector>();
            // Give the room a scriptable, and assign the scriptable's variables to the room
            chunk.SetScriptable(roomChunks);

            switch (chunk.type)
            {
                case ChunkSelector.RoomType.Normal:

                    break;
                case ChunkSelector.RoomType.Entry:

                    break;

                case ChunkSelector.RoomType.Item:

                    break;
                case ChunkSelector.RoomType.Boss:

                    break;
                case ChunkSelector.RoomType.Shop:

                    break;
            }

            // Based on the scriptable's information, pick an allowed room
            chunk.PickRoom();
        }
    }
}