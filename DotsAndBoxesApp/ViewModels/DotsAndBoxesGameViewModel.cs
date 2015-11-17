using System.Collections.Generic;
using System.Linq;
using Caliburn.Micro;
using DotsAndBoxes;
using Microsoft.FSharp.Collections;

namespace DotsAndBoxesApp.ViewModels
{
	class DotsAndBoxesGameViewModel : PropertyChangedBase
	{
		public DotsAndBoxesGameViewModel()
		{
			_currentGame = new Types.Game(4, 4, FSharpList<Types.Move>.Empty);
		}

		public DotsAndBoxesGameViewModel(Types.Game game)
		{
			_currentGame = game;
		}

		Types.Game _currentGame;
		const int DOT_SEPERATION = 30;
	
		public IEnumerable<DotViewModel> Dots
		{
			get
			{
				for (int y = 1; y <= _currentGame.dotsHeight; y++)
					for (int x = 1; x <= _currentGame.dotsHeight; x++)
					{
						yield return new DotViewModel { Top = y * DOT_SEPERATION, Left = x * DOT_SEPERATION };
					}
			}
		}

		public IEnumerable<MoveViewModel> Moves
		{
			get
			{
				var takenMoves = new HashSet<DotsAndBoxes.Types.Move>(_currentGame.moveList);
				return DotsAndBoxes.Game.allMovesForGame(_currentGame).Select(m => new MoveViewModel(m, takenMoves.Contains(m)));
			}
		}

		public Types.GameState State
		{
			get { return Game.statusForGame(_currentGame); }
		}

		public string ScoreLine
		{
			get
			{
				return string.Format("{0} - {1}",
					State.squaresWon.Where(s => s.winner.IsP1).Count(),
					State.squaresWon.Where(s => s.winner.IsP2).Count());
            }
		}

		public string GameOverMessage
		{
			get
			{
				return Game.isFinished(_currentGame) ? "Game Over" : null;
			}
		}
		
		public void MakeMove(MoveViewModel moveVm)
		{
			if (!moveVm.IsPlayed)
			{
				_currentGame = DotsAndBoxes.Game.makeMove(moveVm.Move, _currentGame);
			}
			NotifyOfPropertyChange("Moves");
			NotifyOfPropertyChange("State");
			NotifyOfPropertyChange("ScoreLine");
			NotifyOfPropertyChange("GameOverMessage");
		}
	}

	class DotViewModel
	{
		public int Top { get; set; }
		public int Left { get; set; }
	}

	class MoveViewModel
	{
		public MoveViewModel(DotsAndBoxes.Types.Move move, bool isPlayed)
		{
			Move = move;
			IsPlayed = isPlayed;
		}

		public DotsAndBoxes.Types.Move Move { get; private set; }
		public bool IsPlayed { get; private set; }
	}
}
