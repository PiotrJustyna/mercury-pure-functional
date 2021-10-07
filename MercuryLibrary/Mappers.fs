module MercuryLibrary.Mappers

open System
open MercuryLibrary.Models

let toWhoisResponse (now: DateTime) (domain: string) (whoisRecord: WhoisRecord) =
    if obj.ReferenceEquals(whoisRecord, null) then
        Option<WhoisResponse>.None
    else
        let createdDate =
            match DateTime.TryParse whoisRecord.createdDate with
            | _, date -> date

        let updatedDate =
            match DateTime.TryParse whoisRecord.updatedDate with
            | _, date -> date

        let expiresDate =
            match DateTime.TryParse whoisRecord.expiresDate with
            | _, date -> date

        if obj.ReferenceEquals(whoisRecord.audit, null) then
            let whoisResponse =
                { Domain = domain
                  DomainAgeInDays = BusinessLogic.differenceInDays createdDate now
                  DomainLastUpdatedInDays = BusinessLogic.differenceInDays updatedDate now
                  DomainExpirationInDays = BusinessLogic.differenceInDays now expiresDate
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
                  DomainAgeInDays = BusinessLogic.differenceInDays createdDate now
                  DomainLastUpdatedInDays = BusinessLogic.differenceInDays updatedDate now
                  DomainExpirationInDays = BusinessLogic.differenceInDays now expiresDate
                  AuditCreated = auditCreatedDate
                  AuditUpdated = auditUpdatedDate }

            Option<WhoisResponse>.Some whoisResponse