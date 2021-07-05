# FerryToHelsinki
App to run a live adventure game in twitch

Pre-Requisites:
  1) SQL Server instance needs to be setup to build a record of messages. Utilize SQL Server Development Edition for local testing.

Twitch Setup:
  1) Create a new twitch account - in this case ours was "ferry_to_helsinki"
  2) Navigate to the channel specified in your setup
  3) Find the chat, ensure the bot is added as a mod by ensuring the channel owner puts this in the chat: /mods ferry_to_helsinki

Initial Setup:
  1) In the FerryToHelsinki solution create an appsettings.json file at the root and add the following configuration:
     "ConnectionStrings": {
         "FerryToHelsinkiContext:" "YOUR_CONNECTION_STRING"
     }
  2) In the FerryToHelsinkiBot solution create a config.py file at the root and add the following configuration that the system expects. 

`

"twitch_config":
     'irc': {
        'server': 'irc.twitch.tv',
        'port': 6667
     },
     
     'account': {
        'username': 'YOUR_TWITCH_USERNAME',
        'password': 'TWITCH_OAUTH' (http://twitchapps.com/tmi),
        'channel': 'TWITCH_CHANNEL'
     },
     
     'bot': {
        'name': 'YOUR_BOT_NAME',
        'reminder': 'SECONDS_BEFORE_BOT_SENDS_REMINDER_TEXT' (150)
        'timeout': 'TIMEOUT_OF_MESSAGES_IN_SECONDS' (5)
     }
 `
    
 At this point you should be able to run your branch locally and see output!
