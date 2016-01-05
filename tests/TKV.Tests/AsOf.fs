module TKV.Tests

open TKV
open NUnit
open NUnit.Framework
open FsUnit


[<Test>]
let ``AsOf handles empty data`` () =
  let data = AsOf.New(List.empty<int>)
  data.TryFind 4
  |> should equal None
