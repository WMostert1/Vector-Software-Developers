angular.module("appMain")
    .service("commonReportingService", ["$http", "reportingUrlService", function ($http, reportingUrlService) {
        var species = [];
        var farms = [];
        var speciesLifeStages = [];
        var treatments = [];
        var scoutStops = [];

        var mapSpeciesLifeStage = function (code) {
            if (code === 0) {
                return "Adult";
            } else {
                return "Instar " + code;
            }
        }

        var getSpeciesLifeStage = function (data) {
            var lifeStagesOwners = {};

            Enumerable.From(data).ForEach(function (d) {
                lifeStagesOwners[d.lifeStage] = lifeStagesOwners[d.lifeStage] || [];
                lifeStagesOwners[d.lifeStage] = lifeStagesOwners[d.lifeStage].concat([d.speciesName]);
            });

            for (var l in lifeStagesOwners) {
                lifeStagesOwners[l] = Enumerable.From(lifeStagesOwners[l]).Distinct().ToArray();
            }

            var speciesGroups = Enumerable.From(data)
                .GroupBy(function (d) {
                    return d.speciesName;
                })
                .ToArray();

            return Enumerable.From(speciesGroups)
                .Select(function (s) {
                    return {
                        name: s.Key(),
                        isPest: s.source[0].isPest,
                        lifeStages: Enumerable.From(s.source)
                            .Distinct(function (i) {
                                return i.lifeStage;
                            })
                            .Select(function (i) {
                                return {
                                    name: i.lifeStage,
                                    owners: lifeStagesOwners[i.lifeStage]
                                };
                            })
                            .ToArray()
                    }
                }).ToArray();
        }

        var loadSpecies = function (callback) {
            $http.get(reportingUrlService.speciesUrl, { cache: true }).then(function (response) {
                species = Enumerable.From(response.data.Species)
                    .Select(
                        function (s) {
                            return {
                                isPest: s.IsPest,
                                speciesName: s.SpeciesName,
                                lifeStage: mapSpeciesLifeStage(s.Lifestage)
                            }
                        }
                    )
                    .ToArray();
                speciesLifeStages = getSpeciesLifeStage(species);
                callback(speciesLifeStages);
            });
        }

        var flattenTreatments = function (data) {
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

        var flattenScoutStops = function (data) {
            var result = [];
            var timelessDate;

            Enumerable.From(data)
                .ForEach(function (scoutStop) {
                    result = result.concat(Enumerable.From(scoutStop.ScoutBugs)
                        .Select(function (b) {
                            timelessDate = new Date(scoutStop.Date);
                            timelessDate.setHours(0, 0, 0, 0);

                            return {
                                farmName: scoutStop.BlockFarmFarmName,
                                blockName: scoutStop.BlockBlockName,
                                date: timelessDate,
                                numberOfTrees: parseInt(scoutStop.NumberOfTrees),
                                speciesName: b.SpeciesSpeciesName,
                                isPest: b.SpeciesIsPest,
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

        var getFarmsBlocks = function (data) {
            var blocksOwners = {};

            Enumerable.From(data).ForEach(function (d) {
                blocksOwners[d.blockName] = blocksOwners[d.blockName] || [];
                blocksOwners[d.blockName] = blocksOwners[d.blockName].concat([d.farmName]);
            });

            for (var b in blocksOwners) {
                blocksOwners[b] = Enumerable.From(blocksOwners[b]).Distinct().ToArray();
            }

            var farmGroups = Enumerable.From(data)
                .GroupBy(function (d) {
                    return d.farmName;
                })
                .ToArray();

            return farms = Enumerable.From(farmGroups).Select(function (f) {
                return {
                    name: f.Key(),
                    blocks: Enumerable.From(f.source)
                        .Distinct(function (b) {
                            return b.blockName;
                        })
                        .Select(function (b) {
                            return {
                                name:b.blockName,
                                owners: blocksOwners[b.blockName]
                            };
                        })
                        .ToArray()
                }
            }).ToArray();
        }

        var loadDataRecords = function (callback) {
            $http.get(reportingUrlService.recordsUrl, { cache: true }).then(function (response) {
                treatments = flattenTreatments(response.data.Treatments);
                scoutStops = flattenScoutStops(response.data.ScoutStops);
                farms = getFarmsBlocks(scoutStops.concat(treatments));
                callback(farms);
            });
        }

        this.init = function (callbackRecords, callbackSpecies) {
            loadSpecies(callbackSpecies);
            loadDataRecords(callbackRecords);
        }

        this.getSpecies = function () {
            return speciesLifeStages;
        }

        this.getFarms = function() {
            return farms;
        }

        this.getBlocksForFarms = function (selectedFarms) {
            var blocks = [];
            Enumerable.From(farms)
                .Where(function (f) {
                    return selectedFarms.length === 0 ||
                        Enumerable.From(selectedFarms).Any(function (n) {
                            return n.name === f.name;
                        });
                })
                .ForEach(function (f) {
                    blocks = blocks.concat(f.blocks);
                });

            return Enumerable.From(blocks).Distinct(function(b) {
                return b.name;
            }).ToArray();
        }

        this.getLifeStagesForSpecies = function (selectedSpecies) {
            var lifeStages = [];
            Enumerable.From(speciesLifeStages)
                .Where(function (s) {
                    return selectedSpecies.length === 0 ||
                        Enumerable.From(selectedSpecies).Any(function (n) {
                            return n.name === s.name;
                        });
                })
                .ForEach(function (s) {
                    lifeStages = lifeStages.concat(s.lifeStages);
                });

            return Enumerable.From(lifeStages).Distinct(function(l) {
                return l.name;
            }).ToArray();
        }

        this.getScoutStops = function (filter) {
            var milliDate;

            return Enumerable.From(scoutStops)
                .Where(function (s) {
                    milliDate = (new XDate(s.date)).valueOf();
                    return (Enumerable.From(filter.farms).Any(function (f) {
                                return f.name === s.farmName;
                            }) || filter.farms.length === 0) &&
                        (Enumerable.From(filter.blocks).Any(function (b) {
                            return b.name === s.blockName;
                        }) || filter.blocks.length === 0) &&
                        (Enumerable.From(filter.species).Any(function (p) {
                            return p.name === s.speciesName;
                        }) || filter.species.length === 0) &&
                        (Enumerable.From(filter.lifeStages).Any(function (l) {
                            return l.name === s.lifeStage ;
                        }) || filter.lifeStages.length === 0) &&
                        (filter.dates.all || (filter.dates.from.valueOf() <= milliDate.valueOf() &&
                        filter.dates.to.valueOf() >= milliDate.valueOf()));
                })
                .Select(function (s) {
                    return s;
                })
                .ToArray();
        }

        this.getTreatments = function (filter) {
            var milliDate;

            return Enumerable.From(treatments)
             .Where(function (t) {
                 milliDate = (new XDate(t.date)).valueOf();
                 return (Enumerable.From(filter.farms).Any(function (f) {
                     return f.name === t.farmName;
                 }) || filter.farms.length === 0) &&
                     (Enumerable.From(filter.blocks).Any(function (b) {
                         return b.name === t.blockName;
                     }) || filter.blocks.length === 0) &&
                     (filter.dates.all || (filter.dates.from.valueOf() <= milliDate.valueOf() &&
                     filter.dates.to.valueOf() >= milliDate.valueOf()));
             })
             .Select(function (t) {
                 return t;
             })
             .ToArray();
        }

        this.transformDataForTables = function () {
            Enumerable.From(scoutStops).ForEach(function (s) {
                s.isPest = s.isPest === true ? "Yes" : "No";
                s.numberOfTrees = s.numberOfTrees.toString();
                s.numberOfBugs = s.numberOfBugs.toString();
                s.date = new XDate(s.date).toString("yyyy-MM-dd");
            });

            Enumerable.From(treatments).ForEach(function (t) {
                t.date = new XDate(t.date).toString("yyyy-MM-dd");
            });
        }

    }]);