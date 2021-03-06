﻿namespace InformationSearchBasics.Libs.TermFrequencyParamsCalculation.Common
{
    public class FrequencyCalculationResultBase
    {
        public FrequencyCalculationResultBase(string term, double value)
        {
            Term = term;
            Value = value;
        }

        public string Term { get; set; }

        public double Value { get; set; }
    }
}