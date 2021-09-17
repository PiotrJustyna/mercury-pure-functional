module MercuryLibrary.InputValidation

open System

let whoisInputValidation apiUrlFormat domain =
    if String.IsNullOrWhiteSpace apiUrlFormat then raise (new ArgumentException(nameof(apiUrlFormat))) else ()
    if String.IsNullOrWhiteSpace domain then raise (new ArgumentException(nameof(domain))) else ()