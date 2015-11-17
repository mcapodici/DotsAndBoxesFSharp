module DotsAndBoxes.Game

    open DotsAndBoxes.Util
    open DotsAndBoxes.Types

    let switchTurn player =
        match player with
            | P1 -> P2
            | P2 -> P1

    let squaresForMove move = 
        match move.o with
            | Horiz -> { Square.x = move.x; y=move.y } :: (if move.y > 0 then [{ Square.x = move.x; y=move.y-1 }] else [])
            | Vert  -> { Square.x = move.x; y=move.y } :: (if move.x > 0 then [{ Square.x = move.x-1; y=move.y }] else [])
    
    let movesForSquare {Square.x = x; y = y} = 
        [
            {x = x;   y=y;   o=Horiz};
            {x = x;   y=y;   o=Vert};
            {x = x;   y=y+1; o=Horiz};
            {x = x+1; y=y;   o=Vert}
        ]

    let squaresCompletedFor move moves =
        let squares = squaresForMove move
        let squareIsComplete square = Set.isEmpty (Set.difference (Set.ofList (movesForSquare square)) (Set.ofList (move :: moves)))
        Seq.toList (Seq.where squareIsComplete squares) 

    let makeMove move game = 
        { game with 
            moveList = game.moveList @ [move]; 
        }

    let isFinished { dotsHeight = h; dotsWidth = w; moveList = moves; } =   
        let totalMoves = (h - 1) * w + h * (w - 1)
        (Seq.length moves) >= totalMoves

    let statusForGame { dotsHeight = h; dotsWidth = w; moveList = moves; } =
        Seq.fold
            (fun ({currentPlayer = cp; squaresWon = squaresWonSoFar}, moves) move ->
                let squaresCompleted = squaresCompletedFor move moves
                ({
                    currentPlayer = if not (Seq.isEmpty squaresCompleted) then cp else switchTurn cp;
                    squaresWon =  Set.addSeq squaresWonSoFar (Seq.map (fun s -> { square=s; winner=cp }) squaresCompleted)
                }, move::moves))
            ({ currentPlayer = P1; squaresWon = Set.empty }, [])
            moves
            |> fst

    let allMovesForGame { Game.dotsHeight = h; dotsWidth = w } =
        Seq.append
            (Seq.loopInLoop
                [1 .. w-1]
                [1 .. h]
                (fun x y -> 
                    { Move.x = x; y = y; o = Horiz }
                )) 
            (Seq.loopInLoop
                [1 .. w]
                [1 .. h-1]
                (fun x y -> 
                    { Move.x = x; y = y; o = Vert }
                ))

//    let exampleStart = { dotsHeight = 5; dotsWidth = 5; moveList = [] }
//    let exampleGame = 
//        Seq.fold 
//            (fun g m -> makeMove m g) 
//            exampleStart 
//            [
//                { x = 0; y = 0; o = Horiz };
//                { x = 0; y = 0; o = Vert };
//                { x = 0; y = 1; o = Horiz };
//                { x = 1; y = 0; o = Vert };
//                { x = 1; y = 0; o = Horiz };
//                { x = 1; y = 1; o = Horiz };
//                { x = 3; y = 3; o = Vert };
//                { x = 2; y = 0; o = Vert };
//            ]

// TODO
// Create a WPF control for displaying dots and boxes
// VM for control will handle mouseover and check with fs if move is allowed and current player is UI player
// On click, if move is allowed and current player is UI player then calls fs to get new game state and then sets that as CurrentGame property
// and fires property changed for Current Game and Current Game State
// Current player and won boxes is shown and is from bound CurrentGameState 
// This will mean we have UI to play dots and boxes with 2 human players then work on AI



