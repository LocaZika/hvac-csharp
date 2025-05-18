using hvac_backend.database;
using hvac_backend.global.interfaces;
using hvac_backend.global.types;
using hvac_backend.users.customers.dtos;
using hvac_backend.users.customers.entities;
using hvac_backend.users.customers.response;
using hvac_backend.users.customers.enums;
using hvac_backend.utilities;
using Mapster;
using Microsoft.EntityFrameworkCore;

namespace hvac_backend.users.customers;

public class CustomerService(AppDbContext appDbContext) : IServiceRouteBase<QueryParamsCustomerDto, CustomerResponse, CustomerResponse, CreateCustomerDto, UpdateCustomerDto> {
  private readonly AppDbContext DbContext = appDbContext;
  public async Task<List<CustomerResponse>?> FindAll(QueryParamsCustomerDto queryParams) {
    var query = DbContext.Customers.AsQueryable();
    if (!string.IsNullOrWhiteSpace(queryParams.Q)) {
      query = query.Where(c => EF.Functions.ILike(c.Email, $"%{queryParams.Q}%"));
    }
    return await query.Take(10).Select(c => new CustomerResponse {
      Email = c.Email,
      Phone = c.Phone,
      Address = c.Address,
      AccountType = c.AccountType.ToEnumString(),
      IsActive = c.IsActive
    }).ToListAsync();
  }
  public async Task<CustomerResponse?> FindOne(int id) {
    var customer = await DbContext.Customers.FindAsync(id);
    if (customer == null) return null;
    return new CustomerResponse() {
      Email = customer.Email,
      Phone = customer.Phone,
      AccountType = customer.AccountType.ToEnumString(),
      IsActive = customer.IsActive,
      Address = customer.Address,
    };
  }
  public async Task<bool> Create(CreateCustomerDto customerDto) {
    var count = await DbContext.Customers
      .CountAsync(c => c.Email == customerDto.Email || c.Phone == customerDto.Phone);
    if (count > 0) {
      return false;
    }
    Customer customer = new() {
      Email = customerDto.Email!,
      Password = Bcrypt.Encode(customerDto.Password!),
      Phone = customerDto.Phone!,
      AccountType = EAccountType.Local,
      Role = Role.Customer.ToRoleString(),
      IsActive = false,
      CodeId = Uuid.Generate(),
      CodeExpire = Time.AddMinutes(5),
      Address = customerDto.Address!,
    };
    await DbContext.Customers.AddAsync(customer);
    await DbContext.SaveChangesAsync();
    return true;
  }
  public async Task<bool> Update(int id, UpdateCustomerDto customerDto) {
    var customer = await DbContext.Customers.FindAsync(id);
    if (customer == null) return false;
    customerDto.Adapt(customer);
    DbContext.Customers.Update(customer);
    await DbContext.SaveChangesAsync();
    return true;
  }
  public async Task<bool> Remove(int id) {
    var customer = await DbContext.Customers.FindAsync(id);
    if (customer == null) return false;
    DbContext.Customers.Remove(customer);
    await DbContext.SaveChangesAsync();
    return true;
  }
}
