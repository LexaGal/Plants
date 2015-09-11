namespace Planting.PlantRequirements
{
    public class SoilPh : MeasurableParameter
    {
        public SoilPh(uint optimalSoilPh, uint minSoilPh, uint maxSoilPh)
            : base(optimalSoilPh, minSoilPh, maxSoilPh)
        {
            Type = MeasurableTypesEnum.SoilPh;
        }
    }
}