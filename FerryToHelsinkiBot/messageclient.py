import requests
from errorhandling import print_message

class messageclient(object):
    """client for interacting with message framework"""
    def __init__(self):
        self.URL = "https://ferrytohelsinki.azurewebsites.net/api/Messages";

    def post(self, message):
        myjson = {'UserName': 'josh', 'MessageContents': message}
        response = requests.post(self.URL, json=myjson)
        responseString = response.json()

        print_message("MESSAGE-INFO", responseString)
