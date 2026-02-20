from tkinter import Tk

class KitchenKeeper:
    def __init__(self, root):
        pass #BUILD GUI HERE
    """
    The GUI shoudl enable to admin to...
    - Add a new item to the inventory
    - Remove an item from the inventory
    - submit a recipe to adjust food inventory
    - view the current inventory
    - schedule time to send email with recipes
    """

if __name__ == '__main__':
    root = Tk()
    root.title('Kitchen Keeper')
    app = KitchenKeeper(root)
    root.mainloop()