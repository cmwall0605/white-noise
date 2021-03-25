## Gameplay  
The game takes place in a series of procedurally generated dungeons, each of which are controlled by a manager. Managers act as the main antagonist of the level, both interacting with Charlie in dialouge in the level and being the boss of the level. There are three points in the gameplay which comprise of the main game loop:  
1. **Explore**: Each level contains a series of rooms, most are encounters which the lead to a fight between Charlie and an enemy. These fight rooms should take up 1/2 of the rooms, but there are some unique encounters. These include  
  1. **Shop**: The shop is where Charlie can buy upgrades and addons to their equipment. Usually basic but sometimes there is a slight variation to the shop, either there is an event which occurs at the entrance, or there is a theme to the shop. There should be only 1 shop per level.
  2. **Event**: Events tend to be small occurances where Charlie enters a "Choose your own adventure" situation, naviagating through a series of options which leads to some result, some good, some bad, some even neutral. Events should take up maybe 1/4 of the rooms in a level.
  3. **Other**: There are other types of rooms which are rare. These include
     1. filler
     2. for
     3. now
2. **Fight**: Combat is a turn-based RPG between Charlie and a gang of enemies. Fighting generally goes based on a timer, where there is a timer which represents a single turn. Based on stats, Charlie and each enemy is given a slot in the timer, which increases in size and location of the timer based on agility and endurance. Higher agility means earlier and higher endurance means longer turns. Each action takes a certain amount of time, so the more time Charlie has, the more actions they can do. Charlie can use actions which go over their alloted time, but there is a % chance of the enemy countering it, meaning not only is the action prevented, but they can do a single action for free. This goes both ways though, as some enemies may be cocky and try to bite off more than they can chew, which can be punished with a free move for Charlie. May potentially add small minigames for actions like in paper mario to spice up the game, and vice versa for special actions done by enemies. 
3. **Upgrade**: Upgrading in this game generally consists of items, levelling up, and perks.
  4. **Items**: Items are found from shops, bosses, events, and sometimes drops from enemies in fights. Charlie starts off with a 2x2 equipment slot which they can equip slot items in. Slot items tend to be weapons/armor. While not always the case, weapons generally give more actions to Charlie while armor gives stats buffs to Charlie. Additionally, there are useable items in slots which Charlie can use up in exchange for some utility in the fight. As Charlie levels up, they can expend their equipment slot to provide for more equipment to use. Trinkets are slightly different to slot items in that they can be equipped onto slot items and they provide a buff to whatever that slot item already does.
  5. **Level Up**: Levelling up occurs when the player gets enough experience from fighting. Levelling up gives a boost in hp, can increase a single stat, and a perk.
  6. **Perks**: Perks are a general way of giving a buff to Charlie. Perks widely vary in both what they do and how you can get them. Ie some perks can be gotten by simply levelling up while others are given by a special means. Additionally, some perks stay with Charlie through the rest of the game, though these typically are very special perks.  
**Managers**  
There is a hierachry of managers which is very similar to the system found in Shadows of Modor, where each manager reacts to your last encounter. The interactions from Charlie to the Manager are:  
* Kill  
* Spare  
* Befriend (special requirements are needed to spare per manager)  
The interactions from the manager to Charlie are  
* Kill  
Generally, Charlie will eventually come to the cross roads of continually sparing but not befriending, befriending, or killing the manager (though killing and befriending are the only two big options). Killing the manager gives a special perk, as well a immediately givng an special item (and that item being added to the spawn pool). However, Charlie will never see that character again. Sparing them on the other hand adds them to Charlie's break room as a character they can interact with. Often Charlie can do quests for the past-managers, which either involve doing/getting something special in the run, or going through a special challenege run. The reward for 100%-ing the friend ship of a manager is a new flair to add into Charlie's break room. The reward for being a "good" character is the progression of the story with these managers and the reward for being a "bad" character is the physical progression by the special perk given. When a manager is killed, they will forever be replaced with a basic manager.  