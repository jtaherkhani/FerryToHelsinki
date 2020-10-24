import re

from config import twitch_config

class Message(object):
    """handles creation of twitch messages"""
    #twitch message rules
    max_line_length_title_character_twitch = 43 #reduced as mods get an icon
    max_line_length_body_character_twitch = 75
    end_line_title_character = "-"
    end_line_body_character = "."
    add_message_color = r'/me '
    maxFirstLineLength = max_line_length_title_character_twitch - len(twitch_config['bot']['name'] + ": ")

    def __init__(self, title, messageBody = "", shouldColor = False):
        self.title = title
        self.messageBody = messageBody
        self.shouldColor = shouldColor

    def create_message(self):
        formattedMessage = self.format_title() + self.format_body()
        return self.add_message_color + formattedMessage if self.shouldColor else formattedMessage

    def format_title(self):
        return self.title + self.get_characters_to_add(self.title, self.end_line_title_character, self.max_line_length_title_character_twitch, True)

    def format_body(self):
        messageBodyList = re.compile(r'\n').split(self.messageBody)

        formattedBody = ""
        for messageLine in messageBodyList:
            formattedBody += messageLine

            if (len(messageBodyList) > 1):
                formattedBody += self.get_characters_to_add(messageLine, self.end_line_body_character, self.max_line_length_body_character_twitch, False)

        return formattedBody

    def get_characters_to_add(self, messageLine, characterToAppend, maxLineCharactersOnLine, isFirstLine):
        lineCharacters = len(messageLine)
        maxLineCharacters = self.maxFirstLineLength if isFirstLine else maxLineCharactersOnLine

        if lineCharacters > maxLineCharacters:
            return self.get_characters_to_add(messageLine[maxLineCharacters:], characterToAppend, maxLineCharacters, False)

        return (characterToAppend * (maxLineCharacters - lineCharacters))