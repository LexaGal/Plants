using System;

namespace PlantingLib.MeasurableParameters
{
    public class Nutrient : MeasurableParameter
    {
        public Nutrient(int optimalNutrient, int minNutrient, int maxNutrient)
            : base(optimalNutrient, minNutrient, maxNutrient)
        {
            MeasurableType = MeasurableTypeEnum.Nutrient;
        }

        public Nutrient(Guid id, int optimal, int min, int max) : base(id, optimal, min, max)
        {
            MeasurableType = MeasurableTypeEnum.Nutrient;
        }
    }
}