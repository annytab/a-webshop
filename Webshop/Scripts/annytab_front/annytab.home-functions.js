// Variables
var imageSlideShow;
var currentSlideId = parseInt(0);
var lastSlideId;
var automaticSlideshow;

// Initialize when the form is loaded
$(document).ready(start);

// Start this instance
function start()
{
    // Get the image slideshow and the pager
    imageSlideShow = $('#imageSlideShow');

    // Register events
    imageSlideShow.find('#leftArrow').click(function () {
        previousSlide();
        clearInterval(automaticSlideshow);
    });
    imageSlideShow.find('#rightArrow').click(function () {
        nextSlide();
        clearInterval(automaticSlideshow);
    });
    $(".annytab-slideshow-pager-image").click(function () {
        switchSlide($(this));
        clearInterval(automaticSlideshow);
    });
    $("#toggleImapVisibility").click(toggleImapVisibility);

    // Get all the image slides
    var imageSlides = imageSlideShow.find("img[data-img]");

    // Hide all the images in the slideshow
    imageSlides.css("opacity", "0");

    // Get the length of all the image slides
    lastSlideId = imageSlides.length;

    // Get the next slide every 10 seconds
    if (lastSlideId > 0)
    {
        // Show arrows
        imageSlideShow.find("#leftArrow").fadeIn(2000);
        imageSlideShow.find("#rightArrow").fadeIn(2000);

        // Show containers
        imageSlideShow.slideDown(1000);
        $(".annytab-slideshow-pager-container").fadeIn(200);

        // Set slideshow behaviour
        nextSlide();
        automaticSlideshow = setInterval(function () { nextSlide() }, 10000);
    }

} // End of the start method

// Show the next slide
function nextSlide()
{
    // Calculate the id for the next slide
    currentSlideId += 1;
    if (currentSlideId >= lastSlideId)
    {
        currentSlideId = parseInt(0);
    }

    // Calculate the id of the previous slide
    var previousSlideId = currentSlideId - 1;
    if (previousSlideId < 0)
    {
        previousSlideId = parseInt(lastSlideId - 1)
    }

    // Get slides
    var previousSlide = imageSlideShow.find("img[data-img='" + previousSlideId + "']");
    var nextSlide = imageSlideShow.find("img[data-img='" + currentSlideId + "']");

    // Fade out the old slide and fade in the new slide
    previousSlide.animate({ maxWidth: "10%", opacity: 0 }, 1000);
    nextSlide.animate({ maxWidth: "100%", opacity: 1.0 }, 1000);

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

    // Calculate the id of the previous slide
    var previousSlideId = currentSlideId + 1;
    if (previousSlideId >= lastSlideId)
    {
        previousSlideId = parseInt(0)
    }

    // Get slides
    var previousSlide = imageSlideShow.find("img[data-img='" + previousSlideId + "']");
    var nextSlide = imageSlideShow.find("img[data-img='" + currentSlideId + "']");

    // Fade out the old slide and fade in the new slide
    previousSlide.animate({ maxWidth: "10%", opacity: 0 }, 1000);
    nextSlide.animate({ maxWidth: "100%", opacity: 1.0 }, 1000);

} // End of the previousSlide method

// Switch the slide to the clicked slide
function switchSlide(pagerImage)
{
    // Get the slide to show
    var clickedSlideId = parseInt(pagerImage.attr('data-img'));

    // Calculate the id for slides
    var previousSlideId = currentSlideId
    currentSlideId = clickedSlideId;

    // Get slides
    var previousSlide = imageSlideShow.find("img[data-img='" + previousSlideId + "']");
    var nextSlide = imageSlideShow.find("img[data-img='" + currentSlideId + "']");

    // Fade out the old slide and fade in the new slide
    previousSlide.animate({ maxWidth: "10%", opacity: 0 }, 1000);
    nextSlide.animate({ maxWidth: "100%", opacity: 1.0 }, 1000);

} // End of the switchSlide method

// Toggle the visibility for the image map
function toggleImapVisibility()
{
    // Get the button
    var button = $(this);

    // Get the imap container
    var imapContainer = $("#annytabImapOuterContainer");

    // Get the sign of the button
    var sign = button.text();

    // Check if we should show or hide the image map
    if (sign == "+")
    {
        button.text("-")
        imapContainer.slideDown(500);
    }
    else if (sign == "-") {
        button.text("+")
        imapContainer.slideUp(500);
    }

} // End of the toggleImapVisibility method