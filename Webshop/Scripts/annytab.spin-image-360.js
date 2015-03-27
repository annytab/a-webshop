/*
* annytab.image-spin-360, v1.0.0
* A jQuery plugin to create an image that can be spinned.
* Requires: jQuery v1.0, 
*/
(function ($) {

    $.fn.imageSpin360 = function (options) {

        // Options
        var opts = $.extend({}, $.fn.imageSpin360.defaults, options);

        // Variables
        var image = $(this);
        var imageArray = null;
        var speed = opts.speed;
        var isDragging = false;
        var currentFloatImageIndex = 0;
        var currentIntImageIndex = 0;
        var previousMouseX = -1;
        
        // Register events
        image.on("mousedown touchstart", startSpinning);
        image.on("mousemove touchmove", spinImage);
        $(document).on("mouseup touchend", stopSpinning);

        // Get data
        var imageDirectory = $(this).attr("data-directory");
        var imageData = $(this).attr("data-images");

        // Create the image array
        imageArray = imageData.split("|");

        // Set the start image
        image.attr("src", imageDirectory + imageArray[currentIntImageIndex]);

        // Create the slider
        if (opts.sliderId != "")
        {
            var maxValue = imageArray.length - 1;
            var imageSlider = $("#" + opts.sliderId).slider({ step: 1, min: 0, max: maxValue, value: 0, slide: sliding });
        }

        // Return this object to allow chaining
        return this;

        // Start spinning the image
        function startSpinning(event)
        {
            // Prevent default behavior to get expected dragging behaviour
            event.preventDefault();

            if (isDragging == false)
            {
                isDragging = true;
            }

        } // End of the startSpinning method

        // Spin the image
        function spinImage(event)
        {
            // Check if the user is dragging
            if (isDragging == true)
            {
                // Get x and y values for the mouse movement
                var x = 0;
                var y = 0;

                if (event.type == 'touchmove')
                {
                    x = event.originalEvent.targetTouches[0].pageX;
                    y = event.originalEvent.targetTouches[0].pageY;
                }
                else
                {
                    x = event.pageX - image.offset().left;
                    y = event.pageY - image.offset().top;
                }

                // Calculate the delta value for x
                var deltaX = 0;
                if (previousMouseX > -1)
                {

                    // Calculate the drag direction
                    if ((x - previousMouseX) > 0)
                    {
                        deltaX = 1;
                    }
                    else if ((x - previousMouseX) < 0)
                    {
                        deltaX = -1;
                    }
                        
                    //deltaX = Math.max(-1, Math.min(1, (x - previousMouseX)));
                    currentFloatImageIndex += deltaX * speed;
                }

                // Calculate the int value of the current float value
                currentIntImageIndex = Math.floor(currentFloatImageIndex);

                // Reset values when we have reached end points
                if (currentFloatImageIndex < 0)
                {
                    currentFloatImageIndex = imageArray.length - 1;
                    currentIntImageIndex = imageArray.length - 1;
                }
                else if (currentFloatImageIndex > (imageArray.length - 1))
                {
                    currentFloatImageIndex = 0;
                    currentIntImageIndex = 0;
                }

                // Set the source of the image
                image.attr("src", imageDirectory + imageArray[currentIntImageIndex]);

                // Set the value for the slider
                imageSlider.slider("value", currentIntImageIndex);

                // Set the previous x coord for the mouse
                previousMouseX = x;
            }

        } // End of the spinImage method

        // Stop spinning the image
        function stopSpinning()
        {
            if (isDragging == true)
            {
                isDragging = false;
                previousMouseX = -1;
            }

        } // End of the stopSpinning method

        // Handle the slide event
        function sliding(event, ui)
        {
            // Set the current int index
            currentFloatImageIndex = ui.value;
            currentIntImageIndex = ui.value;

            // Reset values when we have reached end points
            if (currentFloatImageIndex < 0)
            {
                currentFloatImageIndex = imageArray.length - 1;
                currentIntImageIndex = imageArray.length - 1;
            }
            else if (currentFloatImageIndex > (imageArray.length - 1))
            {
                currentFloatImageIndex = 0;
                currentIntImageIndex = 0;
            }

            // Set the source of the image
            image.attr("src", imageDirectory + imageArray[currentIntImageIndex]);

        } // End of the sliding method
    };

    // Plugin defaults
    $.fn.imageSpin360.defaults = {
        speed: 0.2,
        sliderId: ""
    };

}(jQuery));