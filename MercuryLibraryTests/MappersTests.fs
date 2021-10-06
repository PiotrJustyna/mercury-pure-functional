module MercuryLibraryTests.MappersTests

open System
open MercuryLibrary.Models
open Swensen.Unquote
open Xunit

[<Fact>]
let ``toWhoisResponse all properties correct`` () =
    let now = DateTime.UtcNow

    let irrelevant = String.Empty

    let domain = "google.com"

    let oneDayAgo = now.AddDays(-1.0).ToString()

    let twoDaysAgo = now.AddDays(-2.0).ToString()

    let threeDaysFromNow = now.AddDays(3.0).ToString()

    let fourDaysAgo = now.AddDays(-4.0).ToString()

    let fiveDaysAgo = now.AddDays(-5.0).ToString()

    let audit =
        { createdDate = fourDaysAgo
          updatedDate = fiveDaysAgo }

    let whoisRecord = new WhoisRecord()
    whoisRecord.createdDate <- oneDayAgo
    whoisRecord.updatedDate <- twoDaysAgo
    whoisRecord.expiresDate <- threeDaysFromNow
    whoisRecord.status <- irrelevant
    whoisRecord.audit <- audit

    let whoisResponse =
        MercuryLibrary.Mappers.toWhoisResponse now domain whoisRecord

    test <@ whoisResponse.Domain = domain @>

    test <@ whoisResponse.DomainAgeInDays = (now - DateTime.Parse(oneDayAgo)).TotalDays @>

    test <@ whoisResponse.DomainLastUpdatedInDays = (now - DateTime.Parse(twoDaysAgo)).TotalDays @>

    test <@ whoisResponse.DomainExpirationInDays = (DateTime.Parse(threeDaysFromNow) - now).TotalDays @>

    test <@ whoisResponse.AuditCreated.ToString() = fourDaysAgo @>

    test <@ whoisResponse.AuditUpdated.ToString() = fiveDaysAgo @>