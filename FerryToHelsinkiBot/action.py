from message import Message

class Action(object):
    """Handles actions taken by the ferry to helsinki bot"""

    #Action message keys
    command_key = "!commands"
    command_short_key = "!com"
     
    #Action message list
    action_key_set = {
        command_key,
        command_short_key
    }
     
    #Action names
    list_commands = "LIST_COMMANDS"
     
    #Action message mapping
    action_mapping = {
        command_key: list_commands,
        command_short_key: list_commands
    }
     
    #Action message description mapping
    action_synonymns = {
        command_key : [command_short_key]
    }
     
    #Action run parametersion_key
    needs_webhook = "needs_webhook"
    send_message = "send_message"
    message_contents = "message_contents"
     
     
    def __init__(self, actionKey):                        
        self.actionRulesDictionary = self.set_action(self.action_mapping.get(actionKey))

    def set_action(self, actionCommand):
        if actionCommand == self.list_commands:
            return self.action_list_commands_setup()
     
    def action_list_commands_setup(self):
        return {
            self.send_message: True,
            self.message_contents: self.create_list_messages()
        }

    def create_list_messages(self):
        return [Message("List of commands", self.create_action_list_message_body())]
     
    def create_action_list_message_body(self):
        messageBody = ""
        for actionMessage in self.action_key_set:
            if actionMessage in self.action_synonymns:
                messageBody += " " + actionMessage
                for synonymn in self.action_synonymns.get(actionMessage):
                    messageBody += "...(%s) " % (synonymn) #account for when we don't have any synonymns

                messageBody += "\n"
         
        return messageBody
