module Fable.Tests.MK.Main

open Fable.Tests.MK
open Expecto

let allTests =
    [
        IntegrationTests.tests
    ]


[<EntryPoint>]
let main args =
    let config = [ Sequenced ]

    allTests
    |> testList "All"
    |> runTestsWithCLIArgs config args
