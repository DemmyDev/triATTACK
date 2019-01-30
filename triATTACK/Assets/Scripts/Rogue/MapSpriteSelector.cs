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

        switch (type)
        {
            case 0:
                NormalRoom();
                break;
            case 1:
                EntryRoom();
                break;
            case 2:
                ItemRoom();
                break;
            case 3:
                BossRoom();
                break;
            case 4:
                ShopRoom();
                break;
        }
    }

    void NormalRoom()
    {
        // Branching if-else statements for door bools (15 total)
            // Spawn wall chunk with correct door bools
            // Spawn random chunk from list based on bools
        
    }

    void EntryRoom()
    {
        // Branching if-else statements for door bools (15 total)
            // Spawn wall chunk with correct door bools
            // Spawn empty room chunk
    }

    void ItemRoom()
    {
        if (up)
        {
            // Instantiate item room with up door
        }
        else if (down)
        {
            // Instantiate item room with down door
        }
        else if (left)
        {
            // Instantiate item room with left door
        }
        else if (right)
        {
            // Instantiate item room with right door
        }
    }

    void BossRoom()
    {
        if (up)
        {
            // Instantiate boss room with up door
        }
        else if (down)
        {
            // Instantiate boss room with down door
        }
        else if (left)
        {
            // Instantiate boss room with left door
        }
        else if (right)
        {
            // Instantiate boss room with right door
        }
    }

    void ShopRoom()
    {
        if (up)
        {
            // Instantiate shop room with up door
        }
        else if (down)
        {
            // Instantiate shop room with down door
        }
        else if (left)
        {
            // Instantiate shop room with left door
        }
        else if (right)
        {
            // Instantiate shop room with right door
        }
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
}