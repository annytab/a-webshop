// Initialize when the form is loaded
$(document).ready(start);

// Start this instance
function start()
{
    // Animate the background image
    $(".annytab-background-image").fadeIn(2000);

    // Check if we should animate the cart update
    if ($("#cartUpdated").attr("data-cart-updated") == "true")
    {
        // Animate the cart container
        $("#cartContainer").effect("highlight", { color: '#ff0000' }, 1000).dequeue();
    }

} // End of the start method


