module DotsAndBoxes.AI

    open DotsAndBoxes.Util
    open DotsAndBoxes.Types
    open DotsAndBoxes.Game

    let computerMove (game : Game) : Move = 
        let rnd = System.Random()
        let possibleMoves = Seq.toList (Seq.except game.moveList (allMovesForGame game))
        possibleMoves.Item(rnd.Next(Seq.length possibleMoves))