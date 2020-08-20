using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using UnityEditor.SceneManagement;
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

        public virtual void Remove()
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
        //private IAController iaController;

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
        private bool isDynamic = false;
        private BufferGraphic buffer;

        public List<Transform> objs;

        enum Type
        {
            TILE,
            DECORATION
        }

        public Graphic(BufferGraphic buffer, Cell parent) : base(parent)
        {   
            this.buffer = buffer;
            this._parent = parent;
            this.objs = new List<Transform>();

            AddObject(BufferGraphic.TypeTexture.Ground, this._parent.get_type());               
        }

        public void AddObject(BufferGraphic.TypeTexture type, int texture)
        {

            Vector2 resultPos =
                convertTileCoordInScreenCoord((int)this._parent.position.x, (int)(this._parent.position.y)) +
                new Vector2(0, this._parent.position.z);
            Vector3 ScreenPos = new Vector3(resultPos.x, resultPos.y);
            objs.Add(buffer.AddGameObject(ScreenPos, type, texture, this._parent.sortingLayer));
        }

        public override void Init()
        {

        }

        public override void Update(float gameTime)
        {

        }

        public override void Remove()
        {
            Vector2 resultPos = Vector2.zero;

            foreach (var obj in objs)
            {
                buffer.RemoveObjectGraphic(obj);
            }
            objs = null;
            this.buffer = null;
          
        }

        public override GameObject Draw()
        {
            if(!isDynamic)
                return null;
            return null;
        }

        public override void Clean()
        {
         
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
        //private IAController iaController;

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

    public class MTree : CellComponent
    {
        public int type;
        public MTree(Cell parent, int type) : base(parent)
        {
            this.type = type;
        }
    }

    //public class Plant : CellComponent
    //{
    //    public Color _color;
    //    public Cell _parent;
    //    public Graphic _render;

    //    public Plant(Cell parent, Vector2 position, Vector2 dimension, Color color, Rect rect, Texture2D texture) : base(parent)
    //    {
    //        _parent = parent;
    //        _color = color;
            
    //       // _render = new Graphic(_parent, new ObjectTransform(position, dimension), rect, texture);
    //    }

    //    public void SetColor(Color color)
    //    {
    //        _color = color;
    //    }

    //    public override GameObject Draw()
    //    {
    //        GameObject plant = _render.Draw();
    //        return plant;
    //    }

    //    public override void Clean()
    //    {
    //        _render.Clean();
    //    }
    //}
}
