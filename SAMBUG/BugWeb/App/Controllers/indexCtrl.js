angular.module("appMain")
    .controller("IndexCtrl", [
        "$scope", function($scope) {

            $scope.speciesList = [
                {
                    name: "Anolcus Campestris and Coenomorpha Nervosa",
                    lifeStages: [
                        {
                            name: "Adult - Anolcus Campestris",
                            image: {
                                url: "/content/images/wiki/anolcus campestris/anolcus campestris.png",
                                alt: "Adult - Anolcus Campestris"
                            }
                        },
                        {
                            name: "Adult - Coenomorpha Nervosa",
                            image: {
                                url: "/content/images/wiki/coenomorpha nervosa/coenomorpha nervosa.png",
                                alt: "Adult - Coenomorpha Nervosa"
                            }
                        }
                    ],
                    speciesName: {
                        heading: "Scientific Name",
                        body: "Anolcus Campestris/Coenomorpha Nervosa"
                    },
                    information: [
                        {
                            heading: "Appearance",
                            body: "These two species look very similar. Only adults are found on macadamias, and they are approximately 22 – 25 mm in length. The insects are brown to grey with varying colour patterns on the dorsal side which contain green and reddish colours"
                        },
                        {
                            heading: "Host range and biology",
                            body: "Anolcus is only found in avocados when there are fruit on the trees. It has never been observed in macadamias. Coenomorpha occurs on many crops."
                        },
                        {
                            heading: "Is it a problem in macadamias?",
                            body: "No."
                        },
                        {
                            heading: "Time present in macadamias",
                            body: "Coenomorpha occurs in macadamias during the winter months."
                        }
                    ]
                },
                {
                    name: "Cletus",
                    lifeStages: [
                        {
                            name: "Adult",
                            image: {
                                url: "/content/images/wiki/cletus/cletus.png",
                                alt: "Adult - Cletus"
                            }
                        }
                    ],
                    speciesName: {
                        heading: "Scientific Name",
                        body: "Cletus spp."
                    },
                    information: [
                        {
                            heading: "Appearance",
                            body: "Adults are approximately 20 mm in size and they can often be mistaken for coconut bugs. They are red – pinkish in colour. It can easily be distinguished from the coconut bug by the presence of two spike-like structures on the thorax. It also has a short mouth part, whereas coconut bugs have long mouth parts that extends part the legs."
                        },
                        {
                            heading: "Host range and biology",
                            body: "Little is known about this bug. It does occur on Amaranthus spp."
                        },
                        {
                            heading: "Is it a problem in macadamias?",
                            body: "No, due to the short mouth part."
                        },
                        {
                            heading: "Time present in macadamias",
                            body: "During flowering."
                        }
                    ]
                },
                {
                    name: "Coconut Bug",
                    lifeStages: [
                        {
                            name: "Eggs",
                            image: {
                                url: "/content/images/wiki/coconut bug/coconut bug egg.png",
                                alt: "Eggs - Coconut Bug"
                            }
                        },
                        {
                            name: "Instar 1",
                            image: {
                                url: "/content/images/wiki/coconut bug/coconut bug 1st instar.png",
                                alt: "Instar 1 - Coconut Bug"
                            }
                        },
                        {
                            name: "Instar 2",
                            image: {
                                url: "/content/images/wiki/coconut bug/coconut bug 2nd instar.png",
                                alt: "Instar 2 - Coconut Bug"
                            }
                        },
                        {
                            name: "Instar 3",
                            image: {
                                url: "/content/images/wiki/coconut bug/coconut bug 3rd instar.png",
                                alt: "Instar 3 - Coconut Bug"
                            }
                        },
                        {
                            name: "Instar 4",
                            image: {
                                url: "/content/images/wiki/coconut bug/coconut bug 4th instar.png",
                                alt: "Instar 4 - Coconut Bug"
                            }
                        },
                        {
                            name: "Instar 5",
                            image: {
                                url: "/content/images/wiki/coconut bug/coconut bug 5th instar.png",
                                alt: "Instar 5 - Coconut Bug"
                            }
                        },
                        {
                            name: "Adult",
                            image: {
                                url: "/content/images/wiki/coconut bug/coconut bug adult.png",
                                alt: "Adult - Coconut Bug"
                            }
                        }
                    ],
                    speciesName: {
                        heading: "Scientific Name",
                        body: "Pseudotheraptus wayii"
                    },
                    information: [
                        {
                            heading: "Appearance",
                            body: "1st instar nymphs are approximately 5 mm in length and adults 15 - 20 mm in length. Males are slightly smaller than females. All stages are red-pink in colour."
                        },
                        {
                            heading: "Host range and biology",
                            body: "Wide host range, including avocado, mango, litchi and macadamia. Eggs are laid singular, but the female can lay many eggs in her lifetime. They are strong flyers, alert and difficult to catch."
                        },
                        {
                            heading: "Is it a problem in macadamias?",
                            body: "Yes, it has long mouth parts and can cause damage at any stage of nut development, even after shells have hardened."
                        },
                        {
                            heading: "Time present in macadamias",
                            body: "Throughout the period when there are nuts on the trees. Coconut bug population usually peaks around April."
                        }
                    ]
                },
                {
                    name: "Green Vegetable Bug (Southern Green Stink Bug)",
                    lifeStages: [
                        {
                            name: "Adult",
                            image: {
                                url: "/content/images/wiki/Green Vegetable Bug/Green Vegetable Bug.png",
                                alt: "Adult - Green Vegetable Bug"
                            }
                        },
                        {
                            name: "Adult - Green Invariant",
                            image: {
                                url: "/content/images/wiki/Green Vegetable Bug/Green Vegetable Bug_green variant.png",
                                alt: "Adult - Green Vegetable Bug Variant"
                            }
                        }
                    ],
                    speciesName: {
                        heading: "Scientific Name",
                        body: "Nezara viridula"
                    },
                    information: [
                        {
                            heading: "Appearance",
                            body: "Adults are approximately 15 mm long. They are usually uniform green, however other colour variants do occur. This ranges from green with a creamy colour behind the head, or green with yellow spots to completely yellow in colour."
                        },
                        {
                            heading: "Host range and biology",
                            body: "This insect occurs throughout the country on hundreds of host plants. Only adults have been observed in macadamias."
                        },
                        {
                            heading: "Is it a problem in macadamias?",
                            body: "Yes, but limited. It has short mouth parts and can only cause damage when nuts are young with thin husks and shells. It is usually present in macadamias when shells have already hardened."
                        },
                        {
                            heading: "Time present in macadamias",
                            body: "Throughout the year, but the population usually peaks in March and April."
                        }
                    ]
                },
                {
                    name: "Small Green Stink Bug",
                    lifeStages: [
                        {
                            name: "Adult",
                            image: {
                                url: "/content/images/wiki/Nezara Prunasis/Nezara Prunasis.png",
                                alt: "Adult - Nezara Prunasis"
                            }
                        }
                    ],
                    speciesName: {
                        heading: "Scientific Name",
                        body: "Nezara prunasis"
                    },
                    information: [
                        {
                            heading: "Appearance",
                            body: "Adults are 9 – 11 mm in length. At first glance it looks like a small two spotted stink bug, without the two spots behind the thorax on the dorsal side. This insect is almost uniform in colour, but slight yellow patches may be visible on the dorsal side."
                        },
                        {
                            heading: "Host range and biology",
                            body: "Unknown. "
                        },
                        {
                            heading: "Is it a problem in macadamias?",
                            body: "No, the mouth part is too short to cause any damage."
                        },
                        {
                            heading: "Time present in macadamias",
                            body: "Usually during winter months."
                        }
                    ]
                },
                {
                    name: "Powdery Stink Bug",
                    lifeStages: [
                        {
                            name: "Instar 2",
                            image: {
                                url: "/content/images/wiki/Powdery Stink Bug/Pseudatelus raptoria 2nd Instar.png",
                                alt: "Instar 2 - Powdery Stink Bug"
                            }
                        },
                        {
                            name: "Instar 3",
                            image: {
                                url: "/content/images/wiki/Powdery Stink Bug/Pseudatelus raptoria 3rd Instar.png",
                                alt: "Instar 3 - Powdery Stink Bug"
                            }
                        },
                        {
                            name: "Instar 4",
                            image: {
                                url: "/content/images/wiki/Powdery Stink Bug/Pseudatelus raptoria 4th Instar.png",
                                alt: "Instar 4 - Powdery Stink Bug"
                            }
                        },
                        {
                            name: "Instar 5",
                            image: {
                                url: "/content/images/wiki/Powdery Stink Bug/Pseudatelus raptoria 5th Instar.png",
                                alt: "Instar 5 - Powdery Stink Bug"
                            }
                        },
                        {
                            name: "Adult",
                            image: {
                                url: "/content/images/wiki/Powdery Stink Bug/Pseudatelus raptoria.png",
                                alt: "Adult - Powdery Stink Bug"
                            }
                        }
                    ],
                    speciesName: {
                        heading: "Scientific Name",
                        body: "Pseudatelus raptoria"
                    },
                    information: [
                        {
                            heading: "Appearance",
                            body: "Nymphs are often grey to white in colour, and have a fluffy or powdery appearance. The powdery substance can easily be removed when handling the insect. The eggs are also woolly / powdery. Adults are approximately 18 mm in length."
                        },
                        {
                            heading: "Host range and biology",
                            body: "This insect occurs on all crops. It does breed on macadamias and all stages can thus be found on macadamias."
                        },
                        {
                            heading: "Is it a problem in macadamias?",
                            body: "No. Fortunately these insects’ mouth parts are short and they do not cause any damage on macadamias."
                        },
                        {
                            heading: "Time present in macadamias",
                            body: "Throughout the year."
                        }
                    ]
                },
                {
                    name: "Small Brown Stink Bug",
                    lifeStages: [
                        {
                            name: "Adult",
                            image: {
                                url: "/content/images/wiki/Small Brown Bug/Small Brown Bug.png",
                                alt: "Adult - Small Brown Stink Bug"
                            }
                        }
                    ],
                    speciesName: {
                        heading: "Scientific Name",
                        body: "Unknown"
                    },
                    information: [
                        {
                            heading: "Appearance",
                            body: "Adults are 5 – 8 mm in length. They are brown in colour, but have two yellow dots on the dorsal side behind the thorax. Two spike-like structures also appear to protrude on the sides of the thorax."
                        },
                        {
                            heading: "Host range and biology",
                            body: "It occurs in litchi plants during autumn, also on mangoes. It occurs on avocado and macadamias to a lesser extent, and possibly several other crops."
                        },
                        {
                            heading: "Is it a problem in macadamias?",
                            body: "No. The mouth parts is too short to cause damage."
                        },
                        {
                            heading: "Time present in macadamias",
                            body: "Unknown – possibly throughout the year."
                        }
                    ]
                },
                {
                    name: "Two Spotted Stink Bug",
                    lifeStages: [
                        {
                            name: "Eggs",
                            image: {
                                url: "/content/images/wiki/Two Spotted Bug/Two Spotted Bug eggs.png",
                                alt: "Eggs - Two Spotted Stink Bug"
                            }
                        },
                        {
                            name: "Instar 1",
                            image: {
                                url: "/content/images/wiki/Two Spotted Bug/Two Spotted Bug 1st instar.png",
                                alt: "Instar 1 - Two Spotted Stink Bug"
                            }
                        },
                        {
                            name: "Instar 2",
                            image: {
                                url: "/content/images/wiki/Two Spotted Bug/Two Spotted Bug 2nd instar.png",
                                alt: "Instar 2 - Two Spotted Stink Bug"
                            }
                        },
                        {
                            name: "Instar 3",
                            image: {
                                url: "/content/images/wiki/Two Spotted Bug/Two Spotted Bug 3rd instar.png",
                                alt: "Instar 3 - Two Spotted Stink Bug"
                            }
                        },
                        {
                            name: "Instar 4",
                            image: {
                                url: "/content/images/wiki/Two Spotted Bug/Two Spotted Bug 4th instar.png",
                                alt: "Instar 4 - Two Spotted Stink Bug"
                            }
                        },
                        {
                            name: "Instar 5",
                            image: {
                                url: "/content/images/wiki/Two Spotted Bug/Two Spotted Bug 5th instar.png",
                                alt: "Instar 5 - Two Spotted Stink Bug"
                            }
                        },
                        {
                            name: "Adult",
                            image: {
                                url: "/content/images/wiki/Two Spotted Bug/Two Spotted Bug adult.png",
                                alt: "Adult - Two Spotted Stink Bug"
                            }
                        }
                    ],
                    speciesName:
                    {
                        heading: "Scientific Name",
                        body: "Bathycoelia natalicola"
                    },
                    information: [
                        {
                            heading: "Appearance",
                            body: "1st instar nymphs are approximately 2.5 mm in length and adults up to 18 mm long. Adults are green with a yellow edge lining the abdomen. Adults have two whitish spots behind the thorax, delineated with black markings in the centre of each spot, hence the name. Adults become more pale in winter months. It is easy to identify since it is the only green-coloured stink bug with a long mouth part."
                        },
                        {
                            heading: "Host range and biology",
                            body: "It is an indigenous species, but macadamia (which is an exotic plant) is the only true host plant that has been documented. Any information on alternative hosts should be communicated to SAMAC. Eggs are usually 14 in a packet."
                        },
                        {
                            heading: "Is it a problem in macadamias?",
                            body: "Yes, it is the most important pest of macadamias. It has long mouth parts and can cause damage at any stage of nut development, even after shells have hardened."
                        },
                        {
                            heading: "Time present in macadamias",
                            body: "From October until July. The numbers decrease in winter months. The population usually peaks in April. Some of these bugs overwinter in macadamias as nymphs."
                        }
                    ]
                },
                {
                    name: "Yellow Edged Stink Bug",
                    lifeStages: [
                        {
                            name: "Adult",
                            image: {
                                url: "/content/images/wiki/Yellow Edged Bug/Yellow Edged Bug.png",
                                alt: "Adult - Yellow Edged Stink Bug"
                            }
                        }
                    ],
                    speciesName: {
                        heading: "Scientific Name",
                        body: "Nezara pallindoconspersa"
                    },
                    information: [
                        {
                            heading: "Appearance",
                            body: "Nymphs are 1.5 mm and adult females up to 18 mm in length. Males are smaller (11 -  14 mm). Nymphs look different from adults and are black after hatching with yellow dots on the dorsal side. Adults are green with the edge of the abdomen yellowish with dark lines that accentuate each segment. The mouth part is short."
                        },
                        {
                            heading: "Host range and biology",
                            body: "Yellow edged bugs occur on macadamias, avocadoes, castor oil, tassel berry and soy beans."
                        },
                        {
                            heading: "Is it a problem in macadamias?",
                            body: "Yes, in the early stages of nut development. The mouth parts are short and cannot penetrate the husk and shell in order to cause damage on larger macadamia nuts."
                        },
                        {
                            heading: "Time present in macadamias",
                            body: "From March until November."
                        }
                    ]
                }
            ];


        }
    ]);