# EF Core 5 - Generic Repository Pattern

Simple Generic Repository Pattern based on EF core 5 and Dependency Injection.
To create any type of Repository based on the Context's DBSets.

### How to use

In Sturtup after calling AddDbContext<TContext> you can call AddEFRepositoryPattern<TContext>()

```C#
public void ConfigureServices(IServiceCollection services)
{
    services.AddDbContext<MyContext>(options =>
    {
        options.UseInMemoryDatabase(databaseName: $"Select");
    });

    services.AddEFRepositoryPattern<MyContext>();

    ...
}

public class MyContext : DbContext
{
    public DbSet<Customer> Customers { get; set; }
    public DbSet<Order> Orders { get; set; }
}
```

That's all, now you can use all reporitory you want only doing this
```C#
public class MyService {
    private readonly IRepositoryManager<MyContext> repositoryManager;

    public MyService(IRepositoryManager<MyContext> repositoryManager)
    {
        this.repositoryManager = repositoryManager;
    }

    public async Task AddSaveChanges()
    {
        // Create your Repository of Type Customer
        var repCustomer = repositoryManager.GenerateModelRepository<Customer>();
        var bCustomer = new Customer() { Id = 1, City = "New York", FirstName = "Liam" };

        // Adding some customers to DBSet
        await repCustomer.AddModelAsync(bCustomer);
        await repCustomer.AddModelAsync(new Customer() { Id = 2, City = "San Diego", FirstName = "Liam" });
        await repCustomer.AddModelAsync(new Customer() { Id = 3, City = "Los Angeles" });
        await repCustomer.AddModelAsync(new Customer() { Id = 5, City = "New York", FirstName = "Noah" });
        await repCustomer.AddModelAsync(new Customer() { Id = 4, City = "Dallas" });

        // Create your Repository of Type Order
        var repOrder = repositoryManager.GenerateModelRepository<Order>();

        // Adding some customers to DBSet
        await repOrder.AddModelAsync(new Order() { Id = 1, TotalAmount= 100, Customer = bCustomer });
        await repOrder.AddModelAsync(new Order() { Id = 2, TotalAmount= 22, Customer = bCustomer });
        await repOrder.AddModelAsync(new Order() { Id = 3, TotalAmount= 67, Customer = bCustomer });

        // Save all in DB
        await repositoryManager.SaveChangesAsync();
    }

    public async Task Get()
    {
        // Create your Repository of Type Customer
        var repCustomer = repositoryManager.GenerateModelRepository<Customer>();

        // Get First Customer where City == "New York"
        var b = await repCustomer.GetFirstModelAsync(x => x.City == "New York");

        // Get List of Customers where FirstName == Liam orderedBy City and include Orders
        var c = await repCustomer.GetListModelsAsync(
            x => x.FirstName == "Liam",
            x => x.OrderBy(y => y.City),
            x => x.Include(y => y.Orders)
        );

        // Get AllList of Customers Order by City and FirstName
        var d = await repCustomer.GetListModelsAsync(
            orderByFunc: x => x.OrderBy(y => y.City).ThenBy(y => y.FirstName)
        );
    }
}
```
    
The method to search for a pattern by key is available. To use the GetModelByKeyAsync method, the model created via the GenerateModelRepository method must inherit from IBasePropertyKey<TKey> (otherwise the method will throw an error). TKey will set the type of the property id.

```C#
public class OrderItem: IBasePropertyKey<int>
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }
    public int OrderId { get; set; }
}
    
public async Task GetExample(){
    var repOrderItem = repositoryManager.GenerateModelRepository<OrderItem>();
    
    // OrderItem inherit from IBasePropertyKey<TKey> then return correct OrderItem
    var ordItem = await repOrderItem.GetModelByKeyAsync(2);
    
    var repOrder = repositoryManager.GenerateModelRepository<Order>();
    
    // Order not inherit from IBasePropertyKey<TKey> then Throw an ArgumentException
    var ord = await repOrder.GetModelByKeyAsync((2);
}
```
    
### Support
    
Any help is welcome. Bug insertion, new methods to implement, code improvement...
And don't forget, if you want to support me
    
<a href="https://www.buymeacoffee.com/DenMic" target="_blank"><img src="https://cdn.buymeacoffee.com/buttons/v2/default-yellow.png" alt="Buy Me A Coffee" style="height: 60px !important;width: 217px !important;" ></a>
