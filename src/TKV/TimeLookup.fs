namespace TKV

module TimeLookup =
    
    let arraySearch (t: 't) (ts: 't array) =
        let n = System.Array.BinarySearch (ts, t)
        match n with
        | n when n >= 0 -> Some ts.[n]
        | n when n < 0 ->
            let length = ts.Length
            match ~~~n with
            | m when m = 0 -> None
            | m when m > 0 ->
                match m with
                | m when m < length -> Some ts.[m-1]
                | m when m >= length -> Some ts.[length-1]
                | _ -> Some ts.[0]
            | _ -> Some ts.[0]
        | _ -> Some ts.[0]

    let New (ts: 't list) =
        let storage =  new System.Collections.Generic.SortedSet<'t>(ts)
        {new ITimeLookup<'t> with
            member this.Add (x: 't) = storage.Add x |> ignore
            member this.FindAsOf (x: 't) =
                System.Linq.Enumerable.ToArray storage
                |> (arraySearch x)
        }


