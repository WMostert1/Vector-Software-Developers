angular.module("appMain")
    .service("commonReportingService", ["$http", "reportingUrlService", function ($http, reportingUrlService) {
        var species = [];
        var farms = [];
        var speciesLifeStages = [];
        var treatments = [];
        var scoutStops = [];
       
        var mapSpeciesLifeStage = function(code) {
            if (code === 0) {
                return "Adult";
            } else {
                return "Instar " + code;
            }
        }

        var getSpeciesLifeStage = function(data) {
            var speciesGroups = Enumerable.From(data)
                .GroupBy(function(d) {
                    return d.speciesName;
                })
                .ToArray();

            return Enumerable.From(speciesGroups)
                .Select(function(s) {
                    return {
                        speciesName: s.Key(),
                        isPest: s.source[0].isPest,
                        lifeStages: Enumerable.From(s.source)
                            .Select(function (i) {
                                return i.lifeStage;
                            })
                            .ToArray()
                }
                }).ToArray();
        }

        var getFarmNames = function () {
            var farmNames = ["All Farms"];
            return farmNames.concat(
                    Enumerable.From(farms)
                    .Select(function (f) {
                        return f.farmName;
                    })
                    .ToArray()
            );
        }

        var getSpeciesNames = function() {
            var speciesNames = ["All Species"];
            return speciesNames.concat(
                    Enumerable.From(speciesLifeStages)
                    .Select(function (s) {
                        return s.speciesName;
                    })
                    .ToArray()
            );
        }


        var loadSpecies = function(callback) {
            $http.get(reportingUrlService.speciesUrl, { cache: true }).then(function(response) {
                species = Enumerable.From(response.data.Species)
                    .Select(
                        function(s) {
                            return {
                                isPest: s.IsPest,
                                speciesName: s.SpeciesName,
                                lifeStage: mapSpeciesLifeStage(s.Lifestage)
                            }
                        }
                    )
                    .ToArray();
                speciesLifeStages = getSpeciesLifeStage(species);
                callback(getSpeciesNames());
            });
        }

        function flattenTreatments(data) {
            var timelessDate;

            return Enumerable.From(data).Select(
                function (treatment) {
                timelessDate = new Date(treatment.Date);
                timelessDate.setHours(0, 0, 0, 0);
                return {
                    farmName: treatment.BlockFarmFarmName,
                    blockName: treatment.BlockBlockName,
                    comments: treatment.Comments,
                    date: timelessDate
                }
            }).ToArray();
        }

        function flattenScoutStops(data) {
            var result = [];
            var timelessDate;

            Enumerable.From(data)
                .ForEach(function(scoutStop) {
                    result = result.concat(Enumerable.From(scoutStop.ScoutBugs)
                        .Select(function(b) {
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
                                comments: b.Comments,
                                latitude: scoutStop.Latitude,
                                longitude: scoutStop.Longitude
                            };
                        })
                        .ToArray());
                });
            return result;
        }

        function getFarmsBlocks(data) {
            var farmGroups = Enumerable.From(data)
                .GroupBy(function (d) {
                    return d.farmName;
                })
                .ToArray();

            return Enumerable.From(farmGroups).Select(function (f) {
                return {
                    farmName: f.Key(),
                    blockNames: Enumerable.From(f.source)
                        .Distinct(function (b) {
                            return b.blockName;
                        })
                        .Select(function (b) {
                            return b.blockName;
                        })
                        .ToArray()
                }
            }).ToArray();
        }

        function loadDataRecords(callback) {
            $http.get(reportingUrlService.recordsUrl, { cache: true }).then(function(response) {
                treatments = flattenTreatments(response.data.Treatments);
                scoutStops = flattenScoutStops(response.data.ScoutStops);
                farms = getFarmsBlocks(scoutStops.concat(treatments));
                callback(getFarmNames());
            });
        }

        this.init = function (callbackRecords, callbackSpecies) {
            loadSpecies(callbackSpecies);
            loadDataRecords(callbackRecords);
        }

        this.getSpecies = function() {
            return speciesLifeStages;
        }

        this.getBlocksForFarms = function(farmNames) {
            var blockNames = ["All Blocks"];
            Enumerable.From(farms)
                .Where(function(f) {
                    return farmNames.length === 0 ||
                        Enumerable.From(farmNames).Any(function(n) {
                            return n === f.farmName;
                        }) ||
                        Enumerable.From(farmNames).Any(function(n) {
                            return n === "All Farms";
                        });
                })
                .ForEach(function(f) {
                    blockNames = blockNames.concat(f.blockNames);
                });

            return Enumerable.From(blockNames).Distinct().ToArray();
        }

        this.getLifeStagesForSpecies = function(speciesNames) {
            var lifeStages = ["All Life Stages"];
            Enumerable.From(speciesLifeStages)
                .Where(function(s) {
                    return speciesNames.length === 0 ||
                        Enumerable.From(speciesNames).Any(function(n) {
                            return n === s.speciesName;
                        }) ||
                        Enumerable.From(speciesNames).Any(function(n) {
                            return n === "All Species";
                        });
                })
                .ForEach(function(s) {
                    lifeStages = lifeStages.concat(s.lifeStages);
                });

            return Enumerable.From(lifeStages).Distinct().ToArray();
        }

        this.getScoutStops = function() {
            return scoutStops;
        }

        this.getTreatments = function() {
            return treatments;
        }

       /* this.getTreatments = function(filter) {
            
        }

        this.getScoutStops = function(filter) {
            
        }*/
        
    }]);