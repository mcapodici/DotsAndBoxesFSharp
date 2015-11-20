// Contents of this file copied from https://gist.github.com/eulerfx/9808911
// No explicit license or copyright was stated when copied 2015-11-20.

module DotsAndBoxes.State

open DotsAndBoxes.Monoid

/// The state monad - a transition from a state, to a value and a new state.
type State<'s, 'a> = State of ('s -> 'a * 's)

/// The state monad.
module State =   

    let run s (state:State<'s, 'a>) = let (State(run)) = state in run s

    let eval (state:State<'s, 'a>) s = run s state |> fst

    let exec (state:State<'s, 'a>) s = run s state |> snd

    let unit a : State<'s, 'a> = State <| fun s -> (a,s)

    let get : State<'s, 's> = State(fun s -> (s,s))

    let set s : State<'s, unit> = State <| fun _ -> ((),s)

    let map f (state:State<'s, 'a>) : State<'s, 'b> = 
        State <| fun s -> 
            let (a,s1) = run s state
            (f a, s1)
    
    let bind f (state:State<'s, 'a>) : State<'s, 'b> =
        State <| fun s ->
            let (a,s1) = run s state
            run s1 (f a)
    
    let map2 (s1:State<'s, 'a>) (s2:State<'s, 'b>) (f:'a -> 'b -> 'c) : State<'s, 'c> =
        bind (fun a -> map (fun b -> f a b) s2) s1

    let sequenceList (ss:State<'s, 'a> list) : State<'s, 'a list> =
        List.foldBack (fun s acc -> map2 s acc (fun x xs -> x::xs)) ss (unit (List.empty))

    let sequenceSeq (ss:State<'s, 'a> seq) : State<'s, 'a seq> =
        State <| fun s ->            
            let s = ref s
            let ss = 
                ss 
                |> Seq.map (fun x ->                             
                    let (a,s') = x |> run !s
                    s := s'
                    a
                )
                |> Seq.toArray
            (ss |> Seq.ofArray,!s)

    let concatSeq (ss:seq<State<'s, 'a>>) (M:Monoid<'a>) : State<'s, 'a> = ss |> sequenceSeq |> map (Seq.fold M.op M.unit)

    type StateBuilder() =
        member x.Bind(s, f) = bind f s
        member x.Return(value) = unit value
        member x.Yield(value) = unit value
        member x.ReturnFrom(value) = value
        member x.Zero() = unit()
        member x.Combine(s1:State<'S,unit>, s2:State<'S,'a>) = map2 s1 s2 (fun _ s -> s)
        member x.For(xs:seq<'a>, f:'a -> State<'S, 'a>) = xs |> Seq.map f


[<AutoOpen>]
module StateBuilder =

    /// State monad workflow builder.
    let state = new State.StateBuilder()

