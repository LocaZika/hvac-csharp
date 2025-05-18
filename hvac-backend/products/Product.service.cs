using hvac_backend.database;
using hvac_backend.global.interfaces;
using hvac_backend.products.entities;
using hvac_backend.products.dto;
using Microsoft.EntityFrameworkCore;
using Mapster;
using hvac_backend.products.dto.productImg;
using hvac_backend.products.enums;

namespace hvac_backend.products;

public class ProductService(AppDbContext ctx) : IServiceRouteBase<ProductQueryParamsDto, ProductWithImgsDto, ProductWithDetailImgsDto, CreateProductDto, UpdateProductDto> {
  private readonly AppDbContext DbContext = ctx;

  //--- Product ---//
  public async Task<List<ProductWithImgsDto>?> FindAll(ProductQueryParamsDto queryParams) {
    var query = DbContext.Products.AsQueryable();

    // Thiết lập giá trị lastPrice cho sorting
    decimal? lastPrice = queryParams.LastPrice;

    // Áp dụng điều kiện giá và sắp xếp
    if (lastPrice.HasValue == false) {
      lastPrice = 0;
    }
    if (queryParams.SortBy == ESortBy.Asc.ToEnumString()) {
      query = query.Where(p => p.Price > lastPrice);
    }
    else {
      lastPrice = await DbContext.Products.MaxAsync(p => p.Price);
      query = query.Where(p => p.Price < lastPrice);
    }

    // Áp dụng các điều kiện tìm kiếm
    if (!string.IsNullOrWhiteSpace(queryParams.Q))
      query = query.Where(p => EF.Functions.ILike(p.Name, $"%{queryParams.Q}%"));

    if (!string.IsNullOrWhiteSpace(queryParams.Brand))
      query = query.Where(p => p.Brand == queryParams.Brand);

    if (!string.IsNullOrWhiteSpace(queryParams.Type))
      query = query.Where(p => p.Type == queryParams.Type);

    if (queryParams.Transmission is not null)
      query = query.Where(p => p.Transmission.ToEnumString() == queryParams.Transmission);

    if (queryParams.Model is not null)
      query = query.Where(p => p.Model == queryParams.Model);

    // Sắp xếp theo giá
    query = queryParams.SortBy == ESortBy.Asc.ToEnumString()
      ? query.OrderBy(p => p.Price)
      : query.OrderByDescending(p => p.Price);

    // Keyset pagination với Take
    return await query
      .Include(p => p.Imgs)
      .Take(9)
      .Select(p => new ProductWithImgsDto() {
        Id = p.Id,
        Name = p.Name,
        Price = p.Price,
        Model = p.Model,
        Hp = p.Hp,
        TradeType = p.TradeType,
        Transmission = p.Transmission,
        Mileage = p.Mileage,
        Imgs = p.Imgs
          .Where(img => img.ProductId == p.Id)
          .Select(img => new ProductImgDto() {
            Path = img.Path,
          })
          .ToList(),
      })
      .ToListAsync();
  }

  public async Task<ProductWithDetailImgsDto?> FindOne(int id) {
    var product = await DbContext.Products
      .Include(p => p.Imgs)
      .Include(p => p.DetailImgs)
      .FirstOrDefaultAsync(p => p.Id == id);
    if (product == null) {
      return null;
    }
    var result = new ProductWithDetailImgsDto() {
      Name = product.Name,
      Price = product.Price,
      TradeType = product.TradeType,
      Imgs = [..product.Imgs
        .Where(i => i.ProductId == product.Id)
        .Select(i => new ProductImgDto() {
          Path = i.Path,
        })],
      DetailImgs = [.. product.DetailImgs
        .Where(di => di.ProductId == product.Id)
        .Select(di => new ProductImgDto() {
          Path = di.Path,
        })],
    };
    return result;
  }
  public async Task<bool> Create(CreateProductDto productDto) {
    bool isExisted = await DbContext.Products.AnyAsync(p => p.Name == productDto.Name && p.Brand == productDto.Brand);
    if (isExisted) {
      return false;
    }
    Product product = new() {
      Name = productDto.Name!,
      Brand = productDto.Brand!,
      Price = (decimal)productDto.Price!,
      Transmission = productDto.Transmission!.Value,
      FuelType = productDto.FuelType!.Value,
      TradeType = productDto.TradeType!.Value,
      Type = productDto.Type!,
      Model = (short)productDto.Model!,
      Mileage = (short)productDto.Mileage!,
      Vin = productDto.Vin!,
      Stock = productDto.Stock!,
      Hp = (short)productDto.Hp!,
    };
    await DbContext.Products.AddAsync(product);
    await DbContext.SaveChangesAsync();
    return true;
  }
  public async Task<bool> Update(int id, UpdateProductDto productDto) {
    var product = await DbContext.Products.FindAsync(id);
    if (product == null) {
      return false;
    }
    productDto.Adapt(product);
    DbContext.Products.Update(product);
    await DbContext.SaveChangesAsync();
    return true;
  }
  public async Task<bool> Remove(int id) {
    var product = await DbContext.Products.FindAsync(id);
    if (product == null) {
      return false;
    }
    DbContext.Products.Remove(product);
    await DbContext.SaveChangesAsync();
    return true;
  }

  //--- Product Imgs ---//
  public async Task<bool> CreateProductImg(CreateProductImgDto productImgDto) {
    var isExisted = await DbContext.Products.AnyAsync(p => p.Id == productImgDto.ProductId);
    if (!isExisted) {
      return false;
    }
    List<ProductImg>? productImgs = productImgDto.Imgs?.Select(img => new ProductImg() {
      ProductId = productImgDto.ProductId,
      Path = img,
    }).ToList();
    if (productImgs is List<ProductImg> { Count: > 0 })
      await DbContext.ProductImgs.AddRangeAsync(productImgs);
    await DbContext.SaveChangesAsync();
    return true;
  }
  public async Task<bool> CreateProductDetailImg(CreateProductImgDto productDetailImgDto) {
    var isExisted = await DbContext.Products.AnyAsync(p => p.Id == productDetailImgDto.ProductId);
    if (!isExisted) return false;
    var productDetailImgs = productDetailImgDto.Imgs?.Select(img => new ProductImg() {
      ProductId = productDetailImgDto.ProductId,
      Path = img,
    });
    if (productDetailImgs is List<ProductDetailImg> { Count: > 0 })
      await DbContext.ProductDetailImgs.AddRangeAsync(productDetailImgs);
    await DbContext.SaveChangesAsync();
    return true;
  }
}
