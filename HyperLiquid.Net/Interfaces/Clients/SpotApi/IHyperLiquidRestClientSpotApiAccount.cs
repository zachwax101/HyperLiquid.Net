using System.Collections.Generic;
using System.Threading.Tasks;
using System.Threading;
using CryptoExchange.Net.Objects;
using HyperLiquid.Net.Objects.Models;
using HyperLiquid.Net.Enums;
using System;
using HyperLiquid.Net.Interfaces.Clients.BaseApi;

namespace HyperLiquid.Net.Interfaces.Clients.SpotApi
{
    /// <summary>
    /// HyperLiquid spot account endpoints. Account endpoints include balance info, withdraw/deposit info and requesting and account settings
    /// </summary>
    /// <see cref="IHyperLiquidRestClientAccount"/>
    public interface IHyperLiquidRestClientSpotApiAccount : IHyperLiquidRestClientAccount
    {
        /// <summary>
        /// Get user asset balances
        /// <para><a href="https://hyperliquid.gitbook.io/hyperliquid-docs/for-developers/api/info-endpoint/spot#retrieve-a-users-token-balances" /></para>
        /// </summary>
        /// <param name="address">Address to request balances for. If not provided will use the address provided in the API credentials</param>
        /// <param name="ct">Cancellation token</param>
        Task<WebCallResult<HyperLiquidBalance[]>> GetBalancesAsync(string? address = null, CancellationToken ct = default);

        /// <summary>
        /// Get user account ledger
        /// <para><a href="https://hyperliquid.gitbook.io/hyperliquid-docs/for-developers/api/info-endpoint/perpetuals#retrieve-a-users-funding-history-or-non-funding-ledger-updates" /></para>
        /// </summary>
        /// <param name="startTime">Filter by start time</param>
        /// <param name="endTime">Filter by end time</param>
        /// <param name="address">Address to request ledger for. If not provided will use the address provided in the API credentials</param>
        /// <param name="ct">Cancellation token</param>
        Task<WebCallResult<HyperLiquidAccountLedger>> GetAccountLedgerAsync(DateTime startTime, DateTime? endTime = null, string? address = null, CancellationToken ct = default);

        /// <summary>
        /// Get user rate limits
        /// <para><a href="https://hyperliquid.gitbook.io/hyperliquid-docs/for-developers/api/info-endpoint#query-user-rate-limits" /></para>
        /// </summary>
        /// <param name="address">Address to request rate limits for. If not provided will use the address provided in the API credentials</param>
        /// <param name="ct">Cancellation token</param>
        Task<WebCallResult<HyperLiquidRateLimit>> GetRateLimitsAsync(string? address = null, CancellationToken ct = default);

        /// <summary>
        /// Get the approved builder fee
        /// <para><a href="https://hyperliquid.gitbook.io/hyperliquid-docs/for-developers/api/info-endpoint#check-builder-fee-approval" /></para>
        /// </summary>
        /// <param name="builderAddress">The address of the builder. If not provided will use the builder address for this library</param>
        /// <param name="address">Address to request approved builder fee for. If not provided will use the address provided in the API credentials</param>
        /// <param name="ct">Cancellation token</param>
        Task<WebCallResult<int>> GetApprovedBuilderFeeAsync(string? builderAddress = null, string? address = null, CancellationToken ct = default);

        /// <summary>
        /// Send usd to another address. This transfer does not touch the EVM bridge.
        /// <para><a href="https://hyperliquid.gitbook.io/hyperliquid-docs/for-developers/api/exchange-endpoint#l1-usdc-transfer" /></para>
        /// </summary>
        /// <param name="destinationAddress">Address in 42-character hexadecimal format; e.g. 0x0000000000000000000000000000000000000000</param>
        /// <param name="quantity">Quantity of USD to send</param>
        /// <param name="ct">Cancellation token</param>
        Task<WebCallResult> TransferUsdAsync(string destinationAddress, decimal quantity, CancellationToken ct = default);

