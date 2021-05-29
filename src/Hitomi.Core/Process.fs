module Hitomi.Core.Process

open System
open System.ComponentModel
open System.Diagnostics

let processorCount = Environment.ProcessorCount

let updateInterval = TimeSpan.FromSeconds 1.0

[<Struct>]
type ProcessInterval =
    { Name: string
      Interval: TimeSpan }
    member this.cpuPercentage(elapsed: TimeSpan) =
        float this.Interval.Ticks / float elapsed.Ticks
        * 100.0
        / float processorCount
        |> int

/// Represents a process and its total cpu time
[<Struct>]
type ProcessorTime =
    { Name: string
      ProcessorTime: TimeSpan }
    /// Extracts fields from a Process object.
    static member extract(p: Process) =
        let id = p.Id
        let name = p.ProcessName

        try
            Some(id, { Name = name; ProcessorTime = p.TotalProcessorTime })
        with
        // Needs admin privilege
        | :? Win32Exception
        // Process already stopped
        | :? InvalidOperationException as e ->
            eprintfn $"{id}, {name}, {e.Message}"
            None

    static member (-)(p1, p0) =
        { Name = p1.Name
          Interval = p1.ProcessorTime - p0.ProcessorTime }

type ProcessEntry =
    { Name: string
      Percentage: int }
    static member ofProcessInterval elapsed (t: ProcessInterval) =
        { Name = t.Name; Percentage = t.cpuPercentage(elapsed) }

/// Records processes and their total cpu time, indexed by process id.
type ProcessMap = Map<int, ProcessorTime>

module ProcessMap =
    /// Gets all running processes which are available.
    let getAll () =
        Process.GetProcesses()
        |> Array.choose ProcessorTime.extract
        |> Map.ofArray

    /// Get a list of time intervals from
    let listIntervals (m0: ProcessMap) (m1: ProcessMap) =
        m1
        |> Seq.choose (fun p -> m0.TryFind p.Key |> Option.map ((-) p.Value))
