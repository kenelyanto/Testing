// TODO add loading class
(function($) {
    'use strict';
    $.fn['dependentDropdown'] = function(options) {
        var plugin = this;
        var init = function() {
            plugin.settings = $.extend({}, {
                depends: [],
                options: [],
                ajax: true,
                ajaxCallback: null,
                method: 'POST',
                source: null,
                // callback executed if ajax is false
                callback: null
            }, options);

            $.each(plugin.settings.depends, function(i, selector) {
                $(selector).bind('change', function() {
                    refresh();
                });
            });
            plugin.bind('dependent-dropdown.refresh-list', appendData);
            if (!plugin.val()) {
                refresh();
            }
        };
        function refresh() {
            var data = {};
            plugin.options = {};
            plugin.val('').trigger('change').html('').prop('disabled', true);
            plugin.trigger('change');
            for (var i in plugin.settings.depends) {
                var selector = plugin.settings.depends[i];
                if (!$(selector).val()) {
                    return;
                }
                data[$(selector).attr('name')] = $(selector).val();
            }
            if (plugin.settings.ajax) {
                $.ajax({
                    url: plugin.settings.source,
                    data: data,
                    method: plugin.settings.method,
                    dataType: 'json'
                }).done(function(data) {
                    if (plugin.settings.ajaxCallback && typeof(plugin.settings.ajaxCallback) === "function") {
                        plugin.settings.ajaxCallback(plugin, data);
                    } else {
                        plugin.options = data;
                    }
                    plugin.trigger('dependent-dropdown.refresh-list')
                        .prop('disabled', false);
                });
            }
            if (plugin.settings.callback && typeof(plugin.settings.callback) === "function") {
                plugin.settings.callback(plugin, data);
                plugin.trigger('dependent-dropdown.refresh-list');
            }
        }
        function appendData() {
            if (plugin.options) {
                $.each(plugin.options, function (value, label) {
                    plugin.append($('<option>').text(label.name).attr('value', label.id));
                });
                plugin.val(null).trigger('change');
            }
        }
        return this.each(init);
    };
})(jQuery);