class KitchenKeeperEmail:
    def __init__(self, email):
        self.email = email

    def send_email(self, message):
        print(f"Sending email to {self.email} with message: {message}")
    
    def format_message(self):
        return "This is a test message"
    
if __name__ == '__main__':
    pass #test code