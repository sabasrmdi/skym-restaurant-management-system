## SkymRestaurant Front-End Documentation

---

### Overview

This document provides a technical overview of the front-end implementation of the SkymRestaurant project. The front-end is developed using Blazor, a .NET web framework that enables the building of interactive web UIs using C# instead of JavaScript. This document covers the key pages and components, detailing their structure, functionality, and integration with the backend APIs.

---

### Technologies Used

- **Blazor**: For building interactive and dynamic web UIs.
- **ASP.NET Core**: For creating RESTful APIs that the front-end communicates with.
- **HTTP Client**: For making HTTP requests to the backend APIs.
- **Razor Components**: For building reusable UI components.
- **Entity Framework Core**: For interacting with the SQL Server database.

---

### Page Components

---

#### 1. Create Order Page

**File**: `Pages/CreateOrder.razor`

**Route**: `/createOrder`

**Description**:
The Create Order page allows users to create a new order by providing customer details, selecting an order type, assigning a waiter, and choosing a table. This page includes a form that is bound to a `StartOrderForNewCustomerRequest` model.

**Key Elements**:

- **Form**: Uses `EditForm` for form handling.
- **Bindings**: Utilizes `@bind-Value` for two-way data binding.
- **Drop-down Lists**: Populated dynamically with data from the backend.
- **Submission**: The form is submitted via the `HandleValidSubmit` method, which posts the data to the backend API.

**Code Breakdown**:

```razor
@page "/createOrder"
@using SKYM_Blazor.Models
@rendermode InteractiveServer
@inject HttpClient httpClient;
@inject IConfiguration configuration;

<PageTitle>CreateOrder</PageTitle>

<div class="container">
    <h1>Create order</h1>
    <a href="/">back to home page</a>
    <EditForm Model="@order" OnValidSubmit="HandleValidSubmit">
        <div class="form">
            <InputNumber @bind-Value="order.CustomerId" placeholder="customerId" class="form-control" hidden />
            <InputText @bind-Value="order.CustomerName" placeholder="customerName" class="form-control" />
            <InputText @bind-Value="order.CustomerPhoneNumber" placeholder="customerPhoneNumber" class="form-control" />
            <InputSelect @bind-Value="order.OrderType" class="form-control" required>
                @foreach (var item in Enum.GetValues(typeof(OrderType)))
                {
                    <option value="@((int)item)">@((OrderType)(int)item)</option>
                }
            </InputSelect>
            <InputSelect @bind-Value="order.WaiterId" class="form-control" required>
                @foreach (var item in waiters)
                {
                    <option value=@(item.Id) @onselect="() => order.WaiterId=item.Id">@item.FName @item.LName</option>
                }
            </InputSelect>
            <InputSelect @bind-Value="order.TableId" class="form-control" required>
                @foreach (var item in tables)
                {
                    <option value=@(item.Id) @onselect="() => order.TableId=item.Id">table #@item.Id</option>
                }
            </InputSelect>
            <button type="submit" class="btn btn-primary">Submit</button>
        </div>
    </EditForm>
</div>

@code {
    private Models.StartOrderForNewCustomerRequest order = new();
    private List<Models.Table> tables = new();
    private List<Models.Staff> waiters = new();

    protected async override Task OnInitializedAsync()
    {
        string apiUrlTables = configuration.GetValue<string>("api_url") + "/GetAllTables";
        tables = await httpClient.GetFromJsonAsync<List<Models.Table>>(apiUrlTables);
        order.TableId = tables.First().Id;

        string apiUrlWaiters = configuration.GetValue<string>("api_url") + "/GetStaffs";
        waiters = (await httpClient.GetFromJsonAsync<List<Models.Staff>>(apiUrlWaiters)).Where(a => a.Type == StaffType.Waiter).ToList();
        order.WaiterId = waiters.First().Id;
    }

    private async Task HandleValidSubmit()
    {
        var createOrderApiUrl = configuration.GetValue<string>("api_url") + "/StartOrderForNewCustomer";
        await httpClient.PostAsJsonAsync(createOrderApiUrl, order);
    }
}
```

---

#### 2. Day Earning Report Page

**File**: `Pages/DayEarningReport.razor`

**Route**: `/dayEarningReport`

**Description**:
The Day Earning Report page displays the daily earnings from orders, showing the earning amount and order date in a tabular format.

**Key Elements**:

- **Table**: Uses HTML table elements to display the report data.
- **Data Fetching**: Retrieves data from the backend API in the `OnInitializedAsync` method.

**Code Breakdown**:

