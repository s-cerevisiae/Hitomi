module Hitomi.Tests.Process

open System

open Hitomi.Core.Process
open NUnit.Framework
open FsUnit

let processorCount = Environment.ProcessorCount

let span = TimeSpan.FromTicks 100L

let p0 = { Name = "test"; ProcessorTime = TimeSpan.FromTicks 100L }

let p1 = { Name = "test"; ProcessorTime = TimeSpan.FromTicks 150L }


[<TestFixture>]
type TestProcessorTime() =
    [<Test>]
    member _.``calculate the percentage``() =
        (p1 - p0).cpuPercentage span
        |> should equal (50 / processorCount)

let zero = TimeSpan.Zero

let map0 = Map.empty

let map1 = Map.ofList [ 0, p0 ]

let map1' = Map.ofList [ 0, p1 ]

let map2 = Map.ofList [ 0, p0; 1, p1 ]

let p2 = { Name = "test"; ProcessorTime = TimeSpan.FromTicks 250L }

let map2' = Map.ofList [ 0, p1; 1, p2 ]

let interval50 = { Name = "test"; Interval = TimeSpan.FromTicks 50L }

let interval100 = { Name = "test"; Interval = TimeSpan.FromTicks 100L }

[<TestFixture>]
type TestProcessMap() =
    [<Test>]
    member _.``zero on zero``() =
        ProcessMap.listIntervals map0 map0
        |> should equal map0

    [<Test>]
    member _.``zero on one``() =
        ProcessMap.listIntervals map0 map1
        |> should equal map0

    [<Test>]
    member _.``one on zero``() =
        ProcessMap.listIntervals map1 map0
        |> should equal map0

    [<Test>]
    member _.``one on one``() =
        ProcessMap.listIntervals map1 map1'
        |> Seq.exactlyOne
        |> should equal interval50

    [<Test>]
    member _.``one on two``() =
        ProcessMap.listIntervals map1 map2'
        |> Seq.exactlyOne
        |> should equal interval50

    [<Test>]
    member _.``two on one``() =
        ProcessMap.listIntervals map2 map1'
        |> Seq.exactlyOne
        |> should equal interval50

    [<Test>]
    member _.``two on two``() =
        ProcessMap.listIntervals map2 map2'
        |> List.ofSeq
        |> should equal [ interval50; interval100 ]

[<TestFixture>]
type TestProcessEntry() =
    [<Test>]
    member _.``get entry``() =
        ProcessEntry.ofProcessInterval span interval50
        |> should equal { Name = "test"; Percentage = (50 / processorCount) }
