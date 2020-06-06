open Argu
open Currency

type Arguments =
    | [<Mandatory; ExactlyOnce; First; CliPrefix(CliPrefix.None)>] From of amount:float * currency:string
    | [<Mandatory; CliPrefix(CliPrefix.None)>] To of currency:string

    interface IArgParserTemplate with
        member s.Usage =
            match s with
                | From _ -> "specify the currency and amount to convert from."
                | To _ -> "specify the currency to convert to."

[<EntryPoint>]
let main argv =
    try
        let parser = ArgumentParser.Create<Arguments>("currency")
        let results = parser.Parse argv

        if results.IsUsageRequested then printfn "%s" (parser.PrintUsage())
        else
            let from = results.TryGetResult From
            let tos = results.GetResults To
            match from with
                | None -> failwith "No from currency"
                | Some from ->
                    Seq.iter
                    <| formatConversion from
                    <| Seq.map (fun to' -> (convert from to', to')) tos
    with e ->
        printfn "%s" e.Message

    0 // return an integer exit code
