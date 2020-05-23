using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cell
{
    public Vector3 position;
    public bool visited = false;

    public bool IsVisible;
    public Cell came_from;

    private int type; // Element (int): 0 = empty; 1 = wall; 2 = ground
    private int texture;

    public SpriteRenderer render;

    public GameObject Object;

    public SpriteRenderer Decoration;

    public void SetDecoration(SpriteRenderer deco)
    {
        this.Decoration = deco;
    }

    public void SetSpriteRender(SpriteRenderer render)
    {
        this.render = render;
    }

    public void SetIsVisible(bool isVisible)
    {
        this.IsVisible = isVisible;
       render.gameObject.SetActive(isVisible);
        if(Object != null)
            Object.SetActive(isVisible);
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

    public void SetObject(GameObject Object)
    {
        if (Object != null)
        {
            Object.SetActive(this.IsVisible);
        }

        this.Object = Object;
    }

    public GameObject GetObject()
    {
        return this.Object;
    }
}
