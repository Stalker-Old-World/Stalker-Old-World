cmd-strank_get-desc = Get the rank of a player's current character
cmd-strank_get-help = Usage: strank_get <username>
cmd-strank-get-success = {$username}'s character "{$character}": {$rank} (index {$index}, {$time})
cmd-strank-error-args = Invalid arguments.
cmd-strank-error-no-entity = Player has no attached entity.
cmd-strank-error-no-component = Player's entity has no rank component.
cmd-strank-error-invalid-rank = Unknown rank name. Use tab-completion to see available ranks.
cmd-strank-error-unknown-character = unknown

cmd-strank_set-desc = Set the rank of a player's current character
cmd-strank_set-help = Usage: strank_set <username> <rank_name>
cmd-strank-set-success = Set {$username}'s rank to {$rank} (index {$index})

cmd-strank_transfer-desc = Transfer rank hours from an old character name to the current character
cmd-strank_transfer-help = Usage: strank_transfer <username> <old_character_name>
cmd-strank-transfer-success = Transferred {$time} from "{$oldName}" to "{$newName}" for {$username}
cmd-strank-transfer-not-found = No rank record found for character "{$oldName}" under this player
