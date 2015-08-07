// Initialize when the form is loaded
$(document).ready(start);

// Start this instance
function start() 
{
    // Register events
    $(document).on("keydown", "input:text, input:file, input[type=number], input:password, input:radio, input:button, input:checkbox, button, select", enterAsTab);
    $(document).bind("submit", showLoadingAnimation);
    $(document).on("click", "#btnCookieConsent", setCookieConsent);

    // Check if there is a cookie consent
    if (readCookie("CookieConsent") == null)
    {
        $('.annytab-cookie-consent').fadeIn(200);
    }

} // End of the start method

// Make enter work as tab
function enterAsTab(event)
{
    // Check if the enter key is pressed
    if (event.keyCode == 13)
    {
        // Get the current control
        var control = $(this);

        // Check if enter was pressed in the search textbox
        if (control.attr("id") == "txtSearch")
        {
            // Submit the form
            return true;
        }

        // Get all the controls that can gain focus :::: filter(":not([readonly])").
        var controls = $(document).find("button, input, textarea, select").filter("[tabindex!='-1']").filter(":visible");

        // Get the index of the current control
        var index = controls.index(control);

        // Set focus to the next control
        if (index > -1 && (index + 1) < controls.length)
        {
            if (controls.eq(index + 1) != null)
                controls.eq(index + 1).focus();
            else
                return true;
        }

        return false;
    }

} // End of the enterAsTab method

// Show the loading animation
function showLoadingAnimation()
{
    // Get the animation container
    var animationContainer = $("#animationImage");

    // Start and fade in the spinner
    animationContainer.spin({ lines: 13, length: 34, width: 8, radius: 35, corners: 1, rotate: 0, direction: 1,
        speed: 1, trail: 60, shadow: true, hwaccel: false, className: 'spinner', zIndex: 20, top: '50%',
        left: '50%' }, '#000');
    $("#animationContainer").fadeIn(200);

} // End of the showLoadingAnimation method

// Set the cookie consent
function setCookieConsent()
{
    // Create a cookie
    createCookie("CookieConsent", "", 360);

    // Hide the cookie message
    $('.annytab-cookie-consent').fadeOut(200);

} // End of the setCookieConsent method

// Create a cookie
function createCookie(name, value, days)
{
    if (days)
    {
        var date = new Date();
        date.setTime(date.getTime() + (days * 24 * 60 * 60 * 1000));
        var expires = "; expires=" + date.toGMTString();
    }
    else
    {
        var expires = "";
    }

    document.cookie = name + "=" + value + expires + "; path=/";

} // End of the createCookie method

// Read a cookie
function readCookie(name)
{
    var nameEQ = name + "=";
    var ca = document.cookie.split(';');
    for (var i = 0; i < ca.length; i++)
    {
        var c = ca[i];
        while (c.charAt(0) == ' ') c = c.substring(1, c.length);
        if (c.indexOf(nameEQ) == 0) return c.substring(nameEQ.length, c.length);
    }

    return null;

} // End of the readCookie method

// Delete a cookie
function eraseCookie(name)
{
    createCookie(name, "", -1);

} // End of the eraseCookie method