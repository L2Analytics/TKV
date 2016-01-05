namespace TKV

module AsOf =
    
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

    type Basic<'t> (ts: 't list) =
        let storage =  new System.Collections.Generic.SortedSet<'t>(ts)
        interface IAsOf<'t> with
            member this.Add (x: 't) = storage.Add x |> ignore
            member this.TryFind (x: 't) =
                System.Linq.Enumerable.ToArray storage
                |> (arraySearch x)
    let New (ts: 't list) :IAsOf<'t> = new Basic<'t>(ts) :> IAsOf<'t>

