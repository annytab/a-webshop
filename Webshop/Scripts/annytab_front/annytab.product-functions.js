// Variables
var imageSlideShow;
var imageSlides = [];
var currentSlideId = parseInt(0);
var lastSlideId = 0;
var mainProductImageContainer;
var mainProductImage;
var xCenter = 150;
var yCenter = 150;

// Initialize when the form is loaded
$(document).ready(start);

// Start this instance
function start()
{
    // Get the main product container and image
    mainProductImageContainer = $("#mainProductImageContainer");
    mainProductImage = $("#mainProductImage");

    // Register events
    $(document).on("change", "select[data-id='selectProductOption']", changeProductPriceAndCode);
    $(document).on("click", "img[data-id='otherProductImage']", changeMainProductImage);
    mainProductImageContainer.on("mousemove", zoomProductImage);
    mainProductImageContainer.on("mouseout", unzoomProductImage);
    mainProductImage.on("click", showFullscreenImage);
    $(".annytab-fullscreen-close").on("click", closeFullscreenImage);

    // Get the slideshow div
    imageSlideShow = $('#fullscreenContainer');

    // Register events
    imageSlideShow.find('#leftArrow').click(previousSlide);
    imageSlideShow.find('#rightArrow').click(nextSlide);
  
    // Get all images for the slideshow
    $("#productImageContainer").find('img[data-id="otherProductImage"]').each(function (index, value) {
        imageSlides.push($(this).attr('src'));
    });

    // Get the length of all the image slides
    lastSlideId = imageSlides.length;

    // Show arrows if there is more than one slide
    if(lastSlideId > 0)
    {
        imageSlideShow.find("#leftArrow").fadeIn(2000);
        imageSlideShow.find("#rightArrow").fadeIn(2000);
    }
    
} // End of the start method

// Change the product price and the code
function changeProductPriceAndCode()
{
    // Get default values
    var defaultPrice = parseFloat($("#hiddenProductPrice").val());
    var discount = parseFloat($("#hiddenDiscount").val());
    var defaultProductCode = $("#hiddenProductCode").val();
    var defaultManufacturerCode = $("#hiddenManufacturerCode").val();
    var conversionRate = parseFloat($("#hiddenConversionRate").val());
    var currencyBase = parseInt($("#hiddenCurrencyBase").val());
    var decimalMultiplier = parseInt($("#hiddenDecimalMultiplier").val());
    var valueAddedTaxPercent = parseFloat($("#hiddenValueAddedTaxPercent").val());
    var pricesIncludesVat = $("#hiddenPriceIncludesVat").val().toLowerCase();
    var variantImageDirectory = $("#hiddenVariantImageDirectory").val();
    var variantImageUrl = $("#hiddenVariantImageFileName").val();
    var localeCode = $("#hiddenCultureCode").val();

    // Get all the options
    var selectedProductOptions = $("#productOptions").find("option:selected");

    // Loop all of the product options
    var counter = 0;
    selectedProductOptions.each(function ()
    {
        defaultPrice += parseFloat($(this).attr("data-price"));
        defaultProductCode += $(this).attr("data-suffix");
        defaultManufacturerCode += $(this).attr("data-mpn-suffix");
        variantImageUrl = variantImageUrl.replace("[" + counter.toString() + "]", $(this).attr("data-suffix"));
        counter++;
    });

    // Convert the price by the conversion rate
    defaultPrice *= (currencyBase / conversionRate);

    // Round the price to a maximum of 2 decimals
    var ordinaryPrice = Math.round(defaultPrice * decimalMultiplier) / decimalMultiplier;
    defaultPrice = Math.round(defaultPrice * (1 - discount) * decimalMultiplier) / decimalMultiplier;

    // Add vat if prices should include vat
    if (pricesIncludesVat == "true")
    {
        ordinaryPrice += Math.round((ordinaryPrice * valueAddedTaxPercent) * decimalMultiplier) / decimalMultiplier;
        defaultPrice += Math.round((defaultPrice * valueAddedTaxPercent) * decimalMultiplier) / decimalMultiplier;
    }

    // Get the product price control
    var productPrice = $("#txtProductPrice");

    // Update the product price
    $({ value: 0 }).animate({ value: defaultPrice }, {
        duration: 1000,
        easing: 'swing', // can be anything
        step: function () {
            this.value = Math.round(this.value * decimalMultiplier) / decimalMultiplier;
            productPrice.text(this.value.toLocaleString(localeCode));
        },
        complete: function () {
            productPrice.text(defaultPrice.toLocaleString(localeCode));
        }
    });

    // Update the ordinary price
    $("#txtOrdinaryPrice").fadeOut(500, function () { $(this).text(ordinaryPrice.toLocaleString(localeCode)).fadeIn(500); });

    // Update product codes
    $("#txtProductCode").fadeOut(500, function () { $(this).text(defaultProductCode).fadeIn(500); });
    $("#txtManufacturerCode").fadeOut(500, function () { $(this).text(defaultManufacturerCode).fadeIn(500); });

    // Update the main image
    if(variantImageUrl != "")
    {
        mainProductImage.fadeOut(1000, function () { mainProductImage.attr("src", variantImageDirectory + variantImageUrl); }).fadeIn(1000);
    }

} // End of the changeProductPriceAndCode method

