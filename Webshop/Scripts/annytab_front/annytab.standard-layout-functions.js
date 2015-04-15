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
    $(document).on("click", "#toggleMobileMenu", toggleMobileMenu)
    $(document).mouseup(hideMobileMenu);

    // Check if we should animate the cart update
    if ($("#cartUpdated").attr("data-cart-updated") == "true")
    {
        // Animate cart containers
        $("#cartContainer").effect("highlight", { color: '#ff0000' }, 1000).dequeue();
        $("#mobileCartContainer").effect("highlight", { color: '#ff0000' }, 1000).dequeue().effect("bounce", 1000);
    }

} // End of the start method

// Toggle the visibility of the menu
function toggleMobileMenu()
{
    if ($(window).width() < 1046)
    {
        // Get the mobile menu
        var mobileMenu = $("#mobileMenu");

        // Toggle the visibility for the menu
        mobileMenu.slideToggle(500);
    }
    
} // End of the toggleMobileMenu method

// Hide the mobile menu
function hideMobileMenu(event)
{
    if ($(window).width() < 1046)
    {
        /// Get the mobile menu
        var mobileMenu = $("#mobileMenu");

        if (mobileMenu.is(":hidden") == false && $("#toggleMobileMenu").is(event.target) == false
            && mobileMenu.is(event.target) == false && mobileMenu.has(event.target).length == 0) {
            mobileMenu.slideToggle();
        }
    }

} // End of the hideMobileMenu method


