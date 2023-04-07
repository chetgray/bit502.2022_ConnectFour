# ConnectFour

> Greetings Professor Falken. Shall we play a game?

## Building & Running

There are two build profiles, Debug and Release.

The **Debug** profile points the data connection to a (local) instance
of SQL Server running on the machine. For that to work, you'll need to
publish the ConnectFour.Database project to the local SQL Server using
the included ConnectFour.Database\ConnectFour.Database.local.publish.xml
publish profile.

The **Release** profile points to the shared bit502 SQL Server, where
the schema is already set up. You'll just need to be on the Waystar LAN
or VPN for it to work, and it will allow you to play remotely against
other people.

## Enjoy!

Try a game against our AI, _Opportunistic Ophelia_, or start a
human-vs-human room and tell someone the room ID to join. Maybe we'll
start a tournament?
