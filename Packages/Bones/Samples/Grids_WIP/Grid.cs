using System.Collections.Generic;
using UnityEngine;

namespace Bones.Grids
{
    public class Grid<T> where T : GridSpace, new()
    {
        private int _width;
        private int _height;

        private List<T> _gridSpaces;

        public Grid(int width, int height)
        {
            _width = width;
            _height = height;

            _gridSpaces = new List<T>();
            
            for (int y = 0; y < _width; y++)
            {
                for (int x = 0; x < _height; x++)
                {
                    T space = new T();
                    space.SetColumn(x);
                    space.SetRow(y);
                    
                    _gridSpaces.Add(space);
                }
            }

            foreach (T thing in _gridSpaces)
            {
                Debug.Log("Space");
                Debug.Log(thing.Row.ToString() + ":" + thing.Column.ToString());
            }
        }
    }

    public class GridSpace
    {
        private int _column;
        public int Column
        {
            get { return _column; }
        }

        private int _row;
        public int Row
        {
            get { return _row; }
        }

        public void SetColumn(int column)
        {
            _column = column;
        }
        
        public void SetRow(int row)
        {
            _row = row;
        }

        public GridSpace()
        {
        }
    }
}