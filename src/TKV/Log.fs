namespace TKV

module Log =
    let basic (storage: IStorage<'k, 'v>) (clock: IClock) :ILog<'k, 'v> =
        {new ILog<'k, 'v> with
            member this.Append (f: Fact<'k, 'v>) = storage.Store (clock.Tick ()) f
            member this.KeyTimes () = storage.List ()
            member this.TryFind (t: Time) (k: 'k) =
            member Snapshot: Time -> Snapshot<'k, 'v>
            member Timeseries: 'k -> Timeseries<'v>


