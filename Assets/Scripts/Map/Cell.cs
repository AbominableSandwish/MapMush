using System.Collections;
using System.Collections.Generic;
using UnityEngine;

class Cell
{
    private Vector2 position;
    private bool visited = false;

    //var came_from

    private int type; // Element (int): 0 = empty; 1 = wall; 2 = ground
    private int texture;

    public int contain;

    public Cell(int x, int y)
    {
        this.position = new Vector2(x, y);
    }

    public void set_texture(int texture)
    {
        this.texture = texture;
    }

    int get_texture()
    {
        return this.texture;
    }

    public void set_type(int type)
    {
        this.type = type;
    }

    public int get_type()
    {
        return this.type;
    }

    void set_contain(int contain)
    {

        this.contain = contain;
    }

    int get_object()
    {
        return this.contain;
    }
}
