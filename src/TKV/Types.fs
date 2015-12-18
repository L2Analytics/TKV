namespace TKV

type Time = uint64
type Date = System.DateTime

type Fact<'k, 'v> =
    | Assert of 'k*'v
    | Retract of 'k

type Snapshot<'k, 'v when 'k: comparison> = Map<'k, 'v>
type Timeseries<'v> = Map<Time, 'v>

type Agent<'msg> = MailboxProcessor<'msg>

type IClock =
    abstract member Tick: unit -> Time
    abstract member Set: Time list -> unit
    abstract member FindDate: Time -> Date
    abstract member FindTime: Date -> Time

type IStorage<'k, 'v> =
    abstract member Store: Time -> Fact<'k, 'v> -> unit
    abstract member Retrieve: Time -> 'k -> 'v option
    abstract member List: unit -> Time*'k list


type ILog<'k, 'v when 'k: comparison> =
    abstract member TryFind: Time -> 'k -> 'v option
    abstract member Append: Fact<'k, 'v> -> unit
    abstract member KeyTimes: unit -> Time*'k list
    abstract member Snapshot: Time -> Snapshot<'k, 'v>
    abstract member Timeseries: 'k -> Timeseries<'v>