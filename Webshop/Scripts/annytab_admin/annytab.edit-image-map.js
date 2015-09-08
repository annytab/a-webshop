/*
* annytab.edit-image-map, v1.0.0
* A jQuery plugin to edit a image map.
* Requires: jQuery v1.0,
*/
(function ($) {

    $.fn.editImageMap = function (options) {

        // Options
        var opts = $.extend({}, $.fn.editImageMap.defaults, options);

        // Variables
        var imageMapContainer = $(this);
        var movingImagePoint = null;
        var imageMapPointsControl = $("#" + opts.imageMapPointsControlId);
        var productTitlesControl = $("#" + opts.productTitlesControlId);
        var imageMapBackgroundImageControl = $("#" + opts.imageMapBackgroundImageId);
        var containerWidth = opts.containerWidth;
        var containerHeight = opts.containerHeight;
        var zoomInCursor = opts.zoomInCursor;
        var zoomOutCursor = opts.zoomOutCursor;

        // Register events
        imageMapContainer.on("mousemove", moveImagePoint);
        imageMapContainer.on("mousedown", ".annytab-imap-draggable", setImagePoint);
        imageMapContainer.on("mouseup", removeImagePoint);
        imageMapContainer.on("click", ".annytab-imap-delete", deleteImagePoint);
        imageMapBackgroundImageControl.click(toggleFullscreen);
        $("input[data-id='" + opts.addImageMapPointButtonDataId + "']").on("click", addImagePoint);
        
        // Load the image map
        loadImageMap();

        // Return this object to allow chaining
        return this;

        // Load the image map
        function loadImageMap()
        {
            // Set the cursor
            imageMapBackgroundImageControl.css("cursor", zoomInCursor);

            // Get the image map point data
            var imageMapPointData = imageMapPointsControl.val().split("|");
            var productTitlesData = productTitlesControl.val().split("|");

            // Scroll to the top
            imageMapContainer.scrollTop("0");
            imageMapContainer.scrollLeft("0");

            // Make sure that there is points
            if (imageMapPointData == null || imageMapPointData[0] == "")
            {
                return;
            }
                
            // Create image points
            for (var i = 0; i < imageMapPointData.length; i++)
            {
                // Get the map point data (id;name;x;y)
                var mapPoint = imageMapPointData[i].split(";");

                // Create the message box
                var messageBox = "<div class='annytab-imap-box'><div class='annytab-imap-delete'></div>"
                + "<div class='annytab-imap-box-heading'>" + productTitlesData[i] + "</div></div>";

                // Create the image map point
                var div = "<div data-product='" + mapPoint[0] + "' class='annytab-imap-image-point' style='top:" + mapPoint[2] + ";left:" + mapPoint[1]
                + ";'><div class='annytab-imap-draggable'></div>" + messageBox + "</div>";

                // Add the image map point to the image map
                imageMapContainer.append(div);
            }

        } // End of the loadImageMap method

        // Add a image point
        function addImagePoint(event)
        {
            // Get the selected product
            var product_id = $(this).attr("data-product-id");
            var product_title = $(this).attr("data-product-title");

            // Create the message box
            var messageBox = "<div class='annytab-imap-box'><div class='annytab-imap-delete'>X</div>"
                + "<div class='annytab-imap-box-heading'>" + product_title + "</div></div>";

            // Create the image point
            var div = "<div data-product='" + product_id + "' class='annytab-imap-image-point'><div class='annytab-imap-draggable'></div>" + messageBox + "</div>";

            // Add the image point to the image map
            imageMapContainer.append(div);

            // Scroll to the top
            imageMapContainer.scrollTop("0");
            imageMapContainer.scrollLeft("0");

            // Update the image map
            updateImageMapPoints();

        } // End of the addImagePoint method

        // Set the image point that is moving
        function setImagePoint(event)
        {
            // Prevent default behavior to get expected drag behaviour
            event.preventDefault();

            // Get the imagepoint
            var imagePoint = $(this);

            // Set the moving image point if it not is set before
            if (movingImagePoint == null)
            {
                movingImagePoint = imagePoint;
            }

        } // End of the setImagePoint method

        // Remove the image point
        function removeImagePoint()
        {
            if (movingImagePoint != null)
            {
                movingImagePoint = null;
            }
                
        } // End of the removeImagePoint method

        // Move the image point
        function moveImagePoint(event)
        {
            // Make sure that the image point is under movement
            if (movingImagePoint != null)
            {
                // Get the div for the image that caused the event
                var div = movingImagePoint.closest(".annytab-imap-image-point");

                // Get the message box
                var messageBox = div.find(".annytab-imap-box");

                // Get x and y
                var x = event.pageX - imageMapBackgroundImageControl.offset().left - 12;
                var y = event.pageY - imageMapBackgroundImageControl.offset().top - 12;

                // Change the position of the div
                div.css("left", x + "px");
                div.css("top", y + "px");

                // Update the image map
                updateImageMapPoints();
            }

        } // End of the moveImagePoint method

        // Delete the image point
        function deleteImagePoint()
        {
            // Get the image that caused the click
            var deleteButton = $(this);

            // Get the div to delete
            var div = deleteButton.closest(".annytab-imap-image-point");

            // Remove the div
            div.remove();

            // Update the image map
            updateImageMapPoints();

        } // End of the deleteImagePoint method

        // Update image map points
        function updateImageMapPoints()
        {
            // Create the data string
            var dataString = "";

            // Get all the draggable images
            var imagePoints = imageMapContainer.find(".annytab-imap-draggable").closest(".annytab-imap-image-point");

            // Get the number of divs
            var numberOfDivs = imagePoints.length;

            // Get all the divs in the image map
            for (var i = 0; i < numberOfDivs; i++)
            {
                // Get the closest div
                var div = $(imagePoints[i]);

                // Get the data
                var productId = div.data("product");
                var xPosition = div.css("left");
                var yPosition = div.css("top");

                // Add the data to the image points string
                dataString += productId + ";" + xPosition + ";" + yPosition;

                // Add the image data point divider
                if (i != (numberOfDivs - 1))
                {
                    dataString += "|";
                }
            }

            // Set the data to the hidden image data control
            imageMapPointsControl.val(dataString);

        } // End of the updateImageMapPoints method

        // Toggle fullscreen for the imap container
        function toggleFullscreen()
        {
            // Check if we should expand or make the container smaller
            if(imageMapContainer.css("position") == "fixed")
            {
                imageMapContainer.css("position", "relative");
                imageMapContainer.css("width", containerWidth);
                imageMapContainer.css("height", containerHeight);
                imageMapBackgroundImageControl.css("cursor", zoomInCursor);
            }
            else
            {
                imageMapContainer.css("position", "fixed");
                imageMapContainer.css("top", "0px");
                imageMapContainer.css("left", "0px");
                imageMapContainer.css("width", "100%");
                imageMapContainer.css("height", "100%");
                imageMapBackgroundImageControl.css("cursor", zoomOutCursor);
            }

        } // End of the toggleFullscreen method

    };

    // Plugin defaults
    $.fn.editImageMap.defaults = {
        imageMapPointsControlId: "",
        productTitlesControlId: "",
        imageMapBackgroundImageId: "",
        addImageMapPointButtonDataId: "",
        containerWidth: "",
        containerHeight: "",
        zoomInCursor: "",
        zoomOutCursor: ""
    };

}(jQuery));