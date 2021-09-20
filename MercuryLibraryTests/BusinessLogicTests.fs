module MercuryLibraryTests.BusinessLogicTests

open System
open Xunit
open FsUnit

[<Fact>]
let ``differenceInDays finish date after start date`` () =
    let now = DateTime.Now

    let start = now.AddDays(-1.0)

    let finish = now

    MercuryLibrary.BusinessLogic.differenceInDays start finish
    |> should equal 1

[<Fact>]
let ``differenceInDays finish date before start date`` () =
    let now = DateTime.Now

    let finish = now.AddDays(-1.0)

    let start = now

    MercuryLibrary.BusinessLogic.differenceInDays start finish
    |> should equal -1

[<Fact>]
let ``differenceInDays finish date equal to start date`` () =
    let now = DateTime.Now

    let finish = now

    let start = now

    MercuryLibrary.BusinessLogic.differenceInDays start finish
    |> should equal 0