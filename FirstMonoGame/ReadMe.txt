Warning Justification

3 Warning	CA1030	"Consider making 'Peach.FirePowerUpTransition()' an event."
We aren't using events.

4 Warning	CA1030	"Consider making 'IPowerUpState<T>.FireTransition(T)' an event."	
We aren't using events.

5 Warning	CA2227	"Change 'Level.Entities' to be read-only by removing the property setter."
We do not want Level.Entities to be read-only because certain classes need access to it. It is important for certain items upon their collision that they get deleted from this collection, so they no longer exist in the game.

6 Warning	CA1001	"Implement IDisposable on 'Level' because it creates members of the following IDisposable types: 'SpriteBatch'. If 'Level' has 
previously shipped, adding new members that implement IDisposable to this type is considered a breaking change to existing consumers."	
We decided we did not want to implement IDisposable on 'Level.' It is unnecessary.

7 Warning	CA1051	"Because field 'GameMain.level' is visible outside of its declaring type, change its accessibility to private and add a property, 
with the same accessibility as the field has currently, to provide access to it."	
Level cannot be made a property because there is a reference to it, and it cannot be made private because it is accessed outside of GameMain.

8 Warning	CA1815	"'AABB' should override the equality (==) and inequality (!=) operators."	
We did not want to do this for the functionality we wished for AABB to implement.

9 Warning	CA1815	"'AABB' should override Equals."	
We did not want to do this for the functionality we wished for AABB to implement.




