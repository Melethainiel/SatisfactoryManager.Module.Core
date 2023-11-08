using SatisfactoryManager.Module.Core.Models.Dto;
using SatisfactoryManager.Module.Core.Models.Json;

namespace SatisfactoryManager.Module.Core.Mappers;

public static class FuelMapper
{
    public static FuelDto ToDto(this Fuel fuel)
    {
        return new FuelDto(
            fuel.MFuelClass,
            fuel.MSupplementalResourceClass,
            fuel.MByproduct,
            fuel.MByproductAmount);
    }
}