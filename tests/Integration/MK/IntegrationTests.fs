module Fable.Tests.MK.IntegrationTests

open Expecto

let tests =
  testCase "A simple test" <| fun () ->
    let expected = 4
    Expect.equal expected (2+2) "2+2 = 4"
