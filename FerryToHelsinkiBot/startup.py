from sys import exit
from ferrybot import ferrybot
from config import twitch_config

try:
	ferry = ferrybot(twitch_config)
	ferry.run()
except KeyboardInterrupt: # need to fix this
	exit()

