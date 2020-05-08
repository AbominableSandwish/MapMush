using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cell
{
    public Vector3 position;
    public bool visited = false;

    public Cell came_from;

    private int type; // Element (int): 0 = empty; 1 = wall; 2 = ground
    private int texture;

    public SpriteRenderer render;

    public GameObject Object;

    public void SetSpriteRender(SpriteRenderer render)
    {
        this.render = render;
    }

    public Cell(int x, int y)
    {
        this.position = new Vector3(x, y, 0);
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

    public void GetObject(GameObject Object)
    {

        this.Object = Object;
    }

    public GameObject GetObject()
    {
        return this.Object;
    }
}
