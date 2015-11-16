(function (root, factory) {
  if (typeof define === "function" && define.amd) {
    // AMD. Register as an anonymous module.
    define([], function () {
      return (root.returnExportsGlobal = factory());
    });
  } else if (typeof exports === "object") {
    // Node. Does not work with strict CommonJS, but
    // only CommonJS-like enviroments that support module.exports,
    // like Node.
    module.exports = factory();
  } else {
    root["Chartist.plugins.verticalLine"] = factory();
  }
}(this, function () {

  /**
   * Chartist.js plugin to insert vertical line in a line chart.
   *
   */
  /* global Chartist */
  (function (window, document, Chartist) {
    "use strict";

    var defaultOptions = {
      position: undefined,
      label: undefined,
      className: "vertical-line"
    };

    var VerticalLine = function (chart, chartRect, options) {

      var labelClassName = options.className + "-label";
      
      var  $label = $("<span class=\"" + labelClassName + "\" style=\"position: absolute\"></span>")
          .appendTo(chart.container)
          .hide();

      this.show = function (x) {

        $label
          .html(options.label || "")
          .css({ left: x - $label.width() / 2 })
          .show();

        chart.svg.elem("line", {
          x1: x,
          x2: x,
          y1: chartRect.y1,
          y2: chartRect.y2 + $label.height()
        }, options.className);
      };
    };

    Chartist.plugins = Chartist.plugins || {};
    Chartist.plugins.verticalLines = function (options) {
     
      options = Chartist.extend({}, defaultOptions, options);

        
      return function (chart) {

        if (!(chart instanceof Chartist.Line)) {
          return;
        }

        var positions = [];

        chart.on("draw", function (data) {
            if (data.type === "point" && ($.inArray(data.value.x.valueOf(), options.positions)) >= 0 && data.value.y === -1) {
                positions.push(data.x);
                data.element.remove();
            }
        });

        chart.on("created", function (data) {
            $.each(positions, function(i, pos) {
                var verticalLine = new VerticalLine(chart, data.chartRect, options);
                verticalLine.show(pos);
            });
        });
      };
    };

  }(window, document, Chartist));

  return Chartist.plugins.verticalLines;

}));