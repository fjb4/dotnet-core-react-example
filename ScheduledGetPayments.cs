using System;
using System.Net.Http;
using System.Threading.Tasks;
using System.Timers;

namespace react_example
{
    public static class ScheduledGetPayments
    {
        private static Timer _timer;
        private static string _paymentServiceUrl;

        public static void Start(TimeSpan interval)
        {
            if (_timer != null) throw new Exception("Timer already started!");

            _paymentServiceUrl = Environment.GetEnvironmentVariable("PAYMENT_SERVICE_URL");

            if (string.IsNullOrWhiteSpace(_paymentServiceUrl))
            {
                throw new Exception("Unable to retrieve payments; environment variable PAYMENT_SERVICE_URL is not specified");
            }
            
            _timer = new Timer(interval.TotalMilliseconds);
            _timer.Elapsed += OnTimerElapsed;
            _timer.AutoReset = true;
            _timer.Enabled = true;
        }

        public static void Stop()
        {
            if (_timer != null)
            {
                _timer.Dispose();
                _timer = null;
            }
        }

        private static void OnTimerElapsed(object source, ElapsedEventArgs e)
        {
            try
            {
                var task = Task.Run(GetPaymentsResponse);
                task.Wait();
            }
            catch (Exception)
            {
                // intentionally blank
            }
        }

        private static async Task<string> GetPaymentsResponse()
        {

            using var httpClient = new HttpClient();
            using var response = await httpClient.GetAsync(_paymentServiceUrl);
            return await response.Content.ReadAsStringAsync();
        }
    }
}