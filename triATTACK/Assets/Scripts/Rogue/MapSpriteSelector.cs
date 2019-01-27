using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapSpriteSelector : MonoBehaviour {
	
    public Sprite 	spU, spD, spR, spL,
			        spUD, spRL, spUR, spUL, spDR, spDL,
			        spULD, spRUL, spDRU, spLDR, spUDRL;

	public bool up, down, left, right;
	public int type; // 0: Normal, 1: Entry room, 2: Item room, 3: Boss room, 4: Shop room

    [SerializeField]
    Color normalColor, enterColor, itemColor, bossColor, shopColor;
	Color mainColor;
	SpriteRenderer rend;
    LevelGeneration levelGen;
    
	void Start ()
    {
        rend = GetComponent<SpriteRenderer>();
		mainColor = normalColor;
		PickSprite();
		PickColor();
        SpawnRoom();
    }

	void PickSprite() // Picks correct sprite based on the four door bools
    {
        if (up)
        {
			if (down)
            {
				if (right)
                {
					if (left)
                    {
						rend.sprite = spUDRL;
					}
                    else
                    {
						rend.sprite = spDRU;
					}
				}
                else if (left)
                {
					rend.sprite = spULD;
				}
                else
                {
					rend.sprite = spUD;
				}
			}
            else
            {
				if (right)
                {
					if (left)
                    {
						rend.sprite = spRUL;
					}
                    else
                    {
						rend.sprite = spUR;
					}
				}
                else if (left)
                {
					rend.sprite = spUL;
				}
                else
                {
					rend.sprite = spU;
				}
			}
			return;
		}
		if (down)
        {
			if (right)
            {
				if(left)
                {
					rend.sprite = spLDR;
				}
                else
                {
					rend.sprite = spDR;
				}
			}
            else if (left)
            {
				rend.sprite = spDL;
			}
            else
            {
				rend.sprite = spD;
			}
			return;
		}
		if (right)
        {
			if (left)
            {
				rend.sprite = spRL;
			}
            else
            {
				rend.sprite = spR;
			}
		}
        else
        {
			rend.sprite = spL;
		}
	}

	public void PickColor() //changes color based on what type the room is
    {
        switch (type)
        {
            case 0:
                mainColor = normalColor;
                break;
            case 1:
                mainColor = enterColor;
                break;
            case 2:
                mainColor = itemColor;
                break;
            case 3:
                mainColor = bossColor;
                break;
            case 4:
                mainColor = shopColor;
                break;
            default:
                Debug.LogError("Type " + type + "ain't it, chief.");
                break;
        }

        rend.color = mainColor;
	}

    void SpawnRoom()
    {
        // Check for rooms with types 1, 2, 3, 4 (entry, item, boss, shop)
        // For the entry room, check bools (need the branching mess) for which doors to spawn, then instantiate the entry room
        // For each other, check bools (no need for branching mess) for which doors to spawn
        // Instantiate the room for the given type, with the correct doors
        // For type 0s, check bools and determine what set of rooms can be used
        // Spawn the room at the position of the sprite

        switch (type)
        {
            case 0:

                break;
            case 1:

                break;
            case 2:

                break;
            case 3:

                break;
            case 4:

                break;
            default:
                Debug.LogError("Type " + type + "ain't it, chief.");
                break;
        }
    }
}