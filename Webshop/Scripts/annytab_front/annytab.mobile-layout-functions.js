// Initialize when the form is loaded
$(document).ready(start);

// Start this instance
function start()
{
    // Register events
    $(document).on("click", "#toggleMobileMenu", toggleMobileMenu)
    $(document).mouseup(hideMobileMenu);

    // Check if we should animate the cart update
    if ($("#cartUpdated").attr("data-cart-updated") == "true")
    {
        // Animate the cart container
        $("#mobileCartContainer").effect("highlight", { color: '#ff0000' }, 1000).dequeue().effect("bounce", 1000);
    }

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
    /// Get the mobile menu
    var mobileMenu = $("#mobileMenu");

    if (mobileMenu.is(":hidden") == false && $("#toggleMobileMenu").is(event.target) == false
        && mobileMenu.is(event.target) == false && mobileMenu.has(event.target).length == 0)
    {
        mobileMenu.slideToggle();
    }

} // End of the hideMobileMenu method