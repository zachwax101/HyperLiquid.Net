using CryptoExchange.Net.Authentication;
using HyperLiquid.Net;
using HyperLiquid.Net.Clients;
using Nethereum.Signer;

HyperLiquidExchange.SignRequestDelegate = (payloadHex, secretHex) =>
{
    var payload = Convert.FromHexString(payloadHex);
    var keyHex  = secretHex.StartsWith("0x") ? secretHex[2..] : secretHex;

    var sig = new EthECKey(keyHex).SignAndCalculateV(payload); // R,S: byte[] ; V: byte[]

    return new Dictionary<string, object>
    {
        ["r"] = "0x" + Convert.ToHexString(sig.R).ToLowerInvariant(),
        ["s"] = "0x" + Convert.ToHexString(sig.S).ToLowerInvariant(),
        ["v"] = (int)sig.V[0]  // 27/28
    };
};


var masterAccountAddress = Environment.GetEnvironmentVariable("MASTER_ACCOUNT_ADDRESS") ?? "your_master_account_address";
var agentKey = Environment.GetEnvironmentVariable("AGENT_KEY") ?? "your_agent_key";
var toSubAccount = Environment.GetEnvironmentVariable("TO_ACCOUNT_ID") ?? "your_target_subaccount_id";


var restClient = new HyperLiquidRestClient(new (o =>
{
    o.ApiCredentials = new ApiCredentials(masterAccountAddress, agentKey);
    o.Environment = HyperLiquidEnvironment.Live;
}));


var subaccountTransferResult = await restClient.SpotApi.Account.TransferBetweenSubAccountsAsync(toSubAccount, true, 1);
Console.WriteLine(subaccountTransferResult);