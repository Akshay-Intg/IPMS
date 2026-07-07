using DAL.Models;
using DAL.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Services
{
    public class PaymentService : IPaymentService
    {
        private readonly IPaymentRepository _paymentRepository;

        public PaymentService(IPaymentRepository paymentRepository)
        {
            _paymentRepository = paymentRepository;
        }

        public int ProcessPayment(int policyId, decimal amount, string paymentMode)
        {
            var payment = new Payment
            {
                PolicyId = policyId,
                Amount = amount,
                PaymentDate = DateTime.UtcNow,
                PaymentMode = paymentMode,
                PaymentStatus="Paid"
            };
            return _paymentRepository.CreatePayment(payment);
        }
    }
}
