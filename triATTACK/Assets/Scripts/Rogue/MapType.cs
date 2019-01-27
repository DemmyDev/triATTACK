using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapType : MonoBehaviour {

    GameObject[] mapObjs;
    LevelGeneration levelGen;
    bool itemRoomIsSet = false, bossRoomIsSet = false, shopRoomIsSet = false;

    void Start()
    {
        levelGen = GameObject.Find("Level Generator").GetComponent<LevelGeneration>();
        GetMapSprites();
        SetItemRoom();
        SetBossRoom();
        SetShopRoom();
    }

    void GetMapSprites()
    {
        mapObjs = GameObject.FindGameObjectsWithTag("MapSprite");
        Debug.Log(mapObjs.Length);
    }

    void SetItemRoom()
    {
        do
        {
            int rand = Random.Range(0, levelGen.numberOfRooms);
            MapSpriteSelector spr = mapObjs[rand].GetComponent<MapSpriteSelector>();

            if (spr.type == 0)
            {
                SpriteRenderer rend = spr.GetComponent<SpriteRenderer>();

                if (rend.sprite == spr.spD || rend.sprite == spr.spU || rend.sprite == spr.spR || rend.sprite == spr.spL)
                {
                    Debug.Log("Item room!!!");
                    spr.type = 2;
                    itemRoomIsSet = true;
                    spr.PickColor();
                }
            }
        } while (!itemRoomIsSet);
    }

    void SetBossRoom()
    {
        do
        {
            int rand = Random.Range(0, levelGen.numberOfRooms);
            MapSpriteSelector spr = mapObjs[rand].GetComponent<MapSpriteSelector>();

            if (spr.type == 0)
            {
                SpriteRenderer rend = spr.GetComponent<SpriteRenderer>();

                if (rend.sprite == spr.spD || rend.sprite == spr.spU || rend.sprite == spr.spR || rend.sprite == spr.spL)
                {
                    Debug.Log("Boss room!!!");
                    spr.type = 3;
                    bossRoomIsSet = true;
                    spr.PickColor();
                }
            }
        } while (!bossRoomIsSet);
    }

    void SetShopRoom()
    {
        do
        {
            int rand = Random.Range(0, levelGen.numberOfRooms);
            MapSpriteSelector spr = mapObjs[rand].GetComponent<MapSpriteSelector>();

            if (spr.type == 0)
            {
                SpriteRenderer rend = spr.GetComponent<SpriteRenderer>();

                if (rend.sprite == spr.spD || rend.sprite == spr.spU || rend.sprite == spr.spR || rend.sprite == spr.spL)
                {
                    Debug.Log("Shop room!!!");
                    spr.type = 4;
                    shopRoomIsSet = true;
                    spr.PickColor();
                }
            }
        } while (!shopRoomIsSet);
    }
}