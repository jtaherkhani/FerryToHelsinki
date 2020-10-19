import socket
import sys
import re

from message import message
from errorhandling import print_message

class irc(object):
    """handles communication between twitch and the ferry bot"""
    
    #twitch irc rules
    max_line_length_twitch = 44 #reduced as mods get an icon
    end_line_character = "-"

    def __init__(self, config):
        self.add_configuration(config)
        self.sock = self.connect_socket(0)
        self.login_twitch()

    def add_configuration(self, config):
        self.server = config['irc']['server']
        self.port = config['irc']['port']
        self.password = config['account']['password']
        self.channel = config['account']['channel']
        self.botName = config['bot']['name']
        self.timeout = config['bot']['timeout']
        self.maxFirstLineLength = self.max_line_length_twitch - len(self.botName + ": ")

    def connect_socket(self, retryCount):
        sock = socket.socket(socket.AF_INET, socket.SOCK_STREAM) #tcp/ip configuration
        sock.settimeout(self.timeout)

        try:
            sock.connect((self.server, self.port))
            sock.settimeout(None) #reset timeout after connecting
            return sock
        except:
            print_message('ERROR', 'Unable to connect to twitch: ( %s : %i )' % (server, port))

            if retryCount < 2:
                sock.close() #a new socket will be created
                return self.connect_socket(retryCount + 1)
            else:
                sys.exit()

    def login_twitch(self):
        self.sock.send(("USER %s\r\n" % self.botName +
                        "PASS %s\r\n" % self.password +
                        "NICK %s\r\n" % self.botName +
                        "JOIN #%s\r\n" % self.channel).encode())

        if not self.check_login_status(self.receive()):
            print_message('ERROR', 'Invalid login: ( %s : %s)' % (self.channel, self.password))
            sys.exit()
        else:
            print_message('INFO', 'Login was a success!')

        print_message('INFO', 'Joined the chat!')


    # Sources Cited - https://github.com/aidanrwt/twitch-plays/blob/master/lib/irc.py
    def check_login_status(self, messageData):
        if not re.match(r'^:(testserver\.local|tmi\.twitch\.tv) NOTICE \* :Login unsuccessful\r\n$', messageData): return True

    # Sources Cited - https://github.com/aidanrwt/twitch-plays/blob/master/lib/irc.py
    def check_has_message(self, messageData):
        return re.match(r'^:[a-zA-Z0-9_]+\![a-zA-Z0-9_]+@[a-zA-Z0-9_]+(\.tmi\.twitch\.tv|\.testserver\.local) PRIVMSG #[a-zA-Z0-9_]+ :.+$', messageData)

    # Sources Cited - https://github.com/aidanrwt/twitch-plays/blob/master/lib/irc.py
    def parse_message(self, messageData):
        return {
            'channel': re.findall(r'^:.+\![a-zA-Z0-9_]+@[a-zA-Z0-9_]+.+ PRIVMSG (.*?) :', messageData)[0],
            'username': re.findall(r'^:([a-zA-Z0-9_]+)\!', messageData)[0],
            'message': re.findall(r'PRIVMSG #[a-zA-Z0-9_]+ :(.+)', messageData)[0]
        }

    def receive(self, amount=1024):
        return self.sock.recv(amount).decode()

    def receive_messages(self, amount=1024):
        messageData = self.receive(amount)

        if not messageData:
            print_message('ERROR', 'Attempting to reconnect...')
            self.sock.close()
            self.sock = self.connect_socket(0)
            return self.sock

        self.ping(messageData)

        if self.check_has_message(messageData):
            return [self.parse_message(line) for line in filter(None, messageData.split('\r\n'))]

    def ping(self, messageData):
        if messageData.startswith('PING'):
            self.sock.send(messageData.replace('PING', 'PONG').encode())

    def send_message(self, messages):
        for currMessage in messages:
            formattedMessage = self.format_title(currMessage[message.title_key]) + currMessage[message.message_body_key]
            print_message("INFO", ("Message sent : %s" % formattedMessage))
            sent = self.sock.send(("PRIVMSG #%s :%s\n" % (self.channel, formattedMessage)).encode())

    def format_title(self, title):
        return title + self.get_characters_to_add(title, True)

    def get_characters_to_add(self, messageLine, isFirstLine):
        lineCharacters = len(messageLine)
        maxLineCharacters = self.maxFirstLineLength if isFirstLine else self.max_line_length_twitch

        if lineCharacters >= maxLineCharacters:
            return self.get_whitespace_to_add(messageLine[maxLineCharacters:], False)

        return (self.end_line_character * (maxLineCharacters - lineCharacters))


