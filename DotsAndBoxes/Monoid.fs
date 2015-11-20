// Contents of this file copied from https://gist.github.com/eulerfx/9808911
// No explicit license or copyright was stated when copied 2015-11-20.

module DotsAndBoxes.Monoid

type Monoid<'a> = {
    unit : 'a
    op   : 'a -> 'a -> 'a
}        

module Monoid =

    let product (MA:Monoid<'a>) (MB:Monoid<'b>) : Monoid<'a * 'b> =
        { unit = MA.unit,MB.unit
          op   = fun (a1,b1) (a2,b2) -> (MA.op a1 a2),(MB.op b1 b2) }
    
    let min unit = { unit = unit ; op = min }

    let max unit = { unit = unit ; op = max }

    let minInt = min (System.Int32.MaxValue)

    let maxInt = max (System.Int32.MinValue)

    let boolAnd = 
        { unit = true
          op   = (&&) }
    
    let boolOr = 
        { unit = false
          op   = (||) }

    let stringConcat =
        { unit = System.String.Empty 
          op   = (+) }

