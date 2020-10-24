from sys import exit
from ferrybot import Ferrybot
from config import twitch_config

try:
	ferry = Ferrybot(twitch_config)
	ferry.run()
except KeyboardInterrupt: # need to fix this
	exit()

