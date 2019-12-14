# DMPeek

Upon requesting and downloading "your Twitter data" from Twitter, you can find a JavaScript file with your entire direct message (DM) history in machine-readable form inside. A while ago, I cooked up a little HTML+JS thingy to display the contents of these files like a message history on Twitter, and now rewrote it from scratch as a C# application. So far, the functionality is extremely limited, but I hope to add more.

## Current features

### Load File

Loads a JSON file with a single conversation inside. As of now, you have to manually open the direct-messages.js inside your archive and copy a single conversation into a new JSON file, something I hope to automate in the future. The program will then display the contents of said file.

### Check Connection

Checks whether a connection with Twitter can be established by pinging it. So far useless, I plan to implement a feature to look up a user's current username, since the archive Twitter provides only stores their numeric UUID and not their username.

## Planned Features

### Load Bulk

Load the entire direct-messages.js and list all conversations inside, allowing users to choose which one they want to read.
