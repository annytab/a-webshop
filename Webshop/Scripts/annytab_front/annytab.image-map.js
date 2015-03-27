/*
* annytab.image-map, v1.0.0
* A jQuery plugin to show an image map with points
* Requires: jQuery v1.0
*/
(function ($) {

    $.fn.imageMap = function (options) {

        // Options
        var opts = $.extend({}, $.fn.imageMap.defaults, options);

        // Variables
        var imageMapContainer = $(this);
        var imageMap = $("#" + opts.imageMapId);
        var imageMapBackgroundImageControl = $("#" + opts.imageMapBackgroundImageId);
        var imageMapButtonClass = opts.imageMapButtonClass;
        var containerWidth = opts.containerWidth;
        var containerHeight = opts.containerHeight;
        var zoomInCursor = opts.zoomInCursor;
        var zoomOutCursor = opts.zoomOutCursor;
        var showProductString = opts.showProductString;

        // Register events
        imageMapContainer.on("click", "." + imageMapButtonClass, loadImageMap);
        imageMapContainer.on("click", ".annytab-imap-point", showMessageBox);
        imageMapContainer.on("click", ".annytab-imap-close", closeMessageBox);
        imageMapBackgroundImageControl.click(toggleFullscreen);

        // Find the first image and click on it
        $(document).find("." + imageMapButtonClass).first().click();

        // Return this object to allow chaining
        return this;

        // Load the image map
        function loadImageMap()
        {
            // Set the cursor
            imageMapBackgroundImageControl.css("cursor", zoomInCursor);

            // Remove all the image map points in the image map
            imageMap.find(".annytab-imap-point").remove();

            // Get the image src
            var imageSrc = $(this).data("src");

            // Get the data points string
            var dataString = $(this).data("points");

            // Get data points
            var dataPoints = dataString.split("|");

            // Set the main image with effects
            imageMapBackgroundImageControl.effect("drop", 500, function () {
                imageMapBackgroundImageControl.attr("src", imageSrc);
            }).effect("slide", 500);

            // Reset the scroll to the top left corner
            imageMap.scrollTop("0");
            imageMap.scrollLeft("0");

            // Make sure that there is points
            if (dataPoints[0] == "")
            {
                return;
            }

            // Create image points
            for (var i = 0; i < dataPoints.length; i++)
            {
                // Get the map point (id;title;price;image;link;x;y)
                var mapPoint = dataPoints[i].split(";");

                // Create the image point
                var messageBox = '<div data-product-id="' + mapPoint[0] + '" class="annytab-imap-box">'
                    + '<div class="annytab-imap-close">X</div>'
                    + '<div class="annytab-imap-box-heading"><a href="' + mapPoint[4] + '">' + mapPoint[1] + '</a></div>'
                    + '<a href="' + mapPoint[4] + '"><img class="annytab-imap-product-image" alt="' + mapPoint[1] + '" src="' + mapPoint[3] + '" /></a><br />'
                    + '<div class="annytab-imap-price-tag">' + mapPoint[2] + '</div>'
                    + '<a class="annytab-imap-product-button" href="' + mapPoint[4] + '">' + showProductString + '</a>'
                    + '</div>';
                var controlText = '<div class="annytab-imap-point" style="left:' + mapPoint[5] + ';top:' + mapPoint[6] + ';">'
                    + '<div class="annytab-imap-circle"></div>'
                    + messageBox
                    + '</div>';

                // Add the image map point
                imageMap.append(controlText);
            }

        } // End of the loadImageMap method

        // Show the message box
        function showMessageBox(event)
        {
            // Close all the messageboxes in the map
            imageMap.find(".annytab-imap-box").hide();

            // Get the image map point
            var imageMapPoint = $(this);

            // Get the message box
            var messageBox = imageMapPoint.find(".annytab-imap-box");

            // Show the message box
            messageBox.effect("shake", 500);

        } // End of the showMessageBox method

        // Close the message box
        function closeMessageBox(event)
        {
            // Make sure that there is no bubble up
            event.stopPropagation();

            // Get the message box
            var messageBox = $(this).parent();

            // Hide the message box
            messageBox.effect("fold", 1000);

        } // End of the closeMessageBox method

        // Toggle fullscreen for the imap container
        function toggleFullscreen()
        {
            // Check if we should expand or make the container smaller
            if (imageMap.css("position") == "fixed")
            {
                imageMapContainer.css("z-index", "auto");
                imageMap.css("position", "relative");
                imageMap.css("width", containerWidth);
                imageMap.css("height", containerHeight);
                imageMapBackgroundImageControl.css("cursor", zoomInCursor);
            }
            else
            {
                imageMapContainer.css("z-index", 5);
                imageMap.css("position", "fixed");
                imageMap.css("top", "0px");
                imageMap.css("left", "0px");
                imageMap.css("width", "100%");
                imageMap.css("height", "100%");
                imageMapBackgroundImageControl.css("cursor", zoomOutCursor);
            }

        } // End of the toggleFullscreen method

    };

    // Plugin defaults
    $.fn.imageMap.defaults = {
        imageMapId: "",
        imageMapBackgroundImageId: "",
        imageMapButtonClass: "",
        containerWidth: "auto",
        containerHeight: "400px",
        zoomInCursor: "auto",
        zoomOutCursor: "auto",
        showProductString: ""
    };

}(jQuery));