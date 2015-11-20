// Contents of this file copied from https://gist.github.com/eulerfx/9808911
// No explicit license or copyright was stated when copied 2015-11-20.

module DotsAndBoxes.Random

open DotsAndBoxes.State
open DotsAndBoxes.Monoid

/// A random value represented as a transition from an RNG to a random value and the next state of the RNG.
type Rand<'A> = State<System.Random, 'A>

/// RNG combinators.
module Rand =
    
    open System

    let eval (rand:Rand<'a>) (rng:Random) = State.eval rand rng

    let run (rand:Rand<'a>) : 'a = State.eval rand (new System.Random())

    let runSeed (seed:int) (rand:Rand<'a>) : 'a = State.eval rand (new System.Random(seed))

    let unit a : Rand<'a> = State.unit a

    let product (r1:Rand<'a>) (r2:Rand<'b>) : Rand<'a * 'b> = State.map2 r1 r2 (fun a b -> (a,b))

    let product3 (r1:Rand<'a>) (r2:Rand<'b>) : Rand<'a * 'b> =
        r1 |> State.bind (fun r1 -> r2 |> State.map (fun r2 -> (r1,r2)))

    let Int : Rand<int> = State(fun (rng:Random) -> (rng.Next(),rng))
        
    let IntRange min max : Rand<int> = State(fun (rng:Random) -> (rng.Next(min,max + 1),rng))

    let Bool : Rand<bool> = IntRange 0 1 |> State.map (function 0 -> false | _ -> true)

    let Float : Rand<float> = State(fun (rng:Random) -> (rng.NextDouble(),rng))

    let Long : Rand<int64> = Float |> State.map int64

    let seqOfi count (rand:int -> Rand<'a>) : Rand<seq<'a>> = 
        Seq.init count rand |> State.sequenceSeq

    let listOf count (rand:Rand<'a>) : Rand<'a list> = 
        List.init count (fun _ -> rand) |> State.sequenceList

    let listOfRand (count:Rand<int>) (rand:Rand<'a>) : Rand<'a list> = 
        count |> State.bind (fun count -> List.init count (fun _ -> rand) |> State.sequenceList)

    let ofList (list:list<'a>) : Rand<'a> = 
        IntRange 0 ((list |> List.length) - 1) |> State.map (List.nth list)

    let ofArray (array:'a[]) : Rand<'a> = 
        IntRange 0 (array |> Array.length) |> State.map (Array.get array)

    let choice (r1:Rand<'a>) (r2:Rand<'a>) : Rand<'a> = 
        Bool |> State.bind (fun b -> if b then r1 else r2)

    let choice3 (r1:Rand<'a>) (r2:Rand<'a>) (r3:Rand<'a>) : Rand<'a> = 
        IntRange 1 3 |> State.bind (function 1 -> r1 | 2 -> r2 | _ -> r3)    

    let FloatRange min max : Rand<float> =         
        if (min >= max) then invalidArg "min" "min must be less than max"
        let range =
            if min >= 0.0 && max <= 1.0 then id
            else 
                let delta = max - min
                fun dbl -> (dbl * delta) + min        
        Float |> State.map range

    let LongRange (min:int64) (max:int64) : Rand<int64> = 
        FloatRange (float min) (float max) |> State.map int64

    let DecimalRange (min:decimal) (max:decimal) : Rand<decimal> = 
        FloatRange (float min) (float max) |> State.map decimal

    let DecimalRangeRound min max (decimals:int) : Rand<decimal> = 
        DecimalRange min max |> State.map (fun d -> Decimal.Round(d, decimals))

    let Ints count : Rand<list<int>> = 
        Int |> listOf count

    let CharRange (min:char) (max:char) : Rand<char> = 
        IntRange (int min) (int max) |> State.map char

    let CharAsciiNonControl : Rand<char> = 
        CharRange (char 32) (char 126)

    let CharAsciiNumeric : Rand<char> = CharRange (char 48) (char 57)

    let CharAsciiAlphaUpper : Rand<char> = CharRange (char 65) (char 90)

    let CharAsciiAlphaLower : Rand<char> = CharRange (char 97) (char 122)

    let CharAsciiAlpha : Rand<char> = choice CharAsciiAlphaUpper CharAsciiAlphaLower

    let CharAsciiAlphanumeric : Rand<char> = choice CharAsciiAlpha CharAsciiNumeric

    let CharAsciiSpace : Rand<char> = unit (char 20)

    let StringOfChar (length:Rand<int>) (c:Rand<char>) : Rand<string> = 
        length |> State.bind (fun length -> c |> listOf length |> State.map (fun cs -> new String(cs |> List.toArray)))

    let String (length:Rand<int>) : Rand<string> = StringOfChar length CharAsciiAlphanumeric

    let StringList count min max : Rand<string list> = 
        (String (IntRange min max)) |> listOf count

    let StringConcat (strs:seq<Rand<string>>) : Rand<string> = State.concatSeq strs Monoid.stringConcat

    let DateTime (min:DateTime) (max:DateTime) : Rand<DateTime> =
        LongRange (min.Ticks) (max.Ticks) |> State.map (fun ticks -> DateTime(ticks,DateTimeKind.Utc))


[<AutoOpen>]
module RandBuilder =

    /// RNG workflow builder.
    let rand = new State.StateBuilder()