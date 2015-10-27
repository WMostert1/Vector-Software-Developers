angular.module("appMain")
    .service("chartService", [function () {
        //todo: check licence requirements (e.g. MIT) of all borrowed code and assets

        var chart;

        this.initChart = function (mapId) {
           
        }

        this.updateChart = function (stops, settings) {
            if (chart && stops.length > 0) {
                
            }
        }
        
        function updateChart(settings) {
            updateChartTitle();

            var filteredScoutStops = filterScoutStops(scoutStops, settings);

            var filteredTreatments = [];

            var existsScoutStops = filteredScoutStops.length,
                onlyTreatments = false,
                onlyScoutStops = false,
                existsTreatments;


            //only attempt to prepare and plot treatments if selected and correct chart
            if (settings.constraintShowTreatments && !isCategorical(settings.against)) {
                filteredTreatments = filterTreatments(treatments, settings);

                existsTreatments = filteredTreatments.length;

                if (!existsScoutStops && !existsTreatments) {
                    inErrorState = true;
                    displayErrorMessage("We're afraid there is nothing to show for your current selection");
                    return;
                } else if (!existsScoutStops) {
                    onlyTreatments = true;
                } else if (!existsTreatments) {
                    onlyScoutStops = true;
                }
                plot(onlyScoutStops, onlyTreatments, filteredScoutStops, filteredTreatments, settings);
            } else {
                if (!existsScoutStops) {
                    inErrorState = true;
                    displayErrorMessage("We're afraid there is nothing to show for your current selection");
                    return;
                }
                plot(true, false, filteredScoutStops, filteredTreatments, settings);
            }

        }

        function plot(onlyScoutStops, onlyTreatments, filteredScoutStops, filteredTreatments, settings) {
            //get categorised data (or unaltered data if no grouping is applied)
            var categorisedStops = groupBy(viewModelToMemberMap[settings.grouping], filteredScoutStops);

            var parameters = determinePlotParameters(onlyScoutStops, onlyTreatments, settings, categorisedStops, filteredTreatments);

            chart = new Chartist[parameters.chartType]("#chart", parameters.data, parameters.options);

        }

        function determinePlotParameters(onlyScoutStops, onlyTreatments, settings, categorisedStops, filteredTreatments) {
            //mapping to x member
            var xMemberName = viewModelToMemberMap[settings.against];

            //mapping to y member
            //note that this may map to the quasi-member: bugsPerTree
            //  for which there is no corresponding member variable in scoutStops
            var yMemberName = viewModelToMemberMap[settings.view];

            //determine chartType
            var chartType = isCategorical(settings.against) ? "Bar" : "Line";

            //Acquire various chart parameters
            var parameters = {};
            parameters.data = {};
            parameters.data.series = [];
            parameters.chartType = chartType;
            parameters.options = {};
            parameters.options.plugins = [];

            if (chartType === "Line") {
                saveLineChartParameters(onlyScoutStops, settings.constraintShowTreatments, onlyTreatments, parameters, xMemberName, yMemberName, settings.aggregateType, categorisedStops, filteredTreatments);
            } else if (chartType === "Bar") {
                saveBarChartParamaters(parameters, xMemberName, yMemberName, settings.aggregateType, categorisedStops);
            }

            //Add options common to all charts

            //always start y axis at 0
            parameters.options.axisY.low = 0;

            //For all average data, use real valued labels
            if (settings.aggregateType === "Total")
                parameters.options.axisY.onlyInteger = true;

            //Axis Title Plugin
            setAxesTitles(parameters.options.plugins, settings);

            //Tooltip Plugin
            parameters.options.plugins.push(Chartist.plugins.tooltip(parameters.options));

            return parameters;

        }

        function saveLineChartParameters(onlyScoutStops, showTreatments, onlyTreatments, parameters, xMemberName, yMemberName, aggregateType, categorisedStops, filteredTreatments) {
            if (!onlyTreatments) {

                $.each(categorisedStops, function (i, c) {
                    var s = {};

                    //series name is the first element of c
                    s.name = c.shift();

                    //group each x value with its corresponding y values, sorting x in ascending order
                    var sortedXCategorisedPoints = groupBy(xMemberName, c).sort(sortByFirstElement);

                    s.data = getLineSeriesData(yMemberName, aggregateType, filteredTreatments, sortedXCategorisedPoints);

                    parameters.data.series.push(s);
                });

            }

            if (showTreatments && !onlyScoutStops) {
                addTreatments(parameters, filteredTreatments);
            }

            parameters.options.showPoint = true;
            parameters.options.axisY = {};
            parameters.options.axisX = {
                type: Chartist.FixedScaleAxis,
                divisor: 10,
                labelInterpolationFnc: millisToDateString
            };

            parameters.options.tooltipFnc = getLineTooltipText;



        }

        function saveBarChartParamaters(parameters, xMemberName, yMemberName, aggregateType, categorisedStops) {
            var xLabels = [];

            //first extract all x-labels (a particular series might not include a mapping for all x-values)
            $.each(categorisedStops, function (i, c) {

                //group each x value with its corresponding y values
                //add unique x values to xLabels
                //ignore the series name stored at position 0
                addUniqueLabel(groupBy(xMemberName, c.slice(1)).sort(sortByFirstElement), xLabels);

            });

            parameters.data.labels = xLabels;

            $.each(categorisedStops, function (i, c) {
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

        function getLineSeriesData(yMember, aggregateType, filteredTreatments, sortedXCategorisedPoints) {
            var xy = [];

            $.each(sortedXCategorisedPoints, function (i, points) {
                var xValue = points.shift();

                var yAggregate = reduceToSingleY(yMember, aggregateType, points);

                xy.push({ x: xValue, y: yAggregate });

            });

            return xy;

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
                    y.push(reduceToSingleY(yMember, aggregateType, points[curMatchIndex]));

                    ++curMatchIndex;

                } else {
                    y.push(null);
                }

            });

            return y;

        }

        function addTreatments(parameters, treatments) {
            var newSeries = {
                name: "Treatments",
                data: []
            }

            var vLinePositions = [];

            $.each(treatments, function (i, treatment) {
                newSeries.data.push({ x: treatment.date, y: -1 });
                vLinePositions.push(treatment.date.valueOf());
            });

            parameters.data.series.push(newSeries);

            parameters.options.plugins.push(Chartist.plugins.verticalLines({
                positions: vLinePositions
            }));

            parameters.options.series = {
                "Treatments": {
                    showLine: false
                }
            };
        }

        function isCategorical(setting) {
            return (setting !== "Date");
        }

        function groupBy(memberName, stops) {
            //note that the first element of each subarray is used to store the category name
            var categorisedStops = [];

            //don't categorise the stops
            if (memberName === "All Data") {
                stops.unshift(memberName);
                categorisedStops.push(stops);
                return categorisedStops;
            }

            //acquire unique entries to form categories
            var categories = getUniqueEntries(memberName, stops);

            $.each(categories, function (i, c) {
                var category = $.grep(stops, function (s) {
                    return s[memberName].valueOf() === c.valueOf();
                });

                category.unshift(c);
                categorisedStops.push(category);
            });

            return categorisedStops;
        }

        function reduceToSingleY(yMember, aggregateType, yList) {
            var bugs = 0, trees = 0;
            var yAggregate = 0.0;

            if (yMember === "bugsPerTree") {
                //only averages seem to make sense for bugsPerTree
                $.each(yList, function (j, y) {
                    bugs += y.numberOfBugs;
                    trees += y.numberOfTrees;
                });

                yAggregate = bugs / trees;
            } else {
                $.each(yList, function (j, y) {
                    yAggregate += y[yMember];
                });

                if (aggregateType === "Average")
                    yAggregate /= yList.length;
            }

            return yAggregate;
        }

        function millisToDateString(value) {
            return new XDate(value).toString("dd MMM yy");
        }

        function setAxesTitles(plugins, settings) {
            plugins.push(Chartist.plugins.ctAxisTitle({
                axisX: {
                    axisTitle: settings.against,
                    axisClass: "ct-axis-title",
                    offset: {
                        x: 0,
                        y: 30
                    },
                    textAnchor: "middle"
                },
                axisY: {
                    axisTitle: settings.aggregateType + " " + settings.view,
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
        
    }]);