        /// <summary>
        /// Send spot assets to another address. This transfer does not touch the EVM bridge.
        /// <para><a href="https://hyperliquid.gitbook.io/hyperliquid-docs/for-developers/api/exchange-endpoint#l1-spot-transfer" /></para>
        /// </summary>
        /// <param name="destinationAddress">Address in 42-character hexadecimal format; e.g. 0x0000000000000000000000000000000000000000</param>
        /// <param name="asset">Asset name, for example "HYPE"</param>
        /// <param name="quantity">Quantity to send</param>
        /// <param name="ct">Cancellation token</param>
        Task<WebCallResult> TransferSpotAsync(
            string destinationAddress,
            string asset,
            decimal quantity,
            CancellationToken ct = default);
        
        /// <summary>
        /// This generalized method is used to transfer tokens between different perp DEXs, spot balance, users, and/or sub-accounts. Use "" to specify the default USDC perp DEX and "spot" to specify spot. Only the collateral token can be transferred to or from a perp DEX.
        /// <para><a href="https://hyperliquid.gitbook.io/hyperliquid-docs/for-developers/api/exchange-endpoint#send-asset" /></para>
        /// </summary>
        /// <param name="destinationAddress">Address in 42-character hexadecimal format; e.g. 0x0000000000000000000000000000000000000000</param>
        /// <param name="asset">Asset name, for example "HYPE"</param>
        /// <param name="quantity">Quantity to send</param>
        /// <param name="sourceDex">name of perp dex to transfer from. For the default perp dex use the empty string "" as name. Token must match the collateral token if transferring to or from a perp dex.</param>
        /// <param name="destinationDex">name of the perp dex to transfer to</param>
        /// <param name="token">tokenName:tokenId; e.g. "PURR:0xc4bf3f870c0e9465323c0b6ed28096c2" For spot use "spot"</param>
        /// <param name="fromSubAccount">address in 42-character hexadecimal format or empty string if not from a subaccount</param>
        /// <param name="ct">Cancellation token</param>
        Task<WebCallResult> SendAssetAsync(
            string destinationAddress,
            decimal quantity,
            string? sourceDex = null,
            string? destinationDex = null,
            string? token = null,
            string? fromSubAccount = null,
            CancellationToken ct = default);


        /// <summary>
        /// Transfer between subaccount and master account.
        /// <para><a href="https://hyperliquid.gitbook.io/hyperliquid-docs/for-developers/api/exchange-endpoint#send-asset" /></para>
        /// </summary>
        /// <param name="subAccountUser">Subaccount address/param>
        /// <param name="isDeposit">If true, transfer will send to subaccount, otherwise from subaccount</param>
        /// <param name="usd">Quantity to send, 1_000_000 = 1 USD</param>
        Task<WebCallResult> TransferBetweenSubAccountsAsync(string subAccountUser, bool isDeposit, long usd, CancellationToken ct = default);
        

        /// <summary>
        /// Initiate the withdrawal flow. After making this request, the L1 validators will sign and send the withdrawal request to the bridge contract. There is a $1 fee for withdrawing at the time of this writing and withdrawals take approximately 5 minutes to finalize.
        /// <para><a href="https://hyperliquid.gitbook.io/hyperliquid-docs/for-developers/api/exchange-endpoint#initiate-a-withdrawal-request" /></para>
        /// </summary>
        /// <param name="destinationAddress">Address in 42-character hexadecimal format; e.g. 0x0000000000000000000000000000000000000000</param>
        /// <param name="quantity">Quantity of USD to send</param>
        /// <param name="ct">Cancellation token</param>
        Task<WebCallResult> WithdrawAsync(
            string destinationAddress,
            decimal quantity,
            CancellationToken ct = default);

