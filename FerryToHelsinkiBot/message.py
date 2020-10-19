class message(object):
    """handles creation of twitch messages"""

    #message format details
    title_key = "title"
    message_body_key = "messageBody"

    def __init__(self, title, messageBody = ""):
        self.title = title
        self.messageBody = messageBody

    def create_message(self):
        return {
            self.title_key : self.title,
            self.message_body_key : self.messageBody
        }