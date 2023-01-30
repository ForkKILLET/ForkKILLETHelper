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
                ["stand"]   = 1,
                ["climb"]   = 2,
                ["both"]    = 3
            }
        }
    }
}