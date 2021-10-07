module MercuryLibrary.Mappers

open System
open MercuryLibrary.Models

let toWhoisResponse (now: DateTime) (domain: string) (whoisRecord: WhoisRecord) =
    if obj.ReferenceEquals(whoisRecord, null) then
        Option<WhoisResponse>.None
    else
        let createdDate =
            match DateTime.TryParse whoisRecord.createdDate with
            | true, date -> Option.Some date
            | false, _ -> Option.None

        let updatedDate =
            match DateTime.TryParse whoisRecord.updatedDate with
            | true, date -> Option.Some date
            | false, _ -> Option.None

        let expiresDate =
            match DateTime.TryParse whoisRecord.expiresDate with
            | true, date -> Option.Some date
            | false, _ -> Option.None

        if obj.ReferenceEquals(whoisRecord.audit, null) then
            let whoisResponse =
                { Domain = domain
                  DomainAgeInDays = BusinessLogic.differenceInDaysOptionalStart createdDate now
                  DomainLastUpdatedInDays = BusinessLogic.differenceInDaysOptionalStart updatedDate now
                  DomainExpirationInDays = BusinessLogic.differenceInDaysOptionalEnd now expiresDate
                  AuditCreated = Option.None
                  AuditUpdated = Option.None }

            Option<WhoisResponse>.Some whoisResponse
        else
            let auditCreatedDate =
                match DateTime.TryParse whoisRecord.audit.createdDate with
                | true, date -> Option.Some date
                | false, _ -> Option.None

            let auditUpdatedDate =
                match DateTime.TryParse whoisRecord.audit.updatedDate with
                | true, date -> Option.Some date
                | false, _ -> Option.None

            let whoisResponse =
                { Domain = domain
                  DomainAgeInDays = BusinessLogic.differenceInDaysOptionalStart createdDate now
                  DomainLastUpdatedInDays = BusinessLogic.differenceInDaysOptionalStart updatedDate now
                  DomainExpirationInDays = BusinessLogic.differenceInDaysOptionalEnd now expiresDate
                  AuditCreated = auditCreatedDate
                  AuditUpdated = auditUpdatedDate }

            Option<WhoisResponse>.Some whoisResponse