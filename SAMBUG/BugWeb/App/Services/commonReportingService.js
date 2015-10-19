angular.module("appMain")
    .service('commonReportingService', ["$http", function ($http) {
        function loadPests() {
            $.get(pestsUrl, function (data, status) {

                if (status === "error" || status === "timeout") {
                    alert("Some information couldn't be retrieved from the server. Please check your connection and try again.");
                    return;
                }

                species = $.map(data.Species, function (s) {
                    s.Lifestage = mapSpeciesLifeStage(s.Lifestage);
                    return s;
                });

                var speciesNames = getUniqueEntries("SpeciesName", data.Species);
                initSuggestions("constraintSpecies", speciesNames);

                //initialise suggestions to be all available lifestages
                updateSuggestions("constraintSpecies", "constraintLifeStage", species, "SpeciesName", "Lifestage");

                $("#constraintSpecies").change(function () {
                    updateSuggestions("constraintSpecies", "constraintLifeStage", species, "SpeciesName", "Lifestage");
                });

            }, "Json");
        }

        function loadDataRecords() {
            $.get(recordsUrl, function (data, status) {

                if (status === "error" || status === "timeout") {
                    alert("Some information couldn't be retrieved from the server. Please check your connection and try again.");
                    return;
                }

                treatments = flattenTreatments(data.Treatments);
                scoutStops = flattenScoutStops(data.ScoutStops);

                var farmNames = getUniqueEntries("farmName", scoutStops.concat(treatments));

                initSuggestions("constraintFarms", farmNames);

                //initialise suggestions to be all available blocks
                updateSuggestions("constraintFarms", "constraintBlocks", scoutStops.concat(treatments), "farmName", "blockName");

                $("#constraintFarms").change(function () {
                    updateSuggestions("constraintFarms", "constraintBlocks", scoutStops.concat(treatments), "farmName", "blockName");
                });

                //disable loader, update chart settings, finally plot 
                initChart();

            }, "Json");
        }

        function mapSpeciesLifeStage(code) {
            if (code === "0") {
                return "Adult";
            } else {
                return "Instar " + code;
            }
        }

        function getUniqueEntries(uniqueFor, data) {
            var result = [];
            $.each(data, function (index, object) {
                if ($.grep(result, function (d) {
                    return object[uniqueFor].valueOf() === d.valueOf();
                }).length === 0)
                    result.push(object[uniqueFor]);
            });

            return result;
        }

        function flattenTreatments(data) {
            var timelessDate;

            return $.map(data, function (treatment) {
                timelessDate = new Date(treatment.Date);
                timelessDate.setHours(0, 0, 0, 0);

                return {
                    farmName: treatment.BlockFarmFarmName,
                    blockName: treatment.BlockBlockName,
                    comments: treatment.Comments,
                    date: timelessDate
                };
            });
        }

        function flattenScoutStops(data) {
            var result = [];
            var timelessDate;

            $.each(data, function (index, scoutStop) {
                result = result.concat($.map(scoutStop.ScoutBugs, function (b) {
                    timelessDate = new Date(scoutStop.Date);
                    timelessDate.setHours(0, 0, 0, 0);

                    return {
                        farmName: scoutStop.BlockFarmFarmName,
                        blockName: scoutStop.BlockBlockName,
                        date: timelessDate,
                        numberOfTrees: parseInt(scoutStop.NumberOfTrees),
                        speciesName: b.SpeciesSpeciesName,
                        lifeStage: mapSpeciesLifeStage(b.SpeciesLifestage),
                        numberOfBugs: parseInt(b.NumberOfBugs),
                        comments: b.Comments
                    };
                }));
            });

            return result;
        }

        this.fetch = function (url, success) {
            $http.get(url, { cache: true }).then(success);
        };
    }]);