using Bones.Games;
using Bones.Grids;

namespace Bones.TicTacToe
{
    public class TicTacToe : IGame
    {
        private TicTacToeBoard _board;

        public TicTacToe(TicTacToeBoard board)
        {
            _board = board;
        }

        public void SetUpGame()
        {
            // Here we set up the phase map.
            // Setup game
            // Run game
            //     Player One/Two takes a turn
            //     Game Evaluates Win
            // if win
            //     resolve game
            //     restart
            // if not
            //     return to Run Game
            
        }
        
        public void PlayGame()
        {
            
        }
    }

    public enum TicTacToePlayer
    {
        None = 0,
        X = 1,
        O = 2
    }
    
    public class PlayerTurn : GamePhase
    {
        public override string Id
        {
            get { return "PlayerTurn"; }
        }
    }

    public class Evaluation : GamePhase
    {
        public override string Id
        {
            get { return "Evaluation"; }
        }
    }

    public class TicTacToeSpace : GridSpace
    {
        private TicTacToePlayer _owner = TicTacToePlayer.None;

        public void SetOwner(TicTacToePlayer owner)
        {
            _owner = owner;
        }
    }
}