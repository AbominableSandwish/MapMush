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

        public int _texture;

        public Transform obj;

        enum Type
        {
            TILE,
            DECORATION
        }

        public Graphic(BufferGraphic buffer, Cell parent, int texture) : base(parent)
        {
        
            this.buffer = buffer;
            
            _texture = texture;
           
            Vector2 resultPos =
                    convertTileCoordInScreenCoord((int)parent.position.x, (int) (parent.position.y)) +
                    new Vector2(0, _parent.position.z);
                Vector3 ScreenPos = new Vector3(resultPos.x, resultPos.y);
                obj = buffer.AddGameObject(ScreenPos, _texture, parent.sortingLayer);


        }

        public override void Init()
        {

        }

        public override void Update(float gameTime)
        {

        }

        public override void Remove()
        {
            this._texture = 0;

            Vector2 resultPos = Vector2.zero;
            buffer.RemoveObjectGraphic(obj);
            this.buffer = null;
            obj = null;
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

    public class Plant : CellComponent
    {
        public Color _color;
        public Cell _parent;
        public Graphic _render;

        public Plant(Cell parent, Vector2 position, Vector2 dimension, Color color, Rect rect, Texture2D texture) : base(parent)
        {
            _parent = parent;
            _color = color;
            
           // _render = new Graphic(_parent, new ObjectTransform(position, dimension), rect, texture);
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
