// Variables
var imageSlideShow;
var currentSlideId = parseInt(0);
var lastSlideId;

// Initialize when the form is loaded
$(document).ready(start);

// Start this instance
function start()
{
    // Get the image slideshow div
    imageSlideShow = $('#imageSlideShow');

    // Register events
    imageSlideShow.mouseenter(showArrows);
    imageSlideShow.mouseleave(hideArrows);
    imageSlideShow.find('#leftArrow').click(previousSlide);
    imageSlideShow.find('#rightArrow').click(nextSlide);

    // Get all the image slides
    var imageSlides = imageSlideShow.find("img[data-img]");

    // Hide all the images in the slideshow
    imageSlides.css("opacity", "0");

    // Get the length of all the image slides
    lastSlideId = imageSlides.length;

    // Get the next slide every 10 seconds
    if (lastSlideId > 0)
    {
        imageSlideShow.slideDown(1000);
        nextSlide();
        setInterval(function () { nextSlide() }, 10000);
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

// Fade in arrows
function showArrows()
{
    imageSlideShow.find("#leftArrow").fadeIn(2000);
    imageSlideShow.find("#rightArrow").fadeIn(2000);

} // End of the showArrows method

// Fade out arrows
function hideArrows()
{
    imageSlideShow.find("#leftArrow").fadeOut(2000);
    imageSlideShow.find("#rightArrow").fadeOut(2000);

} // End of the hideArrows method