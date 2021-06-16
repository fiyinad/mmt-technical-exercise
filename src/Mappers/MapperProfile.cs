using AutoMapper;
using Customers.Clients.Responses;
using Customers.Database.Entities;
using Customers.Models.Responses;

namespace Customers.Mappers
{
  /// <summary>
  /// Manages type mapping
  /// </summary>
  public class MapperProfile : Profile
  {
    /// <summary>
    /// Creates an instance of a <see cref="MapperProfile"/>
    /// </summary>
    public MapperProfile()
    {
      CreateMap<CustomerDetails, Customer>()
        .ForMember(dest => dest.FirstName, opt => opt.MapFrom(src => src.FirstName))
        .ForMember(dest => dest.LastName, opt => opt.MapFrom(src => src.LastName))
        .ForAllOtherMembers(opts => opts.Ignore());;

      CreateMap<OrderItems, OrderItem>()
        .ForMember(dest => dest.Product, opt => opt.MapFrom(src => src.Products.ProductName))
        .ForMember(dest => dest.Quantity, opt => opt.MapFrom(src => src.Quantity))
        .ForMember(dest => dest.PriceEach, opt => opt.MapFrom(src => src.Price))
        .ForAllOtherMembers(opts => opts.Ignore());;

      CreateMap<Orders, Order>()
        .ForMember(dest => dest.OrderNumber, opt => opt.MapFrom(src => src.OrderId))
        .ForMember(dest => dest.OrderDate, opt => opt.MapFrom(src => src.OrderDate.ToString("dd-MMMM-yyyy")))
        .ForMember(dest => dest.OrderItems, opt => opt.MapFrom(src => src.OrderItems))
        .ForAllOtherMembers(opts => opts.Ignore());
    }
  }
}