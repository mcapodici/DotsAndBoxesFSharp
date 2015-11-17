module DotsAndBoxes.Util

module List =
   let rec skip n xs = 
      match (n, xs) with
      | 0, _ -> xs
      | _, [] -> []
      | n, _::xs -> skip (n-1) xs
      
   let rec take n xs = 
      match (n, xs) with
      | 0, _ -> []
      | _, [] -> []
      | n, x::xs -> x :: take (n-1) xs
      
   let toTupleR rhs = Seq.map (fun v -> (v, rhs))
  
module Set =
   let addSeq set = Seq.fold (fun s v -> Set.add v s) set

module Seq =
   let loop xs f = Seq.map f xs
   let loop2 xs ys f = Seq.map2 f xs ys
   let loop3 xs ys zs f = Seq.map3 f xs ys zs
   let loopInLoop xs ys f = Seq.concat (loop xs (fun x -> loop ys (fun y -> f x y)))