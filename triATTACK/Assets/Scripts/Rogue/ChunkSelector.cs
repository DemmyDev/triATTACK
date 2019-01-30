using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChunkSelector : MonoBehaviour
{
    [SerializeField]
    Transform wallU, wallD, wallR, wallL,
               wallUD, wallRL, wallUR, wallUL, wallDR, wallDL,
               wallULD, wallRUL, wallDRU, wallLDR, wallUDRL;

    public bool up, down, left, right;
    public int type;

    // Start is called before the first frame update
    void Start()
    {
        Transform wall = PickWall();
        var inst = Instantiate(wall, gameObject.transform.position, gameObject.transform.rotation);
        inst.transform.parent = gameObject.transform;
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
}
