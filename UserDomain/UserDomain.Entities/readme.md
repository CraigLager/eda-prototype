Handles entity modelling in the source of truth

**User**
The entity/aggregate. Contains the capability to re-hydrate from past events

**UserRepository**
Provides read-only operations for user retrieval

**UserService**
Service for executing commands against users. Note that deletions delete the encryption key, not the data