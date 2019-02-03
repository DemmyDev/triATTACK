using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapType : MonoBehaviour {

    GameObject[] parentChunks;
    LevelGeneration levelGen;
    bool itemRoomIsSet = false, bossRoomIsSet = false, shopRoomIsSet = false;

    [Header("Special Room Scriptables")]
    [SerializeField] List<RoomChunk> entryRoomChunks;
    [SerializeField] List<RoomChunk> itemRoomChunks;
    [SerializeField] List<RoomChunk> bossRoomChunks;
    [SerializeField] List<RoomChunk> shopRoomChunks;

    [Header("Normal Room Scriptables")]
    [SerializeField] List<RoomChunk> roomChunkU; [SerializeField] List<RoomChunk> roomChunkD; [SerializeField] List<RoomChunk> roomChunkR; [SerializeField] List<RoomChunk> roomChunkL;
    [SerializeField] List<RoomChunk> roomChunkUD; [SerializeField] List<RoomChunk> roomChunkRL; [SerializeField] List<RoomChunk> roomChunkUR; [SerializeField] List<RoomChunk> roomChunkUL; [SerializeField] List<RoomChunk> roomChunkDR; [SerializeField] List<RoomChunk> roomChunkDL;
    [SerializeField] List<RoomChunk> roomChunkULD; [SerializeField] List<RoomChunk> roomChunkRUL; [SerializeField] List<RoomChunk> roomChunkDRU; [SerializeField] List<RoomChunk> roomChunkLDR; [SerializeField] List<RoomChunk> roomChunkUDRL;

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

            switch (chunk.type)
            {
                case ChunkSelector.RoomType.Normal:
                    List<RoomChunk> chosenNormalArray = PickNormalRoomArray(chunk);
                    chunk.SetScriptable(chosenNormalArray);
                    break;
                case ChunkSelector.RoomType.Entry:
                    chunk.SetScriptable(entryRoomChunks);
                    break;
                case ChunkSelector.RoomType.Item:
                    chunk.SetScriptable(itemRoomChunks);
                    break;
                case ChunkSelector.RoomType.Boss:
                    chunk.SetScriptable(bossRoomChunks);
                    break;
                case ChunkSelector.RoomType.Shop:
                    chunk.SetScriptable(shopRoomChunks);
                    break;
            }
        }
    }

    List<RoomChunk> PickNormalRoomArray(ChunkSelector chunk)
    {
        if (chunk.up)
        {
            if (chunk.down)
            {
                if (chunk.right)
                {
                    if (chunk.left)
                    {
                        return roomChunkUDRL;
                    }
                    else
                    {
                        return roomChunkDRU;
                    }
                }
                else if (chunk.left)
                {
                    return roomChunkULD;
                }
                else
                {
                    return roomChunkUD;
                }
            }
            else
            {
                if (chunk.right)
                {
                    if (chunk.left)
                    {
                        return roomChunkRUL;
                    }
                    else
                    {
                        return roomChunkUR;
                    }
                }
                else if (chunk.left)
                {
                    return roomChunkUL;
                }
                else
                {
                    return roomChunkU;
                }
            }
        }
        if (chunk.down)
        {
            if (chunk.right)
            {
                if (chunk.left)
                {
                    return roomChunkLDR;
                }
                else
                {
                    return roomChunkDR;
                }
            }
            else if (chunk.left)
            {
                return roomChunkDL;
            }
            else
            {
                return roomChunkD;
            }
        }
        if (chunk.right)
        {
            if (chunk.left)
            {
                return roomChunkRL;
            }
            else
            {
                return roomChunkR;
            }
        }
        else
        {
            return roomChunkL;
        }
    }
}