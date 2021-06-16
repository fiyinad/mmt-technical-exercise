using System;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Customers.Clients;
using Customers.Database.Repository;
using Customers.Models.Requests;
using Customers.Models.Responses;
using Customers.Response;
using FluentValidation;
using Microsoft.Extensions.Logging;

namespace Customers.Services
{
  public class CustomersService : BaseService, ICustomersService
  {
    private readonly ILogger<CustomersService> _logger;
    private readonly IValidator<CustomerOrderRequest> _customerOrderRequestValidator;
    private readonly ICustomerClient _customerClient;
    private readonly IOrdersRepository _ordersRepository;
    private readonly IMapper _mapper;

    public CustomersService(
      ILogger<CustomersService> logger,
      IValidator<CustomerOrderRequest> customerOrderRequestValidator,
      ICustomerClient customerClient,
      IOrdersRepository ordersRepository,
      IMapper mapper) : base(logger)
    {
      _logger = logger;
      _customerOrderRequestValidator = customerOrderRequestValidator;
      _customerClient = customerClient;
      _ordersRepository = ordersRepository;
      _mapper = mapper;
    }

    
    /// <inheritdoc />
    public async Task<IResponse<CustomerOrderResponse>> GetOrderAsync(CustomerOrderRequest request)
    {
      try
      {
        // validate
        // TODO: cleanup. perhaps move to controller 
        var validationResult =
          await _customerOrderRequestValidator.ValidateAsync(request);
        if (!validationResult.IsValid)
        {
          string errorMessage = string.Join(Environment.NewLine, validationResult.Errors);
          return BadRequest<CustomerOrderResponse>(errorMessage);
        }

        // retrieve customer details via client
        var customerDetailsResult =
          await _customerClient.GetCustomerDetailsAsync(request.User);
        if (!customerDetailsResult.HasSucceeded())
        {
          return new Response.Response<CustomerOrderResponse>(customerDetailsResult.StatusCode);
        }

        // using customer details retrieve most recent order from DB
        var allCustomerOrders =
          await _ordersRepository.GetAllByCustomerIdAsync(customerDetailsResult.Data.CustomerId);
        var mostRecentOrder = allCustomerOrders.OrderByDescending(x => x.OrderDate).FirstOrDefault();

        // mapping
        // TODO: cleanup, perhaps move logic to private method or handler class
        var customerDetails = customerDetailsResult?.Data;
        var customerOrderResponse = new CustomerOrderResponse
        {
          Customer = _mapper.Map<Customer>(customerDetails),
          Order = _mapper.Map<Order>(mostRecentOrder ?? new Database.Entities.Orders())
        };
        if (null != customerOrderResponse.Order)
        {
          customerOrderResponse.Order.DeliveryAddress = $"{customerDetails.HouseNumber} {customerDetails.Street}, {customerDetails.Town} {customerDetails.PostCode}";
          if (mostRecentOrder != null && mostRecentOrder.ContainsGift)
          {
            customerOrderResponse.Order.OrderItems.ForEach(ord => {
              ord.Product = "Gift";
            });
          }
        }


        return Success<CustomerOrderResponse>(customerOrderResponse);
      }
      catch (Exception ex)
      {
        _logger.LogError(ex, ex.Message);
        return Failure<CustomerOrderResponse>(ex.Message);
      }
    }
  }
}