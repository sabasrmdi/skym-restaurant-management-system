# Technical Documentation for SKYM API

## Overview

The SKYM API provides various endpoints to manage and retrieve data related to a restaurant's operations, including tables, orders, customers, staff, and menus. This API is built using ASP.NET Core and interacts with a SQL Server database.

## Controllers and Models

### Models

#### DayEarningReportModel

```csharp
public class DayEarningReportModel
{
    public DateTime OrderDate { get; set; }
    public int Earning { get; set; }
}
```

#### TopCustomersReportModel

```csharp
public class TopCustomersReportModel
{
    public int CustomerId { get; set; }
    public string CustomerName { get; set; }
    public int CountOfOrders { get; set; }
    public int CustomerTotalSpending { get; set; }
}
```

#### Customer

```csharp
public class Customer
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string PhoneNumber { get; set; }
}
```

#### OrderMenu

```csharp
public class OrderMenu
{
    public int OrderId { get; set; }
    public int MenuId { get; set; }
    public int Count { get; set; }
}
```

#### Menu

```csharp
public class Menu
{
    public int Id { get; set; }
    public MenuType Type { get; set; }
    public string Name { get; set; }
    public int Price { get; set; }
    public int Count { get; set; }
}
```

#### Enums

```csharp
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
```

#### TodayOrder

```csharp
public class TodayOrder
{
    public DateTime OrderDate { get; set; }
    public OrderType OrderType { get; set; }
    public bool IsOrderFinished { get; set; }
    public int OrderId { get; set; }
    public string CustomerName { get; set; }
}
```

#### Staff

```csharp
public class Staff
{
    public int Id { get; set; }
    public string FName { get; set; }
    public string LName { get; set; }
    public int Salary { get; set; }
    public DateTime StartDate { get; set; }
    public StaffType Type { get; set; }
}
```

#### Table

```csharp
public class Table
{
    public int Id { get; set; }
    public TableStatus Status { get; set; }
    public int Capacity { get; set; }
}
```

#### StartOrderForNewCustomerRequest

```csharp
public class StartOrderForNewCustomerRequest
{
    public int CustomerId { get; set; }
    public string CustomerName { get; set; }
    public string CustomerPhoneNumber { get; set; }
    public int OrderType { get; set; }
    public int WaiterId { get; set; }
    public int TableId { get; set; }
    public string Address { get; set; } = string.Empty;
}
```

## Endpoints

### Get All Tables

#### Description

Retrieves all tables in the restaurant.

#### Method

`GET /GetAllTables`

#### Response

- `200 OK`: A list of `Table` objects.

### Day Earning Report

#### Description

Retrieves the earning report for each day.

#### Method

`GET /DayEarningReport`

#### Response

- `200 OK`: A list of `DayEarningReportModel` objects.

### Today Orders

#### Description

Retrieves the list of today's orders.

#### Method

`GET /TodayOrders`

#### Response

- `200 OK`: A list of `TodayOrder` objects.

### Top Customers Report

#### Description

Retrieves the top customers based on their spending.

#### Method

`GET /TopCustomersReport`

#### Response

- `200 OK`: A list of `TopCustomersReportModel` objects.

### Get Menus

#### Description

Retrieves all menu items.

#### Method

`GET /GetMenus`

#### Response

- `200 OK`: A list of `Menu` objects.

### Get Staffs

#### Description

Retrieves all staff members.

#### Method

`GET /GetStaffs`

#### Response

- `200 OK`: A list of `Staff` objects.

### Get Customers

#### Description

Retrieves all customers.

#### Method

`GET /GetCustomers`

#### Response

- `200 OK`: A list of `Customer` objects.

### End Order

#### Description

Ends an order.

#### Method

`POST /EndOrder`

#### Parameters

- `int orderId`

#### Response

- `200 OK`

### Create Staff

#### Description

Creates a new staff member.

#### Method

`POST /CreateStaff`

#### Parameters

- `string fName`
- `string lName`
- `int salary`
- `DateTime startDate`
- `StaffType type`

#### Response

- `200 OK`

### Create Menu

#### Description

Creates a new menu item.

#### Method

`POST /CreateMenu`

#### Parameters

- `MenuType type`
- `string name`
- `string ingrediants`
- `int price`
- `int count`

#### Response

- `200 OK`

### Start Order for Existing Customer

#### Description

Starts an order for an existing customer.

#### Method

`POST /StartOrderForExistingCustomer`

#### Parameters

- `int customerId`
- `int tableId`
- `int waiter`
- `OrderType orderType`
- `string address`

#### Response

- `200 OK`

### Get Order Menu Items

#### Description

Retrieves the menu items for a specific order.

#### Method

`GET /GetOrderMenuItems`

#### Parameters

- `int orderId`

#### Response

- `200 OK`: A list of `OrderMenu` objects.

### Add Item to Order

#### Description

Adds an item to an existing order.

#### Method

`POST /AddItemToOrder`

#### Parameters

- `int orderId`
- `int menuId`
- `int count`

#### Response

- `200 OK`

### Start Order for New Customer

#### Description

Starts an order for a new customer.

#### Method

`POST /StartOrderForNewCustomer`

#### Request Body

```json
{
    "customerId": int,
    "customerName": string,
    "customerPhoneNumber": string,
    "orderType": int,
    "waiterId": int,
    "tableId": int,
    "address": string
}
```

#### Response

- `200 OK`

## Database Stored Procedures

### update_table_status

Updates the status of a table based on the given order ID.

#### Parameters

- `int order_id`

### end_order

Ends an order.

#### Parameters

- `int order_id`

### start_order_for_existing_customer

Starts an order for an existing customer.

#### Parameters

- `int customer_id`
- `int table_id`
- `int waiter`
- `int order_type`
- `string address`

### start_order_for_new_customer

Starts an order for a new customer.

#### Parameters

- `string customerName`
- `string customerPhoneNumber`
- `int orderType`
- `int table_id`
- `int waiter`
- `string address`

## Usage Examples

### Retrieve All Tables

```http
GET /GetAllTables
```

### Create a New Staff Member

```http
POST /CreateStaff
Content-Type: application/json

{
    "fName": "John",
    "lName": "Doe",
    "salary": 50000,
    "startDate": "2024-05-31T00:00:00",
    "type": 1
}
```

### Start Order for a New Customer

```http
POST /StartOrderForNewCustomer
Content-Type: application/json

{
    "customerId": 1,
    "customerName": "Jane Doe",
    "customerPhoneNumber": "0123456789",
    "orderType": 1,
    "waiterId": 2,
    "tableId": 3,
    "address": "123 Main St"
}
```

This documentation provides an overview of the SKYM API, its endpoints, and models. It also includes usage examples for common operations.
