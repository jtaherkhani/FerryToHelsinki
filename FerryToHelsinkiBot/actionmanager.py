from action import action
from irc import irc
from errorhandling import print_message

class actionmanager(object):
    """Manages actions the bot can take"""
    
    def __init__(self, irc):
        self.irc = irc


    def run(self, actionKey):
        currAction = action(actionKey)

        if currAction.needs_webhook in currAction.actionRulesDictionary:
            print_message('INFO', 'NEXT TO BUILD')

        if self.should_send_message(currAction):
            self.irc.send_message(currAction.actionRulesDictionary.get(currAction.message_contents))

    def should_send_message(self, currAction):
        return action.send_message in currAction.actionRulesDictionary and currAction.actionRulesDictionary.get(action.send_message) == True

    @staticmethod
    def is_action_key_recognized(actionKey):
        return actionKey in action.action_key_set



