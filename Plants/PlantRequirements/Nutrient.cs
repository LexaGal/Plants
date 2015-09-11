namespace Planting.PlantRequirements
{
    public class Nutrient : MeasurableParameter
    {
        public Nutrient(uint optimalNutrient, uint minNutrient, uint maxNutrient)
            : base(optimalNutrient, minNutrient, maxNutrient)
        {
            Type = MeasurableTypesEnum.Nutrient;
        }

    }
}