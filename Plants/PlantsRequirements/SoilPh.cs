namespace PlantingLib.PlantsRequirements
{
    public class SoilPh : MeasurableParameter
    {
        public SoilPh(int optimalSoilPh, int minSoilPh, int maxSoilPh)
            : base(optimalSoilPh, minSoilPh, maxSoilPh)
        {
            MeasurableType = MeasurableTypesEnum.SoilPh;
        }
    }
}