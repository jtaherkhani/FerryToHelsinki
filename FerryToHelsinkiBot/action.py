from message import message

class action(object):
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
    action_descriptions = {
        command_key : "lists available commands (/short !com)"   
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
        listMessages = [message("List of commands").create_message()]
        listMessages.extend(self.create_action_list_messages())

        return listMessages
     
    def create_action_list_messages(self):
        action_list = []
         
        for actionMessage in self.action_key_set:
            if actionMessage in self.action_descriptions:
                action_list.append(message(actionMessage, self.action_descriptions.get(actionMessage)).create_message())
     
        return action_list
