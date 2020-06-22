using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace react_example.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class PaymentController : ControllerBase
    {
        private readonly ILogger<PaymentController> _logger;

        public PaymentController(ILogger<PaymentController> logger)
        {
            _logger = logger;
        }

        [HttpGet]
        public async Task<IEnumerable<Payment>> Get()
        {
            var paymentServiceUrl = Environment.GetEnvironmentVariable("PAYMENT_SERVICE_URL");
            _logger.LogInformation($"Payment Service URL: {paymentServiceUrl ?? string.Empty}");
            
            List<Payment> paymentList;

            try
            {
                using var httpClient = new HttpClient();
                using var response = await httpClient.GetAsync(paymentServiceUrl);
                var apiResponse = await response.Content.ReadAsStringAsync();
                paymentList = JsonConvert.DeserializeObject<List<Payment>>(apiResponse);
            }
            catch (Exception ex)
            {
                _logger.LogError($@"Error retrieving payments from '{paymentServiceUrl ?? string.Empty}'", ex);
                throw;
            }

            return paymentList ?? new List<Payment>();
        }
    }
}