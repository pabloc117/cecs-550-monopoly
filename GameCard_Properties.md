# Schema #

```
<GCards>
<Card Type="Chance" Jump="" Move="0" Collect="200" Pay="0" PayTo="NULL" num="1" CollectFrom="NULL" Text="Advance to Go. Collect $200" />
</GCards>
```

# Introduction #

Properties of Game cards - Chance and Community Chest


# Details #

  * Type = Chance or Community
  * Jump = location of the property to jump to (if Jump != Null Move must = null)
  * Move = number of spaces to move (if Move != null, Jump must = null)
  * Collect = the amount of money to be collected by players
  * Pay = amount the player have to pay
  * PayTo = who is the player paying to?
  * num = card ID
  * CollectFrom = who is the player collecting from?
  * Text = the text to be display for the card