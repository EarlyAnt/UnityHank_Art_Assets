namespace Hank.Api
{
    public class GetPaymentResultResponse : DataBase
    {
        public OrderInfo order { get; set; }
    }

    public class OrderInfo
    {
        public const int ORDER_STATUS_UNPAID = 0;
        public const int ORDER_STATUS_PAID = 10;
        public const int ORDER_STATUS_TIMEOUT = 20;
        public const int ORDER_STATUS_CLOSED = 30;
        public const int ORDER_STATUS_REFUND = 40;
        public int order_status { get; set; }
    }
}