﻿using System.Threading.Tasks;
using Abp.Webhooks;

namespace CruisePMS.WebHooks
{
    public interface IWebhookEventAppService
    {
        Task<WebhookEvent> Get(string id);
    }
}
