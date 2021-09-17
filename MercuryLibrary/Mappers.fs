module MercuryLibrary.Mappers

open System
open MercuryLibrary.Models

let toWhoisResponse (domain: string) (whoisRecord: WhoisRecord) =
    let now = DateTime.UtcNow

    let createdDate =
        match System.DateTime.TryParse whoisRecord.createdDate with
        | _, date -> date

    let updatedDate =
        match System.DateTime.TryParse whoisRecord.updatedDate with
        | _, date -> date

    let expiresDate =
        match System.DateTime.TryParse whoisRecord.expiresDate with
        | _, date -> date

    let auditCreatedDate =
        match System.DateTime.TryParse whoisRecord.audit.createdDate with
        | _, date -> date

    let auditUpdatedDate =
        match System.DateTime.TryParse whoisRecord.audit.updatedDate with
        | _, date -> date

    { Domain = domain
      DomainAgeInDays = BusinessLogic.differenceInDays createdDate now
      DomainLastUpdatedInDays = BusinessLogic.differenceInDays updatedDate now
      DomainExpirationInDays = BusinessLogic.differenceInDays now expiresDate
      AuditCreated = auditCreatedDate
      AuditUpdated = auditUpdatedDate }