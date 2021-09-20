module MercuryLibraryTests.MappersTests

open System
open MercuryLibrary.Models
open Xunit
open FsUnit

[<Fact>]
let ``toWhoisResponse all properties correct`` () =
    let now = DateTime.UtcNow

    let irrelevant = String.Empty

    let domain = "google.com"

    let whoisRecord =
        { createdDate = now.AddDays(-1.0).ToString()
          updatedDate = now.AddDays(-2.0).ToString()
          expiresDate = now.AddDays(3.0).ToString()
          status = irrelevant
          audit =
              { createdDate = now.AddDays(-4.0).ToString()
                updatedDate = now.AddDays(-5.0).ToString() } }

    let whoisResponse =
        MercuryLibrary.Mappers.toWhoisResponse domain whoisRecord

    whoisResponse.Domain |> should equal domain

    whoisResponse.DomainAgeInDays
    |> should equal (now - now.AddDays(-1.0)).Days

    whoisResponse.DomainLastUpdatedInDays
    |> should equal (now - now.AddDays(-2.0)).Days

    // 2021-09-20 PJ:
    // due to ms rounding, now + 3 - now is less than 3 full days, hence - 1.0
    whoisResponse.DomainExpirationInDays
    |> should equal (now.AddDays(3.0).AddDays(-1.0) - now).Days

    whoisResponse.AuditCreated.Date
    |> should equal (now.AddDays(-4.0).Date)

    whoisResponse.AuditUpdated.Date
    |> should equal (now.AddDays(-5.0).Date)