        /// <summary>
        /// Transfer USD between Spot and Futures account
        /// <para><a href="https://hyperliquid.gitbook.io/hyperliquid-docs/for-developers/api/exchange-endpoint#transfer-from-spot-account-to-perp-account-and-vice-versa" /></para>
        /// </summary>
        /// <param name="direction">Transfer direction</param>
        /// <param name="quantity">Quantity of USD to send</param>
        /// <param name="ct">Cancellation token</param>
        Task<WebCallResult> TransferInternalAsync(
            TransferDirection direction,
            decimal quantity,
            CancellationToken ct = default);

        /// <summary>
        /// Deposit into staking
        /// <para><a href="https://hyperliquid.gitbook.io/hyperliquid-docs/for-developers/api/exchange-endpoint#deposit-into-staking" /></para>
        /// </summary>
        /// <param name="wei">Quantity</param>
        /// <param name="ct">Cancellation token</param>
        Task<WebCallResult> DepositIntoStakingAsync(long wei, CancellationToken ct = default);

        /// <summary>
        /// Withdraw from staking into the user's spot account. Note that transfers from staking to spot account go through a 7 day unstaking queue.
        /// <para><a href="https://hyperliquid.gitbook.io/hyperliquid-docs/for-developers/api/exchange-endpoint#withdraw-from-staking" /></para>
        /// </summary>
        /// <param name="wei">Quantity</param>
        /// <param name="ct">Cancellation token</param>
        Task<WebCallResult> WithdrawFromStakingAsync(long wei, CancellationToken ct = default);

        /// <summary>
        /// Delegate or undelegate native tokens to or from a validator. Note that delegations to a particular validator have a lockup duration of 1 day.
        /// <para><a href="https://hyperliquid.gitbook.io/hyperliquid-docs/for-developers/api/exchange-endpoint#delegate-or-undelegate-stake-from-validator" /></para>
        /// </summary>
        /// <param name="direction">Direction</param>
        /// <param name="validator">Validator address in hex format, for example 0x0000000000000000000000000000000000000000</param>
        /// <param name="wei">Quantity</param>
        /// <param name="ct">Cancellation token</param>
        Task<WebCallResult> DelegateOrUndelegateStakeFromValidatorAsync(DelegateDirection direction, string validator, long wei, CancellationToken ct = default);

        /// <summary>
        /// Deposit or withdraw from vault
        /// <para><a href="https://hyperliquid.gitbook.io/hyperliquid-docs/for-developers/api/exchange-endpoint#deposit-or-withdraw-from-a-vault" /></para>
        /// </summary>
        /// <param name="direction">Direction</param>
        /// <param name="vaultAddress">Vault address</param>
        /// <param name="usd">USD to withdraw or deposit</param>
        /// <param name="expireAfter">Timestamp after which the request expires and is rejected by the server</param>
        /// <param name="ct">Cancellation token</param>
        Task<WebCallResult> DepositOrWithdrawFromVaultAsync(DepositWithdrawDirection direction, string vaultAddress, long usd, DateTime? expireAfter = null, CancellationToken ct = default);

        /// <summary>
        /// Approve a builder address of the library to charge the fee percentage as defined in the BuilderFeePercentage client options field
        /// <para><a href="https://hyperliquid.gitbook.io/hyperliquid-docs/for-developers/api/exchange-endpoint#approve-a-builder-fee" /></para>
        /// </summary>
        /// <param name="ct">Cancellation token</param>
        Task<WebCallResult> ApproveBuilderFeeAsync(CancellationToken ct = default);

        /// <summary>
        /// Approve a builder address to charge a certain fee
        /// <para><a href="https://hyperliquid.gitbook.io/hyperliquid-docs/for-developers/api/exchange-endpoint#approve-a-builder-fee" /></para>
        /// </summary>
        /// <param name="builderAddress">The address of the builder in hex format, for example 0x0000000000000000000000000000000000000000</param>
        /// <param name="maxFeePercentage">Max fee percentage the builder can charge</param>
        /// <param name="ct">Cancellation token</param>
        Task<WebCallResult> ApproveBuilderFeeAsync(string builderAddress, decimal maxFeePercentage, CancellationToken ct = default);
    }
}
