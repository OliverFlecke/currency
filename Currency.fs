module Currency

open System
open FSharp.Data

type Amount = float * string
let currencyUrl = "https://api.exchangeratesapi.io/latest"

let rates (currency: string) =
  Http.RequestString
    (currencyUrl,
    httpMethod="GET",
    query=["base", currency])
  |> JsonValue.Parse

let rate (from: string) (to': string) =
  let rs = (rates <| from.ToUpperInvariant()).GetProperty("rates")
  match rs.TryGetProperty(to'.ToUpperInvariant()) with
    | Some x -> x.AsFloat()
    | _ ->
      let supported =
        rs.Properties()
        |> Seq.map fst
        |> String.concat ", "
      failwith <| String.Format("Currency {0} not supported. Supported currencies: {1}", to', supported)

let convert ((amount, currency): Amount) (to': string) =
  amount * rate currency to'

let formatConversion (from: Amount) (to': Amount) =
  printfn "%f %s = %f %s" (fst from) (snd from) (fst to') (snd to')
