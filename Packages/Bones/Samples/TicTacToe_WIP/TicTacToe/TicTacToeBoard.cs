using Grids = Bones.Grids;
using UnityEngine;

namespace Bones.TicTacToe
{
    public class TicTacToeBoard
    {
        private Grids.Grid<TicTacToeSpace> _grid;
        
        public TicTacToeBoard()
        {
            _grid = new Grids.Grid<TicTacToeSpace>(3, 3);
        }
    }
}