// Initialize when the form is loaded
$(document).ready(start);

// Start this instance
function start() 
{
    // Register events
    $("#sortableTable").sortable({
        items: '.annytab-sortable-row', placeholder: 'annytab-sortable-placeholder'
    });
    $("#sortableTable").disableSelection();
    $(document).on("click", "img[data-id='toggleVisibilityOptions']", toggleOptionsVisibility);
    $(document).on("click", "input[data-id='checkAllOptions']", checkAllOptions);
    $(document).on("keydown", "input:text, input:file, input[type=number], input:password, input:radio, button, input:checkbox, select", enterAsTab);
    $(document).on("click", "input[data-id='addRow']", addNewRow);
    $(document).on("click", "input[data-id='deleteRow']", deleteRow);
    $(document).on("click", "input[data-id='printOrder']", printOrder);
    $(document).on("change", "input[data-id='previewDomainImage']", previewDomainImage);
    $(document).on("change", "#uploadCampaignImage", previewCampaignImage);
    $("#uploadInspirationImage").change(previewInspirationImage);
    $(document).on("click", "#messageBoxOkButton, #messageBoxCancelButton, #messageBoxClose", unlockForm);
    $(document).on("click", "#copyInvoiceAddress", copyInvoiceAddress);
    $("#uploadMainImage").change(previewMainImage);
    $("#uploadOtherImages").change(previewOtherImages);
    $(document).on("click", "img[data-id='deleteOtherImage']", deleteOtherImage);
    $("#clearFileUpload").click(clearFileUpload);
    $(document).on("click", ".annytab-list-row-main, .annytab-list-row-alt", toggleRowBgColor);
    $("input[data-id='deletePost']").each(function()
    {
        var deleteButton = $(this);
        deleteButton.click({ url: deleteButton.attr("data-url") }, deletePostConfirmation);
    });

    // Get the message box container
    var messageBoxContainer = $("#messageBoxContainer");

    // Check for an error
    var errorCode = $("#errorCode").attr("data-error");
    if (errorCode != "0")
    {
        // Show the message box
        messageBoxContainer.find("span").css("display", "none");
        messageBoxContainer.find("[data-number='" + errorCode + "']").css("display", "inline");
        messageBoxContainer.fadeIn(500);
    }

} // End of the start method

// Unlock the form
function unlockForm()
{
    // Get the url
    var url = $(this).attr("data-url");

    if (url != null) 
    {
        // Fade out the message box
        $("#messageBoxContainer").fadeOut(500);

        // Redirect the user
        window.setTimeout(function () {
            window.location.href = url;
        }, 500);
        
        return false;
    }
    else
    {
        // Fade out the message box
        $("#messageBoxContainer").fadeOut(500);
    }

} // End of the unlockForm method

