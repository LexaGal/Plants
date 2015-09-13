namespace Planting.PlantsRequirements
{
    public class SoilPh : MeasurableParameter
    {
        public SoilPh(int optimalSoilPh, int minSoilPh, int maxSoilPh)
            : base(optimalSoilPh, minSoilPh, maxSoilPh)
        {
            Type = MeasurableTypesEnum.SoilPh;
        }
    }
}