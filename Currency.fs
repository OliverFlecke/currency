module Currency

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
  (rates from)
    .GetProperty("rates")
    .GetProperty(to')
    .AsFloat()

let convert ((amount, currency): Amount) (to': string) =
  amount * rate currency to'

let formatConversion (from: Amount) (to': Amount) =
  printfn "%f %s = %f %s" (fst from) (snd from) (fst to') (snd to')
