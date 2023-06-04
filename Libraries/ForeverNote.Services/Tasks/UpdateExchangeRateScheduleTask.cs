﻿using ForeverNote.Core.Domain.Directory;
using ForeverNote.Core.Domain.Tasks;
using ForeverNote.Services.Directory;
using System;
using System.Threading.Tasks;

namespace ForeverNote.Services.Tasks
{
    /// <summary>
    /// Represents a task for updating exchange rates
    /// </summary>
    public partial class UpdateExchangeRateScheduleTask : ScheduleTask, IScheduleTask
    {
        private readonly ICurrencyService _currencyService;
        private readonly CurrencySettings _currencySettings;
        public UpdateExchangeRateScheduleTask(ICurrencyService currencyService, CurrencySettings currencySettings)
        {
            _currencyService = currencyService;
            _currencySettings = currencySettings;
        }

        /// <summary>
        /// Executes a task
        /// </summary>
        public async Task Execute()
        {
            if (!_currencySettings.AutoUpdateEnabled)
                return;

            var primaryCurrencyCode = (await _currencyService.GetPrimaryExchangeRateCurrency()).CurrencyCode;
            var exchangeRates = await _currencyService.GetCurrencyLiveRates(primaryCurrencyCode);

            foreach (var exchageRate in exchangeRates)
            {
                var currency = await _currencyService.GetCurrencyByCode(exchageRate.CurrencyCode);
                if (currency != null)
                {
                    currency.Rate = exchageRate.Rate;
                    currency.UpdatedOnUtc = DateTime.UtcNow;
                    await _currencyService.UpdateCurrency(currency);
                }
            }
        }
    }
}
