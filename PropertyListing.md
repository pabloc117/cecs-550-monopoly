# Schema #

```
<?xml version="1.0" encoding="UTF-8"?>
<Properties>
	<Property Name="" Cost="" GroupColor="" Location="" BoardSide="" Rent="" House1="" House2="" House3="" House4="" Hotel="" HouseCost="" Mortgage="" IsSpecial="" IsCorner="" ImageLocation="" />
</Properties>
```


# Attributes #
  * Name: The name of the property
  * Cost: The value of the property
  * GroupColor: The group color the property belongs to; this is the RGB values seperatated by semicolons. ex: Red = "255;0;0"
  * Location: The location on the board.  "GO" is 0, location number increments by one going clockwise.
  * BoardSide: The The side of the board the property is found using the following integer values:
```
0: Top
1: Right
2: Bottom
3: Left
```
  * House#: Cost of rent with # of houses
  * Hotel: Rent with hotel
  * HouseCost: Cost of developing houses/hotel
  * Mortgage: Mortgage price
  * IsSpecial: Determines whether or not it is a special property (not part of a color group)
  * IsCorner: determines whether or not it is a corner property
  * ImageLocation: Location on disk relative to the install path of property image.