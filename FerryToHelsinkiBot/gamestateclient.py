import requests
from errorhandling import print_message

class gamestateclient(object):
    """client for interacting with game state framework"""
    def __init__(self):
        self.URL = "https://ferrytohelsinki.azurewebsites.net/api/GameState";

    def isGameStarted(self):
        response = requests.get(self.URL)
        return response.text