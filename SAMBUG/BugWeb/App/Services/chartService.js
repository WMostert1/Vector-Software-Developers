angular.module("appMain")
    .service("chartService", [function () {
        //todo: check licence requirements (e.g. MIT) of all borrowed code and assets

        var chart;
        var projectedStops;

        var getProjectedStops = function(settings, scoutStops) {
            return Enumerable.From(scoutStops).Select(function(s) {
                if (settings.series === "none") {
                    if (settings.y === "bugsPerTree") {
                        return {
                            x: s[settings.x].valueOf(),
                            yB: s.numberOfBugs.valueOf(),
                            yT: s.numberOfTrees.valueOf()
                        };
                    } else {
                        return {
                            x: s[settings.x].valueOf(),
                            y: s[settings.y].valueOf()
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
                        x: 0,
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

        //todo might have to change the name back to treatments
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
                labelInterpolationFnc: millisToDateString
            };

            parameters.options.tooltipFnc = getLineTooltipText;



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
                /*saveBarChartParamaters(parameters, stopSeries, settings);*/
            }

            //Add options common to all charts
            
            parameters.options.chartPadding = {
                top: 25,
                right: 25,
                bottom: 25,
                left: 25
            }

            //always start y axis at 0
            parameters.options.axisY.low = 0;

            //For all summed data, use integer labels
            if (settings.aggregate === "Total")
                parameters.options.axisY.onlyInteger = true;

            //Axis Title Plugin
            setAxesTitles(parameters.options.plugins, settings);

            //Tooltip Plugin
            parameters.options.plugins.push(Chartist.plugins.tooltip(parameters.options));

            return parameters;
        }

        this.updateChart = function (chartId, scoutStops, treatments, settings) {
            //project the required fields, have accessible by later functions
            projectedStops = getProjectedStops(settings, scoutStops);

            //get categorised data (this is the data series)
            var stopSeries = getStopSeries(settings.series, projectedStops);
            var parameters = determinePlotParameters(stopSeries, treatments, settings);
            chart = new Chartist[settings.type](chartId, parameters.data, parameters.options);
        }
        
        

        function saveBarChartParamaters(parameters, stopSeries, settings) {
            var xLabels = [];

            //first extract all x-labels (a particular series might not include a mapping for all x-values)
            $.each(stopSeries, function (i, c) {

                //group each x value with its corresponding y values
                //add unique x values to xLabels
                //ignore the series name stored at position 0
                addUniqueLabel(groupBy(xMemberName, c.slice(1)).sort(sortByFirstElement), xLabels);

            });

            parameters.data.labels = xLabels;

            $.each(stopSeries, function (i, c) {
                var s = {};

                //series name is the first element of c
                s.name = c.shift();

                //group each x value with its corresponding y values, sorting x in ascending order
                var sortedXCategorisedPoints = groupBy(xMemberName, c).sort(sortByFirstElement);

                s.data = getBarSeriesData(xLabels, yMemberName, aggregateType, sortedXCategorisedPoints);

                parameters.data.series.push(s);

            });


            parameters.options.axisY = {};
            parameters.options.axisX = {};
            parameters.options.tooltipFnc = getBarTooltipText;
        }

        function addUniqueLabel(series, labels) {
            $.each(series, function (index, points) {
                if ($.grep(labels, function (d) {
                    return points[0].valueOf() === d.valueOf();
                }).length === 0) {
                    labels.push(points[0]);
                }
            });
        }

       

        function getBarSeriesData(xLabels, yMember, aggregateType, points) {
            var y = [];
            var curMatchIndex = 0;

            //A common labels array is used to store all mapped x values, "xLabels"
            //If a particular series excludes information for a particular x
            //the mapping for said x should be null
            $.each(xLabels, function (i, label) {

                if (points.length && curMatchIndex < points.length && points[curMatchIndex][0] === label) {
                    //remove the x value from the array
                    points[curMatchIndex].shift();

                    //reduce all the y values according to aggregateType and save into y
                    y.push(aggregate(yMember, aggregateType, points[curMatchIndex]));

                    ++curMatchIndex;

                } else {
                    y.push(null);
                }

            });

            return y;

        }

        

        function groupBy(memberName, stops) {
            //note that the first element of each subarray is used to store the category name
            var stopSeries = [];

            //don't categorise the stops
            if (memberName === "All Data") {
                stops.unshift(memberName);
                stopSeries.push(stops);
                return stopSeries;
            }

            //acquire unique entries to form categories
            var categories = getUniqueEntries(memberName, stops);

            $.each(categories, function (i, c) {
                var category = $.grep(stops, function (s) {
                    return s[memberName].valueOf() === c.valueOf();
                });

                category.unshift(c);
                stopSeries.push(category);
            });

            return stopSeries;
        }

        

        
        
    }]);

