angular.module("appMain")
    .service("chartService", [function () {
        //todo: check licence requirements (e.g. MIT) of all borrowed code and assets

        var chart;
        var projectedStops;
        var stopSeries;

        var getProjectedStops = function(settings, scoutStops) {
            return Enumerable.From(scoutStops).Select(function(s) {
                if (settings.series === "none") {
                    if (settings.y === "bugsPerTree") {
                        return {
                            x: s[settings.x].valueOf(),
                            yB: s.numberOfBugs.valueOf(),
                            yT: s.numberOfTrees.valueOf(),
                            series: "All Data"
                        };
                    } else {
                        return {
                            x: s[settings.x].valueOf(),
                            y: s[settings.y].valueOf(),
                            series: "All Data"
                        };
                    }
                } else {
                    if (settings.y === "bugsPerTree") {
                        return {
                            x: s[settings.x].valueOf(),
                            yB: s.numberOfBugs.valueOf(),
                            yT: s.numberOfTrees.valueOf(),
                            series: s[settings.series].valueOf()
                        };
                    } else {
                        return {
                            x: s[settings.x].valueOf(),
                            y: s[settings.y].valueOf(),
                            series: s[settings.series].valueOf()
                        };
                    }
                }
            }).ToArray();
        }

        var getStopSeries = function(series, scoutStops) {
            return Enumerable.From(scoutStops).GroupBy(function(s) {
                    return s.series;
            }).ToArray();
        }

        function aggregate(settings, points) {
            var bugs = 0, trees = 0;
            var yAggregate = 0.0;

            if (settings.y === "bugsPerTree") {
                //only averages seem to make sense for bugsPerTree
                Enumerable.From(points).ForEach(function (p) {
                    bugs += p.yB;
                    trees += p.yT;
                });

                yAggregate = bugs / trees;
            } else {
                Enumerable.From(points).ForEach(function (p) {
                    yAggregate += p.y;
                });

                if (settings.aggregate === "Average")
                    yAggregate /= points.length;
            }

            return yAggregate;
        }

        function getLineSeriesData(settings, orderedPoints) {
            var xy = [];

            Enumerable.From(orderedPoints).ForEach(function (p) {
                xy.push({
                    x: p.Key(),
                    y: aggregate(settings, p.source)
                });
            });

            return xy;
        }

        function millisToDateString(value) {
            return new XDate(value).toString("dd MMM yy");
        }

        function setAxesTitles(plugins, settings) {
            plugins.push(Chartist.plugins.ctAxisTitle({
                axisX: {
                    axisTitle: settings.xTitle,
                    axisClass: "ct-axis-title",
                    offset: {
                        x: -25,
                        y: 45
                    },
                    textAnchor: "middle"
                },
                axisY: {
                    axisTitle: settings.aggregate + " " + settings.yTitle,
                    axisClass: "ct-axis-title",
                    offset: {
                        x: 0,
                        y: 15
                    },
                    textAnchor: "middle",
                    flipTitle: true
                }
            }));
        }

        function getLineTooltipText(meta, values, seriesName, classes) {
            if (classes.indexOf("ct-line") >= 0) {
                return seriesName;
            } else {
                var x = millisToDateString(parseInt(values.split(",")[0]));
                var y = parseFloat(values.split(",")[1]).toFixed(2).toString();
                return seriesName + "<br/>" + x + ", " + y;
            }
        }

        function getBarTooltipText(meta, values, seriesName, classes) {
            var y = parseFloat(values.split(",")[0]).toFixed(2).toString();
            return seriesName + "<br/>" + y;
        }

        function addTreatments(parameters, treatments) {
            var newSeries = {
                name: "Spray Data",
                data: []
            }

            var vLinePositions = [];

            Enumerable.From(treatments).ForEach(function (t) {
                newSeries.data.push({ x: t.date, y: -1 });
                vLinePositions.push(t.date.valueOf());
            });

            parameters.data.series.push(newSeries);

            parameters.options.plugins.push(Chartist.plugins.verticalLines({
                positions: vLinePositions
            }));

            parameters.options.series = {
                "Spray Data": {
                    showLine: false
                }
            };
        }

        function addTrendlines(parameters, series) {
            var i = 0;
            Enumerable.From(series).ForEach(function(s) {
                var trendSeries = {
                    name: "Trend",
                    className: "trendline-" + ++i,
                    data: []
                }

                var vLinePositions = [];

                Enumerable.From(treatments).ForEach(function (t) {
                    newSeries.data.push({ x: t.date, y: -1 });
                    vLinePositions.push(t.date.valueOf());
                });

                parameters.data.series.push(newSeries);

                parameters.options.plugins.push(Chartist.plugins.verticalLines({
                    positions: vLinePositions
                }));

                parameters.options.series = {
                    "Spray Data": {
                        showLine: false
                    }
                };
            });
            
        }

        function saveLineChartParameters(parameters, stopSeries, treatments, settings) {
            Enumerable.From(stopSeries).ForEach(function (s) {
                var series = {};
                series.name = s.Key();

                //group each x value with its corresponding y values, sorting x in ascending order
                var orderedPoints = Enumerable.From(s.source)
                    .OrderBy(function(p) {
                        return p.x;
                    })
                    .GroupBy(function(p) {
                        return p.x;
                    }).ToArray();


                series.data = getLineSeriesData(settings, orderedPoints, treatments);
                parameters.data.series.push(series);
            });

            if (treatments.length > 0){
                addTreatments(parameters, treatments);
            }

            parameters.options.lineSmooth = Chartist.Interpolation.none();
            parameters.options.showPoint = true;
            parameters.options.axisY = {};
            parameters.options.axisX = {
                type: Chartist.AutoScaleAxis,
                scaleMinSpace: 50,
                labelInterpolationFnc: millisToDateString
            };

            parameters.options.tooltipFnc = getLineTooltipText;
        }

        function getBarSeriesData(xLabels, settings, orderedPoints) {
            var y = [];
            var curMatchIndex = 0;

            //A common labels array is used to store all mapped x values, "xLabels"
            //If a particular series excludes information for a particular x
            //the mapping for said x should be null
            //todo might need to check for orderedPoints.length in if, I removed it
            Enumerable.From(xLabels).ForEach(function (label) {
                if (curMatchIndex < orderedPoints.length && orderedPoints[curMatchIndex].Key() === label) {
                    //reduce all the y values according to aggregateType and save into y
                    y.push(aggregate(settings, orderedPoints[curMatchIndex].source));
                    ++curMatchIndex;
                } else {
                    y.push(null);
                }
            });

            return y;

        }

        function saveBarChartParamaters(parameters, stopSeries, settings) {
            //first extract all x-labels (a particular series might not include a mapping for all x-values)
            parameters.data.labels = Enumerable.From(projectedStops).Distinct(function(s) {
                    return s.x;
                })
                .Select(function(s) {
                    return s.x;
                })
                .OrderBy()
                .ToArray();
            
            Enumerable.From(stopSeries).ForEach(function (s) {
                var series = {};
                series.name = s.Key();

                //group each x value with its corresponding y values, sorting x in ascending order
                var orderedPoints = Enumerable.From(s.source)
                    .OrderBy(function (p) {
                        return p.x;
                    })
                    .GroupBy(function (p) {
                        return p.x;
                    }).ToArray();


                series.data = getBarSeriesData(parameters.data.labels, settings, orderedPoints);
                parameters.data.series.push(series);
            });

            parameters.options.axisY = {};
            parameters.options.axisX = {};
            parameters.options.tooltipFnc = getBarTooltipText;
        }

        function determinePlotParameters(stopSeries, treatments, settings) {
            //Acquire various chart parameters
            var parameters = {};
            parameters.data = {};
            parameters.data.series = [];
            parameters.options = {};
            parameters.options.plugins = [];

            if (settings.type === "Line") {
                saveLineChartParameters(parameters, stopSeries, treatments, settings);
            } else if (settings.type === "Bar") {
                saveBarChartParamaters(parameters, stopSeries, settings);
            }

            //Add options common to all charts
            
            parameters.options.chartPadding = {
                top: 25,
                right: 0,
                bottom: 25,
                left: 35
            }

            //always start y axis at 0
            parameters.options.axisY.low = 0;

            //For all summed data, use integer labels
            if (settings.aggregate === "Total")
                parameters.options.axisY.onlyInteger = true;
            else {
                parameters.options.axisY.labelInterpolationFnc = function(value) {
                    return value.toFixed(2);
                }
            }

            //Axis Title Plugin
            setAxesTitles(parameters.options.plugins, settings);

            //Tooltip Plugin
            parameters.options.plugins.push(Chartist.plugins.tooltip(parameters.options));

            return parameters;
        }

        function setAnimations(c, seriesCount) {
            var totalDuration = 2000;
            var duration = totalDuration / seriesCount;

            var pointsEasing = Chartist.Svg.Easing.easeOutQuint;
            var lineEasing = Chartist.Svg.Easing.easeOutQuint;
            var barEasing = Chartist.Svg.Easing.easeOutQuint;

            c.on("draw", function (data) {
                if (data.type === "line") {
                    data.element.animate({
                        d: {
                            begin: duration * data.seriesIndex,
                            dur: duration,
                            from: data.path.clone().scale(1, 0).translate(0, data.chartRect.height()).stringify(),
                            to: data.path.clone().stringify(),
                            easing: lineEasing
                        }
                    });
                }

                if (data.type === "bar") {
                    data.element.animate({
                        y2: {
                            begin: duration * data.seriesIndex,
                            dur: duration,
                            from: data.y1,
                            to: data.y2,
                            easing: barEasing
                        }
                    });
                }

                if (data.type === "point") {
                    data.element.animate({
                        y1: {
                            begin: duration * data.seriesIndex,
                            dur: duration,
                            from: data.y - 20,
                            to: data.y,
                            easing: pointsEasing
                        },
                        y2: {
                            begin: duration * data.seriesIndex,
                            dur: duration,
                            from: data.y - 20,
                            to: data.y,
                            easing: pointsEasing
                        },
                        x1: {
                            begin: duration * data.seriesIndex,
                            dur: duration,
                            from: data.x - 20,
                            to: data.x,
                            easing: pointsEasing
                        },
                        x2: {
                            begin: duration * data.seriesIndex,
                            dur: duration,
                            from: data.x - 20,
                            to: data.x,
                            easing: pointsEasing
                        },
                        opacity: {
                            begin: duration * data.seriesIndex,
                            dur: duration,
                            from: 0,
                            to: 1,
                            easing: pointsEasing
                        }
                    });
                }
            });
        }

        this.updateChart = function (chartId, scoutStops, treatments, settings) {
            //project the required fields, have accessible by later functions
            projectedStops = getProjectedStops(settings, scoutStops);
            //get categorised data (this is the data series)
            stopSeries = getStopSeries(settings.series, projectedStops);
            var parameters = determinePlotParameters(stopSeries, treatments, settings);
            chart = new Chartist[settings.type](chartId, parameters.data, parameters.options);
            setAnimations(chart, stopSeries.length);
        }
        
    }]);

