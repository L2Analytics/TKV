namespace TKV

[<AutoOpen>]
module Types =
    type Time = uint64
    type Date = System.DateTime

    type ITimeLookup<'t> =
        abstract member Add: 't -> unit
        abstract member FindAsOf: 't -> 't option
        abstract member FindAsOfIndex: 't -> int option

    type Value<'v> =
        | Asserted of 'v
        | Retracted

    type Snapshot<'k, 'v when 'k: comparison> = Map<'k, 'v>
    type Timeseries<'v> = Map<Time, Value<'v>>

    type Agent<'msg> = MailboxProcessor<'msg>

    type IClock =
        abstract member Tick: unit -> Time
        abstract member Set: Time list -> unit
        abstract member FindDate: Time -> Date
        abstract member FindTime: Date -> Time

    type IStorage<'k, 'v> =
        abstract member Store: Time -> 'k -> Value<'v> -> unit
        abstract member Retrieve: Time -> 'k -> Value<'v> option
        abstract member List: unit -> (Time*'k) list


    type ILog<'k, 'v when 'k: comparison> =
        abstract member TryFind: Time -> 'k -> Value<'v> option
        abstract member Append: 'k -> Value<'v> -> unit
        abstract member KeyTimes: unit -> (Time*'k) list
        abstract member Snapshot: Time -> Snapshot<'k, 'v>
        abstract member Timeseries: 'k -> Timeseries<'v>