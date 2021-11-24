Feature: Posts
		User send and receive posts from JSONPlaceholder service

Scenario: Happy Path READ post requests
		Given a user posts request to service
		When user makes a request to all post
		Then there should be 100 posts in the response
		And posts should have all properties from response

Scenario: Happy Path Create post requests
		Given a user posts request to service
		And the user has userId is between 1 and 100
		When user posts with following title and body:
        | Title                     | Body                               |
        | Some random post title    | New sentence inside this post body |
		Then response has userId, posted title and body
		And response has post Id '101'

Scenario: Happy Path to Update posts
		Given a user posts request to service
		When user updates a post between 1 and 100 with values:
		| UserId | Title                     | Body                               |
		| 20     | Some random post title    | New sentence inside this post body |
		Then response has userId, posted title and body

Scenario: Deleting a post
		Given a user posts request to service
		When user deletes a post between 1 and 100
		Then response has 200 status code