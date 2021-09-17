module MercuryLibrary.Models

open System

[<CLIMutable>]
type Audit =
    { createdDate: string
      updatedDate: string }

[<CLIMutable>]
type WhoisRecord =
    { createdDate: string
      updatedDate: string
      expiresDate: string
      status: string
      audit: Audit }

type WhoisResponse =
    { Domain: string
      DomainAgeInDays: int
      DomainLastUpdatedInDays: int
      DomainExpirationInDays: int
      AuditCreated: DateTime
      AuditUpdated: DateTime }
    override x.ToString() =
        $"\"{x.Domain}\": {x.DomainAgeInDays} days since domain creation, {x.DomainLastUpdatedInDays} days since domain last updated, {x.DomainExpirationInDays} until domain expires"