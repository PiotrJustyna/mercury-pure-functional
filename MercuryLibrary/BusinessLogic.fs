module MercuryLibrary.BusinessLogic

open System

let differenceInDays (start: DateTime) (finish: DateTime) = (finish - start).Days