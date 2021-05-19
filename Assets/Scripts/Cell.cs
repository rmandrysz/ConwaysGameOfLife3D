using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cell
{
    private bool alive = false;
    public int numberOfNeighbors;

    public void setAlive(bool alive) 
    {
        this.alive = alive;
    }

    public bool isAlive() 
    {
        return alive;
    }
}
