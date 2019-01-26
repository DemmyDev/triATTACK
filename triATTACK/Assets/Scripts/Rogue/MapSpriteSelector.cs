﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapSpriteSelector : MonoBehaviour {
	
	public Sprite 	spU, spD, spR, spL,
			        spUD, spRL, spUR, spUL, spDR, spDL,
			        spULD, spRUL, spDRU, spLDR, spUDRL;

	public bool up, down, left, right;
	public int type; // 0: Normal, 1: First room, 2: Item room, 3: Boss room
	public Color normalColor, enterColor, itemColor, bossColor, shopColor;
	Color mainColor;
	SpriteRenderer rend;
    LevelGeneration levelGen;
    
	void Start ()
    {
        rend = GetComponent<SpriteRenderer>();
		mainColor = normalColor;
		PickSprite();
		PickColor();
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
		if (type == 0)
        {
			mainColor = normalColor;
		}
        else if (type == 1)
        {
			mainColor = enterColor;
		}
        else if (type == 2)
        {
            mainColor = itemColor;
        }
        else if (type == 3)
        {
            mainColor = bossColor;
        }
        else if (type == 4)
        {
            mainColor = shopColor;
        }
		rend.color = mainColor;
	}
}