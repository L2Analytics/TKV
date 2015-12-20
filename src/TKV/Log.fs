namespace TKV

module Log =
    let basic (storage: IStorage<'k, 'v>) (clock: IClock) :ILog<'k, 'v> =
        {new ILog<'k, 'v> with
            member this.Append (f: Fact<'k, 'v>) = storage.Store (clock.Tick( )) f
            member this.KeyTimes () = storage.List ()
            member this.TryFind (t: Time) (k: 'k) = 
                let changeTime =
                    this.KeyTimes ()
                    |> List.filter (fun (_, k0) -> k0 = k)
                    |> List.map fst
                    |> List.sortDescending
                    |> List.tryFind (fun t0 -> t0 < t)
                Option.bind (fun t0 -> storage.Retrieve t0 k) changeTime

            member this.Snapshot (t: Time) :Snapshot<'k, 'v> =
                this.KeyTimes ()
                |> List.map (fun (t0, k0) -> (k0, t0))
                |> List.groupBy (fun (k0, _) -> k0)
                |> Map.ofList
                |> Map.map (fun _ kts -> List.map snd kts)
                |> Map.map (fun _ ts -> 
                    ts
                    |> List.sortDescending
                    |> List.tryFind (fun t0 -> t0 < t)
                    )
                |> Map.filter (fun _ t0 -> Option.isSome t0)
                |> Map.map (fun _ t -> Option.get t)
                |> Map.toList
                |> List.map (fun (k, t) -> (k, storage.Retrieve t k))
                |> List.filter (fun (_, v) -> Option.isSome v)
                |> List.map (fun (k, v) -> (k, Option.get v))
                |> Map.ofList

            member this.Timeseries (k: 'k) =
                this.KeyTimes ()
                |> List.filter (fun (_, k0) -> k0=k)
                |> List.map (fun (t0, k0) -> (t0, storage.Retrieve t0 k0))
                |> List.filter (fun (_, v0) -> Option.isSome v0)
                |> List.map (fun (t0, v0) -> (t0, Option.get v0))
                |> Map.ofList
            }


