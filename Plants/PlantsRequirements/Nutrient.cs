namespace Planting.PlantsRequirements
{
    public class Nutrient : MeasurableParameter
    {
        public Nutrient(int optimalNutrient, int minNutrient, int maxNutrient)
            : base(optimalNutrient, minNutrient, maxNutrient)
        {
            Type = MeasurableTypesEnum.Nutrient;
        }

    }
}