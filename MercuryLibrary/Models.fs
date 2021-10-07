﻿module MercuryLibrary.Models

open System

[<AllowNullLiteral>]
type Audit() =
    member val createdDate = String.Empty with get, set
    member val updatedDate = String.Empty with get, set

[<AllowNullLiteral>]
type WhoisRecord() =
    member val createdDate = String.Empty with get, set
    member val updatedDate = String.Empty with get, set
    member val expiresDate = String.Empty with get, set
    member val status = String.Empty with get, set
    member val audit = Audit() with get, set

type WhoisResponse =
    { Domain: string
      DomainAgeInDays: float
      DomainLastUpdatedInDays: float
      DomainExpirationInDays: float
      AuditCreated: DateTime
      AuditUpdated: DateTime }
    override x.ToString() =
        $"\"{x.Domain}\": {x.DomainAgeInDays} days since domain creation, {x.DomainLastUpdatedInDays} days since domain last updated, {x.DomainExpirationInDays} until domain expires"