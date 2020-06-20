using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace MapGame
{
    

    [System.Serializable]
    public class Cell
    {
        public static Vector3 convertTileCoordInScreenCoord(Vector3 position)
        {
            Vector3 screenCoord = new Vector3();
            screenCoord.x = (float)(((position.x - position.y)));
            screenCoord.y = (float)(((position.x + position.y) * (0.5f)));
            return screenCoord;
        }

        public List<CellComponent> _components = new List<CellComponent>();
        public Map _parent;
        public Vector3 position;
        public bool visited = false;
        public float height;

        public bool _isVisible;
        public Cell came_from;

        private int type; // Element (int): 0 = empty; 1 = wall; 2 = ground
        private int texture;

        public SpriteRenderer render;

        public GameObject Object;

        public void SetSpriteRender(SpriteRenderer render)
        {
            this.render = render;
        }

        public void SetIsVisible(bool isVisible)
        {
            if (isVisible == true)
            {
                if (isVisible = !this._isVisible)
                {
                    if (_components != null)
                    {
                        foreach (var component in _components)
                        {
                            if (component.GetType() == typeof(Graphic))
                            {
                                GameObject render = component.Draw();
                            }

                            //if (component.GetType() == typeof(Plant))
                            //{
                            //    GameObject plant = component.Draw();
                            //    plant.transform.position =
                            //        convertTileCoordInScreenCoord(position) + new Vector3(-0.1f, position.z + 0.7f);

                            //}
                        }
                    }

                    this._isVisible = true;
                }
            }
            else
            {
                if (_components != null)
                {
                    foreach (var component in _components)
                    {
                        if (component.GetType() == typeof(Plant))
                        {
                            component.Clean();
                        }

                        if (component.GetType() == typeof(Graphic))
                        {
                            component.Clean();
                        }
                    }
                }
                this._isVisible = false;
            }
        }

        public Cell(Map parent, int x, int y)
        {
            _parent = parent;
            this.position = new Vector3(x, y, 0);
            if (_components == null)
            {
                _components = new List<CellComponent>();
            }
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
                Object.SetActive(this._isVisible);
            }

            this.Object = Object;
        }

        public GameObject GetObject()
        {
            return this.Object;
        }

        //NEW TODO
        

        public T GetComponent<T>() where T : CellComponent
        {
            foreach (CellComponent c in _components)
                if (c.GetType().Equals(typeof(T)))
                    return (T) c;
            return null;
        }

        public void AddComponent(CellComponent component)
        {
            if (_components == null)
            {
                _components = new List<CellComponent>();
            }

            _components.Add(component);
            component._parent = this;
            component.Init();
            Instantiate(component);
        }

        public virtual void Update(float gameTime)
        {
            foreach (CellComponent c in _components)
                c.Update(gameTime);
        }

        public static void Instantiate(CellComponent component)
        {
             Map._AddedC.Add(component);
        }

        public static void Destroy(CellComponent component)
        {
            Map._RemoveC.Add(component);
        }
    }
}
