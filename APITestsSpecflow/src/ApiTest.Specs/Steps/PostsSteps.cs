using System;
using System.Collections.Generic;
using System.Net;
using System.Reflection;
using ApiTesting.CSharp.Framework.Resources;
using ApiTesting.CSharp.Framework.Models;
using NUnit.Framework;
using RestSharp;
using TechTalk.SpecFlow;
using TechTalk.SpecFlow.Assist;

namespace ApiTesting.CSharp.Specs.Steps
{
    [Binding]
    public class PostsSteps
    {
        private UserContext UserContext;
        private PostsObject PostsObject;

        public PostsSteps(UserContext userContext)
        {
            UserContext = userContext;
        }

        [Given(@"a user posts request to service")]
        public void GivenAUserPostsRequestToService()
        {
            PostsObject = new PostsObject();
        }


        [When(@"user makes a request to all post")]
        public void WhenUserMakesARequestToAllPosts()
        {
            ScenarioContext.Current.Add("Response", PostsObject.GetPosts());
        }

        [Then(@"there should be (.*) posts in the response")]
        public void ThenThereShouldBePostsInTheResponse(int postsCount)
        {
            var response = ScenarioContext.Current.Get<IRestResponse<List<Post>>>("Response");
            Assert.IsTrue(response.Data.Count.Equals(postsCount));
        }


        [Then(@"posts should have all properties from response")]
        public void ThenPostsShouldHaveAllPropertiesFromResponse()
        {
            var response = ScenarioContext.Current.Get<IRestResponse<List<Post>>>("Response");
            foreach (var post in response.Data)
            {
                Assert.Multiple(() =>
                {
                    Assert.IsNotNull(post.Id);
                    Assert.IsNotNull(post.UserId);
                    Assert.IsNotNull(post.Title);
                    Assert.IsNotNull(post.Body);
                });
            }
        }


        [Given(@"the user has userId is between (.*) and (.*)")]
        public void GivenTheUserHasUserIdBetweenAnd(int minUserId, int maxUserId)
        {
            UserContext.Post.UserId = RandomNumGenerator(minUserId, maxUserId);
        }

        [When(@"user posts with following title and body:")]
        public void WhenUserPostsWithFollowingTitleAndBody(Table table)
        {
            UserContext.Post = table.CreateInstance<Post>();
            ScenarioContext.Current.Add("Response", PostsObject.SendPost(UserContext.Post));
        }

        [Then(@"response has post Id '(.*)'")]
        public void ThenResponseHasPostId(int postId)
        {
            var response = ScenarioContext.Current.Get<IRestResponse<Post>>("Response");
            Assert.AreEqual(response.Data.Id, postId);
        }

        [Then(@"response has userId, posted title and body")]
        public void ThenResponseHasUserIdPostedTitleAndBody()
        {
            var response = ScenarioContext.Current.Get<IRestResponse<Post>>("Response");
            Assert.Multiple(() =>
            {
                Assert.AreEqual(response.Data.UserId, UserContext.Post.UserId);
                Assert.AreEqual(response.Data.Title, UserContext.Post.Title);
                Assert.AreEqual(response.Data.Body, UserContext.Post.Body);
            });
        }

        [Then(@"response contains new '(.*)'")]
        public void ThenResponseContainsNew(string propertyName)
        {
            var response = ScenarioContext.Current.Get<IRestResponse<Post>>("Response");
            Assert.AreEqual(GetPropertyValue(response, propertyName), GetPropertyValue(UserContext.Post, propertyName));
        }

        [When(@"user updates a post between (.*) and (.*) with values:")]
        public void WhenUserUpdatesAPostBetweenAndWithValues(int minPostId, int maxPostId, Table table)
        {
            UserContext.Post = table.CreateInstance<Post>();
            UserContext.Post.Id = RandomNumGenerator(minPostId, maxPostId);
            ScenarioContext.Current.Add("Response", PostsObject.UpdatePost(UserContext.Post));
        }

        [When(@"user deletes a post between (.*) and (.*)")]
        public void WhenUserDeletesAPostBetweenAnd(int minPostId, int maxPostId)
        {
            UserContext.Post.Id = RandomNumGenerator(minPostId, maxPostId);
            ScenarioContext.Current.Add("Response", PostsObject.DeletePost(UserContext.Post));
        }

        [Then(@"response has (.*) status code")]
        public void ThenResponseHasStatusCode(int expectedStatusCode)
        {
            var response = ScenarioContext.Current.Get<IRestResponse>("Response");
            Assert.AreEqual(response.StatusCode, (HttpStatusCode)expectedStatusCode);
        }

        private static object GetPropertyValue(object src, string propName)
        {
            return src.GetType().GetProperty(propName)?.GetValue(src, null);
        }

        private static int RandomNumGenerator(int min, int max)
        {
            var random = new Random();
            return random.Next(min, max);
        }

    }
}
