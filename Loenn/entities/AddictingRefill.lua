local utils = require("utils")

return {
    name = "ForkKILLETHelper/AddictingRefill",
    texture = "objects/ForkKILLETHelper/addictingRefill/idle00",
    placements = {
        {
            name = "Addicting Refill",
            data = {
                oneUse = false,
                wave = true,
                oneTimeSatisfication = false,
                respawnTime = 2.5
            }
        }
    },
    rectangle = function(room, entity)
        return utils.rectangle(entity.x - 5, entity.y - 5, 10, 10)
    end
}
