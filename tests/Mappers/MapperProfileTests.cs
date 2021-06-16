using AutoMapper;
using Customers.Mappers;
using Xunit;

namespace CustomerTests.Mappers
{
  public class MapperProfileTests
  {
    [Fact]
    public void MapperConfigurationIsValid()
    {
      // arrange 
      var mapperConfiguration = new MapperConfiguration(cfg =>
      {
        cfg.AddProfile<MapperProfile>();
      });

      // act
      IMapper mapper = new Mapper(mapperConfiguration);

      // assert 
      mapper.ConfigurationProvider.AssertConfigurationIsValid();
    }
  }
}