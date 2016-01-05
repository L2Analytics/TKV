namespace TKV

module Time =
    let Zero () :Time = uint64 0
    let Successor (t: Time) :Time = t+(uint64 1)
