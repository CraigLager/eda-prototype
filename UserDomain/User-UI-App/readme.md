A sample UI which does some work with users.

Key concepts:

* Dependency inversion. Swapping out the user repository or message bus implementation is simple and done once.
* No business logic around entities, everything abstracted through service layer
* Event subscription to the message bus - would normally be part of a different service