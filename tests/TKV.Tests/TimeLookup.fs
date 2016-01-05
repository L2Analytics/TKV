module TKV.Tests

open TKV
open NUnit
open NUnit.Framework
open FsUnit


[<Test>]
let ``TimeLookup handles empty data`` () =
  let data = TimeLookup.New(List.empty<int>)
  data.FindAsOf 4
  |> should equal None

[<Test>]
let ``TimeLookup handles lower bound`` () =
  let data = TimeLookup.New(List.empty<int>)
  data.Add -5
  data.Add 1
  data.Add 7
  data.FindAsOf -6
  |> should equal None

[<Test>]
let ``TimeLookup handles upper bound`` () =
  let data = TimeLookup.New(List.empty<int>)
  data.Add -5
  data.Add 1
  data.Add 7
  data.FindAsOf 8
  |> should equal (Some 7)