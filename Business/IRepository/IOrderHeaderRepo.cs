using Data.Models;

namespace Business.IRepository
{
    public  interface IOrderHeaderRepo : IRepository<OrderHeader>
    {
        void UpdatePaymentId(OrderHeader orderHeader, string newpaymentId);
        void UpdateSessionId(OrderHeader orderHeader, string SessionId);
        void Update(OrderHeader orderHeader);
    }
}
