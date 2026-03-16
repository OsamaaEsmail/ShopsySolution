


using System.Reflection;

namespace User.Domain.Consts;

public static class Permissions
{
    public static string Type { get; } = "permissions";

    // Products
    public const string GetProducts = "products:read";
    public const string AddProducts = "products:add";
    public const string UpdateProducts = "products:update";
    public const string DeleteProducts = "products:delete";

    // Orders
    public const string GetOrders = "orders:read";
    public const string AddOrders = "orders:add";
    public const string UpdateOrders = "orders:update";
    public const string DeleteOrders = "orders:delete";

    // Sales
    public const string GetSales = "sales:read";
    public const string AddSales = "sales:add";
    public const string UpdateSales = "sales:update";
    public const string DeleteSales = "sales:delete";

    // Cart
    public const string GetCarts = "carts:read";
    public const string ManageCarts = "carts:manage";

    // Users
    public const string GetUsers = "users:read";
    public const string AddUsers = "users:add";
    public const string UpdateUsers = "users:update";

    // Roles
    public const string GetRoles = "roles:read";
    public const string AddRoles = "roles:add";
    public const string UpdateRoles = "roles:update";


    public static IList<string> GetAllPermissions() =>
     typeof(Permissions)
         .GetFields(BindingFlags.Public | BindingFlags.Static | BindingFlags.DeclaredOnly)
         .Select(x => x.GetValue(null) as string)
         .Where(x => x is not null)
         .ToList()!;




}

