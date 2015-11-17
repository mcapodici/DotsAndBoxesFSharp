module DotsAndBoxes.Types

    type Orientation = Horiz | Vert
 
    // Moves - x and y are 0-indexed coordinates of a dot from which the line made to the right (for horiz) and down (for vert)
    // Squares - x and y are 0-indexed
    //
    // .-.
    // | |
    // .-.
    type Move = { x : int; y : int; o : Orientation }
    type Player = P1 | P2
    type Result = Win of Player | Draw
    type Status = Turn of Player | Complete of Result 
    type Square =  { x : int; y : int }
    type WonSquare = { square : Square; winner:Player}
    type Game = { dotsHeight : int; dotsWidth: int; moveList: Move list }
    type GameState = { currentPlayer : Player; squaresWon : Set<WonSquare> }
    

 