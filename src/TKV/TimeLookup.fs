namespace TKV

module TimeLookup =
    
    let arraySearchIndex (t: 't) (ts: 't array) =
        let n = System.Array.BinarySearch (ts, t)
        match n with
        | n when n >= 0 -> Some n
        | n when n < 0 ->
            let length = ts.Length
            match ~~~n with
            | m when m = 0 -> None
            | m when m > 0 ->
                match m with
                | m when m < length -> Some (m-1)
                | m when m >= length -> Some (length-1)
                | _ -> Some 0
            | _ -> Some 0
        | _ -> Some 0
    let arraySearch (t: 't) (ts: 't array) = 
        Option.map (fun x -> ts.[x]) (arraySearchIndex t ts)
    let New (ts: 't list) =
        let storage =  new System.Collections.Generic.SortedSet<'t>(ts)
        {new ITimeLookup<'t> with
            member this.Add (x: 't) = storage.Add x |> ignore
            member this.FindAsOf (x: 't) =
                System.Linq.Enumerable.ToArray storage
                |> (arraySearch x)
            member this.FindAsOfIndex (x: 't) =
                System.Linq.Enumerable.ToArray storage
                |> (arraySearchIndex x)
        }


