module Hitomi.Core.Program

open Elmish
open Elmish.WPF

open Hitomi.Core.View

let designVm = ViewModel.designInstance (init ()) (bindings ())

let main window =
    Program.mkSimpleWpf init update bindings
    |> Program.withSubscription timer
    |> Program.startElmishLoop ElmConfig.Default window