// Make enter work as tab
function enterAsTab(event)
{
    
    // Check if the enter key is pressed
    if (event.keyCode == 13)
    {
        // Get the current control
        var control = $(this);

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

// Add a new row
function addNewRow()
{
    // Get the current control
    var control = $(this);

    // Get the table
    var table = $("#sortableTable")

    // Get the current row
    var currentRow = control.closest(".annytab-sortable-row");

    if (currentRow.find(":text").val() == "")
    {
        // Get all the controls that can gain focus
        var controls = $(document).find('button, input, textarea, select').filter("[tabindex!='-1'][id!='deleteRow']");

        // Get the index of the current control
        var index = controls.index(control);

        // Set focus to the next control
        if (index > -1 && (index + 1) < controls.length)
        {
            controls.eq(index + 1).focus();
        }
    }
    else
    {
        // Clone the current row
        var tableRowClone = currentRow.clone();
        tableRowClone.find(":text").val("");

        // Add the row after the current row
        tableRowClone.insertAfter(currentRow);

        // Set focus to the new row
        currentRow.next(".annytab-sortable-row").find(":text").first().focus();
    }

} // End of the AddNewOptionRow function

// Delete a row
function deleteRow()
{
    // Count the number of rows in the table
    var table = $("#sortableTable");
    var rowCount = table.find(".annytab-sortable-row").length;

    if (rowCount > 2)
    {
        // Get the closest table row
        var tableRow = $(this).closest(".annytab-sortable-row");

        // Check if we are on the first row
        if (tableRow.prevAll().length == 1)
        {
            // Set focus to a new row
            tableRow.next().find(":text").first().focus();
        }
        else
        {
            // Set focus to a new row
            tableRow.prev().find(":text").first().focus();
        }

        // Remove the table row
        tableRow.remove();
    }
    else
    {
        // Set focus to the first textbox in the first row
        table.find(".annytab-sortable-row").eq(1).find(":text").first().focus();
    }

} // End of the deleteOptionRow method

// Toggle the visibility of options in a optiontype
function toggleOptionsVisibility()
{
    // Get the closest tr 
    var currentRow = $(this).closest("div[data-id='optionTypeContainer']");

    // Get the option input table
    var optionsInputTable = currentRow.find("div[data-id='optionsTable']").first();

    // Toggle the visibility for the table
    optionsInputTable.fadeToggle(500);

} // End of the toggleOptionsVisibility method

// Check or uncheck all options
function checkAllOptions()
{
    // Get the checkbox that is clicked
    var currentCheckbox = $(this);

    // Get the table
    var optionsTable = currentCheckbox.closest("div[data-id='optionsTable']");

    // Get all checkboxes in the table
    var checkBoxes = optionsTable.find("input:checkbox:not(:first)");

    // Toggle the checked attribute of the checkbox
    checkBoxes.prop("checked", currentCheckbox.prop("checked"));

} // End of the checkAllOptions method

// Preview domain images
function previewDomainImage(event)
{
    // Get the file upload control
    var control = $(this);
    
    // Get the file collection
    var fileCollection = event.target.files;

    // Get the file extension
    var fileExtension = control.val().substring(control.val().lastIndexOf('.') + 1);

    if(fileExtension != "jpg" && fileExtension != "jpeg")
    {
        // Replace the control
        control.replaceWith(control = control.clone(true));

        // Show the message box
        var messageBoxContainer = $("#messageBoxContainer");
        messageBoxContainer.find("span").css("display", "none");
        messageBoxContainer.find("[data-number='3']").css("display", "inline");
        messageBoxContainer.fadeIn(500);
    }
    else if (fileCollection[0].size >= 1048576)
    {
        // Replace the file upload control
        control.replaceWith(control = control.clone(true));

        // Show the message box
        var messageBoxContainer = $("#messageBoxContainer");
        messageBoxContainer.find("span").css("display", "none");
        messageBoxContainer.find("[data-number='4']").css("display", "inline");
        messageBoxContainer.fadeIn(500);
    }
    else
    {
        // Get image id
        var imageId = control.attr("data-img");

        // Create a file reader
        var reader = new FileReader();

        // Load the image
        reader.onload = function (e)
        {
            $("#" + imageId).attr("src", e.target.result);
        }

        // Read the image file
        reader.readAsDataURL(fileCollection[0]);
    }

} // End of the previewDomainImage method

// Preview the campaign image to upload
function previewCampaignImage(event)
{
    // Get the file upload control
    var control = $(this);
    
    // Get the file collection
    var fileCollection = event.target.files;

    // Make sure that there is files
    if (fileCollection[0].size >= 1048576)
    {
        // Replace the file upload control
        control.replaceWith(control = control.clone(true));

        // Show the message box
        var messageBoxContainer = $("#messageBoxContainer");
        messageBoxContainer.find("span").css("display", "none");
        messageBoxContainer.find("[data-number='4']").css("display", "inline");
        messageBoxContainer.fadeIn(500);
    }
    else 
    {
        // Create a file reader
        var reader = new FileReader();

        // Load the image
        reader.onload = function (e)
        {
            $("#campaignImage").attr("src", e.target.result);
        }

        // Read the image file
        reader.readAsDataURL(fileCollection[0]);
    }

} // End of the previewCampaignImage method

// Preview the inspiration image to upload
function previewInspirationImage(event)
{
    // Get the file upload control
    var uploadControl = $(this);

    // Get the file collection
    var fileCollection = event.target.files;

    // Make sure that there is files
    if (fileCollection[0].size >= 1048576)
    {
        // Replace the file upload control
        uploadControl.replaceWith(uploadControl = uploadControl.clone(true));

        // Show the message box
        var messageBoxContainer = $("#messageBoxContainer");
        messageBoxContainer.find("span").css("display", "none");
        messageBoxContainer.find("[data-number='4']").css("display", "inline");
        messageBoxContainer.fadeIn(500);
    }
    else
    {
        // Create a file reader
        var reader = new FileReader();

        // Load the image
        reader.onload = function (e)
        {
            $("#annytabImapImage").attr("src", e.target.result);
        }

        // Read the image file
        reader.readAsDataURL(fileCollection[0]);
    }

} // End of the previewInspirationImage method

// Preview the image to upload
function previewMainImage(event)
{
    // Get the file upload control
    var control = $(this);

    // Get the file collection
    var fileCollection = event.target.files;
    
    // Get the file extension
    var fileExtension = control.val().substring(control.val().lastIndexOf('.') + 1);

    // Make sure that there is files
    if (fileCollection[0].size >= 1048576)
    {
        // Replace the file upload control
        control.replaceWith(control = control.clone(true));

        // Show the message box
        var messageBoxContainer = $("#messageBoxContainer");
        messageBoxContainer.find("span").css("display", "none");
        messageBoxContainer.find("[data-number='4']").css("display", "inline");
        messageBoxContainer.fadeIn(500);

    }
    else if (fileExtension == "jpg" || fileExtension == "jpeg")
    {
        // Create a file reader
        var reader = new FileReader();

        // Load the image
        reader.onload = function (e) {
            $("#mainImage").attr("src", e.target.result);
        }

        // Read the image file
        reader.readAsDataURL(fileCollection[0]);
    }
    else
    {
        // Replace the control
        control.replaceWith(control = control.clone(true));

        // Show the message box
        var messageBoxContainer = $("#messageBoxContainer");
        messageBoxContainer.find("span").css("display", "none");
        messageBoxContainer.find("[data-number='3']").css("display", "inline");
        messageBoxContainer.fadeIn(500);
    }

} // End of the previewMainImage method

// Preview other images that have been added
function previewOtherImages(event)
{
    // Get the file upload control
    var control = $(this);

    // Get the file collection
    var fileCollection = event.target.files;

    // Get the other image container
    var otherImagesContainer = $("#divOtherImagesContainer");

    // Delete images that has been created temporary
    otherImagesContainer.find("[data-temp='true']").remove();

    // Get the size of all the images
    var fileSize = 0;
    for (var i = 0; i < fileCollection.length; i++)
    {
        fileSize += fileCollection[i].size;
    }

    if (fileSize > 4194304) // 4 mb
    {
        // Replace the file upload control
        control.replaceWith(control = control.clone(true));

        // Show the message box
        var messageBoxContainer = $("#messageBoxContainer");
        messageBoxContainer.find("span").css("display", "none");
        messageBoxContainer.find("[data-number='4']").css("display", "inline");
        messageBoxContainer.fadeIn(500);
    }
    else
    {
        for(var i = 0; i < fileCollection.length; i++)
        {
            // Create a file reader
            var reader = new FileReader();
            
            // Load the image
            reader.onload = function (e)
            {
                var control = "<div data-id='otherImageContainer'" + " data-temp='true' " + "class='annytab-image-container' >";
                control += "<img src='" + e.target.result + "' style='max-width:128px;' />";
                control += "</div>";

                otherImagesContainer.prepend(control);
            }

            // Read the image file
            reader.readAsDataURL(fileCollection[i]);
        }
    }

} // End of the previewOtherImages method

// Delete the image
function deleteOtherImage()
{
    // Get the div to delete
    var otherImageContainer = $(this).closest("div[data-id='otherImageContainer']");

    // Delete the image container
    otherImageContainer.remove();

} // End of the deleteOtherImage method

// Clear the file upload field
function clearFileUpload()
{
    // Get the file upload control
    var control = $("#uploadOtherImages");

    // Get the other image container
    var otherImagesContainer = $("#divOtherImagesContainer");

    // Delete images that has been created temporary
    otherImagesContainer.find("[data-temp='true']").remove();

    // Replace the file upload control
    control.replaceWith(control = control.clone(true));

    // Return false to supress submit
    return false;

} // End of the clearFileUpload method

// Give the user a chance to confirm the deletion
function deletePostConfirmation(event)
{
    // Show the message box
    var messageBoxContainer = $("#messageBoxContainer");
    messageBoxContainer.find("span").css("display", "none");
    messageBoxContainer.find("#messageBoxOkButton").attr("data-url", event.data.url);
    messageBoxContainer.find("[data-number='2']").css("display", "inline");
    messageBoxContainer.fadeIn(500);

} // End of the deletePostConfirmation method

// Copy the invoice address
function copyInvoiceAddress()
{
    // Copy across values
    $("#txtDeliveryName").val($("#txtInvoiceName").val());
    $("#txtDeliveryAddress1").val($("#txtInvoiceAddress1").val());
    $("#txtDeliveryAddress2").val($("#txtInvoiceAddress2").val());
    $("#txtDeliveryPostCode").val($("#txtInvoicePostCode").val());
    $("#txtDeliveryCity").val($("#txtInvoiceCity").val());
    $("#selectDeliveryCountry").val($("#selectInvoiceCountry").val());

    // Return false (to supress the form submit)
    return false;

} // End of the copyInvoiceAddress method

// Print the order
function printOrder()
{
    // Get the order id
    var orderId = $(this).attr("data-order");

    // Calculate offsets
    var left = (screen.width / 2) - (800 / 2);
    var top = (screen.height / 2) - (400 / 2);

    // Open the window
    window.open("/admin_orders/print/" + orderId, "", "resizable=yes,scrollbars=yes,height=400,width=800,top=" + top + ",left=" + left, true);

} // End of the printOrder method

// Toggle the background color of the row
function toggleRowBgColor()
{
    // Toggle the background color
    $(this).toggleClass("highlight");
    
} // End of the toggleRowBgColor method