// Initialize when the form is loaded
$(document).ready(start);

// Start this instance
function start()
{
    // Register events
    $(document).on("click", "#btnShowDiscountCode", toggleDiscountCode);
    $(document).on("click", "#copyInvoiceAddress", copyInvoiceAddress);
    $(document).on("change", "select[data-id='selectCountry']", changeTotalSumInCart);
    $(document).on("click", "#btnPrint", printDocument);

    // Change total sums in the cart
    changeTotalSumInCart();

} // End of the start method

// Copy the invoice address
function copyInvoiceAddress()
{
    // Copy across values
    $("#txtDeliveryName").val($("#txtInvoiceName").val());
    $("#txtDeliveryAddress1").val($("#txtInvoiceAddress1").val());
    $("#txtDeliveryAddress2").val($("#txtInvoiceAddress2").val());
    $("#txtDeliveryPostCode").val($("#txtInvoicePostCode").val());
    $("#txtDeliveryCity").val($("#txtInvoiceCity").val());
    $("#selectDeliveryCountry").val($("#selectInvoiceCountry").val());

    // Return false (to supress the form submit)
    return false;

} // End of the copyInvoiceAddress method

// Change total sums in the cart
function changeTotalSumInCart()
{
    // Get the data that we need
    var customerType = $("#hiddenCustomerType").val();
    var invoiceCountryVatCode = $("#selectInvoiceCountry").find("option:selected").attr("data-vat_code");
    var deliveryCountry = $("#selectDeliveryCountry").find("option:selected");
    var deliveryCountryVatCode = deliveryCountry.attr("data-vat_code");
    var vatCode = 0;

    // Find the vat code (0:Domestic, 1:Intra-Union Trade, 2:Export)
    if (customerType == 0 && invoiceCountryVatCode == 0 && deliveryCountryVatCode == 2)
    {
        // Export
        vatCode = 2;
    }
    else if (customerType == 0 && invoiceCountryVatCode == 1 && deliveryCountryVatCode == 1)
    {
        // Intra-Union Trade
        vatCode = 1;
    }
    else if (customerType == 0 && invoiceCountryVatCode == 1 && deliveryCountryVatCode == 2)
    {
        // Export
        vatCode = 2;
    }
    else if (invoiceCountryVatCode == 2 && deliveryCountryVatCode == 2)
    {
        // Export
        vatCode = 2;
    }

    // Calculate and set total sums in the cart
    calculateAndSetTotalSumsInCart(vatCode);

} // End of the changeTotalSumInCart method

// Calculate and set total sums in cart
function calculateAndSetTotalSumsInCart(vatCode)
{
    // Get the data that we need
    var decimalMultiplier = parseInt($("#hiddenDecimalMultiplier").val());
    var productUnitValues = $("#divTable").find("input[data-id='unitValues']");
    var localeCode = $("#hiddenCultureCode").val();

    // Declare sum variables
    var priceSum = 0;
    var freightSum = 0;
    var vatSum = 0;
    var roundingSum = 0;
    var totalSum = 0;

    // Loop products
    productUnitValues.each(function ()
    {
        // Get price values
        var price = parseFloat($(this).attr("data-price"));
        var freight = parseFloat($(this).attr("data-freight"));
        var vat = parseFloat($(this).attr("data-vat"));
        var quantity = parseFloat($(this).attr("data-quantity"));

        // Round the price and the freight
        price = Math.round(price * decimalMultiplier) / decimalMultiplier;
        freight = Math.round(freight * decimalMultiplier) / decimalMultiplier;

        // Calculate the combined price and freight value
        var priceFreightValue = (price + freight) * quantity;
        priceFreightValue = Math.round(priceFreightValue * 100) / 100;

        // Calculate the price value
        var priceValue = price * quantity;
        priceValue = Math.round(priceValue * 100) / 100;

        // Calculate the freight value
        var freightValue = freight * quantity;
        freightValue = Math.round(freightValue * 100) / 100;

        // Calculate the vat
        var vatValue = priceFreightValue * vat;

        // Add to sums
        priceSum += priceValue;
        freightSum += freightValue;
        vatSum += vatValue;

    });

    // Round sums
    priceSum = Math.round(priceSum * decimalMultiplier) / decimalMultiplier;
    freightSum = Math.round(freightSum * decimalMultiplier) / decimalMultiplier;
    vatSum = Math.round(vatSum * decimalMultiplier) / decimalMultiplier;

    // Set the vat to zero if the sale not is domestic
    if (vatCode != 0)
    {
        vatSum = 0;
    }

    // Calculate the total sum without rounding
    totalNotRounded = priceSum + freightSum + vatSum;

    // Calculate the total sum with rounding
    totalSum = Math.round(totalNotRounded);

    // Calculate the rounding
    roundingSum = Math.round((totalSum - totalNotRounded) * decimalMultiplier) / decimalMultiplier;

    // Get the controls that we need
    var netSumControl = $("#netSum");
    var freightSumControl = $("#freightSum");
    var vatSumControl = $("#vatSum");
    var roundingSumControl = $("#roundingSum");
    var totalSumControl = $("#totalSum");
    var hiddenVatCode = $("#hiddenVatCode");

    // Set values for controls
    hiddenVatCode.val(vatCode);
    netSumControl.text(priceSum.toLocaleString(localeCode));
    freightSumControl.text(freightSum.toLocaleString(localeCode));
    vatSumControl.text(vatSum.toLocaleString(localeCode));
    roundingSumControl.text(roundingSum.toLocaleString(localeCode));
    totalSumControl.text(totalSum.toLocaleString(localeCode));

} // End of the calculateAndSetTotalSumsInCart method

// Print the document
function printDocument()
{
    // Get the print text
    var printText = $("#btnPrint").text();

    // Calculate offsets
    var left = (screen.width / 2) - (800 / 2);
    var top = (screen.height / 2) - (400 / 2);

    var printArea = $("#printArea").html();
    var printWindow = window.open("", printText.toString(), "resizable=yes,scrollbars=yes,height=400,width=800,top=" + top + ",left=" + left, true);
    printWindow.document.write("<html><head><title>" + printText.toString() + "</title></head><body>");
    printWindow.document.write(printArea);
    printWindow.document.write("</body></html>");
    printWindow.document.close();

} // End of the printDocument method

// Toggle the visibility for the discount code container
function toggleDiscountCode()
{
    // Get the discount code container
    var discountCodeContainer = $("#discountCodeContainer");

    // Toggle the visibility for the container
    discountCodeContainer.slideToggle(500);

} // End of the toggleDiscountCode method