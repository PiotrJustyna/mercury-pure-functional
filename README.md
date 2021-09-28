# mercury-pure-functional

Functional type of code with explicitly separated pure and effectful responsibilities.

## output

```
12:01:33.288: function is starting...
12:01:34.507: "google.com": 8768 days since domain creation, 738 days since domain last updated, 2552 until domain expires
12:01:34.507: function execution finished
```

## unit testing

This repository uses [xunit](https://github.com/xunit/xunit) + [unquote](https://github.com/SwensenSoftware/unquote).

The repository used to use [fsunit](https://fsprojects.github.io/FsUnit/) but unquote provides better:

* type checking strictness

  example (`differenceInDays` returns `float`):

    * fsunit:

        ```f#
        MercuryLibrary.BusinessLogic.differenceInDays start finish
        |> should equal -1
        ```
      
        The test passes even though `-1` and the value returned by `differenceInDays` are of different types.

  * unquote:

      ```f#
      test <@ MercuryLibrary.BusinessLogic.differenceInDays start finish = -1.0 @>
      ```

    The test passes provided that the `-1` is strictly represented as a `float` (`-1.0`, not `-1`), so that both compared values are of exactly the same type.

* error messages for failing tests
  
    sample failing test

    ```f#
    test <@  whoisResponse.DomainAgeInDays < (now - DateTime.Parse(oneDayAgo)).TotalDays @>
    ```
    
    output:
    
    ```
    MercuryLibraryTests.MappersTests.toWhoisResponse all properties correct
    
    Xunit.Sdk.TrueException
    
    
    whoisResponse.DomainAgeInDays < (let mutable copyOfStruct = now - DateTime.Parse(oneDayAgo) in copyOfStruct.TotalDays)
    { Domain = "google.com"
      DomainAgeInDays = 1.000006465
      DomainLastUpdatedInDays = 2.000006465
      DomainExpirationInDays = 2.999993535
      AuditCreated = 09/24/2021 10:12:57
      AuditUpdated = 09/23/2021 10:12:57 }.DomainAgeInDays < (let mutable copyOfStruct = 2021-09-28T10:12:57.5585520Z (Utc) - DateTime.Parse("09/27/2021 10:12:57") in copyOfStruct.TotalDays)
    1.000006465 < (let mutable copyOfStruct = 2021-09-28T10:12:57.5585520Z (Utc) - 2021-09-27T10:12:57.0000000 (Unspecified) in copyOfStruct.TotalDays)
    1.000006465 < (let mutable copyOfStruct = 1.00:00:00.5585520 in copyOfStruct.TotalDays)
    1.000006465 < 1.000006465
    false
    
    Expected: True
    Actual:   False
       at MercuryLibraryTests.MappersTests.toWhoisResponse all properties correct() in /Users/justpi01/Documents/code/mercury-pure-functional/MercuryLibraryTests/MappersTests.fs:line 41
    ```