```razor
@page "/dayEarningReport"
@using SKYM_Blazor.Models
@rendermode InteractiveServer
@inject HttpClient httpClient;
@inject IConfiguration configuration;

<PageTitle>Day Earning Report</PageTitle>

<div class="container">
    <h1>Day earning report</h1>
    <a href="/">back to home page</a>
    <table class="table">
        <thead>
            <tr>
                <th>Earning</th>
                <th>OrderDate</th>
            </tr>
        </thead>
        <tbody>
            @foreach (var item in data)
            {
                <tr>
                    <td>@item.Earning</td>
                    <td>@item.OrderDate</td>
                </tr>
            }
        </tbody>
    </table>
</div>

@code {
    private List<Models.DayEarningReportModel> data = new();
    protected async override Task OnInitializedAsync()
    {
        string apiUrl = configuration.GetValue<string>("api_url") + "/DayEarningReport";
        data = await httpClient.GetFromJsonAsync<List<Models.DayEarningReportModel>>(apiUrl);
    }
}
```

---

#### 3. Home Page

**File**: `Pages/Index.razor`

**Route**: `/`

**Description**:
The Home page serves as the main navigation hub, providing links to various reports and order creation functionalities within the SkymRestaurant application.

**Key Elements**:

- **Navigation Menu**: Uses a list of links for easy navigation to different pages.

**Code Breakdown**:

```razor
@page "/"

<PageTitle>SKYM Restaurant</PageTitle>

<h1>SKYM Restaurant Management System</h1>

<a href="/todayOrders">Today Orders</a>
<a href="/dayEarningReport">Day Earning Report</a>
<a href="/topCustomersReport">Top Customers Report</a>
<a href="/createOrder">Create Order</a>
```

---

#### 4. Order Menu Item Page

**File**: `Pages/OrderMenuItem.razor`

**Route**: `/orderMenuItem/{orderId:int}`

**Description**:
The Order Menu Item page allows users to add or remove menu items for a specific order. It displays the menu items grouped by type and provides controls to increment or decrement the item count.

**Key Elements**:

- **Parameter Binding**: Uses route parameter binding to get the order ID.
- **Data Manipulation**: Contains methods to increment and decrement item counts, updating the order accordingly.

**Code Breakdown**:

```razor
@page "/orderMenuItem/{orderId:int}"
@using SKYM_Blazor.Models
@rendermode InteractiveServer
@inject HttpClient httpClient;
@inject IConfiguration configuration;

<PageTitle>Order Menu Item</PageTitle>

<div class="container">
    <h1>Choose items for the Order</h1>
    <a href="#">back to home page</a>
    <div class="row">
        @foreach (var item in menuItems.GroupBy(a => a.Type).Select(a => new { a.Key, Data = a.ToList() }))
        {
            <div class="col-6">
                <h2>@((MenuType)item.Key)</h2>
                <table class="table">
                    <tbody>
                        @foreach (var subItem in item.Data)
                        {
                            <tr>
                                <td>@subItem.Name</td>
                                <td>@subItem.Price</td>
                                <td>
                                    <span class="text-center btn btn-danger" @onclick="async() => await DecreaseCount(subItem.Id)">-</span>
                                    <span class="text-secondary">@subItem.Count</span>
                                    <span class="text-primary">@orderMenuItems.FirstOrDefault(a => a.MenuId == subItem.Id)?.Count</span>
                                    <span class="text-center btn btn-success" @onclick="async()=>await IncrementCount(subItem.Id)">+</span>
                                </td>
                            </tr>
                        }
                    </tbody>
                </table>
            </div>
        }
    </div>
</div>

@code {
    [Parameter]
    public int orderId { get; set; }
    private List<Models.OrderMenu> orderMenuItems = new();
    private List<Models.Menu> menuItems = new();
    protected async override Task OnInitializedAsync()
    {
        await Refresh();
    }
    private async Task Refresh()
    {
        string apiUrl = configuration.GetValue<string>("api_url") + "/GetOrderMenuItems?orderId=" + orderId;
        string apiUrlMenus

 = configuration.GetValue<string>("api_url") + "/GetMenus";
        orderMenuItems = await httpClient.GetFromJsonAsync<List<Models.OrderMenu>>(apiUrl);
        menuItems = await httpClient.GetFromJsonAsync<List<Models.Menu>>(apiUrlMenus);
    }
    private async Task IncrementCount(int menuId)
    {
        string apiUrl = configuration.GetValue<string>("api_url") + "/IncrementMenuItemCount";
        await httpClient.PostAsJsonAsync(apiUrl, new OrderMenu { MenuId = menuId, OrderId = orderId });
        await Refresh();
    }
    private async Task DecreaseCount(int menuId)
    {
        string apiUrl = configuration.GetValue<string>("api_url") + "/DecreaseMenuItemCount";
        await httpClient.PostAsJsonAsync(apiUrl, new OrderMenu { MenuId = menuId, OrderId = orderId });
        await Refresh();
    }
}
```

---

### Additional Notes

- The application leverages Blazor's component model, enabling the creation of reusable and maintainable UI components.
- Integration with backend APIs is facilitated through `HttpClient`, configured to use base URLs defined in the `appsettings.json`.
- Form validation and submission are handled using Blazor's `EditForm` component, ensuring robust data handling and user interactions.
- The application structure promotes clean separation of concerns, making it easier to manage and extend.

---
