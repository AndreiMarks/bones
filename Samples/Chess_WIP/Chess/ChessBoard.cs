namespace Bones.Chess
{
    public class ChessBoard
    {
        private Grids.Grid<ChessSpace> _grid;
        
        public ChessBoard()
        {
            _grid = new Grids.Grid<ChessSpace>(width: 8, height: 8);
        }
    }

    public class ChessSpace : Grids.GridSpace
    {
        /// <summary>
        /// Indicates the color of the space, but doesn't actually impact gameplay.
        /// </summary>
        private ChessColor _chessColor;

        public ChessSpace()
        {
        }

        public ChessSpace(ChessColor chessColor)
        {
            _chessColor = chessColor;
        }
    }

    public enum ChessColor
    {
        White = 0,
        Black = 1
    }
}