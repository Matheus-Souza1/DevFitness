using System;

namespace DevFitness.Api.Models.InputModels
{
    public class UpdateMealInputModel
    {
        public string Description { get; set; }
        public int Calories { get; set; }
        public DateTime Date { get; set; }

    }
}
