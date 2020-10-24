import time
import threading
import re

from actionmanager import ActionManager
from errorhandling import print_message
from irc import Irc
from message import Message

class Ferrybot(object):

    def __init__(self, config):
        self.add_configuration(config)

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
            self.irc.send_message([Message("Looking for information?", "Type !commands to see how to interact with the Ferry to Helsinki!", True)])
            self.lastReminder = currTime

    def run_message_pull(self):
        while True:
            self.read_messages()

    def read_messages(self):
        newMessage = self.irc.receive_messages()

        print_message("INFO", newMessage)

        if not newMessage or not self.should_parse_message_contents(newMessage[0]['message']):
            return

        messageList = self.parse_message_for_action(newMessage[0]['message'])

        if not self.actionManager.is_action_key_recognized(messageList[0]):
            self.irc.send_message([Message("Command %s is not recognized", "Type !commands for possible commands" % messageList[0]).create_message()])
            return

        self.actionManager.run(messageList[0])


    def should_parse_message_contents(self, messageContents):
        return messageContents[0] == '!'
            
    def parse_message_for_action(self, messageContents):
        return re.compile(r'\s').split(messageContents, 1)

        

