module DotsAndBoxes.AI

    open DotsAndBoxes.Util
    open DotsAndBoxes.Types
    open DotsAndBoxes.Game
    open DotsAndBoxes.Random

    let computerMove game : Rand<Move> = 
        let possibleMoves =
            allMovesForGame game |> 
            Seq.except game.moveList |> 
            Seq.toList
        Rand.ofList possibleMoves
    
    let runComputerMove game = computerMove game |> Rand.run
