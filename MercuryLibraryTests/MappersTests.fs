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

    let oneDayAgo = now.AddDays(-1.0)

    let twoDaysAgo = now.AddDays(-2.0)

    let threeDaysFromNow = now.AddDays(3.0)

    let fourDaysAgo = now.AddDays(-4.0)

    let fiveDaysAgo = now.AddDays(-5.0)

    let whoisRecord =
        { createdDate = oneDayAgo.ToString()
          updatedDate = twoDaysAgo.ToString()
          expiresDate = threeDaysFromNow.ToString()
          status = irrelevant
          audit =
              { createdDate = fourDaysAgo.ToString()
                updatedDate = fiveDaysAgo.ToString() } }

    let whoisResponse =
        MercuryLibrary.Mappers.toWhoisResponse now domain whoisRecord

    whoisResponse.Domain |> should equal domain

    // 2021-09-20 PJ:
    // ToString'ing the dates to simulate using the serialized dates.
    whoisResponse.DomainAgeInDays
    |> should equal (now - DateTime.Parse(oneDayAgo.ToString())).TotalDays

    whoisResponse.DomainLastUpdatedInDays
    |> should equal (now - DateTime.Parse(twoDaysAgo.ToString())).TotalDays

    whoisResponse.DomainExpirationInDays
    |> should equal (DateTime.Parse(threeDaysFromNow.ToString()) - now).TotalDays

    whoisResponse.AuditCreated.Date
    |> should equal fourDaysAgo.Date

    whoisResponse.AuditUpdated.Date
    |> should equal fiveDaysAgo.Date