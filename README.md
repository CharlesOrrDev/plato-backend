## create commenting system
  # Done #
  1. add variable into blog model to hold an array of comments √

  2. make the comment array hold the initial comment and then an array of replys √
  
  3. Save total amount of stars from reviews √

  4. save total amount of reviews √

  5. divide total amount of stars by total amount of reviews √

  6. save how many likes a post has gotten as well as save what posts you liked √

  7. validate the user attempting to like or review to see if they have already liked or reviewed that post √

  8. add a place to store which userId is attempting to friend request you √

  9. add place to store which userId is your friend √

  10. add place to store out going friend requests and incoming friend request separately √

  11. create a model to hold an
    Name: MessageModel
    ~ Id √
    ~ ConversationId √
    ~ UserId √
    ~ Message √

  12. create a Model to hold an
    Name: ConversationModel
    ~ Id √
    ~ UserOneId √
    ~ UserTwoId √
    ~ List containing multiple MessageModel objects √

  13. create ConversationServices which will take in the users Id and the Id of the user they want to send a message to and create a ConversationModel in the database with that information √

  14. in the conversation services take in the conversation Id which will be obtained by the two Id's present in the conversation and add a MessageModel message into the conversation Message List holding the senders Id as well as the Id of the Conversation it belongs to √

  15. conversations services will need
    ~ GetAllConversations √
    ~ GetConversationById √
    ~ GetConversationsByUserId √
    ~ Delete/Edit Conversation √

  16. make conversations controller with
    ~ GetAllConversations √
    ~ AddConversation √
    ~ AddMessage √
    ~ GetConversationById √
    ~ GetMessageById √
    ~ GetConversationsByUserOneId √
    ~ GetConversationsByUserTwoId √
    ~ GetMessageByUserId √
    ~ GetMessageByUserIdAndConversationId √
    ~ EditConversations √
    ~ EditMessage √

  17. do testing to make sure everything is functioning as it was planned to √

  18. made changes to blog model to match additions requested by team lead √

  19. added premium boolean to user model √

  20. added interests and saved recipes to user model √
  
  # To-Do #