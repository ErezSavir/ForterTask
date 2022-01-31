# Forter Task
#### Crypto Performance

## To Run
`cd ForterTask`

`dotnet build`

`dotnet run`

Swagger is available at HOST:PORT/Swagger

#### API
POST /SearchCoinPerformance

##### Body
```json
{
    "symbols": ["BTC","ETH","BNB","DOGE"],
    "date": "2020-01-01"
}
```
##### Response 
HTTP 200
```json
{
  "DOGE": "6,706.76%",
  "BNB": "2,691.90%",
  "ETH": "1,965.85%",
  "BTC": "433.23%"
}
```

In case of a failure there will be HTTP 400 (Validation) or 500 (Server error)