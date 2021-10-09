import requests
from errorhandling import print_message

class messageclient(object):
    """client for interacting with message framework"""
    def __init__(self):
        self.URL = "https://ferrytohelsinki.azurewebsites.net/api/Messages";

    def post(self, username, message):
        myjson = {'UserName': username, 'MessageContents': message}
        response = requests.post(self.URL, json=myjson)
