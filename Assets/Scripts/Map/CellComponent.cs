using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;
using Random = UnityEngine.Random;

namespace MapGame
{
    [System.Serializable]
    public class CellComponent
    {
        public Cell _parent;
        public Graphic _render;

        public CellComponent(Cell parent)
        {
            _parent = parent;
            //_render = new Graphic(_parent);
        }

        public virtual void Init()
        {

        }

        public virtual void Update(float gameTime)
        {

        }

        public virtual GameObject Draw()
        {
            return null;
        }

        public virtual void Clean()
        {

        }
    }

    class Character : CellComponent
    {
        private string _name;
        private string _description;
        private int _life;
        private int _pm;

        public Character(Cell parent, String name, int life, int pm) : base(parent)
        {
            _name = name;
            _life = life;
            _pm = pm;
        }

        public override void Init()
        {

        }

        public override void Update(float gameTime)
        {

        }

    }

    class IsMoving : CellComponent
    {
        private IAController iaController;

        public IsMoving(Cell parent) : base(parent) 
        {

        }

        public override void Init()
        {

        }

        public override void Update(float gameTime)
        {

        }

    }

    public class ObjectTransform
    {
        public Vector2 _position;
        public Vector2 _dimension;

        public ObjectTransform(Vector2 position, Vector2 dimension = new Vector2())
        {
            _position = position;
            _dimension = dimension;
        }

        public void SetPosition(Vector2 position)
        {
            _position = position;
        }
    }

    public class Graphic : CellComponent
    {
        private Texture2D _texture;
        private int _sortingLayer;

        public ObjectTransform _transform;
        public GameObject _render;

        public Rect _rect;

        enum Type
        {
            TILE,
            DECORATION
        }

        public Graphic(Cell parent, ObjectTransform transform , Rect rect = new Rect(), Texture2D texture = null) : base(parent)
        {
            _texture = texture;
            _rect = rect;
            _transform = transform;

        }

        public override void Init()
        {

        }

        public override void Update(float gameTime)
        {

        }

        public override GameObject Draw()
        {
            _render = new GameObject("GameObject");
            _render.AddComponent<SpriteRenderer>();
            _render.transform.localScale= new Vector3(2.5f,2.5f,1);
            _render.GetComponent<SpriteRenderer>().sprite = Sprite.Create(_texture, _rect, Vector2.zero);
            _render.GetComponent<SpriteRenderer>().sortingOrder = (int)((this._parent._parent.height - _parent.position.y) + (this._parent._parent.width - _parent.position.x)) * 5 + 1;
            _render.transform.position = convertTileCoordInScreenCoord((int)_transform._position.x, (int)(_transform._position.y)) + new Vector2(0, _parent.position.z);
            return _render;
        }

        public override void Clean()
        {
            MapManager.Destroy(_render);
        }

        private static Vector2 convertTileCoordInScreenCoord(int tileCoordX, int tileCoordY)
        {
            Vector2 screenCoord;
            screenCoord.x = (float)(((tileCoordX - tileCoordY)));
            screenCoord.y = (float)(((tileCoordX + tileCoordY) * (0.5f)));
            return screenCoord;
        }



    }

    class IsBlocking : CellComponent
    {
        private IAController iaController;

        public IsBlocking(Cell parent) : base(parent)
        {

        }

        public override void Init()
        {

        }

        public override void Update(float gameTime)
        {

        }

    }

    class Pnj : CellComponent
    {
        private Character _character;
       

        public Pnj(Cell parent, Character character) : base(parent)
        {
            _character = character;
        }

        public override void Init()
        {
         
        }

        public override void Update(float gameTime)
        {

        }

    }

    public class Plant : CellComponent
    {
        public Color _color;
        public Cell _parent;
        public Graphic _render;

        public Plant(Cell parent, Vector2 position, Vector2 dimension, Color color, Rect rect, Texture2D texture) : base(parent)
        {
            _parent = parent;
            _color = color;
            
            _render = new Graphic(_parent, new ObjectTransform(position, dimension), rect, texture);
        }

        public void SetColor(Color color)
        {
            _color = color;
        }

        public override GameObject Draw()
        {
            GameObject plant = _render.Draw();
            return plant;
        }

        public override void Clean()
        {
            _render.Clean();
        }
    }
}
