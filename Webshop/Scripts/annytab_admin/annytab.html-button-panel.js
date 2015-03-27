/*
* annytab.html-button-panel, v1.0.0
* A jQuery plugin to create a button panel that is used to insert html-tags in a textarea.
* Requires: jQuery v1.0, rangyinputs-jquery.js (https://github.com/timdown/rangyinputs)
*/
(function ($) {

    $.fn.htmlButtonPanel = function (options) {

        // Options
        var opts = $.extend({}, $.fn.htmlButtonPanel.defaults, options);

        // Add buttons
        this.append('<input data-id="btnAddTag" type="button" class="' + opts.buttonClass + '" style="font-weight:bold;" value="b" />');
        this.append('<input data-id="btnAddTag" type="button" class="' + opts.buttonClass + '" style="font-style:italic" value="i" />');
        this.append('<input data-id="btnAddTag" type="button" class="' + opts.buttonClass + '" value="br" />');
        this.append('<input data-id="btnAddTag" type="button" class="' + opts.buttonClass + '" value="h1" />');
        this.append('<input data-id="btnAddTag" type="button" class="' + opts.buttonClass + '" value="h2" />');
        this.append('<input data-id="btnAddTag" type="button" class="' + opts.buttonClass + '" value="h3" />');
        this.append('<input data-id="btnAddTag" type="button" class="' + opts.buttonClass + '" value="h4" />');
        this.append('<input data-id="btnAddTag" type="button" class="' + opts.buttonClass + '" value="p" />');
        this.append('<input data-id="btnAddTag" type="button" class="' + opts.buttonClass + '" value="code" />');
        this.append('<input data-id="btnAddTag" type="button" class="' + opts.buttonClass + '" value="span" />');
        this.append('<input data-id="btnAddTag" type="button" class="' + opts.buttonClass + '" value="div" />');
        this.append('<input data-id="btnAddTag" type="button" class="' + opts.buttonClass + '" value="line" />');
        this.append('<input data-id="btnAddTag" type="button" class="' + opts.buttonClass + '" value="space" />');
        this.append('<input data-id="btnAddTag" type="button" class="' + opts.buttonClass + '" value="url" />');
        this.append('<input data-id="btnAddTag" type="button" class="' + opts.buttonClass + '" value="img" />');
        this.append('<input data-id="btnAddTag" type="button" class="' + opts.buttonClass + '" value="font" />');
        this.append('<input data-id="btnAddTag" type="button" class="' + opts.buttonClass + '" value="list" />');
        this.append('<input data-id="btnAddTag" type="button" class="' + opts.buttonClass + '" value="table" />');

        // Register a click event
        $("input[data-id='btnAddTag']").click(addTag);

        // Return this object to allow chaining
        return this;

        // Add tags to the selection
        function addTag()
        {
            // Get data
            var text_area_id = '#' + $(this).parent().attr('data-textarea');
            var text_area = $(text_area_id);
            var tag_value = $(this).val();
            var selection = text_area.getSelection();

            // Insert the tag
            if (tag_value == "b") {
                text_area.surroundSelectedText('<b>', '</b>');
            }
            else if (tag_value == "i") {
                text_area.surroundSelectedText('<i>', '</i>');
            }
            else if (tag_value == "br") {
                text_area.insertText('<br />', selection.end, 'collapseToEnd');
            }
            else if (tag_value == "h1") {
                text_area.surroundSelectedText('<h1>', '</h1>');
            }
            else if (tag_value == "h2") {
                text_area.surroundSelectedText('<h2>', '</h2>');
            }
            else if (tag_value == "h3") {
                text_area.surroundSelectedText('<h3>', '</h3>');
            }
            else if (tag_value == "h4") {
                text_area.surroundSelectedText('<h4>', '</h4>');
            }
            else if (tag_value == "p") {
                text_area.surroundSelectedText('<p>', '</p>');
            }
            else if (tag_value == "code") {
                text_area.surroundSelectedText('<pre class="prettyprint annytab-code-container">', '</pre>');
            }
            else if (tag_value == "span") {
                text_area.surroundSelectedText('<span>', '</span>');
            }
            else if (tag_value == "div") {
                text_area.surroundSelectedText('<div>', '</div>');
            }
            else if (tag_value == "line") {
                text_area.surroundSelectedText('<div class="annytab-basic-line">', '</div>');
            }
            else if (tag_value == "space") {
                text_area.surroundSelectedText('<div class="annytab-basic-space">', '</div>');
            }
            else if (tag_value == "url") {
                text_area.surroundSelectedText('<a href="http://www.annytab.se" rel="nofollow" target="_blank">', '</a>');
            }
            else if (tag_value == "img") {
                text_area.surroundSelectedText('<img src="/source.jpg" style="max-width:100%;" />', '');
            }
            else if (tag_value == "font") {
                text_area.surroundSelectedText('<span style="font-family:Arial;font-size:12px;color:#ff0000;">', '</span>');
            }
            else if (tag_value == "list") {
                text_area.insertText('<ul><li>r1</li><li>r2</li></ul>', selection.end, 'select');
            }
            else if (tag_value == "table") {
                text_area.insertText('<table style="width:400px;text-align:center;"><tr><th>r1:c1</th><th>r1:c2</th></tr><tr><td>r2:c1</td><td>r2:c2</td></tr></table>', selection.end, 'select');
            }

        } // End of the addTag method
    };

    // Plugin defaults
    $.fn.htmlButtonPanel.defaults = {
        buttonClass: "annytab-form-button"
    };

}(jQuery));