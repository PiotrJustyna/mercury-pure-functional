module Tests.InputValidationTests

open Xunit
open FsUnit

[<Theory>]
[<InlineData("", "")>]
[<InlineData("http://www.sample.url", "")>]
[<InlineData("", "sample.domain")>]
let ``whoisInputValidation negative`` (apiUrlFormat: string, domain: string) =
    (fun () -> MercuryLibrary.InputValidation.whoisInputValidation apiUrlFormat domain |> ignore)
    |> should throw typeof<System.ArgumentException>

[<Theory>]
[<InlineData("http://www.sample.url", "sample.domain")>]
let ``whoisInputValidation positive`` (apiUrlFormat: string, domain: string) =
    MercuryLibrary.InputValidation.whoisInputValidation apiUrlFormat domain