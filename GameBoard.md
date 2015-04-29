# Creating a New Instance #
GameBoard is intended to be used only once, as a child of the main window or its children.

On creation, GameBoard will access the ThemeParser class to get a dictionary of PropertyListings.  With this dictionary, the game board will appropriately generate the board with desired properties.

The game board will also include a DiceRack, which will handle rolls.