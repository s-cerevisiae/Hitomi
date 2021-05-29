module Hitomi.Core.View

open Hitomi.Core.Process

open System
open System.Diagnostics
open Elmish
open Elmish.WPF

type Model =
    { LastMap: ProcessMap
      Watch: Stopwatch
      LastTime: TimeSpan
      Entry: ProcessEntry }

let init () =
    { LastMap = ProcessMap.getAll ()
      Watch = Stopwatch.StartNew()
      LastTime = TimeSpan.Zero
      Entry = { Name = "null"; Percentage = 0 } }

type Msg = Tick

let update Tick model =
    let currentTime = model.Watch.Elapsed
    let currentMap = ProcessMap.getAll ()

    let entry =
        ProcessMap.listIntervals model.LastMap currentMap
        |> Seq.maxBy (fun t -> t.Interval)
        |> ProcessEntry.ofProcessInterval (currentTime - model.LastTime)

    { model with
          LastMap = currentMap
          Entry = entry
          LastTime = currentTime }


let bindings () : Binding<Model, Msg> list =
    [ "ProcessName"
      |> Binding.oneWay (fun m -> m.Entry.Name)
      "Percentage"
      |> Binding.oneWay (fun m -> m.Entry.Percentage) ]

let timer _ =
    let timer = new Timers.Timer(float updateInterval.TotalMilliseconds)
    timer.Start()
    let sub dispatch = timer.Elapsed.Add(fun _ -> dispatch Tick)
    Cmd.ofSub sub
