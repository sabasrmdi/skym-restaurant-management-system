namespace SKYM_Blazor.Models
{
    public class TodayOrder
    {
        public DateTime OrderDate { get; set; }
        public OrderType OrderType { get; set; }
        public bool IsOrderFinished { get; set; }
        public int OrderId { get; set; }
        public string CustomerName { get; set; }
    }
    public class DayEarningReportModel
    {
        public DateTime OrderDate { get; set; }
        public int Earning { get; set; }
    }
    public class TopCustomersReportModel
    {
        public int CustomerId { get; set; }
        public string CustomerName { get; set; }
        public int CountOfOrders { get; set; }
        public int CustomerTotalSpending { get; set; }
    }
    public class OrderLog
    {
        public int Id { get; set; }
        public int OrderId { get; set; }
        public DateTime HappeningDateTime { get; set; }
        public string Username { get; set; }
        public string Description { get; set; }
        public OrderLogOperationType Type { get; set; }
    }
    public enum OrderLogOperationType
    {
        NewRecord = 1,
        DeleteRecord = 2,
        UpdateRecord = 3
    }


    public class Customer
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string PhoneNumber { get; set; }
    }
    public class OrderMenu
    {
        public int OrderId { get; set; }
        public int MenuId { get; set; }
        public int Count { get; set; }
    }
    public class Menu
    {
        public int Id { get; set; }
        public MenuType Type { get; set; }
        public string Name { get; set; }
        public int Price { get; set; }
        public int Count { get; set; }
    }
    public enum OrderType
    {
        InPersonCustomerOrder = 1,
        TakeawayCustomerOrder = 2,
        DeliveryCustomerOrder = 3
    }
    public enum TableStatus
    {
        Free = 1,
        Busy = 2
    }
    public enum MenuType
    {
        Appetizer = 1,
        Entress = 2,
        Desserts = 3,
        Drinks = 4,
        SideDish = 5
    }
    public enum StaffType
    {
        Waiter = 1
    }
    public class StartOrderForNewCustomerRequest
    {
        public int CustomerId { get; set; }
        public string CustomerName { get; set; }
        public string CustomerPhoneNumber { get; set; }
        public int OrderType { get; set; }
        public int WaiterId { get; set; }
        public int TableId { get; set; }
        public string address { get; set; } = string.Empty;
    }
    public class Staff
    {
        public int Id { get; set; }
        public string FName { get; set; }
        public string LName { get; set; }
        public int Salary { get; set; }
        public DateTime StartDate { get; set; }
        public StaffType Type { get; set; }
    }
    public class Table
    {
        public int Id { get; set; }
        public TableStatus Status { get; set; }
        public int Capacity { get; set; }
    }
    public class StartOrderForNewCustomer
    {
        public string CustomerId { get; set; }
        public string CustomerName { get; set; }
        public string CustomerPhoneNumber { get; set; }
        public int OrderType { get; set; }
        public int WaiterId { get; set; }
        public int TableId { get; set; }
        public string address { get; set; } = string.Empty;
    }
}