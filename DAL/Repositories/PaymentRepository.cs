using DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL.Data;

namespace DAL.Repositories
{
    public class PaymentRepository : IPaymentRepository
    {
        private readonly _IpmsContext _context;

        public PaymentRepository(_IpmsContext context)
        {
            _context = context;
        }

        public int CreatePayment(Payment payment)
        {
            _context.Payments.Add(payment);
            _context.SaveChanges();
            return payment.PaymentId;
        }
        public bool UpdatePaymentStatusByPolicyId(int policyId, string status)
        {
            var payment = _context.Payments.FirstOrDefault(p => p.PolicyId == policyId);
            if (payment == null) return false;

            payment.PaymentStatus = status;
            _context.SaveChanges();
            return true;
        }
    }
}
