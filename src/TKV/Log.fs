namespace TKV

module Log =
    let basic (storage: IStorage<'k, 'v>) (clock: IClock) :ILog<'k, 'v> =
        {new ILog<'k, 'v> with
            member this.Append (f: Fact<'k, 'v>) = storage.Store (clock.Tick( )) f
            member this.KeyTimes () = storage.List ()
            member this.TryFind (t: Time) (k: 'k) = 
                let changeTime =
                    storage.List ()
                    |> List.filter (fun (t0, k0) -> k0 = k)
                    |> List.map fst
                    |> List.sortDescending
                    |> List.tryFind (fun t0 -> t0 < t)
                Option.bind (fun t0 -> storage.Retrieve t0 k) changeTime

            member this.Snapshot (t: Time) :Snapshot<'k, 'v> =
                storage.List ()
                |> List.map (fun (t0, k0) -> (k0, t0))
                |> List.groupBy (fun (k0, t0) -> k0)
                |> Map.ofList
                |> Map.map (fun k0 kts -> List.map snd kts)
                |> Map.map (fun k ts -> 
                    ts
                    |> List.sortDescending
                    |> List.tryFind (fun t0 -> t0 < t)
                    )
                |> Map.filter (fun k0 t0 -> Option.isSome t0)
                |> Map.map (fun k t -> Option.get t)
                |> Map.toList
                |> List.map (fun (k, t) -> (k, storage.Retrieve t k))
                |> List.filter (fun (k, v) -> Option.isSome v)
                |> List.map (fun (k, v) -> (k, Option.get v))
                |> Map.ofList

            member this.Timeseries (k: 'k) =
                storage.List ()
                |> List.filter (fun (t0, k0) -> k0=k)
                |> List.map (fun (t0, k0) -> (t0, storage.Retrieve t0 k0))
                |> List.filter (fun (t0, v0) -> Option.isSome v0)
                |> List.map (fun (t0, v0) -> (t0, Option.get v0))
                |> Map.ofList
            }
             

