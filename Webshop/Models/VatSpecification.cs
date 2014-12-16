using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

/// <summary>
/// This class represent a vat specification
/// </summary>
public class VatSpecification
{
    #region Variables

    public decimal price_amount;
    public decimal vat_amount;

    #endregion

    #region Constructors

    /// <summary>
    /// Create a new vat specification with default properties
    /// </summary>
    public VatSpecification()
    {
        // Set values for instance variables
        this.price_amount = 0;
        this.vat_amount = 0;

    } // End of the constructor

    #endregion

    #region Get methods

    /// <summary>
    /// Get vat specifications as a dictionary
    /// </summary>
    /// <param name="orderRows">A list of order rows</param>
    /// <param name="decimalMultiplier">A decimal multiplier</param>
    /// <returns>A dictionary with vat specifications</returns>
    public static Dictionary<decimal, VatSpecification> GetVatSpecifications(List<OrderRow> orderRows, Int32 decimalMultiplier)
    {
        // Create the dictionary to return
        Dictionary<decimal, VatSpecification> vatSpecificationDictionary = new Dictionary<decimal, VatSpecification>(10);

        // Loop the order rows and calculate the vat specification
        for (int i = 0; i < orderRows.Count; i++)
        {
            // Calculate the price and vat value
            decimal priceValue = Math.Round(orderRows[i].unit_price * orderRows[i].quantity * 100, MidpointRounding.AwayFromZero) / 100;
            decimal vatValue = priceValue * orderRows[i].vat_percent;

            // Check if the dicitionary contains the key
            if (vatSpecificationDictionary.ContainsKey(orderRows[i].vat_percent) == true)
            {
                // Get the vat specification
                VatSpecification vatSpecification = vatSpecificationDictionary[orderRows[i].vat_percent];

                // Add to amounts
                vatSpecification.price_amount += priceValue;
                vatSpecification.vat_amount += vatValue;
            }
            else
            {
                // Create a new vat specification
                VatSpecification vatSpec = new VatSpecification();

                // Add amounts
                vatSpec.price_amount = priceValue;
                vatSpec.vat_amount = vatValue;

                // Add the key and value to the dictionary
                vatSpecificationDictionary.Add(orderRows[i].vat_percent, vatSpec);
            }
        }

        // Round sums
        foreach(KeyValuePair<decimal, VatSpecification> entry in vatSpecificationDictionary)
        {
            // Get the vat specifiction
            VatSpecification vatSpec = entry.Value;

            // Round sums
            vatSpec.price_amount = Math.Round(vatSpec.price_amount * decimalMultiplier, MidpointRounding.AwayFromZero) / decimalMultiplier;
            vatSpec.vat_amount = Math.Round(vatSpec.vat_amount * decimalMultiplier, MidpointRounding.AwayFromZero) / decimalMultiplier;
        }

        // Return the dictionary
        return vatSpecificationDictionary;

    } // End of the GetVatSpecifications method

    #endregion

} // End of the class
