// Initialize when the form is loaded
$(document).ready(start);

// Start this instance
function start()
{
    // Animate the background image
    if ($(window).width() >= 1046)
    {
        var bgImage = $("#backgroundImage");
        bgImage.attr("src", bgImage.attr("data-src"));
        bgImage.fadeIn(2000);
    }
    
    // Register events
    $(document).on("click", "#toggleMobileMenu", toggleMobileMenu);

} // End of the start method

// Toggle the visibility of the menu
function toggleMobileMenu()
{
    // Get the mobile menu
    var mobileMenu = $("#mobileMenu");

    // Toggle the visibility for the menu
    mobileMenu.slideToggle(500);

} // End of the toggleMobileMenu method

// Hide the mobile menu
function hideMobileMenu(event)
{
    if ($(window).width() < 1000)
    {
        // Get the mobile menu
        var mobileMenu = $("#mobileMenu");

        if (mobileMenu.is(":hidden") == false && $("#toggleMobileMenu").is(event.target) == false
            && mobileMenu.is(event.target) == false && mobileMenu.has(event.target).length == 0) {
            mobileMenu.slideToggle();
        }
    }

} // End of the hideMobileMenu method


