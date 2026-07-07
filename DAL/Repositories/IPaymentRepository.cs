using DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Repositories
{
    public interface IPaymentRepository
    {
        int CreatePayment(Payment payment);
        bool UpdatePaymentStatusByPolicyId(int policyId, string status);
    }
}
