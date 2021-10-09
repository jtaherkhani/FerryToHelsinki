import time
import threading
import re

from actionmanager import ActionManager
from errorhandling import print_message
from irc import Irc
from message import Message
from messageclient import messageclient

class Ferrybot(object):

    def __init__(self, config):
        self.add_configuration(config)
        self.messageclient = messageclient()

    def add_configuration(self, config):
        self.irc = Irc(config)
        self.actionManager = ActionManager(self.irc)
        self.reminder = config['bot']['reminder']
        self.lastReminder = 0.0 #set to float as this will represent time

    def run(self):
        pushThread = threading.Thread(target = self.run_message_push)
        pullThread = threading.Thread(target = self.run_message_pull)

        pushThread.start()
        pullThread.start()

    def run_message_push(self):
        while True:
            self.send_reminder()

    def send_reminder(self):
        currTime = time.monotonic()

        if self.lastReminder == 0 or currTime - self.lastReminder > self.reminder:
            self.irc.send_message([Message("", "Type >{COMMAND} to interact with the Ferry to Helsinki!", True)])
            self.lastReminder = currTime

    def run_message_pull(self):
        while True:
            self.read_messages()

    def read_messages(self):
        newMessage = self.irc.receive_messages()

        print_message("INFO", newMessage)

        if not newMessage or not self.should_parse_message_contents(newMessage[0]['message']):
            return

        message = newMessage[0]['message']
        fixedMesage = message[1:]

        self.messageclient.post(newMessage[0]['username'], fixedMesage)


    def should_parse_message_contents(self, messageContents):
        return messageContents[0] == '>'


        

