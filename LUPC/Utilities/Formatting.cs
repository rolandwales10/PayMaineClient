using System;

namespace LUPC.Utilities
{
    public static class Formatting2
    {
        public static string displayAsCurrency(decimal? input)
        {
            var currencyFormat = String.Format("{0:C}", input);
            //if (String.IsNullOrEmpty(currencyFormat)) currencyFormat = "$";
            return currencyFormat;
        }

        public static string displayAsCurrency(decimal input)
        {
            return String.Format("{0:C}", input);
        }

        public static decimal? stripCurrencyFormatting(string input)
        {
            decimal? output = null;
            if (!String.IsNullOrEmpty(input))
            {
                try
                {
                    input = input.Replace(",", "");
                    input = input.Replace("$", "");
                    if (input.Length > 0)
                        output = Convert.ToDecimal(input);
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
            return output;
        }

        //public static decimal copyDecimalAmount(string input, ModelState ModelStateParm, string fieldName, string fieldLabel = "")
        //{
        //    decimal output = 0;
        //    decimal? stripOutput = 0;
        //    try
        //    {
        //        if (!String.IsNullOrEmpty(input))
        //        {
        //            stripOutput = Utilities.Formatting.stripCurrencyFormatting(input);
        //            if (stripOutput != null)
        //                output = (decimal)stripOutput;
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        ModelStateParm.AddModelError(fieldName, "Invalid number for " + (fieldLabel.Length > 0 ? fieldLabel : fieldName));
        //    }
        //    return output;
        //}

        public static string nullableDateToString(DateTime? dt)
        {
            return dt.HasValue ? dt.Value.ToString("MM/dd/yyyy") : "";
        }
    }
}