// Change the main product image
function changeMainProductImage()
{
    // Get image that was clicked
    var clickedImage = $(this);

    // Change the source of the main image
    mainProductImage.fadeOut(1000, function () {mainProductImage.attr("src", clickedImage.attr("src"));}).fadeIn(1000);

} // End of the changeMainProductImage method

// Show the next slide
function nextSlide()
{
    // Calculate the id for the next slide
    currentSlideId += 1;
    if (currentSlideId >= lastSlideId)
    {
        currentSlideId = parseInt(0);
    }

    // Set the image source
    $("#fullscreenImage").attr("src", imageSlides[currentSlideId]);

} // End of the nextSlide method

// Show the previous slide
function previousSlide()
{
    // Calculate the id of the next slide
    currentSlideId -= 1;
    if (currentSlideId < 0)
    {
        currentSlideId = parseInt(lastSlideId - 1);
    }

    // Set the image source
    $("#fullscreenImage").attr("src", imageSlides[currentSlideId]);

} // End of the previousSlide method

// Zoom the product image
function zoomProductImage(event)
{
    // Get x and y values for the mouse movement
    var x = event.pageX - mainProductImage.offset().left;
    var y = event.pageY - mainProductImage.offset().top;

    // Get the width and height for the image
    var widthValue = mainProductImage.css("width").replace("px", "");
    var heightValue = mainProductImage.css("height").replace("px", "");
    widthValue = parseFloat(widthValue);
    heightValue = parseFloat(heightValue);

    // Get the center point for the mouse
    xCenter = (event.pageX - mainProductImageContainer.offset().left);
    yCenter = (event.pageY - mainProductImageContainer.offset().top);
    widthValue = 1024;
    heightValue = 1024;

    // Calculate the scale
    var xScale = widthValue / 290;
    var yScale = heightValue / 290;

    // Calculate the center
    var xDiff = xCenter * xScale - xCenter;
    var yDiff = yCenter * yScale - yCenter;

    // Set the center of the image
    mainProductImage.animate({ top: -yDiff + "px", left: -xDiff + "px", width: widthValue + "px", height: heightValue + "px" }, { queue: false, duration: 50, easing: 'linear' });

} // End of the zoomProductImage method

// Unzoom the product image
function unzoomProductImage(event)
{
    // Get x and y values for the mouse movement
    var x = event.pageX - mainProductImage.offset().left;
    var y = event.pageY - mainProductImage.offset().top;

    // Get the width and height for the image
    var widthValue = mainProductImage.css("width").replace("px", "");
    var heightValue = mainProductImage.css("height").replace("px", "");
    widthValue = parseFloat(widthValue);
    heightValue = parseFloat(heightValue);

    // Get the center point for the mouse
    xCenter = (event.pageX - mainProductImageContainer.offset().left);
    yCenter = (event.pageY - mainProductImageContainer.offset().top);
    widthValue = 290;
    heightValue = 290;

    // Calculate the scale
    var xScale = widthValue / 290;
    var yScale = heightValue / 290;

    // Calculate the center
    var xDiff = xCenter * xScale - xCenter;
    var yDiff = yCenter * yScale - yCenter;

    // Set the center of the image
    mainProductImage.animate({ top: -yDiff + "px", left: -xDiff + "px", width: widthValue + "px", height: heightValue + "px" }, { queue: false, duration: 1000, easing: 'swing' });

} // End of the zoomProductImage method

// Show the image on fullscreen
function showFullscreenImage()
{
    // Set the image source
    $("#fullscreenImage").attr("src", mainProductImage.attr("src"));

    // Toggle the visibility
    $("#fullscreenContainer").fadeIn(1000);

} // Show a fullscreen image

// Show the image on fullscreen
function closeFullscreenImage()
{
    // Toggle the visibility
    $("#fullscreenContainer").fadeOut(1000);

} // Show a fullscreen image