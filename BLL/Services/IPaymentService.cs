using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Services
{
    public interface IPaymentService
    {
        int ProcessPayment(int policyId, decimal amount, string paymentMode);
    }
}
