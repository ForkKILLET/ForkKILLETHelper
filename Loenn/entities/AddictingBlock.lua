return {
    name = "ForkKILLETHelper/AddictingBlock",
    fillColor = { 0 / 255.0, 0 / 255.0, 0 / 255.0 },
    borderColor = { 1.0, 1.0, 1.0 },
    nodeLineRenderType = "line",
    nodeLimits = { 0, 1 },
    placements = {
        {
            name = "AddictingBlock",
            data = {
                width = 10,
                height = 10,
                triggerMethod = 1
            }
        }
    },
    fieldInformation = {
        triggerMethod = {
            options = {
                ["top"]                 = 1,        -- 0b001
                ["left"]                = 2,        -- 0b010
                ["right"]               = 4,        -- 0b100
                ["top & left"]          = 3,        -- 0b011
                ["top & right"]         = 5,        -- 0b101
                ["left & right"]        = 6,        -- 0b110
                ["top & left & right"]  = 7         -- 0b111
            }
        }
    }
}