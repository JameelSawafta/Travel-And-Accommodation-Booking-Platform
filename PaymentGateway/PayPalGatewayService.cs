using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using PayPal.Api;
using TravelAndAccommodationBookingPlatform.Domain.Interfaces.Services;

namespace PaymentGateway;

public class PayPalGatewayService : IPaymentGatewayService
{
    private readonly APIContext _apiContext;
    private readonly IConfiguration _configuration;

    public PayPalGatewayService(APIContext apiContext, IConfiguration configuration)
    {
        _apiContext = apiContext;
        _configuration = configuration;
    }
    
    public async Task<(string approvalUrl, string transactionId)> CreatePaymentAsync(decimal amount, string currency)
    {
        var returnUrl = _configuration["PayPal:ReturnUrl"];
        var cancelUrl = _configuration["PayPal:CancelUrl"];
        var payment = new Payment
        {
            intent = "sale",
            payer = new Payer { payment_method = "paypal" },
            transactions = new List<Transaction>
            {
                new Transaction
                {
                    amount = new Amount
                    {
                        currency = currency,
                        total = amount.ToString("0.00")
                    },
                    description = "Payment for booking"
                }
            },
            redirect_urls = new RedirectUrls
            {
                return_url = returnUrl,
                cancel_url = cancelUrl
            }
        };
        
        var createdPayment = await Task.Run(() => payment.Create(_apiContext));
        var approvalUrl = createdPayment.links.FirstOrDefault(link => link.rel == "approval_url")?.href;
        return (approvalUrl, createdPayment.id);
    }

    public async Task ExecutePaymentAsync(string paymentId, string payerId)
    {
        var paymentExecution = new PaymentExecution { payer_id = payerId };
        var payment = new Payment { id = paymentId };
        await Task.Run(() => payment.Execute(_apiContext, paymentExecution));
    }
}
